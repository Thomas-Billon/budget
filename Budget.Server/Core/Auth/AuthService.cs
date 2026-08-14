using Budget.Server.Api.Users.Models.Requests;
using Budget.Server.Api.Users.Models.Responses;
using Budget.Server.Core.Auth.Jwt;
using Budget.Server.Core.Auth.Models;
using Budget.Server.Core.Email.Senders;
using Budget.Server.Core.Email.Templates;
using Budget.Server.Data;
using Budget.Server.Data.Users;
using Budget.Server.Middleware.Configuration;
using Budget.Server.Middleware.Conventions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Budget.Server.Core.Auth
{
    public class AuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly AuthConfiguration _authConfiguration;
        private readonly IEmailSender _emailSender;
        private readonly AppConfiguration _appConfiguration;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context,
            AuthConfiguration authConfiguration,
            IEmailSender emailSender,
            AppConfiguration appConfiguration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _authConfiguration = authConfiguration;
            _emailSender = emailSender;
            _appConfiguration = appConfiguration;
        }

        public async Task<bool> RegisterAsync(RegisterRequest request)
        {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                // TODO: Handle error
                return false;
            }

            await SendEmailEmailConfirmationAsync(user);

            return true;
        }

        public async Task<AccessTokenResponse?> LoginAsync(LoginRequest request, HttpContext httpContext)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return null;
            }

            // INFO: Respects lockout (3 failed attempts → 1-hour lockout, configured in IdentityOptions)
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);
            if (!result.Succeeded)
            {
                // TODO: Handle error
                return null;
            }

            if (!user.EmailConfirmed)
            {
                return null;
            }

            return await IssueTokensAsync(httpContext, user);
        }

        public async Task<AccessTokenResponse?> RefreshAsync(HttpContext httpContext)
        {
            UserRefreshToken? refreshToken = null;

            var refreshTokenValue = GetRefreshTokenCookie(httpContext);

            if (!string.IsNullOrEmpty(refreshTokenValue))
            {
                var refreshTokenHash = HashRefreshToken(refreshTokenValue);

                refreshToken = await _context.UserRefreshTokens
                    .Include(r => r.User)
                    .Where(r => r.TokenHash == refreshTokenHash)
                    .FirstOrDefaultAsync();
            }

            if (refreshToken == null || refreshToken.User == null)
            {
                return null;
            }

            if (refreshToken.IsRevoked)
            {
                // INFO: Concurrent refreshes can present the same token (ex: multiple tabs reloading at the same time).
                // This concurrency is tolerated for a short grace period, instead of being mistaken for a stolen copy.
                if (refreshToken.IsWithinReuseGracePeriod(_authConfiguration.RefreshToken.ReuseGraceInSeconds))
                {
                    return await IssueTokensAsync(httpContext, refreshToken.User);
                }

                // INFO: Token was already revoked, meaning this is most likely a stolen copy, so we revoke all active tokens for this user to prevent further misuse.
                await RevokeAllRefreshTokensAsync(refreshToken.UserId);
                return null;
            }

            if (refreshToken.ExpiresAt < DateTimeOffset.UtcNow)
            {
                return null;
            }

            await RevokeRefreshTokenAsync(httpContext, refreshToken);

            return await IssueTokensAsync(httpContext, refreshToken.User);
        }

        public async Task LogoutAsync(HttpContext httpContext)
        {
            UserRefreshToken? refreshToken = null;

            var refreshTokenValue = GetRefreshTokenCookie(httpContext);

            if (!string.IsNullOrEmpty(refreshTokenValue))
            {
                var refreshTokenHash = HashRefreshToken(refreshTokenValue);

                refreshToken = await _context.UserRefreshTokens
                    .Where(r => r.TokenHash == refreshTokenHash)
                    .FirstOrDefaultAsync();
            }

            await RevokeRefreshTokenAsync(httpContext, refreshToken);
        }

        #region Reset Password

        public async Task ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return;
            }

            await SendEmailForgotPasswordAsync(user);
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
            if (!result.Succeeded)
            {
                return false;
            }

            await RevokeAllRefreshTokensAsync(user.Id);
            return true;
        }

        private async Task SendEmailForgotPasswordAsync(ApplicationUser user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetUrl = $"{_appConfiguration.Url}/reset-password?email={Uri.EscapeDataString(user.Email)}&token={Uri.EscapeDataString(token)}";
            var htmlBody = EmailTemplatePasswordReset.BuildHtml(resetUrl, _authConfiguration.ResetPassword.ExpirationInSeconds);

            await _emailSender.SendAsync(user.Email, EmailTemplatePasswordReset.Subject, htmlBody);
        }

        #endregion Reset Password

        #region Confirm Email

        public async Task ResendEmailConfirmationAsync(ResendEmailConfirmationRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || user.EmailConfirmed)
            {
                return;
            }

            await SendEmailEmailConfirmationAsync(user);
        }

        private async Task SendEmailEmailConfirmationAsync(ApplicationUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmUrl = $"{_appConfiguration.Url}/confirm-email?email={Uri.EscapeDataString(user.Email)}&token={Uri.EscapeDataString(token)}";
            var htmlBody = EmailTemplateEmailConfirmation.BuildHtml(confirmUrl, _authConfiguration.EmailConfirmation.ExpirationInSeconds);

            await _emailSender.SendAsync(user.Email, EmailTemplateEmailConfirmation.Subject, htmlBody);
        }

        public async Task<bool> ConfirmEmailAsync(ConfirmEmailRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.ConfirmEmailAsync(user, request.Token);
            return result.Succeeded;
        }

        #endregion Confirm Email

        private async Task<AccessTokenResponse> IssueTokensAsync(HttpContext httpContext, ApplicationUser user)
        {
            await IssueRefreshTokenAsync(httpContext, user);
            var accessToken = IssueAccessToken(user);

            return new AccessTokenResponse
            {
                Token = accessToken.Token,
                ExpiresAt = accessToken.ExpiresAt,
            };
        }

        #region Access Token

        private AccessToken IssueAccessToken(ApplicationUser user)
        {
            var accessTokenValue = GenerateAccessToken(user);
            var accessTokenExpiresAt = DateTimeOffset.UtcNow.AddSeconds(_authConfiguration.AccessToken.ExpirationInSeconds);
            
            return new AccessToken
            {
                Token = accessTokenValue,
                ExpiresAt = accessTokenExpiresAt
            };
        }

        private string GenerateAccessToken(ApplicationUser user)
        {
            // INFO: MapInboundClaims is set to false so we can use claim names instead of .NET-style URIs.
            var claims = new List<Claim>
            {
                new(ClaimNames.Id, user.Id),
                new(ClaimNames.Email, user.Email),
            };

            if (user.FirstName != null)
            {
                claims.Add(new(ClaimNames.FirstName, user.FirstName));
            }
            if (user.LastName != null)
            {
                claims.Add(new(ClaimNames.LastName, user.LastName));
            }

            var expiresAt = DateTime.UtcNow.AddSeconds(_authConfiguration.AccessToken.ExpirationInSeconds);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authConfiguration.AccessToken.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _authConfiguration.AccessToken.Issuer,
                audience: _authConfiguration.AccessToken.Audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        #endregion Access Token

        #region Refresh Token

        private async Task<UserRefreshToken> IssueRefreshTokenAsync(HttpContext httpContext, ApplicationUser user)
        {
            var refreshTokenValue = GenerateRefreshToken();
            var refreshTokenExpiresAt = DateTimeOffset.UtcNow.AddSeconds(_authConfiguration.RefreshToken.ExpirationInSeconds);

            var refreshToken = new UserRefreshToken
            {
                TokenHash = HashRefreshToken(refreshTokenValue),
                UserId = user.Id,
                ExpiresAt = refreshTokenExpiresAt,
                IsRevoked = false,
            };

            _context.UserRefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            SetRefreshTokenCookie(httpContext, refreshTokenValue, refreshTokenExpiresAt);

            return refreshToken;
        }

        private string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        private static string HashRefreshToken(string refreshTokenValue)
        {
            var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(refreshTokenValue));
            return Convert.ToBase64String(hashBytes);
        }

        private async Task RevokeRefreshTokenAsync(HttpContext httpContext, UserRefreshToken? refreshToken)
        {
            if (refreshToken != null)
            {
                refreshToken.IsRevoked = true;
                refreshToken.RevokedAt = DateTimeOffset.UtcNow;
                await _context.SaveChangesAsync();
            }

            DeleteRefreshTokenCookie(httpContext);
        }

        private async Task RevokeAllRefreshTokensAsync(string userId)
        {
            var revokedAt = DateTimeOffset.UtcNow;

            var refreshTokens = await _context.UserRefreshTokens
                .Where(x => x.UserId == userId)
                .Where(x => x.IsRevoked == false)
                .ToListAsync();

            foreach (var refreshToken in refreshTokens)
            {
                refreshToken.IsRevoked = true;
                refreshToken.RevokedAt = revokedAt;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<int> CleanupExpiredRefreshTokensAsync()
        {
            var now = DateTimeOffset.UtcNow;

            return await _context.UserRefreshTokens
                .Where(x => x.IsRevoked || x.ExpiresAt < now)
                .ExecuteDeleteAsync();
        }

        #endregion Refresh Token

        #region Refresh Token Cookie

        private const string RefreshTokenCookiePath = $"/{ApiRoutePrefixConvention.PREFIX}/auth";

        private string? GetRefreshTokenCookie(HttpContext httpContext)
        {
            return httpContext.Request.Cookies[_authConfiguration.RefreshToken.CookieKey];
        }

        private void SetRefreshTokenCookie(HttpContext httpContext, string value, DateTimeOffset expiresAt)
        {
            httpContext.Response.Cookies.Append(_authConfiguration.RefreshToken.CookieKey, value, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = RefreshTokenCookiePath,
                Expires = expiresAt,
            });
        }

        private void DeleteRefreshTokenCookie(HttpContext httpContext)
        {
            httpContext.Response.Cookies.Delete(_authConfiguration.RefreshToken.CookieKey, new CookieOptions
            {
                Path = RefreshTokenCookiePath,
            });
        }

        #endregion Refresh Token Cookie
    }
}

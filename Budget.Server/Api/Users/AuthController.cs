using Budget.Server.Api.Users.Models.Requests;
using Budget.Server.Api.Users.Models.Responses;
using Budget.Server.Core.Auth;
using Budget.Server.Core.Errors;
using Budget.Server.Middleware.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Net;

namespace Budget.Server.Api.Users
{
    [Route("[controller]")]
    public class AuthController : ApiControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        [EnableRateLimiting(RateLimiterConfiguration.RegisterPolicy)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var success = await _authService.RegisterAsync(request);

            if (!success)
            {
                return Failure(HttpStatusCode.BadRequest, ErrorCodes.Auth.RegistrationFailed);
            }

            return Ok();
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [EnableRateLimiting(RateLimiterConfiguration.LoginPolicy)]
        public async Task<ActionResult<AccessTokenResponse>> Login([FromBody] LoginRequest request)
        {
            var response = await _authService.LoginAsync(request, HttpContext);

            if (response == null)
            {
                return Failure(HttpStatusCode.Unauthorized, ErrorCodes.Auth.InvalidCredentials);
            }

            return Ok(response);
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<ActionResult<AccessTokenResponse>> Refresh()
        {
            var response = await _authService.RefreshAsync(HttpContext);

            if (response == null)
            {
                return Failure(HttpStatusCode.Unauthorized, ErrorCodes.Auth.TokenExpired);
            }

            return Ok(response);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync(HttpContext);

            return Ok();
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        [EnableRateLimiting(RateLimiterConfiguration.ForgotPasswordPolicy)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            await _authService.ForgotPasswordAsync(request);

            return Ok();
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        [EnableRateLimiting(RateLimiterConfiguration.ResetPasswordPolicy)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var success = await _authService.ResetPasswordAsync(request);

            if (!success)
            {
                return Failure(HttpStatusCode.BadRequest, ErrorCodes.Auth.ResetPasswordFailed);
            }

            return Ok();
        }

        [HttpPost("confirm-email")]
        [AllowAnonymous]
        [EnableRateLimiting(RateLimiterConfiguration.ConfirmEmailPolicy)]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request)
        {
            var success = await _authService.ConfirmEmailAsync(request);

            if (!success)
            {
                return Failure(HttpStatusCode.BadRequest, ErrorCodes.Auth.EmailConfirmationFailed);
            }

            return Ok();
        }

        [HttpPost("resend-email-confirmation")]
        [AllowAnonymous]
        [EnableRateLimiting(RateLimiterConfiguration.ResendEmailConfirmationPolicy)]
        public async Task<IActionResult> ResendEmailConfirmation([FromBody] ResendEmailConfirmationRequest request)
        {
            await _authService.ResendEmailConfirmationAsync(request);

            return Ok();
        }
    }
}

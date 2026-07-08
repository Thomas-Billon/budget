using System.IdentityModel.Tokens.Jwt;

namespace Budget.Server.Core.Auth.Jwt
{
    public static class ClaimNames
    {
        // Standard JWT claim names (RFC 7519)
        public const string Id = JwtRegisteredClaimNames.Sub;        // "sub"
        public const string Email = JwtRegisteredClaimNames.Email;   // "email"

        // OpenID Connect standard claim names (not in JwtRegisteredClaimNames)
        public const string FirstName = "given_name";
        public const string LastName = "family_name";
    }
}

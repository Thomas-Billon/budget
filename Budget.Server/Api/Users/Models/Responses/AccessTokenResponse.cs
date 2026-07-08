namespace Budget.Server.Api.Users.Models.Responses
{
    public class AccessTokenResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTimeOffset ExpiresAt { get; set; }
    }
}

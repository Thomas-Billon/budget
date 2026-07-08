namespace Budget.Server.Core.Auth.Models
{
    public class AccessToken
    {
        public string Token { get; set; } = string.Empty;
        public DateTimeOffset ExpiresAt { get; set; }
    }
}

namespace Budget.Server.Middleware.Configuration
{
    public class SmtpConfiguration
    {
        public const string CONFIG_KEY = "Smtp";

        public required string Host { get; init; }
        public required int Port { get; init; }
        public required bool EnableSsl { get; init; }
        public required string Username { get; init; }
        public required string Password { get; init; }
        public required string FromAddress { get; init; }
        public required string FromName { get; init; }
    }
}

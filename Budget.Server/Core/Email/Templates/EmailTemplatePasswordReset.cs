namespace Budget.Server.Core.Email.Templates
{
    public static class EmailTemplatePasswordReset
    {
        public const string Subject = "Reset your Budget password";

        public static string BuildHtml(string resetUrl, int expirationInSeconds)
        {
            var expirationMinutes = expirationInSeconds / 60;

            var content = $"""
                <p>We received a request to reset your password. Click the button below to choose a new one:</p>
                <p>
                    <a href="{resetUrl}" style="display:inline-block;padding:10px 20px;background-color:#0d6efd;color:#ffffff;text-decoration:none;border-radius:4px;">
                        Reset password
                    </a>
                </p>
                <p>This link expires in {expirationMinutes} minutes. If you didn't request a password reset, you can safely ignore this email.</p>
                """;

            return EmailTemplateBase.BuildHtml("Budget", content);
        }
    }
}

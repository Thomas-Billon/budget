namespace Budget.Server.Core.Email.Templates
{
    public static class EmailTemplateEmailConfirmation
    {
        public const string Subject = "Confirm your Budget email address";

        public static string BuildHtml(string confirmUrl, int expirationInSeconds)
        {
            var expirationHours = expirationInSeconds / 3600;

            var content = $"""
                <p>Thanks for signing up! Click the button below to confirm your email address:</p>
                <p>
                    <a href="{confirmUrl}" style="display:inline-block;padding:10px 20px;background-color:#0d6efd;color:#ffffff;text-decoration:none;border-radius:4px;">
                        Confirm email
                    </a>
                </p>
                <p>This link expires in {expirationHours} hours. If you didn't create an account, you can safely ignore this email.</p>
                """;

            return EmailTemplateBase.BuildHtml("Budget", content);
        }
    }
}

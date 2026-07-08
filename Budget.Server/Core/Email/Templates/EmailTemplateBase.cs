namespace Budget.Server.Core.Email.Templates
{
    public static class EmailTemplateBase
    {
        public static string BuildHtml(string title, string content)
        {
            return $"""
                <html>
                <body style="font-family: sans-serif;">
                    <h2>{title}</h2>
                    {content}
                </body>
                </html>
                """;
        }
    }
}

using Budget.Server.Core.Auth;
using Budget.Server.Middleware.Configuration;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Budget.Server.Core.Email.Senders
{
    public class MailKitEmailSender : IEmailSender
    {
        private readonly SmtpConfiguration _smtpConfiguration;
        private readonly ILogger<IEmailSender> _logger;

        public MailKitEmailSender(
            SmtpConfiguration smtpConfiguration,
            ILogger<IEmailSender> logger)
        {
            _smtpConfiguration = smtpConfiguration;
            _logger = logger;
        }

        public async Task SendAsync(string to, string subject, string htmlBody)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_smtpConfiguration.FromName, _smtpConfiguration.FromAddress));
                message.To.Add(MailboxAddress.Parse(to));
                message.Subject = subject;
                message.Body = new BodyBuilder { HtmlBody = htmlBody }.ToMessageBody();

                using var client = new SmtpClient();

                var secureSocketOptions = _smtpConfiguration.EnableSsl
                    ? SecureSocketOptions.SslOnConnect
                    : SecureSocketOptions.StartTlsWhenAvailable;

                await client.ConnectAsync(_smtpConfiguration.Host, _smtpConfiguration.Port, secureSocketOptions);
                await client.AuthenticateAsync(_smtpConfiguration.Username, _smtpConfiguration.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                // INFO: Never let email delivery failures surface to the caller.
                // Returning an error may allow account enumeration.
                _logger.LogError(ex, "Failed to send email confirmation email to {to}", to);
            }
        }
    }
}

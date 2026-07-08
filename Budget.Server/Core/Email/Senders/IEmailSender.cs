namespace Budget.Server.Core.Email.Senders
{
    public interface IEmailSender
    {
        Task SendAsync(string to, string subject, string htmlBody);
    }
}

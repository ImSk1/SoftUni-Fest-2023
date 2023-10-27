namespace SoftwareFest.MailSending
{
    public interface IMailSender
    {
        Task SendEmailAsync(MailMessage message);

    }
}

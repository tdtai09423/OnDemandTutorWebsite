namespace ODTDemoAPI.Services
{
    public interface IEmailService
    {
        Task SendMailAsync(string toEmail, string subject, string body);

        Task SendMailWithTransactionAsync(string toEmail, string subject, string body, int transactionId);
    }
}

using System.Net;
using System.Net.Mail;

namespace ODTDemoAPI.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMailAsync(string toEmail, string subject, string body)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("tqthang1210@gmail.com", "pnhrsjntrtbhuuhf"),
                EnableSsl = true
            };
            var mailMessage = new MailMessage
            {
                From = new MailAddress("tqthang1210@gmail.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(toEmail);

            smtpClient.Send(mailMessage);
        }
    }
}
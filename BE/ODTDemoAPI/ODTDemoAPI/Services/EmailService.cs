using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace ODTDemoAPI.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSetting _smtpSetting;

        public EmailService(IOptionsMonitor<SmtpSetting> optionsMonitor)
        {
            _smtpSetting = optionsMonitor.CurrentValue;
        }

        public async Task SendMailAsync(string toEmail, string subject, string body)
        {
            var smtpClient = new SmtpClient(_smtpSetting.Host)
            {
                Port = 587,
                Credentials = new NetworkCredential(_smtpSetting.UserName, _smtpSetting.Password),
                EnableSsl = true
            };
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpSetting.UserName, _smtpSetting.SenderName),
                Subject = subject,
                Body = Body(subject, "Mr/Ms", body, "On Demand Tutor", "/"),
                IsBodyHtml = true
            };
            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }

        private string Body(string subject, string name, string content, string senderName, string buttonUrl)
        {
            string body = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <title>{subject}</title>
                <style>
                    body {{ font-family: Arial, sans-serif; }}
                    .container {{ width: 80%; margin: auto; padding: 20px; }}
                    .header {{ background-color: #f8f8f8; padding: 10px 20px; border-bottom: 1px solid #ddd; }}
                    .content {{ margin: 20px 0; }}
                    .footer {{ background-color: #f8f8f8; padding: 10px 20px; border-top: 1px solid #ddd; text-align: center; }}
                    .button {{ background-color: #4CAF50; color: white; padding: 10px 20px; text-align: center; text-decoration: none; display: inline-block; margin: 10px 0; }}
               </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1>{subject}</h1>
                    </div>
                    <div class='content'>
                        <p>Dear {name},</p>
                        <p>{content}</p>
                        <p><a href='{buttonUrl}' class='button'>Click here</a></p>
                    </div>
                    <div class='footer'>
                        <p>Best regards,<br>{senderName}</p>
                    </div>
                </div>
            </body>
            </html>
            ";
            return body;
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ODTDemoAPI.Entities;
using System.Net;
using System.Net.Mail;

namespace ODTDemoAPI.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSetting _smtpSetting;
        private readonly IServiceScopeFactory _scopeFactory;

        public EmailService(IOptionsMonitor<SmtpSetting> optionsMonitor, IServiceScopeFactory scopeFactory)
        {
            _smtpSetting = optionsMonitor.CurrentValue;
            _scopeFactory = scopeFactory;
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
                Body = Body(subject, "Mr/Ms", body, "On Demand Tutor", "https://localhost:3000"),
                IsBodyHtml = true
            };
            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }

        public async Task SendMailWithTransactionAsync(string toEmail, string subject, string body, int transactionId)
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
                Body = BodyWithTransaction(subject, "Mr/Ms", body, "On Demand Tutor", "https://localhost:3000", transactionId, toEmail),
                IsBodyHtml = true
            };
            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }

        private string Body(string subject, string name, string content, string senderName, string buttonUrl)
        {
            string body = $@"
    <!DOCTYPE html>
    <html lang=""en"">
    <head>
        <meta charset=""UTF-8"">
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
        <title>{subject}</title>
        <style>
            body {{
                font-family: 'Poppins', Arial, sans-serif;
                margin: 0;
                padding: 0;
                background-color: #f4f4f4;
            }}
            .container {{
                max-width: 600px;
                margin: 0 auto;
                background-color: #ffffff;
                border: 1px solid #cccccc;
            }}
            .header {{
                background-color: #ff7aac;
                padding: 40px;
                text-align: center;
                color: white;
                font-size: 24px;
            }}
            .body {{
                padding: 40px;
                text-align: left;
                font-size: 16px;
                line-height: 1.6;
            }}
            .cta {{
                padding: 10px 20px;
                border-radius: 5px;
                border: 1px solid black;
                display: inline-block;
                color: black;
                text-decoration: none;
                font-weight: bold;
            }}
            .footer {{
                background-color: #333333;
                padding: 40px;
                text-align: center;
                color: white;
                font-size: 14px;
            }}
        </style>
    </head>
    <body>
        <div class=""container"">
            <div class=""header"">
            </div>
            <div class=""body"">
                <p>Dear {name},</p>
                <p>{content}</p>
                <p>Thank you for using our website.</p>
                <div style=""text-align: center;"">
                    <a href=""{buttonUrl}"" target=""_blank"" class=""cta"">Click the Button</a>
                </div>
            </div>
            <div class=""footer"">
                <p>Copyright &copy; 2024 | {senderName}</p>
            </div>
        </div>
    </body>
    </html>";
            return body;
        }

        private string BodyWithTransaction(string subject, string name, string content, string senderName, string buttonUrl, int transactionId, string toEmail)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<OnDemandTutorContext>();

                var transaction = context.Transactions.Include(t => t.Account).FirstOrDefault(t => t.TransactionId == transactionId);

                string transactionTable = $@"
        <table style=""width: 100%; border-collapse: collapse;"">
            <thead>
                <tr style=""background-color: #f2f2f2;"">
                    <th style=""border: 1px solid #dddddd; padding: 8px;"">Tên</th>
                    <th style=""border: 1px solid #dddddd; padding: 8px;"">Email</th>
                    <th style=""border: 1px solid #dddddd; padding: 8px;"">Ngày giờ giao dịch</th>
                    <th style=""border: 1px solid #dddddd; padding: 8px;"">Số tiền giao dịch</th>
                    <th style=""border: 1px solid #dddddd; padding: 8px;"">Loại giao dịch</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td style=""border: 1px solid #dddddd; padding: 8px;"">{transaction!.Account.FirstName} {transaction!.Account.LastName}</td>
                    <td style=""border: 1px solid #dddddd; padding: 8px;"">{toEmail}</td>
                    <td style=""border: 1px solid #dddddd; padding: 8px;"">{transaction!.TransactionDate}</td>
                    <td style=""border: 1px solid #dddddd; padding: 8px;"">{transaction!.Amount}</td>
                    <td style=""border: 1px solid #dddddd; padding: 8px;"">{transaction!.TransactionType}</td>
                </tr>
            </tbody>
        </table>";

                string body = $@"
    <!DOCTYPE html>
    <html lang=""en"">
    <head>
        <meta charset=""UTF-8"">
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
        <title>{subject}</title>
        <style>
            body {{
                font-family: 'Poppins', Arial, sans-serif;
                margin: 0;
                padding: 0;
                background-color: #f4f4f4;
            }}
            .container {{
                max-width: 600px;
                margin: 0 auto;
                background-color: #ffffff;
                border: 1px solid #cccccc;
            }}
            .header {{
                background-color: #ff7aac;
                padding: 40px;
                text-align: center;
                color: white;
                font-size: 24px;
            }}
            .body {{
                padding: 40px;
                text-align: left;
                font-size: 16px;
                line-height: 1.6;
            }}
            .cta {{
                padding: 10px 20px;
                border-radius: 5px;
                border: 1px solid black;
                display: inline-block;
                color: black;
                text-decoration: none;
                font-weight: bold;
            }}
            .footer {{
                background-color: #333333;
                padding: 40px;
                text-align: center;
                color: white;
                font-size: 14px;
            }}
        </style>
    </head>
    <body>
        <div class=""container"">
            <div class=""header"">
            </div>
            <div class=""body"">
                <p>Dear {name},</p>
                <p>{content}</p>
                <p>Here are the details of your recent transaction:</p>
                {transactionTable}
                <p>Thank you for using our website.</p>
                <div style=""text-align: center;"">
                    <a href=""{buttonUrl}"" target=""_blank"" class=""cta"">Click the Button</a>
                </div>
            </div>
            <div class=""footer"">
                <p>Copyright &copy; 2024 | {senderName}</p>
            </div>
        </div>
    </body>
    </html>";
                return body;
            }
        }
    }
}
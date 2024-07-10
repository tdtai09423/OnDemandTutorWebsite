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
<html lang=""en"">

<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>{subject}</title>
</head>

<body>
    <div style=""display: flex; justify-content: center;"">
        <table class=""table"">
            <tr>
                <td>
                    <table>
    
                        <body style=""font-family: 'Poppins', Arial, sans-serif"">
                            <table>
                                <tr>
                                    <td>
                                        <table class=""content""
                                            style=""border-collapse: collapse; border: 1px solid #cccccc;"">
                                            <!-- Header -->
                                            <tr>
                                                <td class=""header""
                                                    style=""background-color: #ff7aac; padding: 40px; text-align: center; color: white; font-size: 24px;"">
                                                    <img src=""/images/logo.png"" style=""width: auto; height: 100px;"" />
                                                </td>
                                            </tr>
    
                                            <!-- Body -->
                                            <tr>
                                                <td class=""body""
                                                    style=""padding: 40px; text-align: left; font-size: 16px; line-height: 1.6;"">
                                                    Hello {name}<br>
                                                    {content}
                                                    email validation code
                                                </td>
                                            </tr>
    
                                            <!-- Call to action Button -->
                                            <tr>
                                                <td style=""padding: 0px 40px 0px 40px; text-align: center;"">
                                                    <!-- CTA Button -->
                                                    <table cellspacing=""0"" cellpadding=""0"" style=""margin: auto;"">
                                                        <tr>
                                                            <td
                                                                style=""padding: 10px 20px; border-radius: 5px; border: 1px solid black;"">
                                                                <a href=""{buttonUrl}"" target=""_blank""
                                                                    style=""color: black; text-decoration: none; font-weight: bold;"">
                                                                    Go back to website
                                                                </a>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class=""body""
                                                    style=""padding: 40px; text-align: left; font-size: 16px; line-height: 1.6;"">
                                                    Thanks you for using our website {senderName}
                                                </td>
                                            </tr>
                                            <!-- Footer -->
                                            <tr>
                                                <td class=""footer""
                                                    style=""background-color: #333333; padding: 40px; text-align: center; color: white; font-size: 14px;"">
                                                    Copyright &copy; 2024 | On Demand Tutor Website
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </body>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</body>

</html>
            ";
            return body;
        }
    }
}
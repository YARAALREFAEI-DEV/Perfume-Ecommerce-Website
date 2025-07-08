using System.Net.Mail;
using System.Net;

namespace WebAppPerfumes01.Models
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendOrderConfirmationEmail(string subject, string body)
        {
            var smtpClient = new System.Net.Mail.SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("PERFUMECOLLECTION2025@gmail.com", "hlszuvuddptveuot"),
                EnableSsl = true // Ensures a secure connection
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("PERFUMECOLLECTION2025@gmail.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };

            mailMessage.To.Add("PERFUMESALES2025@gmail.com");

            smtpClient.Send(mailMessage);

        }
    }
}

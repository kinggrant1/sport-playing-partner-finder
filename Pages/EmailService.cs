using System.Net.Mail;
using System.Net;

namespace MyWebApp.Services
{
    public class EmailService
    {
        public void SendEmail(string toEmail, string subject, string body)
        {
            var client = new SmtpClient("sandbox.smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential("554adbe87302ec", "c4a2fe1c5b02ab"),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("help@partnerfinder.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(toEmail);

            client.Send(mailMessage);
        }
    }
}

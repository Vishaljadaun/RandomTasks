using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RandomTasks.Models;
using System.Net.Mail;
using System.Net;

namespace RandomTasks.Controllers
{
    public class EmailController : Controller
    {
        private readonly EmailSettings _emailSettings;

        public EmailController(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendEmail(string emailBody)
        {
            try
            {
                var mail = new MailMessage
                {
                    From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
                    Subject = "Test Email from .NET Core App",
                    Body = emailBody,
                    IsBodyHtml = true
                };

                // Change this to the recipient's email
                mail.To.Add("tm0122333444455555@gmail.com");

                using (var smtp = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port))
                {
                    smtp.Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.Password);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }

                ViewBag.Message = "Email sent successfully!";
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Error: {ex.Message}";
            }

            return View("Index");
        }
    }
}

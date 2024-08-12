using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizationWebsite.Models;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;

namespace OrganizationWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Home()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Contact(string Name, string Email, string Message)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("prabinraj48@gmail.com");
            mail.To.Add(new MailAddress("prabinraj48@gmail.com"));
            mail.Subject = "Student information";
            mail.IsBodyHtml = false;
            mail.Body = $"Name: {Name}\nEmail Address: {Email}\nMessage: {Message}";

            using var smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("prabinraj48@gmail.com", "wwmu libo dyrz fhzt");
            smtp.EnableSsl = true;

            try
            {
                smtp.Send(mail);
                TempData["message"] = "Mail sent successfully! We will contact you soon.";
                TempData["isError"] = false;
            }
            catch (Exception)
            {
                TempData["message"] = "Unable to send an email right now. Please try again.";
                TempData["isError"] = true;
            }

            return RedirectToAction("Contact");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

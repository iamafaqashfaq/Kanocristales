using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BlyckBox.Models;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace BlyckBox.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;
        private readonly DataContext context;
        public HomeController(ILogger<HomeController> logger, IConfiguration config, DataContext _context)
        {
            _logger = logger;
            _config = config;
            context = _context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }
        public IActionResult Services()
        {
            return View();
        }
        public IActionResult ContactUs()
        {
            return View();
        }

        public async Task<IActionResult> Blogs()
        {
            var blogs = await context.Blogs!.ToListAsync();
            return View(blogs);
        }

        public IActionResult PrivacyPolicy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult ContactUs(ContactUs model)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(_config.GetSection("emailcred:email").Value);
            message.To.Add(new MailAddress(model.Email!));
            message.Subject = model.Name + "- Contacted From BlyckBox Website";
            var emailTemplate = model.Message;
            message.IsBodyHtml = false;
            message.Body = emailTemplate;
            SmtpClient smtp = new SmtpClient();
            smtp.Port = 465;
            smtp.Host = _config.GetSection("emailcred:host").Value;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(_config.GetSection("emailcred:email").Value, _config.GetSection("emailcred:password").Value);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(message);
            return View("Index");
        }
    }
}

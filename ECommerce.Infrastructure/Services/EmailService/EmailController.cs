using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;


namespace ECommerce.Infrastructure.Services.EmailService
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController(IEmailService emailService) : ControllerBase
    {
        // Service for sending emails
        private readonly IEmailService _emailService = emailService;

        // POST api/email
        [HttpPost]
        public IActionResult SendEmail(EmailDto request)
        {
            // Call the service to send the email
            _emailService.SendEmailAsync(request);

            // Return success response
            return Ok(new { message = "Email sent successfully" });
        }
    }
}

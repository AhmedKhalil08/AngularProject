using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System.Net.Mail;
using System.Net;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using ECommerce.Infrastructure.Persistence.Repositories;
using ECommerce.Domain.Entities;
using ECommerce.Application.DTOs;


namespace ECommerce.Infrastructure.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailService> _logger;
        public EmailService(IConfiguration config, ILogger<EmailService> logger)
        {
            this._config = config ?? throw new ArgumentNullException(nameof(config));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task SendEmail(EmailDto request)
        {
            ArgumentNullException.ThrowIfNull(request);

            try
            {
                // Send confirmation email to the user
                var confirmationEmail = CreateConfirmationEmail(request);
                await SendEmailMessageAsync(confirmationEmail);

                // Send original message to the business
                var businessEmail = CreateBusinessEmail(request);
                await SendEmailMessageAsync(businessEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email to {Recipient}", request.To);
                throw;
            }
        }

        
        private MimeMessage CreateConfirmationEmail(EmailDto request)
        {
            var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse(_config["EmailUsername"]));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = "Thank you for your message";

            email.Body = new TextPart(TextFormat.Html)
            {
                Text = "<p>Thank you for your email. We will get in touch with you shortly.</p>"
            };

            return email;
        }

        private MimeMessage CreateConfirmationOrderEmail(EmailDto request,OrderDto order)
        {
            var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse(_config["EmailUsername"]));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = "Order confirmed!";

            email.Body = new TextPart(TextFormat.Html)
            {
                Text = $"<p>Thanks for your order,with ID {order.Id} , " +
                $"Your total cost is {order.TotalAmount}.</p>" +
                $"<table><tr><th>Item</th><th>Quantity</th><th>Price</th></tr>" +
                $"{string.Join("", order.OrderItems.Select(item => $"<tr><td>{item.ProductName}</td><td>{item.Quantity}</td><td>{item.Price}</td></tr>"))}</table>"
            };

            return email;
        }



        // Email to the business with the user's message
        private MimeMessage CreateBusinessEmail(EmailDto request)
        {
            var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse(_config.GetValue<string>("EmailSettings:EmailUsername")));
            email.To.Add(MailboxAddress.Parse(_config.GetValue<string>("EmailSettings:EmailUsername")));
            email.Subject = $"New message from {request.ContactName}";

            email.Body = new TextPart(TextFormat.Html)
            {
                Text = $@"
                    <h2>New contact form submission</h2>
                    <p><strong>From:</strong> {request.ContactName} ({request.To})</p>
                    <p><strong>Message:</strong></p>
                    <p>{request.Body}</p>"
            };

            return email;
        }

        // Connects and sends the email
        private async Task SendEmailMessageAsync(MimeMessage email)
        {
            using var smtp = new SmtpClient();

            var host = _config.GetValue<string>("EmailSettings:EmailHost");
            var port = _config.GetValue<int>("EmailSettings:Port");
            var username = _config.GetValue<string>("EmailSettings:EmailUsername");
            var password = _config.GetValue<string>("EmailSettings:EmailPassword");

            if (string.IsNullOrWhiteSpace(host))
                throw new InvalidOperationException("EmailHost configuration is missing");
            if (port <= 0)
                throw new InvalidOperationException("Invalid Port configuration");
            if (string.IsNullOrWhiteSpace(username))
                throw new InvalidOperationException("EmailUsername configuration is missing");
            if (string.IsNullOrWhiteSpace(password))
                throw new InvalidOperationException("EmailPassword configuration is missing");

            smtp.Connect(host, port, SecureSocketOptions.StartTls);
            smtp.Authenticate(username, password);

            try
            {
                smtp.Send(email);
            }
            finally
            {
                smtp.Disconnect(true);
            }
        }
    }


}

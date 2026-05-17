using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ECommerce.Application.DTOs;

namespace ECommerce.Infrastructure.Services.EmailService
{
    public class EmailService(IConfiguration config, ILogger<EmailService> logger) : IEmailService
    {
        private readonly IConfiguration _config = config ?? throw new ArgumentNullException(nameof(config));
        private readonly ILogger<EmailService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task SendEmailAsync(EmailDto request)
        {
            ArgumentNullException.ThrowIfNull(request);

            try
            {
                var confirmationEmail = CreateConfirmationEmail(request);
                await SendEmailMessageAsync(confirmationEmail);

                var businessEmail = CreateBusinessEmail(request);
                await SendEmailMessageAsync(businessEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email to {Recipient}", request.To);
                throw;
            }
        }

        public async Task SendEmailConf(EmailDto request)
        {
            ArgumentNullException.ThrowIfNull(request);

            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_config["EmailSettings:EmailUsername"]));
                email.To.Add(MailboxAddress.Parse(request.To));
                email.Subject = "Confirm your Registration!";
                email.Body = new TextPart(TextFormat.Html)
                {
                    Text = $@"
                  from confirmation function :
                   <br>
                    <p>{request.Body}</p>"
                };
                await SendEmailMessageAsync(email);

                
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
            email.From.Add(MailboxAddress.Parse(_config["EmailSettings:EmailUsername"]));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = "Thank you for your message";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = "<p>Thank you for your email. We will get in touch with you shortly.</p>"
            };
            return email;
        }

        private MimeMessage CreateBusinessEmail(EmailDto request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config["EmailSettings:EmailUsername"]));
            email.To.Add(MailboxAddress.Parse(_config["EmailSettings:EmailUsername"]));
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

        private async Task SendEmailMessageAsync(MimeMessage email)
        {
            var host = _config["EmailSettings:EmailHost"];
            var port = _config.GetValue<int>("EmailSettings:Port");
            var username = _config["EmailSettings:EmailUsername"];
            var password = _config["EmailSettings:EmailPassword"];

            if (string.IsNullOrWhiteSpace(host))
                throw new InvalidOperationException("EmailHost configuration is missing");
            if (port <= 0)
                throw new InvalidOperationException("Invalid Port configuration");
            if (string.IsNullOrWhiteSpace(username))
                throw new InvalidOperationException("EmailUsername configuration is missing");
            if (string.IsNullOrWhiteSpace(password))
                throw new InvalidOperationException("EmailPassword configuration is missing");

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(host, port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(username, password);

            try
            {
                await smtp.SendAsync(email);
            }
            finally
            {
                await smtp.DisconnectAsync(true);
            }
        }
        public async Task SendCustomEmailAsync(string to, string subject, string body)
        {
            if (string.IsNullOrEmpty(to)) throw new ArgumentNullException(nameof(to));

            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_config["EmailSettings:EmailUsername"]));
                email.To.Add(MailboxAddress.Parse(to));

                email.Subject = subject; 
                email.Body = new TextPart(TextFormat.Html)
                {
                    Text = body
                };

                await SendEmailMessageAsync(email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending custom email to {Recipient}", to);
                throw;
            }
        }
    }
}
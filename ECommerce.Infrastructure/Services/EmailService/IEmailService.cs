using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Services.EmailService
{
    public interface IEmailService
    {
        // void SendEmail(EmailDto request);
        Task SendEmailAsync(EmailDto request);
        Task SendEmailConf(EmailDto request);
    }
}

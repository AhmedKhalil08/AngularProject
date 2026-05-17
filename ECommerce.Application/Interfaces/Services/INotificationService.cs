using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Interfaces.Services
{
    public interface INotificationService
    {
        Task SendNewMessageNotification(int id, string name, string subject, DateTime sentAt);
    }
}

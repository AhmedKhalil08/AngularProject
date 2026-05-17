using ECommerce.API.Hubs;
using ECommerce.Application.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;

namespace ECommerce.API.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendNewMessageNotification(int id, string name, string subject, DateTime sentAt)
        {
            await _hubContext.Clients.Group("Admins").SendAsync("NewMessage", new { id, name, subject, sentAt });
        }
    }
}

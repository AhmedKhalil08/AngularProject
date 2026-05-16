using ECommerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Entities
{
    public class NewsletterSubscriber :BaseEntite<int>
    {
        public string Email { get; set; }
        public DateTime SubscribedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
}

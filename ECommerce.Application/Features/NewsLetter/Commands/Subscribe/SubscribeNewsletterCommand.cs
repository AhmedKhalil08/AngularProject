using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.NewsLetter.Commands.Subscribe
{
    public class SubscribeNewsletterCommand : IRequest<bool>
    {
        public string Email { get; set; }
    }
}

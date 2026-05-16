using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.ContactMessages.Commands.MarkAsRead
{
    public class MarkAsReadCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}

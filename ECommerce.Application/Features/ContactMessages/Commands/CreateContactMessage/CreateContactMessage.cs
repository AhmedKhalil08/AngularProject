using ECommerce.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.ContactMessages.Commands.CreateContactMessage
{
    public class CreateContactMessageCommand : IRequest<ContactMessageDto>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}

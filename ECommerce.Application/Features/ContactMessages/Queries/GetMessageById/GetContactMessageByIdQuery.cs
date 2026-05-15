using ECommerce.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.ContactMessages.Queries.GetMessageById
{
    public class GetContactMessageByIdQuery : IRequest<ContactMessageDto>
    {
        public int Id { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using ECommerce.Application.DTOs;

namespace ECommerce.Application.Features.PromoCodes.Queries.GetPromoCodeById
{
    public class GetPromoCodeByIdQuery : IRequest<PromoCodeDto>
    {
        public int Id { get; set; }
    }
}

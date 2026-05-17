using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using ECommerce.Application.DTOs;

namespace ECommerce.Application.Features.PromoCodes.Queries.GetAllPromoCodes
{
    public class GetAllPromoCodesQuery: IRequest<List<PromoCodeDto>>
    {

    }
}

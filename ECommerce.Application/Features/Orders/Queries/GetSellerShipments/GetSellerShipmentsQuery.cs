using ECommerce.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Orders.Queries.GetSellerShipments
{
    public class GetSellerShipmentsQuery:IRequest<List<SellerShipmentDto>>
    {
    }
}

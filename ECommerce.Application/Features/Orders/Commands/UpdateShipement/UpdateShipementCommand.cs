using ECommerce.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Orders.Commands.UpdateShipement
{
    public class UpdateShipementCommand : IRequest<bool>
    {
        public int ShipmentId { get; set; }
        public ShipmentStatus ShipmentStatus { get; set; }


    }
}

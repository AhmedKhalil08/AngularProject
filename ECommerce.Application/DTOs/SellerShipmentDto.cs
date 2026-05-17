using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.DTOs
{
    public class SellerShipmentDto:ShipmentDto
    {
        public int orderId;
        public AddressDto address;
    }
}

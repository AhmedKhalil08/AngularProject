using ECommerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Entities
{
    public class Cart : BaseEntite<int>
    {
        //-------------------------------------------------------------------------
        //                             SELF PROPS
        //-------------------------------------------------------------------------
        public int Id { get; set; }
        //-------------------------------------------------------------------------
        //                             FOREIGN KEY
        //-------------------------------------------------------------------------
        public string UserId { get; set; }
        //-------------------------------------------------------------------------
        //                             NAVIGATION
        //-------------------------------------------------------------------------
        public ApplicationUser User { get; set; }
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}

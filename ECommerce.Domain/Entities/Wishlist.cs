using ECommerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Entities
{
    public class Wishlist : BaseEntite<int>
    {
        //-------------------------------------------------------------------------
        //                             SELF PROPS
        //-------------------------------------------------------------------------
        public int Id { get; set; }
        public DateTime AddedAt { get; set; }
        //-------------------------------------------------------------------------
        //                             FOREIGN KEY
        //-------------------------------------------------------------------------
        public string UserId { get; set; }
        public int ProductId { get; set; }
        //-------------------------------------------------------------------------
        //                             NAVIGATION
        //-------------------------------------------------------------------------
        public ApplicationUser User { get; set; }
        public Product Product { get; set; }
    }
}

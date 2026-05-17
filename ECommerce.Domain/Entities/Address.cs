using ECommerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Entities
{
    public class Address : BaseEntity<int>
    {
        //-------------------------------------------------------------------------
        //                             SELF PROPS
        //-------------------------------------------------------------------------
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public bool IsDefault { get; set; }
        //-------------------------------------------------------------------------
        //                             FOREIGN KEY
        //-------------------------------------------------------------------------
        public string UserId { get; set; }

        //-------------------------------------------------------------------------
        //                             NAVIGATION
        //-------------------------------------------------------------------------
        public ApplicationUser User { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Entities
{
    public class Banner  : BaseEntity<int>
    {
        //-------------------------------------------------------------------------
        //                             SELF PROPS
        //-------------------------------------------------------------------------

       
        public string Title { get; set; } 
        public string ImageUrl { get; set; }
        public string? Link { get; set; }
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }

        //-------------------------------------------------------------------------
        //                             FOREIGN KEY
        //-------------------------------------------------------------------------

        /*-------------------------------------------------------------------------*/
        //                          Navigation Props 
        /*-------------------------------------------------------------------------*/
    }
}

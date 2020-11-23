using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlnBlogAndShop.ViewModel
{
    [Serializable]
    public class CartItem
    {

        public int cId { get; set; }
        public string cName { get; set; }
        public decimal cPrice { get; set; }
        public int cQuantity { get; set; }
        public decimal cTotal
        {
            get
            {
                return cPrice * cQuantity;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace SlnBlogAndShop.ViewModel
{
    public static class Operation
    {
        [WebMethod(EnableSession = true)]
        public static Cart getCurrentCart()
        {
            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Session["Cart"] == null)
                {
                    var order = new Cart();
                    HttpContext.Current.Session["Cart"] = order;
                }
                return (Cart)HttpContext.Current.Session["Cart"];
            }
            else
            {
                throw new InvalidOperationException("System.Web.HttpContext.Current為空，請檢查");
            }
        }
    }

}
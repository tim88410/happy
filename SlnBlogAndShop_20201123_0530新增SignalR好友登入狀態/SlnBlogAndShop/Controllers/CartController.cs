using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SlnBlogAndShop.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult GetCart()
        {
            return PartialView("_CartPartial");
        }

        //以id加入Product至購物車，並回傳購物車頁面
        public ActionResult AddToCart(int id)
        {
            var currentCart = ViewModel.Operation.getCurrentCart();
            currentCart.addProduct(id);
            return PartialView("_CartPartial");
        }
    }
}
using prjBlog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlnBlogAndShop.ViewModel
{
    [Serializable]
    public class Cart : IEnumerable<CartItem>
    {

        public Cart()
        {
            cartItems = new List<CartItem>();
        }
        private List<CartItem> cartItems;
        public int count
        {
            get
            {
                return cartItems.Count;
            }
        }


        public decimal Totalprice
        {
            get
            {
                decimal totalprice = 0.0m;
                foreach (var cartItem in cartItems)
                {
                    totalprice = totalprice + cartItem.cTotal;
                }
                return totalprice;
            }
        }
        public bool addProduct(int id)
        {
            var findItem = cartItems
                            .Where(m => m.cId == id)
                            .Select(m => m)
                            .FirstOrDefault();


            if (findItem == default(CartItem))
            {
                happyEntities db = new happyEntities();
                var product = db.tProducts.Where(m => m.fProductId == id).FirstOrDefault();
                if (product != default(tProduct))
                {
                    addProduct(product);
                }

            }
            else
            {
                findItem.cQuantity += 1;
            }
            return true;
        }

        private bool addProduct(tProduct t)
        {

            var cartItem = new CartItem()
            {
                cId = t.fProductId,
                cName = t.fProductName,
                cPrice = t.fUnitprice,
                cQuantity = 1
            };


            cartItems.Add(cartItem);
            return true;
        }

        IEnumerator<CartItem> IEnumerable<CartItem>.GetEnumerator()
        {
            return this.cartItems.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.cartItems.GetEnumerator();
        }

        //public IEnumerator<CartItem> GetEnumerator()
        //{
        //    throw new NotImplementedException();
        //}

        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
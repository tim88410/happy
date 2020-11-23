using SlnBlogAndShop;
using SlnBlogAndShop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjBlog.Controllers.Shop
{
    public class ShopController : Controller
    {
        // GET: Shop
        public ActionResult Index()
        {



            happyEntities db = new happyEntities();

            IEnumerable<tProduct> newtable = null;
            //table = db.tProduct.OrderByDescending(p => p.fProductlaunch).ToList().Take(3);
            newtable = (from q in db.tProducts
                        orderby q.fProductlaunch descending
                        select q).ToList().Take(3);

            var hottable = db.tProducts.OrderBy(p => p.fProductlaunch).ToList().Take(3);

            var nametable = db.tSuppliers.ToList();

            CShop cShop = new CShop();
            List<tProduct> s最新 = new List<tProduct>();
            List<tProduct> s熱門 = new List<tProduct>();
            List<tSupplier> s品牌廠商 = new List<tSupplier>();

            foreach (var item in newtable)
            {
                s最新.Add(item);
            }
            foreach (var item in hottable)
            {
                s熱門.Add(item);
            }
            foreach (var item in nametable)
            {
                s品牌廠商.Add(item);
            }
            cShop.shopNew最新 = s最新;
            cShop.shopHot熱門 = s熱門;
            cShop.shop品牌廠商 = s品牌廠商;

            return View(cShop);
        }



        public ActionResult shop(int id)
        {
            happyEntities db = new happyEntities();

            List<tProduct> protable = null;
            if (id == 0)
            {
                protable = db.tProducts.ToList();
            }
            else
            {
                protable = db.tProducts.Where(m => m.fSupplierId == id).ToList();
            }

            var nametable = db.tSuppliers.ToList();

            CShop cShop = new CShop();
            List<tProduct> s品牌商品 = new List<tProduct>();
            List<tSupplier> s品牌廠商 = new List<tSupplier>();
            foreach (var item in protable)
            {
                s品牌商品.Add(item);
            }

            foreach (var item in nametable)
            {
                s品牌廠商.Add(item);
            }
            cShop.shop品牌商品 = s品牌商品;
            cShop.shop品牌廠商 = s品牌廠商;

            return View(cShop);
        }

        public ActionResult product_details()
        {
            happyEntities db = new happyEntities();
            var nametable = db.tSuppliers.ToList();
            CShop cShop = new CShop();
            List<tSupplier> s品牌廠商 = new List<tSupplier>();
            foreach (var item in nametable)
            {
                s品牌廠商.Add(item);
            }
            cShop.shop品牌廠商 = s品牌廠商;

            return View(cShop);
        }

        public ActionResult cart()
        {
            happyEntities db = new happyEntities();
            var nametable = db.tSuppliers.ToList();
            CShop cShop = new CShop();
            List<tSupplier> s品牌廠商 = new List<tSupplier>();
            foreach (var item in nametable)
            {
                s品牌廠商.Add(item);
            }
            cShop.shop品牌廠商 = s品牌廠商;

            return View(cShop);
        }

        public ActionResult checkout()
        {
            happyEntities db = new happyEntities();
            var nametable = db.tSuppliers.ToList();
            CShop cShop = new CShop();
            List<tSupplier> s品牌廠商 = new List<tSupplier>();
            foreach (var item in nametable)
            {
                s品牌廠商.Add(item);
            }
            cShop.shop品牌廠商 = s品牌廠商;

            return View(cShop);


        }

        public ActionResult wishlist()
        {
            happyEntities db = new happyEntities();
            var nametable = db.tSuppliers.ToList();
            CShop cShop = new CShop();
            List<tSupplier> s品牌廠商 = new List<tSupplier>();
            foreach (var item in nametable)
            {
                s品牌廠商.Add(item);
            }
            cShop.shop品牌廠商 = s品牌廠商;

            return View(cShop);
        }
    }
}
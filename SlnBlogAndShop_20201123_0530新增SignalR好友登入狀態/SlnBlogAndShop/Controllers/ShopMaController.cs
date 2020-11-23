using PagedList;
using prjBlog;
using prjBlog.Models;
using prjBlog.ViewModel;
using SlnBlogAndShop;
using SlnBlogAndShop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sellerprojectmvc.Controllers
{
    public class ShopMaController : Controller
    {
        // GET: seller

        public ActionResult homepage()
        {
            return View();
        }
        public ActionResult message()
        {
            return View();
        }
        happyEntities db = new happyEntities();
        int pagesize = 3;
        public ActionResult product(int page = 1)
        {
            IEnumerable<tProduct> table = null;
            page = page < 1 ? 1 : page;
            var products = db.tProducts.OrderBy(p => p.fProductId).ToList();

            table = from p in (new happyEntities()).tProducts
                    select p;

            List<CProduct> list = new List<CProduct>();

            foreach (tProduct p in table)
            {
                list.Add(new CProduct() { entity = p });
            }
            IPagedList<CProduct> pages = list.ToPagedList(page, pagesize);

            if (Request.IsAjaxRequest())
            {
                return PartialView(pages);
            }
            else
            {

                return View(pages);
            }

        }
        public ActionResult ShowProductTable(int page = 1)
        {
            int pageSize = 1;
            page = page < 1 ? 1 : page;
            List<tProduct> products = db.tProducts.ToList();
            IPagedList<tProduct> pages = products.ToPagedList(page, pageSize);
            return PartialView(pages);
        }

        public ActionResult create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult create(tProduct p, CImage img)
        {
            //使用Session取得fSupplierId  &  建立DB happyEntities實體=======================
            TMember mem = Session[CDictionary.SK_LOGINED_USER] as TMember;
            happyEntities db = new happyEntities();
            var q = db.tSuppliers.Where(m => m.fMemberId == mem.fMemberId).FirstOrDefault();
            int sid = q.fSupplierId;
            p.fSupplierId = sid;


            //商品照片fImagePath===========================================================            
            int index = img.image.FileName.IndexOf(".");
            string extention = img.image.FileName.Substring(index, img.image.FileName.Length - index);
            string photoName = Guid.NewGuid().ToString() + extention;
            p.fProductpicture = photoName;
            img.image.SaveAs(Server.MapPath("/Content/Shop/ProductPic/") + photoName);

            db.tProducts.Add(p);
            db.SaveChanges();

            return RedirectToAction("product");
        }
        public ActionResult delete(int id)
        {
            happyEntities db = new happyEntities();
            tProduct pro = db.tProducts.FirstOrDefault(p => p.fProductId == id);
            if (pro != null)
            {
                db.tProducts.Remove(pro);
                db.SaveChanges();
            }

            return RedirectToAction("product");
        }

        public ActionResult updateproduct(int id)
        {

            tProduct prod = (new happyEntities()).tProducts.FirstOrDefault(p => p.fProductId == id);

            if (prod == null)
                return RedirectToAction("product");
            CProduct cpro = new CProduct() { entity = prod };
            return View(cpro);
        }
        [HttpPost]
        public ActionResult updateproduct(tProduct p, CImage img)

        {
            TMember mem = Session[CDictionary.SK_LOGINED_USER] as TMember;
            happyEntities db = new happyEntities();
            tProduct prod = db.tProducts.FirstOrDefault(m => m.fProductId == p.fProductId);


            int index = img.image.FileName.IndexOf(".");
            string extention = img.image.FileName.Substring(index, img.image.FileName.Length - index);
            string photoName = Guid.NewGuid().ToString() + extention;
            p.fProductpicture = photoName;
            img.image.SaveAs(Server.MapPath("/Content/Shop/ProductPic/") + photoName);

            if (prod != null)
            {
                var q = db.tSuppliers.Where(m => m.fMemberId == mem.fMemberId).FirstOrDefault();

                int sid = q.fSupplierId;
                prod.fProductName = p.fProductName;
                prod.fProductinstock = p.fProductinstock;
                prod.fProductdescription = p.fProductdescription;
                prod.fUnitprice = p.fUnitprice;
                prod.fSupplierId = sid;
                prod.fCategoryId = p.fCategoryId;
                prod.fProductsize = p.fProductsize;
                prod.fProductpicture = p.fProductpicture;


                db.SaveChanges();
            }
            return RedirectToAction("product");
        }


        public ActionResult orderdetial()
        {
            return View();
        }

        public ActionResult shippedorderdetial()
        {
            return View();
        }
        public ActionResult profile()
        {
            return View();
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlnBlogAndShop.ViewModel
{
    public class CShop
    {

        public List<tProduct> shopNew最新 { get; set; }
        public List<tProduct> shopHot熱門 { get; set; }
        public List<tProduct> shop品牌商品 { get; set; }
        public List<tSupplier> shop品牌廠商 { get; set; }

    }
}
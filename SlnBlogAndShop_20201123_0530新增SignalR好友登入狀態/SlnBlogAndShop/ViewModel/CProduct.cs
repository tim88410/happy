using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SlnBlogAndShop.ViewModel
{
    public class CProduct
    {
        public tProduct entity { get; set; }
        [DisplayName("編號")]
        public int fProductId { get { return entity.fProductId; } }
        public string fProductName { get { return entity.fProductName; } }
        public int fSupplierId { get { return entity.fSupplierId; } }
        public int fCategoryId { get { return entity.fCategoryId; } }
        public string fProductdescription { get { return entity.fProductdescription; } }
        public string fProductpicture { get { return entity.fProductpicture; } }
        public int fProductinstock { get { return entity.fProductinstock; } }
        public Nullable<int> fBrandId { get { return entity.fBrandId; } }
        public int fUnitprice { get { return entity.fUnitprice; } }
        public string fexamplesize { get { return entity.fProductsize; } }

        public virtual tSupplier tSupplier { get; set; }
        //public List<tProduct> hot { get; set; }
        //public List<tProduct> newpro { get; set; }

        public HttpPostedFileBase image { get; set; }


    }
}
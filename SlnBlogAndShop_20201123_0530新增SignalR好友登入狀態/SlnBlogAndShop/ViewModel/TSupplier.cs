using SlnBlogAndShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjBlog.ViewModel
{
    public class TSupplier
    {
        public int fSupplierId { get; set; }
        public Nullable<int> fMemberId { get; set; }
        public string fName { get; set; }

        public virtual tMember tMember { get; set; }
        public HttpPostedFileBase file_photo { get; set; }
    }
}
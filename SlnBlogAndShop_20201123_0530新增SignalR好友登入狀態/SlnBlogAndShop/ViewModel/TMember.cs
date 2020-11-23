using SlnBlogAndShop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;

namespace prjBlog.ViewModel
{
    public class TMember
    {
       
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TMember()
        {
            this.tSuppliers = new HashSet<tSupplier>();
        }

        public int fMemberId { get; set; }
        [Required(ErrorMessage = "請輸入Email")]
        [DisplayName("Email")]
        public string fEmail { get; set; }
        [Required(ErrorMessage = "請輸入姓名")]
        [DisplayName("姓名")]
        public string fName { get; set; }
        [DisplayName("電話")]
        public string fPhone { get; set; }
        [Required(ErrorMessage = "請輸入密碼")]
        [DisplayName("密碼")]
        public string fPassword { get; set; }
        public string fAddress { get; set; }
        public string fCity { get; set; }
        public string fRegion { get; set; }
        public Nullable<int> fPostalcode { get; set; }
        public string fCountry { get; set; }
        public string fFax { get; set; }
        public string fFirstname { get; set; }
        public string fLastname { get; set; }
        public string fUsername { get; set; }
        [DisplayName("頭像")]
        public string fIdPhoto { get; set; }
        public string fCoverPhoto { get; set; }
        public HttpPostedFileBase file_photo { get; set; }
        public string file_image {get; set;}
        public string fSignalRUserId { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tSupplier> tSuppliers { get; set; }

    }
}
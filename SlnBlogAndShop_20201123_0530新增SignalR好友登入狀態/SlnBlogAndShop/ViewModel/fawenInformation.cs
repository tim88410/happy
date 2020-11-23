using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlnBlogAndShop.ViewModel
{
    public class fawenInformation
    {
        public int fPostId { get; set; }
        public int fMemberId { get; set; }
        public string fTitle { get; set; }
        public string fDescription { get; set; }
        public System.DateTime fPosttime { get; set; }
        public Nullable<bool> fPersonal { get; set; }
        public Nullable<bool> fEarn { get; set; }
        public Nullable<bool> fHasImage { get; set; }
        public string fIdPhoto { get; set; }
        public string fCoverPhoto { get; set; }
        public string fName { get; set; }
    }
}
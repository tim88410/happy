using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlnBlogAndShop.ViewModel
{
    public class articleContextJson
    {
        public string text { get; set; }
        public string img { get; set; }
        public string fName { get; set; }
        public string fTitle { get; set; }
        public string fIdPhoto { get; set; }
        public System.DateTime fPosttime { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace prjBlog.ViewModel
{
    public class CLoginViewModel
    {
        public string txtAccount { get; set; }
        public string txtPassword { get; set; }
    }
}
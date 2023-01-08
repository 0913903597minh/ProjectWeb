using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectWeb.Areas.Admin.Models
{
    public class UserSignup
    {
        public int UserId { set; get; }
        public string UserName { set; get; }
        public string UserEmail { set; get; }
        public string Password { set; get; }
    }
}
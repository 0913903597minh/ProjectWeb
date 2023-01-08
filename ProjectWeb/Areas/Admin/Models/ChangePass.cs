using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Models
{
    public class ChangePass
    {
        int? id { get; set; }
        public string oldPass { get; set; }
        public string newPass { get; set; }
        public string renewPass { get; set; }
    }
}

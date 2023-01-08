using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LibData.Utilities
{
    public class HtmlHelper
    {
        public class Alert
        {
            public string Warning;
            public AlertType Type;

            public enum AlertType
            {
                Success = 0,
                Fail = 1,
                Warning = 2
            }
            public Alert(string warning, AlertType type)
            {
                this.Warning = warning;
                this.Type = type;
            }

            public string AlertStyle
            {
                get
                {
                    if (this.Type == AlertType.Success) return "alert-success";
                    if (this.Type == AlertType.Warning) return "alert-warning";
                    if (this.Type == AlertType.Fail) return "alert-danger";
                    return "alert-info";
                }
            }

            public string ToHtmlString()
            {
                return 
                    "<div class='alert " + AlertStyle + " alert-dismissable sharp'>"
                    + "<button type='button' class='close' data-dismiss='alert' aria-hidden='true'>×</button>"
                    + Warning
                    + "</div>";
            }

            public override string ToString()
            {
                return this.ToHtmlString();
            }
        }

        public static string Placeholder(HtmlString htmlstring)
        {
            return HttpUtility.HtmlDecode(htmlstring.ToHtmlString());
        }
    }
}

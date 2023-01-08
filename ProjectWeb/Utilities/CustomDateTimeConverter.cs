using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;

namespace LibData.Utilities
{
    /// <summary>
    /// yyyy-MM-dd HH:mm:ss
    /// </summary>
    public class CustomDateTimeConverter : IsoDateTimeConverter
    {
        public CustomDateTimeConverter()
        {
            base.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        }
    }
    /// <summary>
    /// MM/dd/yyyy HH:mm:sss
    /// </summary>
    public class CustomDateTimeConverter2 : IsoDateTimeConverter
    {
        public CustomDateTimeConverter2()
        {
            base.DateTimeFormat = "MM/dd/yyyy HH:mm:sss";
        }
    }
    /// <summary>
    /// yyyy-MM-dd
    /// </summary>
    public class CustomDateConverter : IsoDateTimeConverter
    {
        public CustomDateConverter()
        {
            base.DateTimeFormat = "yyyy-MM-dd";
        }
    }
}

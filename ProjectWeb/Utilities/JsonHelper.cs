using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data;
using System.IO;

namespace LibData.Utilities
{
    public class JsonHelper
    {
        public static Models.MessagesFileUpload GetURLAndType(String jsonString)
        {
            try
            {
                Models.MessagesFileUpload uploadedFileProperties = new Models.MessagesFileUpload();
                dynamic stuff = JsonConvert.DeserializeObject(jsonString);
                uploadedFileProperties.url = stuff.attachments[0]["url"];
                uploadedFileProperties.filename = Path.GetFileName(uploadedFileProperties.url);
                uploadedFileProperties.type = stuff.attachments[0]["type"];
                return uploadedFileProperties;
            }
            catch (Exception)
            {
                return null;
            }



        }
    }
}

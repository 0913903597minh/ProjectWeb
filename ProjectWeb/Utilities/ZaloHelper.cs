using log4net;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LibData.Utilities
{
    public class ZaloHelper
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static bool sendMsg(string token, string recipient, string text)
        {
            try
            {
                var client = new RestClient("https://openapi.zalo.me/v2.0/oa/message?access_token=" + token);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", "{\r\n  \"recipient\":{\r\n    \"user_id\":\"" + recipient + "\"\r\n  },\r\n  \"message\":{\r\n    \"text\":\"" + Regex.Replace(text, @"\n", "\\n") + "\"\r\n  }\r\n}", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                log.Info(response.Content);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }
        public static bool sendUrlImage(string token, string recipient, string url)
        {
            try
            {
                var client = new RestClient("https://openapi.zalo.me/v2.0/oa/message?access_token=" + token);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", "{\r\n  \"recipient\": {\r\n    \"user_id\": \"" + recipient + "\"\r\n  },\r\n  \"message\": {\r\n    \"text\": \"\",\r\n    \"attachment\": {\r\n        \"type\": \"template\",\r\n        \"payload\": {\r\n            \"template_type\": \"media\",\r\n            \"elements\": [{\r\n                \"media_type\": \"image\",\r\n                \"url\": \""+url+"\"\r\n            }]\r\n        }\r\n    }\r\n  }\r\n}", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                log.Error(response.Content);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }
        public static bool sendFile(string token, string recipient, string attachment_id,string type,int width, int height)
        {
            try
            {
                var client = new RestClient("https://openapi.zalo.me/v2.0/oa/upload/image?access_token="+ token);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", "{\r\n  \"recipient\": {\r\n    \"user_id\": \""+ recipient + "\"\r\n  },\r\n  \"message\": {\r\n    \"text\": \"Title_of_gif\",\r\n    \"attachment\": {\r\n        \"type\": \"template\",\r\n        \"payload\": {\r\n            \"template_type\": \"media\",\r\n            \"elements\": [{\r\n                \"media_type\": \""+type+"\",\r\n                \"attachment_id\": \""+attachment_id+"\",\r\n                \"width\": "+ width + ",\r\n                \"height\": "+ height + "\r\n            }]\r\n        }\r\n    }\r\n  }\r\n}", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
                log.Info(response.Content);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }
        public static string UploadImage(string token,string url)
        {
            try
            {
                var client = new RestClient("https://openapi.zalo.me/v2.0/oa/upload/image?access_token=" + token);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddFile("file", url);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
                log.Info(response.Content);
                dynamic obj = JsonConvert.DeserializeObject<dynamic>(response.Content);
                return obj.data.attachment_id;
            }
            catch (Exception)
            {

            }
            return "";
        }
        public static string getAvatar(string token, string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    var client = new RestClient("https://openapi.zalo.me/v2.0/oa/getprofile?access_token="+token+"&data={\"user_id\":\""+id+"\"} ");
                    client.Timeout = -1;
                    var request = new RestRequest(Method.GET);
                    IRestResponse response = client.Execute(request);

                    dynamic obj = JsonConvert.DeserializeObject<dynamic>(response.Content);
                    return obj.data.avatar + "";
                }
                else
                {
                    return "";
                }
            }
            catch (Exception)
            {
                return "";
            }

        }
        //public static bool UploadFile(string token, string recipient, string text, string type)
        //{
        //    try
        //    {
        //        var client = new RestClient("https://openapi.zalo.me/v2.0/oa/upload/file?access_token=" + token);
        //        client.Timeout = -1;
        //        var request = new RestRequest(Method.GET);
        //        request.AddHeader("Content-Type", "application/json");
        //        request.AddParameter("application/json", "{\r\n  \"recipient\": {\r\n    \"user_id\": \"" + recipient + "\"\r\n  },\r\n  \"message\": {\r\n    \"text\": \"\",\r\n    \"attachment\": {\r\n        \"type\": \"template\",\r\n        \"payload\": {\r\n            \"template_type\": \"media\",\r\n            \"elements\": [{\r\n                \"media_type\": \"image\",\r\n                \"url\": \"https://developers.zalo.me/web/static/zalo.png\"\r\n            }]\r\n        }\r\n    }\r\n  }\r\n}", ParameterType.RequestBody);
        //        IRestResponse response = client.Execute(request);
        //        log.Error(response.Content);
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //    }
        //    return false;
        //}
    }
}

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
    public class FacebookHelper
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static bool sendMsg(string token,string src,string dest,string text)
        {
            try
            {
                var client = new RestClient("https://graph.facebook.com/v5.0/" + src + "/messages?access_token=" + token);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json; charset=utf-8", "{\r\n  \"messaging_type\": \"RESPONSE\",\r\n  \"recipient\": {\r\n    \"id\": \"" + dest + "\"\r\n  },\r\n  \"message\": {\r\n    \"text\": \"" + Regex.Replace(text, @"\n", "\\n") + "\"\r\n  }\r\n}", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                log.Info("sendMsg " + response.Content);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
           
        }
        public static string UploadFile(string token, string src,string urlFile,string type)
        {
            try
            {
                log.Info("UploadFile src : " + src);
                log.Info("UploadFile urlFile : " + urlFile);
                log.Info("UploadFile type : " + type);
                //image video audio file
                var client = new RestClient("https://graph.facebook.com/v5.0/"+src+"/message_attachments?access_token="+token);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", "{\r\n  \"message\":{\r\n    \"attachment\":{\r\n      \"type\":\""+ type + "\", \r\n      \"payload\":{\r\n        \"is_reusable\": true,\r\n        \"url\":\""+urlFile+"\"\r\n      }\r\n    }\r\n  }\r\n}", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                log.Info("UploadFile " + response.Content);
                dynamic obj = JsonConvert.DeserializeObject<dynamic>(response.Content);
                return obj.attachment_id;
            }
            catch (Exception)
            {
                return "";
            }
            
        }
        public static bool sendFile(string token, string src, string dest, string attachment_id,string type)
        {
            try
            {
                log.Info("sendFile attachment_id : " + attachment_id);
                log.Info("sendFile type : " + type);
                var client = new RestClient("https://graph.facebook.com/v5.0/"+ src + "/messages?access_token="+ token);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", "{\r\n  \"recipient\":{\r\n    \"id\":\""+ dest + "\"\r\n  },\r\n  \"message\":{\r\n    \"attachment\":{\r\n      \"type\":\""+type+"\", \r\n      \"payload\":{\r\n        \"attachment_id\": \""+ attachment_id + "\"\r\n      }\r\n    }\r\n  }\r\n}", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                log.Info("sendFile " + response.Content);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
           
        }
        public static string getAvatar(string token, string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(token))
                {


                    var client = new RestClient("https://graph.facebook.com/"+ id + "?fields=profile_pic&access_token="+ token);
                    client.Timeout = -1;
                    var request = new RestRequest(Method.GET);
                    IRestResponse response = client.Execute(request);
                    dynamic obj = JsonConvert.DeserializeObject<dynamic>(response.Content);
                    return obj.profile_pic + "";
                }
                else
                {
                    return "https://graph.facebook.com/" + id + "/picture?redirect=true&type=large";
                }
                
            }
            catch (Exception)
            {

                return "";
            }
           
        }
    }
}

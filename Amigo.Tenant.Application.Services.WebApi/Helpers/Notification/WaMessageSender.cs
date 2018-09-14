using System;
using System.Net;
using System.Web.Script.Serialization; // requires the reference 'System.Web.Extensions'
using System.IO;
using System.Text;
using RestSharp;
using System.Collections.Generic;

namespace Amigo.Tenant.Application.Services.WebApi.Helpers.Notification
{
    public class WaMessageSender
    {
        // TODO: Replace the following with your gateway instance ID, Forever Green client ID and secret:
        //private static string INSTANCE_ID = "YOUR_INSTANCE_ID_HERE";
        //private static string CLIENT_ID = "YOUR_CLIENT_ID_HERE";
        //private static string CLIENT_SECRET = "YOUR_CLIENT_SECRET_HERE";

        //private static string API_URL = "http://api.whatsmate.net/v3/whatsapp/single/text/message/" + INSTANCE_ID;

        //public WaMessageSender(string instanceId, string clientId, string clientSecret)
        //{

        //}

        private readonly string UrlEndPoint;
        private readonly string ApiKey;
        private readonly string Uid;

        public WaMessageSender()
        {
            UrlEndPoint = System.Configuration.ConfigurationManager.AppSettings["waUrlSvcEndPoint"].ToString();
            ApiKey = System.Configuration.ConfigurationManager.AppSettings["waApikey"].ToString();
            Uid = System.Configuration.ConfigurationManager.AppSettings["waUserId"].ToString();
        }

        public string sendMessage(List<string> numbers, string textMessage)
        {
            var errors = new List<string>();
            foreach(var to in numbers)
            {
                try
                {
                    sendWaMessage(to, textMessage);
                }
                catch (Exception ex)
                {
                    errors.Add(ex.Message);
                }
            }
            return String.Join(Environment.NewLine, errors);
        }

        public string sendMessage(string to, string textMessage)
        {
            return sendWaMessage(to, textMessage);
        }

        private string sendWaMessage(string to, string textMessage)
        {
            var client = new RestClient(UrlEndPoint);
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");

            var msgId = Guid.NewGuid();
            request.AddParameter("application/x-www-form-urlencoded", 
                "token="+ ApiKey + 
                "&uid=" + Uid + 
                "&to=" + to + 
                "&custom_uid=" + msgId.ToString() + 
                "&text=" + textMessage, 
                ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            return response.Content;
        }

        //public bool sendMessage(string number, string message)
        //{
        //    bool success = true;

        //    try
        //    {
        //        using (WebClient client = new WebClient())
        //        {
        //            client.Headers[HttpRequestHeader.ContentType] = "application/json";
        //            client.Headers["X-WM-CLIENT-ID"] = CLIENT_ID;
        //            client.Headers["X-WM-CLIENT-SECRET"] = CLIENT_SECRET;

        //            Payload payloadObj = new Payload() { number = number, message = message };
        //            string postData = (new JavaScriptSerializer()).Serialize(payloadObj);

        //            client.Encoding = Encoding.UTF8;
        //            string response = client.UploadString(API_URL, postData);
        //            Console.WriteLine(response);
        //        }
        //    }
        //    catch (WebException webEx)
        //    {
        //        Console.WriteLine(((HttpWebResponse)webEx.Response).StatusCode);
        //        Stream stream = ((HttpWebResponse)webEx.Response).GetResponseStream();
        //        StreamReader reader = new StreamReader(stream);
        //        String body = reader.ReadToEnd();
        //        Console.WriteLine(body);
        //        success = false;
        //    }

        //    return success;
        //}

        //public class Payload
        //{
        //    public string number { get; set; }
        //    public string message { get; set; }
        //}


        //// TODO: Replace the following with your gateway instance ID, Forever Green client ID and secret:
        //private static string INSTANCE_ID = "YOUR_INSTANCE_ID_HERE";
        //private static string CLIENT_ID = "YOUR_CLIENT_ID_HERE";
        //private static string CLIENT_SECRET = "YOUR_CLIENT_SECRET_HERE";

        //private static string GROUP_API_URL = "http://api.whatsmate.net/v3/whatsapp/group/text/message/" + INSTANCE_ID;

        //static void Main(string[] args)
        //{
        //    WaMessageSender msgSender = new WaMessageSender();
        //    string groupAdmin = "51920132774"; // TODO: Specify the WhatsApp number of the group creator, including the country code
        //    string groupName = "Happy Club";   // TODO: Specify the name of the group
        //    string message = "Guys, let's party tonight!";  // TODO: Specify the content of your message

        //    msgSender.sendGroupMessage(groupAdmin, groupName, message);
        //    Console.WriteLine("Press Enter to exit.");
        //    Console.ReadLine();
        //}

        //public bool sendGroupMessage(string groupAdmin, string groupName, string message)
        //{
        //    bool success = true;

        //    try
        //    {
        //        using (WebClient client = new WebClient())
        //        {
        //            client.Headers[HttpRequestHeader.ContentType] = "application/json";
        //            client.Headers["X-WM-CLIENT-ID"] = CLIENT_ID;
        //            client.Headers["X-WM-CLIENT-SECRET"] = CLIENT_SECRET;

        //            GroupPayload groupPayloadObj = new GroupPayload() { group_admin = groupAdmin, group_name = groupName, message = message };
        //            string postData = (new JavaScriptSerializer()).Serialize(groupPayloadObj);

        //            client.Encoding = Encoding.UTF8;
        //            string response = client.UploadString(GROUP_API_URL, postData);
        //            Console.WriteLine(response);
        //        }
        //    }
        //    catch (WebException webEx)
        //    {
        //        Console.WriteLine(((HttpWebResponse)webEx.Response).StatusCode);
        //        Stream stream = ((HttpWebResponse)webEx.Response).GetResponseStream();
        //        StreamReader reader = new StreamReader(stream);
        //        String body = reader.ReadToEnd();
        //        Console.WriteLine(body);
        //        success = false;
        //    }

        //    return success;
        //}

        //public class GroupPayload
        //{
        //    public string group_admin { get; set; }
        //    public string group_name { get; set; }
        //    public string message { get; set; }
        //}
    }
}
using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using usis.Framework.Portable;
using usis.PushNotification;

namespace Playground
{
    public class Test
    {
        //public int Value { get; set; }
        public string Base64DeviceToken { get; set; }
    }
    internal static class PNRouter
    {
        internal static void Main()
        {
            //Test();
            ListDevices();
        }

        internal static void Test()
        {
            //var input = new OperationResult();
            //input.ReportWarning("Test", 42);
            var s = "{\"ReturnCode\":42,\"ResultItems\":[{\"ResultType\":0}],\"ReturnValue\":[{\"Base64DeviceToken\":\"xyz\"},{\"Base64DeviceToken\":\"abc\"}]}";
            //var s2 = JsonConvert.SerializeObject(input);
            var result = JsonConvert.DeserializeObject<OperationResult<Test[]>>(s);
            Console.WriteLine(result);
            ConsoleTool.PressAnyKey();
        }

        internal static void ListDevices()
        {
            var request = WebRequest.CreateHttp($"http://82.223.9.88/APNsRouter/ListDevices");
            request.Credentials = new NetworkCredential("usis", "Master12");
            request.Method = "POST";
            request.ContentType = "application/json";
            using (var stream = request.GetRequestStream())
            {
                var body = new JObject(
                    new JProperty("bundleId", "de.usis.PNRouter"),
                    new JProperty("environment", 0));
                var data = Encoding.ASCII.GetBytes(body.ToString(Formatting.None));
                stream.Write(data, 0, data.Length);
            }
            var reponse = request.GetResponse();
            using (var reader = new StreamReader(reponse.GetResponseStream()))
            {
                string body = reader.ReadToEnd();
                //var json = JObject.Parse(body);
                //var result = JsonConvert.DeserializeObject<OperationResult<APNsDeviceInfo[]>>(json["ListDevicesResult"].ToString(Formatting.None));
                var result = JsonConvert.DeserializeObject<OperationResult<ApnsReceiverInfo[]>>(body);
                Console.WriteLine(result);
            }
            ConsoleTool.PressAnyKey();
        }
    }
}

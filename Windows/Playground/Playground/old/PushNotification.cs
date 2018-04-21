using System;
using System.ServiceModel;
using System.ServiceModel.Web;
using usis.PushNotification;
using usis.Framework.Portable;
using System.Xml.Serialization;
using System.IO;
using System.Globalization;

namespace Playground
{
    internal static class PushNotification
    {
        internal static void Main()
        {
            #region APNs

            using (var client = CreateApnsClient2())
            {
                var router = client.CreateChannel();

                Console.WriteLine();
                ListChannels(router);
                ConsoleTool.PressAnyKey();

                //Console.WriteLine();
                //ListDevices(router);
                //ConsoleTool.PressAnyKey();

                //Console.WriteLine();
                //SendNotification(router);
                //ConsoleTool.PressAnyKey();

                //Console.WriteLine();
                //Broadcast(router);
                //ConsoleTool.PressAnyKey();
            }

            #endregion APNs

            #region WNS notification

            //using (var client = CreateWnsClient())
            //{
            //    var router = client.CreateChannel();

            //    //Console.WriteLine();
            //    //ListChannels(router);
            //    //ConsoleTool.PressAnyKey();

            //    Console.WriteLine();
            //    var payload = "<toast><visual><binding template='ToastText01'><text id='1'>Hello World!</text></binding></visual></toast>";
            //    var toast = new WnsToastNotification();
            //    toast.Visual.Binding.Text.Text = "Hello Windows!";
            //    var serializer = new XmlSerializer(typeof(WnsToastNotification));
            //    using (var writer = new StringWriter(CultureInfo.InvariantCulture))
            //    {
            //        serializer.Serialize(writer, toast);
            //        Console.WriteLine(writer.ToString());
            //        payload = writer.ToString();
            //    }
            //    ConsoleTool.PressAnyKey();

            //    Console.WriteLine();
            //    SendNotification(router, payload);
            //    ConsoleTool.PressAnyKey();
            //}

            #endregion WNS notification
        }

        #region methods

        private static void Broadcast(IApnsRouter router)
        {
            var result = router.ListDevices("de.usis.PNRouter", usis.PushNotification.Environment.Development, null);
            var notification = new ApnsNotification()
            {
                Alert = "Broadcast Test"
            };
            foreach (var device in result.ReturnValue)
            {
                router.SendNotification(device.Base64DeviceToken, null, notification.ToString());
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object,System.Object)")]
        private static void ListChannels(IApnsRouter router)
        {
            foreach (var channel in router.ListChannels().ReturnValue)
            {
                Console.WriteLine(channel);
                Console.WriteLine($"{DateTime.Now} - {channel.Changed}");
            }
        }

        //private static void ListChannels(IWnsRouter router)
        //{
        //    foreach (var channel in router.ListChannels().ReturnValue)
        //    {
        //        Console.WriteLine(channel);
        //    }
        //}

        private static void ListDevices(IApnsRouter router)
        {
            var result = router.ListDevices("de.usis.PNRouter", usis.PushNotification.Environment.Development, null);
            foreach (var device in result.ReturnValue)
            {
                Console.WriteLine(device);
            }
        }

        private static void SendNotification(IApnsRouter router)
        {
            var notification = new ApnsNotification()
            {
                Alert = "Test Notification",
                Badge = 23,
                Sound = ApnsNotification.DefaultSound
            };
            var result = router.SendNotification(null, "1B37B57C444D6C981AB4FF1BA7D3DF3988BFAEFD52DE2AD59AB00B2D20DA3CDB", notification.ToString());
            if (result.Failed)
            {
                Console.WriteLine(result.CreateException());
            }
            else
            {
                NotificationState state;
                do
                {
                    Console.WriteLine("Get notification state ...");
                    state = router.GetNotificationState(result.ReturnValue).ReturnValue;

                } while (state != NotificationState.Sent);
                Console.WriteLine("... notification send.");
            }
        }

        //private static void SendNotification(IWnsRouter router, string payload)
        //{
        //    //var notification = new ApnsNotification()
        //    //{
        //    //    Alert = "Test Notification",
        //    //    Badge = 23,
        //    //    Sound = ApnsNotification.DefaultSound
        //    //};
        //    //var result = router.SendNotification(null, "1B37B57C444D6C981AB4FF1BA7D3DF3988BFAEFD52DE2AD59AB00B2D20DA3CDB", notification.ToString());
        //    var result = router.SendNotification("30e6f006-bc69-47aa-9048-32297209526f", payload);
        //    if (result.Failed)
        //    {
        //        Console.WriteLine(result.CreateException());
        //    }
        //    else
        //    {
        //        NotificationState state;
        //        do
        //        {
        //            Console.WriteLine("Get notification state ...");
        //            state = router.GetNotificationState(result.ReturnValue).ReturnValue;

        //        } while (state != NotificationState.Sent);
        //        Console.WriteLine("... notification send.");
        //    }
        //}

        private static ChannelFactory<IApnsRouter> CreateApnsClient2()
        {
            var myBinding = new BasicHttpBinding();
            var myEndpoint = new EndpointAddress("http://localhost/APNsRouter.wcf");
            var myChannelFactory = new ChannelFactory<IApnsRouter>(myBinding, myEndpoint);
            return myChannelFactory;
        }

        private static WebChannelFactory<IApnsRouter> CreateApnsClient()
        {
            var baseAddress = new Uri("http://localhost/APNsRouter");
            //var baseAddress = new Uri("http://82.223.9.88/APNsRouter");

            var binding = new WebHttpBinding(WebHttpSecurityMode.TransportCredentialOnly);
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

            WebChannelFactory<IApnsRouter> tmp = null;
            WebChannelFactory<IApnsRouter> client = null;

            try
            {
                tmp = new WebChannelFactory<IApnsRouter>(binding, baseAddress);
                tmp.Credentials.UserName.UserName = "demo";
                tmp.Credentials.UserName.Password = "P8ssw0rd";
                client = tmp;
                tmp = null;
            }
            finally
            {
                if (tmp != null) (tmp as IDisposable)?.Dispose();
            }
            return client;
        }

        //private static WebChannelFactory<IWnsRouter> CreateWnsClient()
        //{
        //    //var baseAddress = new Uri("http://localhost/APNsRouter");
        //    var baseAddress = new Uri("http://82.223.9.88/WNSRouter");

        //    var binding = new WebHttpBinding(WebHttpSecurityMode.TransportCredentialOnly);
        //    binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

        //    WebChannelFactory<IWnsRouter> tmp = null;
        //    WebChannelFactory<IWnsRouter> client = null;

        //    try
        //    {
        //        tmp = new WebChannelFactory<IWnsRouter>(binding, baseAddress);
        //        tmp.Credentials.UserName.UserName = "demo";
        //        tmp.Credentials.UserName.Password = "P8ssw0rd";
        //        client = tmp;
        //        tmp = null;
        //    }
        //    finally
        //    {
        //        if (tmp != null) (tmp as IDisposable)?.Dispose();
        //    }
        //    return client;
        //}

        #endregion methods
    }

    [XmlRoot("toast")]
    public class WnsToastNotification
    {
        public WnsToastNotification()
        {
            Visual = new WnsToastNotificationVisual();
        }

        [XmlElement("visual")]
        public WnsToastNotificationVisual Visual { get; set; }
    }

    public class WnsToastNotificationVisual
    {
        public WnsToastNotificationVisual()
        {
            Binding = new WnsToastNotificationBinding();
        }

        [XmlElement("binding")]
        public WnsToastNotificationBinding Binding { get; set; }
    }

    public class WnsToastNotificationBinding
    {
        public WnsToastNotificationBinding()
        {
            Template = "ToastText01";
            Text = new WnsToastNotificationText()
            {
                Id = "1"
            };
        }

        [XmlAttribute("template")]
        public string Template { get; set; }

        [XmlElement("text")]
        public WnsToastNotificationText Text { get; set; }
    }

    public class WnsToastNotificationText
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlText]
        public string Text { get; set; }
    }
}

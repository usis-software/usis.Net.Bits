using System;
using System.Globalization;
using System.Net;
using System.Threading;

namespace Playground
{
    public static class HttpServer
    {
        public static void Main()
        {
            using (var server = new MyHttpServer())
            {
                server.Start("http://*/download/");
                Console.Write("Press any key to stop the HTTP server ... ");
                Console.ReadKey(true);
                server.Stop();
                Thread.Sleep(1000);
            }
        }
    }

    internal class MyHttpServer : usis.Platform.Windows.HttpServer
    {
        protected override void Process(HttpListenerContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            System.Diagnostics.Debug.WriteLine(context.Request.RawUrl);
            System.Diagnostics.Debug.WriteLine(context.Request.Url);

            //HttpListenerRequest request = context.Request;
            // Obtain a response object.
            HttpListenerResponse response = context.Response;
            // Construct a response.
            string responseString = string.Format(CultureInfo.CurrentCulture, "<HTML><BODY>{0}: Hello world!</BODY></HTML>", DateTime.Now);
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            // Get a response stream and write the response to it.
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            // You must close the output stream.
            output.Close();

            //base.Process(context);
        }
    }
}

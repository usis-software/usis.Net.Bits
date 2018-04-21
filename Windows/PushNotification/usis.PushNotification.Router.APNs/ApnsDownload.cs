using System;
using System.ComponentModel;
using System.Globalization;
using System.Net;

namespace usis.PushNotification
{
    internal sealed class ApnsDownloadSnapIn : Framework.Portable.SnapIn, IDisposable
    {
        private ApnsDownloadServer server = new ApnsDownloadServer();

        public void Dispose()
        {
            server.Dispose();
        }

        protected override void OnConnecting(CancelEventArgs e)
        {
            server.Start("http://*/APNsDownload/");

            base.OnConnecting(e);
        }

        protected override void OnDisconnected(EventArgs e)
        {
            base.OnDisconnected(e);

            server.Stop();
        }
    }

    internal sealed class ApnsDownloadServer : Platform.Windows.HttpServer
    {
        protected override void Process(HttpListenerContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            System.Diagnostics.Debug.WriteLine(context.Request.RawUrl);
            System.Diagnostics.Debug.WriteLine(context.Request.Url);

            // Obtain a response object.
            HttpListenerResponse response = context.Response;
            // Construct a response.
            string responseString = string.Format(CultureInfo.CurrentCulture, "<HTML><BODY>{0}: Hello Downloader!</BODY></HTML>", DateTime.Now);
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            // Get a response stream and write the response to it.
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            // You must close the output stream.
            output.Close();
        }
    }
}

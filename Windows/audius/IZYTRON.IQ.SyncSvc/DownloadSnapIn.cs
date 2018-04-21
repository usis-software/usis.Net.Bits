//
//  @(#) DownloadSnapIn.cs
//
//  Project:    IZYTRON.IQ.SyncSvc
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 audius GmbH. All rights reserved.

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace IZYTRON.IQ
{
    //  --------------------
    //  DownloadSnapIn class
    //  --------------------

    internal class DownloadSnapIn : usis.Framework.Windows.HttpServerServiceSnapIn
    {
        public DownloadSnapIn() { Prefix = "http://*/download/"; }

        protected override void ProcessRequest(HttpListenerContext context)
        {
            Console.WriteLine(context.Request.HttpMethod);
            foreach (var key in context.Request.Headers.AllKeys)
            {
                Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0}={1}", key, context.Request.Headers.Get(key)));
            }
            Console.WriteLine();

            //var fileName = @"Z:\usis\Movies\video.mp4";
            //var size = new FileInfo(fileName).Length;
            ProcessRangeRequest(context, @"Z:\usis\Movies\video.mp4");
        }

        private static void ProcessRangeRequest(HttpListenerContext context, Stream source)
        {
            if (HttpMethod.Head.Method.Equals(context.Request.HttpMethod, StringComparison.Ordinal))
            {
                context.Response.Headers.Add(HttpResponseHeader.AcceptRanges, "bytes");
                context.Response.ContentLength64 = source.SeekLength();
                context.Response.StatusCode = 200; // OK
            }
            else if (HttpMethod.Get.Method.Equals(context.Request.HttpMethod, StringComparison.Ordinal))
            {
                var rangeHeader = context.Request.Headers.Get("Range");
                if (!string.IsNullOrWhiteSpace(rangeHeader))
                {
                    var keyValues = rangeHeader.Split('=');
                    if (keyValues.Length == 2 && "bytes".Equals(keyValues[0], StringComparison.Ordinal))
                    {
                        var range = keyValues[1].Split('-').Select(r => long.Parse(r, CultureInfo.InvariantCulture)).ToArray();
                        if (range.Length == 2)
                        {
                            var low = Convert.ToInt32(range[0]);
                            var high = Convert.ToInt32(range[1]);

                            source.Seek(low, SeekOrigin.Begin);
                            var buffer = new byte[high - low + 1];
                            source.Read(buffer, 0, buffer.Length);

                            context.Response.Headers.Add(string.Format(CultureInfo.InvariantCulture, "Content-Range: bytes {0}-{1}/{2}", low, high, source.SeekLength()));
                            context.Response.ContentLength64 = buffer.Length;
                            context.Response.StatusCode = 206; // Partial Content
                            context.Response.Close(buffer, true);
                            return;
                        }
                    }
                    context.Response.StatusCode = 416; // Range Not Satisfiable
                }
                else context.Response.StatusCode = 400; // Bad Request
            }
            else context.Response.StatusCode = 405; // Method Not Allowed
            context.Response.Close();
        }

        private static void ProcessRangeRequest(HttpListenerContext context, string path)
        {
            using (var source = File.OpenRead(path))
            {
                ProcessRangeRequest(context, source);
            }
        }
    }
}

// eof "DownloadSnapIn.cs"

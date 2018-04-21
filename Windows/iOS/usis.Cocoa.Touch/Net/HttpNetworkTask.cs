//
//  @(#) HttpNetworkTask.cs
//
//  Project:    usis Cocoa
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using Foundation;
using System;
using UIKit;
using usis.Cocoa.Foundation;
using usis.Framework.Net;

#pragma warning disable 1591

namespace usis.Cocoa.Net
{
    //  ----------------------
    //  HttpNetworkTask method
    //  ----------------------

    public sealed class HttpNetworkTask : IHttpNetworkTask
    {
        #region fields

        private IDisposable task;

        #endregion fields

        #region methods

        #region Dispose method

        //  --------------
        //  Dispose method
        //  --------------

        public void Dispose()
        {
            if (task != null)
            {
                task.Dispose();
                task = null;
            }
        }

        #endregion Dispose method

        //  --------------
        //  Perform method
        //  --------------

        public void Perform(HttpRequest httpRequest, Action<IHttpResponse, Exception> handler)
        {
            if (httpRequest == null) throw new ArgumentNullException(nameof(httpRequest));

            using (var request = CreateUrlRequest(httpRequest))
            {
                if (UIDevice.CurrentDevice.CheckSystemVersion(7, 0))
                {
                    var dataTask = NSUrlSession.SharedSession?.CreateDataTask(request, (data, response, error) =>
                    {
                        Exception exception = error?.CreateException();
                        var httpResponse = response?.CreateHttpResponse(data);
                        if (exception == null && httpResponse != null && httpResponse.StatusCode >= 400)
                        {
                            exception = new HttpException(httpResponse.StatusCode, httpResponse.StatusText);
                        }
                        handler(httpResponse, exception);
                    });
                    dataTask?.Resume();
                    task = dataTask;
                }
                else
                {
                    task = new UrlConnectionNetworkTask();
                    ((UrlConnectionNetworkTask)task).Perform(httpRequest, handler);
                }
            }
        }

        //  -----------------------
        //  CreateUrlRequest method
        //  -----------------------

        internal static NSUrlRequest CreateUrlRequest(HttpRequest httpRequest)
        {
            NSMutableUrlRequest tmp = null;
            NSMutableUrlRequest request = null;
            try
            {
                var url = NSUrl.FromString(httpRequest.Url.ToString());
                tmp = new NSMutableUrlRequest(url);
                foreach (var header in httpRequest.Headers)
                {
                    tmp[header.Key] = header.Value;
                }
                var body = httpRequest.GetBody();
                if (body != null) tmp.Body = NSData.FromArray(body);
                tmp.HttpMethod = httpRequest.Method;

                request = tmp;
                tmp = null;
            }
            finally
            {
                if (tmp != null) tmp.Dispose();
            }
            return request;
        }

        #endregion methods
    }

    #region NSUrlResponseExtension class

    //  ----------------------------
    //  NSUrlResponseExtension class
    //  ----------------------------

    internal static class NSUrlResponseExtension
    {
        //  -------------------------
        //  CreateHttpResponse method
        //  -------------------------

        public static HttpResponse CreateHttpResponse(this NSUrlResponse response, NSData data)
        {
            return new HttpResponse(response, data);
        }
    }

    #endregion NSUrlResponseExtension class
}

// eof "HttpNetworkTask.cs"

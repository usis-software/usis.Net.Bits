//
//  @(#) UrlConnectionNetworkTask.cs
//
//  Project:    usis iOS
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using Foundation;
using System;
using usis.Cocoa.Foundation;
using usis.Framework.Net;

#pragma warning disable 1591

namespace usis.Cocoa.Net
{
    //  -------------------------------
    //  UrlConnectionNetworkTask method
    //  -------------------------------

    internal sealed class UrlConnectionNetworkTask : NSUrlConnectionDataDelegate
    {
        #region fields

        private Action<HttpResponse, Exception> Handler;
        private NSMutableData receivedData;
        private NSUrlResponse receivedResponse;

        #endregion fields

        #region Perform method

        //  --------------
        //  Perform method
        //  --------------

        public void Perform(HttpRequest httpRequest, Action<HttpResponse, Exception> handler)
        {
            Handler = handler;

            using (var request = HttpNetworkTask.CreateUrlRequest(httpRequest))
            {
                using (var connection = new NSUrlConnection(request, this))
                {
                    if (connection != null)
                    {
                        receivedData = new NSMutableData();
                    }
                    else throw new InvalidOperationException();
                }
            }
        }

        #endregion Perform method

        #region overrides

        //  -----------------------
        //  ReceivedResponse method
        //  -----------------------

        public override void ReceivedResponse(NSUrlConnection connection, NSUrlResponse response)
        {
            receivedResponse = response;
            receivedData.Length = 0;
        }

        //  -------------------
        //  ReceivedData method
        //  -------------------

        public override void ReceivedData(NSUrlConnection connection, NSData data)
        {
            receivedData.AppendData(data);
        }

        //  ----------------------
        //  FinishedLoading method
        //  ----------------------

        public override void FinishedLoading(NSUrlConnection connection)
        {
            Handler(new HttpResponse(receivedResponse, receivedData), null);
        }

        //  ----------------------
        //  FailedWithError method
        //  ----------------------

        public override void FailedWithError(NSUrlConnection connection, NSError error)
        {
            Handler(null, error.CreateException());
        }

        //  --------------
        //  Dispose method
        //  --------------

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (receivedData != null)
                {
                    receivedData.Dispose();
                    receivedData = null;
                }
                if (receivedResponse != null)
                {
                    receivedResponse.Dispose();
                    receivedResponse = null;
                }
            }
            base.Dispose(disposing);
        }

        #endregion overrides
    }
}

// eof "UrlConnectionNetworkTask.cs"

//
//  @(#) HttpResponse.cs
//
//  Project:    usis Cocoa
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using Foundation;
using System;
using System.Text;
using usis.Framework.Net;

#pragma warning disable 1591

namespace usis.Cocoa.Net
{
    //  ------------------
    //  HttpResponse class
    //  ------------------

    public sealed class HttpResponse : IHttpResponse
    {
        #region properties

        //  -------------
        //  Data property
        //  -------------

        private byte[] Data { get; set; }

        //  ---------------------
        //  TextEncoding property
        //  ---------------------

        private string TextEncoding { get; set; }

        //  -------------------
        //  StatusCode property
        //  -------------------

        public int StatusCode { get; private set; }

        //  -------------------
        //  StatusText property
        //  -------------------

        public string StatusText { get; private set; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        public HttpResponse(NSUrlResponse response, NSData data)
        {
            if (response == null) throw new ArgumentNullException(nameof(response));
            if (data == null) throw new ArgumentNullException(nameof(data));

            if (response is NSHttpUrlResponse httpResponse)
            {
                StatusCode = (int)httpResponse.StatusCode;
                StatusText = NSHttpUrlResponse.LocalizedStringForStatusCode(httpResponse.StatusCode);
            }
            TextEncoding = response.TextEncodingName;
            Data = data.Length == 0 ? new byte[] { } : data.ToArray();
        }

        #endregion construction

        #region methods

        //  -------------------
        //  BodyToString method
        //  -------------------

        public string BodyToString()
        {
            var encoding = string.Equals(TextEncoding, "utf-8", StringComparison.OrdinalIgnoreCase) ? Encoding.UTF8 : Encoding.GetEncoding(28591);
            return encoding.GetString(Data);
        }

        #endregion methods
    }
}

// eof "HttpResponse.cs"

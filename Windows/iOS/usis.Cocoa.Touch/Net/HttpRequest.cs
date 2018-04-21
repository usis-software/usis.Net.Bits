//
//  @(#) HttpRequest.cs
//
//  Project:    usis Cocoa
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Text;

namespace usis.Framework.Portable.Net
{
    //  -----------------
    //  HttpRequest class
    //  -----------------

    public class HttpRequest
    {
        #region fields

        private Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private byte[] data;

        #endregion fields

        #region constants

        public const string Post = "POST";

        private const string ContentTypeKey = "Content-Type";

        #endregion constants

        #region properties

        //  ------------
        //  Url property
        //  ------------

        public Uri Url { get; private set; }

        //  ---------------
        //  Method property
        //  ---------------

        public string Method { get; set; }

        //  ----------------
        //  Headers property
        //  ----------------

        public IDictionary<string, string> Headers { get { return headers; } }

        //  --------------------
        //  ContentType property
        //  --------------------

        public string ContentType
        {
            get { return Headers[ContentTypeKey]; }
            set { Headers[ContentTypeKey] = value; }
        }

        #endregion properties

        #region methods

        //  --------------
        //  GetBody method
        //  --------------

        public byte[] GetBody() { return data; }

        //  --------------
        //  SetBody method
        //  --------------

        public void SetBody(string body) { SetBody(body, Encoding.UTF8); }

        public void SetBody(string body, Encoding encoding)
        {
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));
            data = encoding.GetBytes(body);
        }

        #endregion methods

        #region construction

        //  ------------
        //  construction
        //  ------------

        private HttpRequest() { }

        public HttpRequest(Uri url)
        {
            if (url == null) throw new ArgumentNullException(nameof(url));
            Url = url;
        }

        #endregion construction
    }
}

// eof "HttpRequest.cs"

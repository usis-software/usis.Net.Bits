//
//  @(#) HttpRequest.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Text;

namespace usis.Framework.Net
{
    //  -----------------
    //  HttpRequest class
    //  -----------------

    /// <summary>
    /// Represents a HTTP request with information to send to an server.
    /// </summary>

    public class HttpRequest
    {
        #region fields

        private Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private byte[] data;

        #endregion fields

        #region constants

        /// <summary>
        /// The HTTP POST method.
        /// </summary>

        public const string Post = "POST";

        private const string ContentTypeKey = "Content-Type";

        #endregion constants

        #region properties

        //  ------------
        //  Url property
        //  ------------

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>

        public Uri Url { get; private set; }

        //  ---------------
        //  Method property
        //  ---------------

        /// <summary>
        /// Gets or sets the HTTP method.
        /// </summary>
        /// <value>
        /// The HTTP method.
        /// </value>

        public string Method { get; set; }

        //  ----------------
        //  Headers property
        //  ----------------

        /// <summary>
        /// Gets the dictionary of the HTTP request headers associated with the <b>HttpRequest</b>.
        /// </summary>
        /// <value>
        /// The headers.
        /// </value>

        public IDictionary<string, string> Headers => headers;

        //  --------------------
        //  ContentType property
        //  --------------------

        /// <summary>
        /// Gets or sets the <c>Content-Type</c> header of the HTTP request.
        /// </summary>
        /// <value>
        /// The <c>Content-Type</c> header of the HTTP request.
        /// </value>

        public string ContentType
        {
            get => Headers[ContentTypeKey];
            set => Headers[ContentTypeKey] = value;
        }

        #endregion properties

        #region methods

        //  --------------
        //  GetBody method
        //  --------------

        /// <summary>
        /// Gets the body of the HTTP request.
        /// </summary>
        /// <returns>
        /// A byte array that represents the body of the HTTP request.
        /// </returns>

        public byte[] GetBody() { return data; }

        //  --------------
        //  SetBody method
        //  --------------

        /// <summary>
        /// Sets the body of the HTTP request to the specified UTF-8 encoded string.
        /// </summary>
        /// <param name="body">The body.</param>

        public void SetBody(string body) { SetBody(body, Encoding.UTF8); }

        /// <summary>
        /// Sets the body of the HTTP request to the specified string.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <param name="encoding">The encoding of the body string.</param>
        /// <exception cref="ArgumentNullException"></exception>

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

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequest"/> class
        /// with the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <exception cref="ArgumentNullException"></exception>

        public HttpRequest(Uri url)
        {
            Url = url ?? throw new ArgumentNullException(nameof(url));
        }

        #endregion construction
    }
}

// eof "HttpRequest.cs"

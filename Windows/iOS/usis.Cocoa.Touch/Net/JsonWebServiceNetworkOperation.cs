//
//  @(#) JsonWebServiceNetworkOperation.cs
//
//  Project:    usis Cocoa
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using usis.Framework.Net;

#pragma warning disable 1591

namespace usis.Cocoa.Net
{
    #region JsonWebServiceNetworkOperation method

    //  -------------------------------------
    //  JsonWebServiceNetworkOperation method
    //  -------------------------------------

    public class JsonWebServiceNetworkOperation : HttpNetworkOperation
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        public JsonWebServiceNetworkOperation(Uri url, string method, object args, Action<string, Exception> handler) :
            this(CreateJsonRequest(url, method, args), handler)
        { }

        protected JsonWebServiceNetworkOperation() : base(null) { }

        protected JsonWebServiceNetworkOperation(Action<string, Exception> handler) : this(null, handler) { }

        private JsonWebServiceNetworkOperation(HttpRequest request, Action<string, Exception> handler) :
            base(request, (response, exception) =>
            {
                string body = response?.BodyToString();
                handler?.Invoke(body, exception);
            })
        { }

        #endregion construction

        #region CreateJsonRequest method

        //  ------------------------
        //  CreateJsonRequest method
        //  ------------------------

        protected static HttpRequest CreateJsonRequest(Uri url, string method, object value)
        {
            var request = new HttpRequest(url)
            {
                Method = method,
            };
            request.ContentType = "application/json";
            if (value != null)
            {
                string body = JsonConvert.SerializeObject(value, Formatting.None);
                request.SetBody(body);
            }
            return request;
        }

        #endregion CreateJsonRequest method
    }

    #endregion JsonWebServiceNetworkOperation method

    #region JsonWebServiceNetworkOperation<TResult> class

    //  ---------------------------------------------
    //  JsonWebServiceNetworkOperation<TResult> class
    //  ---------------------------------------------

    public class JsonWebServiceNetworkOperation<TResult> : JsonWebServiceNetworkOperation
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        public JsonWebServiceNetworkOperation(Action<TResult, Exception> handler) : base((body, exception) =>
        {
            if (!string.IsNullOrWhiteSpace(body))
            {
                var settings = new JsonSerializerSettings()
                {
                    Error = delegate (object sender, ErrorEventArgs args)
                    {
                        args.ErrorContext.Handled = true;
                    }
                };
                handler?.Invoke(JsonConvert.DeserializeObject<TResult>(body, settings), exception);
            }
            else handler?.Invoke(default(TResult), exception);
        })
        { }

        public JsonWebServiceNetworkOperation(Uri url, string method, object args, Action<TResult, Exception> handler) : this(handler)
        {
            Request = CreateJsonRequest(url, method, args);
        }

        #endregion construction
    }

    #endregion JsonWebServiceNetworkOperation<TResult> class
}

// eof "JsonWebServiceNetworkOperation.cs"

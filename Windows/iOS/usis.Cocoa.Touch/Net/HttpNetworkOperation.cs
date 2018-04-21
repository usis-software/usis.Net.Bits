//
//  @(#) HttpNetworkOperation.cs
//
//  Project:    usis Cocoa
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using System;
using usis.Framework;
using usis.Framework.Net;

#pragma warning disable 1591

namespace usis.Cocoa.Net
{
    //  ---------------------------
    //  HttpNetworkOperation method
    //  ---------------------------

    public class HttpNetworkOperation : NetworkOperation, INetworkOperation
    {
        #region fields

        private IHttpNetworkTask task;

        #endregion fields

        #region properties

        //  -------------
        //  Task property
        //  -------------

        protected IHttpNetworkTask Task
        {
            get => task;
            set
            {
                if (task != value)
                {
                    task?.Dispose();
                    task = value;
                }
            }
        }

        //  ----------------
        //  Request property
        //  ----------------

        public HttpRequest Request { get; protected set; }

        //  ----------------
        //  Handler property
        //  ----------------

        private Action<IHttpResponse, Exception> Handler { get; set; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        public HttpNetworkOperation(HttpRequest request, Action<IHttpResponse, Exception> handler)
        {
            Request = request;
            Handler = handler;
        }

        protected HttpNetworkOperation(Action<IHttpResponse, Exception> handler) { Handler = handler; }

        #endregion construction

        #region overrides

        //  --------------
        //  Perform method
        //  --------------

        public override void Perform(IApplication application, usis.Framework.Net.NetworkOperationPool pool)
        {
            if (Request == null) CreateRequest(application);
            if (Request == null) throw new InvalidOperationException();
            Task = application.With<HttpNetworkTaskApplicationExtension>(true).CreateNetworkTask();
            Task?.Perform(Request, (response, exception) =>
            {
                Handler?.Invoke(response, exception);
                Finish(pool, exception);
            });
        }

        //  --------------
        //  Dispose method
        //  --------------

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Task?.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion overrides

        #region methods

        //  --------------------
        //  CreateRequest method
        //  --------------------

        protected virtual void CreateRequest(IApplication application) { }

        #endregion methods
    }
}

// eof "HttpNetworkOperation.cs"

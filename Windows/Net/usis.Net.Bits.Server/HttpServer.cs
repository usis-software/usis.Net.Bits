//
//  @(#) HttpServer.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net;
using System.Threading;

namespace usis.Net.Bits
{
    //  ----------------
    //  HttpServer class
    //  ----------------

    /// <summary>
    /// Provides a base class to implement a HTTP server.
    /// </summary>
    /// <seealso cref="IDisposable" />

    public class HttpServer : IDisposable
    {
        #region fields

        private HttpListener httpListener = new HttpListener();
        private Thread runThread;
        private EventWaitHandle stoppingEvent = new EventWaitHandle(false, EventResetMode.ManualReset);
        private EventWaitHandle stoppedEvent = new EventWaitHandle(false, EventResetMode.ManualReset);

        #endregion fields

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpServer"/> class.
        /// </summary>

        public HttpServer() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpServer"/> class with the specified URI prefix.
        /// </summary>
        /// <param name="prefix">
        /// An Uniform Resource Identifier (URI) prefix that is handled by this server.
        /// </param>

        public HttpServer(string prefix) { AddPrefix(prefix); }

        #endregion construction

        #region IDisposable implementation

        //  --------------
        //  Dispose method
        //  --------------

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.
        /// </param>

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (httpListener != null)
                {
                    httpListener.Close();
                    httpListener = null;
                }
                if (stoppingEvent != null)
                {
                    stoppingEvent.Close();
                    stoppingEvent = null;
                }
                if (stoppedEvent != null)
                {
                    stoppedEvent.Close();
                    stoppedEvent = null;
                }
            }
        }

        //  ---------
        //  Finalizer
        //  ---------

        /// <summary>
        /// Finalizes an instance of the <see cref="HttpServer"/> class.
        /// </summary>

        ~HttpServer() { Dispose(false); }

        #endregion IDisposable implementation

        #region properties

        //  ----------------
        //  Handler property
        //  ----------------

        private Action<HttpListenerContext> Handler { get; set; }

        #endregion properties

        #region public methods

        //  ----------------
        //  AddPrefix method
        //  ----------------

        /// <summary>
        /// Adds an URI prefix to the list of prefixes handled by this server.
        /// </summary>
        /// <param name="prefix">
        /// An Uniform Resource Identifier (URI) prefix that is handled by this server.
        /// </param>

        public void AddPrefix(string prefix)
        {
            httpListener.Prefixes.Add(prefix);
        }

        //  ------------------
        //  AddPrefixes method
        //  ------------------

        /// <summary>
        /// Adds the provided URI prefixes to the list of prefixes handled by this server.
        /// </summary>
        /// <param name="prefixes">The prefixes.</param>

        public void AddPrefixes(params string[] prefixes)
        {
            if (prefixes == null) throw new ArgumentNullException(nameof(prefixes));

            foreach (var prefix in prefixes)
            {
                AddPrefix(prefix);
            }
        }

        //  ------------
        //  Start method
        //  ------------

        /// <summary>
        /// Starts the HTTP server by listening for requests from clients.
        /// </summary>

        public void Start()
        {
            Start(null);
        }

        /// <summary>
        /// Starts the HTTP server by listening for requests from clients
        /// by passing the request to the specified handler.
        /// </summary>
        /// <param name="handler">The action that handles an client request.</param>

        public void Start(Action<HttpListenerContext> handler)
        {
            Handler = handler;
            httpListener.Start();

            runThread = new Thread(Run);
            runThread.Start();
        }

        //  -----------
        //  Stop method
        //  -----------

        /// <summary>
        /// Stops the HTTP server.
        /// </summary>

        public void Stop()
        {
            if (stoppingEvent.Set())
            {
                while (runThread.IsAlive)
                {
                    Thread.Sleep(100);
                }
            }
        }

        #endregion public methods

        #region virtual methods

        //  --------------
        //  Process method
        //  --------------

        /// <summary>
        /// Processes as request that is specified by the provided context.
        /// </summary>
        /// <param name="context">The context of the HTTP request.</param>
        /// <exception cref="ArgumentNullException"><paramref name="context" /> is a <c>null</c> reference.</exception>
        /// <remarks>
        /// The default implementation invokes an action,
        /// if one was passed as an argument to the <see cref="Start(Action{HttpListenerContext})" /> method.
        /// </remarks>

        protected virtual void Process(HttpListenerContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            Handler?.Invoke(context);
        }

        //  ----------------------
        //  HandleException method
        //  ----------------------

        /// <summary>
        /// Called to handle an exception that occurred during processing a request.
        /// </summary>
        /// <param name="exception">The exception that occurred.</param>
        /// <param name="context">The context of the HTTP request.</param>
        /// <returns>
        /// <c>true</c> when the exception was handled, otherwise <c>false</c>
        /// to set the HTTP status code to <b>500</b> (Internal Server Error).
        /// </returns>

        protected virtual bool HandleException(Exception exception, HttpListenerContext context)
        {
            Trace.WriteLine(exception);

            return false; // not handled: set status code 500 - Internal Server Error
        }

        #endregion virtual methods

        #region private methods

        //  ----------
        //  Run method
        //  ----------

        private void Run()
        {
            int index;
            do
            {
                var result = httpListener.BeginGetContext(new AsyncCallback(ListenerCallback), httpListener);
                index = WaitHandle.WaitAny(new WaitHandle[] { result.AsyncWaitHandle, stoppingEvent });
            }
            while (index == 0);
            httpListener.Stop();
            while (!stoppedEvent.WaitOne(500))
            {
                foreach (var item in httpListener.DefaultServiceNames)
                {
                    Trace.WriteLine(string.Format(CultureInfo.InvariantCulture, "stopping '{0}'...", item));
                }
            }
        }

        //  ------------------------
        //  ListenerCallback metthod
        //  ------------------------

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void ListenerCallback(IAsyncResult result)
        {
            var listener = result.AsyncState as HttpListener;
            if (listener.IsListening)
            {
                try
                {
                    var context = listener.EndGetContext(result);
                    try
                    {
                        Process(context);
                    }
                    catch (Exception exception)
                    {
                        if (!HandleException(exception, context)) context.Response.StatusCode = 500;
                        context.Response.Close();
                    }
                }
                catch (HttpListenerException exception)
                {
                    Debug.Print("ErrorCode=0x{0:x}", exception.ErrorCode);
                    if (exception.ErrorCode == 0x3e3) // 0x80004005
                    {
                        stoppedEvent.Set();
                        return;
                    }
                    else throw;
                }
            }
            else stoppedEvent.Set();
        }

        #endregion private methods
    }
}

// eof "HttpServer.cs"

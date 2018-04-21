//
//  @(#) HttpServerServiceSnapIn.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

using System;
using System.ComponentModel;
using usis.Platform.Windows;

namespace usis.Framework.Windows
{
    //  -----------------------------
    //  HttpServerServiceSnapIn class
    //  -----------------------------

    /// <summary>
    /// Provides a base class for snap-ins that implement a HTTP server with the <see cref="HttpServer"/> class.
    /// </summary>
    /// <typeparam name="TServer">The type of the HTTP server.</typeparam>
    /// <seealso cref="ServiceSnapIn" />
    /// <seealso cref="IDisposable" />

    public abstract class HttpServerServiceSnapIn<TServer> : ServiceSnapIn, IDisposable where TServer : HttpServer
    {
        #region fields

        private TServer server;

        #endregion fields

        #region properties

        //  ---------------
        //  Prefix property
        //  ---------------

        /// <summary>
        /// Gets or sets the URI prefix that is handled by this server.
        /// </summary>
        /// <value>
        /// The URI prefix that is handled by this server.
        /// </value>

        protected string Prefix { get; set; }

        //  ---------------
        //  Server property
        //  ---------------

        /// <summary>
        /// Gets the HTTP server.
        /// </summary>
        /// <value>
        /// The HTTP server.
        /// </value>

        public TServer Server => server;

        #endregion properties

        #region overrides

        //  -------------------
        //  OnConnecting method
        //  -------------------

        /// <summary>
        /// Raises the <see cref="SnapIn.Connecting" /> event.
        /// </summary>
        /// <param name="e">A <see cref="CancelEventArgs" /> object that contains the event data.</param>

        protected override void OnConnecting(CancelEventArgs e)
        {
            server = CreateServer();
            if (!string.IsNullOrWhiteSpace(Prefix)) server.AddPrefix(Prefix);

            base.OnConnecting(e);
        }

        //  ------------------
        //  OnConnected method
        //  ------------------

        /// <summary>
        /// Raises the <see cref="SnapIn.Connected" /> event.
        /// </summary>
        /// <param name="e">A <see cref="EventArgs" /> object that contains the event data.</param>

        protected override void OnConnected(EventArgs e)
        {
            server.Start();

            base.OnConnected(e);
        }

        //  ----------------------
        //  OnDisconnecting method
        //  ----------------------

        /// <summary>
        /// Raises the <see cref="SnapIn.Disconnecting" /> event.
        /// </summary>
        /// <param name="e">A <see cref="CancelEventArgs" /> object that contains the event data.</param>

        protected override void OnDisconnecting(CancelEventArgs e)
        {
            server.Stop();

            base.OnDisconnecting(e);
        }

        #endregion overrides

        #region methods

        //  -------------------
        //  CreateServer method
        //  -------------------

        /// <summary>
        /// Creates a <see cref="HttpServer"/> object to be used by this snap-in.
        /// </summary>
        /// <returns>
        /// A newly created server object that derives from <see cref="HttpServer"/>.
        /// </returns>

        protected abstract TServer CreateServer();

        #endregion methods

        #region IDisposable implementation

        //  --------------
        //  Dispose method
        //  --------------

        private bool disposed = false; // to detect redundant calls

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources;
        /// <c>false</c> to release only unmanaged resources.
        /// </param>

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (server != null) server.Dispose();
                }
                disposed = true;
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable implementation
    }
}

// eof "HttpServerServiceSnapIn.cs"

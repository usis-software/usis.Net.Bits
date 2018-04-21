//
//  @(#) ServiceClient.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System;
using System.ServiceModel.Description;

namespace usis.Platform.ServiceModel
{
    //  -----------------------------
    //  ServiceClient<TService> class
    //  -----------------------------

    /// <summary>
    /// Represents a Windows Communication Foundation (WCF) client
    /// that can call operations of a specified service type.
    /// </summary>
    /// <typeparam name="TService">The type of the service.</typeparam>
    /// <seealso cref="IDisposable" />

    public class ServiceClient<TService> : IDisposable where TService : class
    {
        #region fields

        private Client client;

        #endregion fields

        #region properties

        //  ----------------
        //  Service property
        //  ----------------

        /// <summary>
        /// Gets the service through which operations can be called.
        /// </summary>
        /// <value>
        /// The service through which operations can be called.
        /// </value>

        public TService Service => client.GetChannel();

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceClient{TService}"/> class
        /// using the default target endpoint from the application configuration file.
        /// </summary>

        public ServiceClient() { client = new Client(); }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceClient{TService}"/> class
        /// with the specified endpoint.
        /// </summary>
        /// <param name="endpoint">The endpoint for the service.</param>

        public ServiceClient(ServiceEndpoint endpoint) { client = new Client(endpoint); }

        #endregion construction

        #region IDisposable implementation

        private bool disposed = false; // to detect redundant calls

        //  --------------
        //  Dispose method
        //  --------------

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
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
        /// <c>true</c> to release both managed and unmanaged resources;
        /// <c>false</c> to release only unmanaged resources.
        /// </param>

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;
            if (disposing) client.Close();
            disposed = true;
        }

        #endregion IDisposable implementation

        #region Client class

        //  ------------
        //  Client class
        //  ------------

        private class Client : System.ServiceModel.ClientBase<TService>
        {
            //  ------------
            //  construction
            //  ------------

            internal Client() { }

            internal Client(ServiceEndpoint endpoint) : base(endpoint) { }

            //  -----------------
            //  GetChannel method
            //  -----------------

            internal TService GetChannel() { return Channel; }
        }

        #endregion Client class
    }
}

// eof "ServiceClient.cs"

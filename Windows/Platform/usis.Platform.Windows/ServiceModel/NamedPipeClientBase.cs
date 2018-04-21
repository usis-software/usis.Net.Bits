//
//  @(#) NamedPipeClientBase.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using System;
using System.Globalization;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace usis.Platform.ServiceModel
{
    //  -----------------------------------
    //  NamedPipeClientBase<TChannel> class
    //  -----------------------------------

    /// <summary>
    /// Provides the base implementation used to create Windows Communication Foundation (WCF) client objects
    /// that can call services over the named pipe protocol.
    /// </summary>
    /// <typeparam name="TChannel">The channel to be used to connect to the service.</typeparam>
    /// <seealso cref="System.ServiceModel.ClientBase{TChannel}" />

    [Obsolete("Do not use this class. It will be removed in the next version.")]
    public class NamedPipeClientBase<TChannel> : ClientBase<TChannel> where TChannel : class
    {
        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedPipeClientBase{TChannel}"/> class.
        /// </summary>
        /// <param name="serverName">The name of the server.</param>
        /// <param name="servicePath">The service path.</param>

        public NamedPipeClientBase(string serverName, string servicePath) : this(CreateEndpoint(serverName, servicePath)){ }

        private NamedPipeClientBase(ServiceEndpoint endpoint) : base(endpoint) { }

        private static ServiceEndpoint CreateEndpoint(string serverName, string servicePath)
        {
            return new ServiceEndpoint(
                ContractDescription.GetContract(typeof(TChannel)),
                new NetNamedPipeBinding(),
                new EndpointAddress(string.Format(CultureInfo.InvariantCulture, "{0}://{1}/{2}", Uri.UriSchemeNetPipe, serverName, servicePath)));
        }

        //  -------------
        //  Invoke method
        //  -------------

        /// <summary>
        /// Invokes the specified function to call a service method.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="serverName">The Name of the server.</param>
        /// <param name="servicePath">The service path.</param>
        /// <param name="function">The function to be invoked by passing it a reference to a newly created <typeparamref name="TChannel" />.</param>
        /// <returns>
        /// The return value of the service method that was called.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="function"/> is a null reference (<c>Nothing</c> in Visual Basic).</exception>

        protected static TResult Invoke<TResult>(string serverName, string servicePath, Func<TChannel, TResult> function)
        {
            if (function == null) throw new ArgumentNullException(nameof(function));

            using (var client = new NamedPipeClientBase<TChannel>(serverName, servicePath))
            {
                return function.Invoke(client.Service);
            }
        }
    }

    //  -------------------
    //  WcfClientBase class
    //  -------------------

    /// <summary>
    /// This class is deprecated. Do not use it.
    /// </summary>
    /// <typeparam name="TChannel">The type of the channel.</typeparam>
    /// <seealso cref="ServiceModel.ClientBase{TChannel}" />

    [Obsolete("Do not use this class. It will be removed in the next version.")]
    public class WcfClientBase<TChannel> : ClientBase<TChannel> where TChannel : class
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="WcfClientBase{TChannel}"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>

        public WcfClientBase(Uri url) : base(CreateEndpoint(url)) { }

        #endregion construction

        #region methods

        //  ---------------------
        //  CreateEndpoint method
        //  ---------------------

        private static ServiceEndpoint CreateEndpoint(Uri url)
        {
            return new ServiceEndpoint(
                ContractDescription.GetContract(typeof(TChannel)),
                new BasicHttpBinding(),
                new EndpointAddress(url));
        }

        //  -------------
        //  Invoke method
        //  -------------

        /// <summary>
        /// Invokes the specified function to call a service method.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="function">The function.</param>
        /// <returns>
        /// The return value of the service method that was called.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="function"/> is a <c>null</c> reference (<c>Nothing</c> in Visual Basic).
        /// </exception>

        protected static TResult Invoke<TResult>(Uri url, Func<TChannel, TResult> function)
        {
            if (function == null) throw new ArgumentNullException(nameof(function));

            using (var client = new WcfClientBase<TChannel>(url))
            {
                return function.Invoke(client.Service);
            }
        }

        #endregion methods
    }

    //  ----------------
    //  ClientBase class
    //  ----------------

    /// <summary>
    /// This class is deprecated. Do not use it.
    /// </summary>
    /// <typeparam name="TChannel">The type of the channel.</typeparam>
    /// <seealso cref="System.ServiceModel.ClientBase{TChannel}" />

    [Obsolete("Do not use this class. It will be removed in the next version.")]
    public abstract class ClientBase<TChannel> : System.ServiceModel.ClientBase<TChannel> where TChannel : class
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientBase{TChannel}"/> class.
        /// </summary>
        /// <param name="endpoint">
        /// The endpoint for a service that allows clients to find and communicate with the service.
        /// </param>

        protected ClientBase(ServiceEndpoint endpoint) : base(endpoint) { }

        #endregion construction

        #region properties

        //  ----------------
        //  Service property
        //  ----------------

        /// <summary>
        /// Gets the service used to send messages.
        /// </summary>
        /// <value>
        /// The service used to send messages.
        /// </value>

        public TChannel Service => Channel;

        #endregion properties
    }
}

// eof "NamedPipeClientBase.cs"

//
//  @(#) NamedPipeServiceClient.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System;
using System.Globalization;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace usis.Platform.ServiceModel
{
    //  --------------------------------------
    //  NamedPipeServiceClient<TService> class
    //  --------------------------------------

    /// <summary>
    /// Represents a Windows Communication Foundation (WCF) client
    /// that can call operations of a specified service type
    /// through the named pipe protocol.
    /// </summary>
    /// <typeparam name="TService">The type of the service.</typeparam>
    /// <seealso cref="ServiceModel.ServiceClient{TService}" />

    public class NamedPipeServiceClient<TService> : ServiceClient<TService> where TService : class
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedPipeServiceClient{TService}"/> class.
        /// </summary>
        /// <param name="serverName">Name of the server.</param>
        /// <param name="servicePath">The service path.</param>

        public NamedPipeServiceClient(string serverName, string servicePath) : base(CreateEndpoint(serverName, servicePath)) { }

        #endregion construction

        #region methods

        //  ---------------------
        //  CreateEndpoint method
        //  ---------------------

        private static ServiceEndpoint CreateEndpoint(string serverName, string servicePath)
        {
            return new ServiceEndpoint(
                ContractDescription.GetContract(typeof(TService)),
                new NetNamedPipeBinding(),
                new EndpointAddress(string.Format(
                    CultureInfo.InvariantCulture, "{0}://{1}/{2}",
                    Uri.UriSchemeNetPipe, serverName, servicePath)));
        }

        //  -------------
        //  Invoke method
        //  -------------

        /// <summary>
        /// Invokes the specified function to call a service method.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="serverName">The name of the server.</param>
        /// <param name="servicePath">The service path.</param>
        /// <param name="function">
        /// The function to be invoked by passing it a reference to a newly created <typeparamref name="TService" /> object.
        /// </param>
        /// <returns>
        /// The return value of the service method that was called.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="function"/> is a <c>null</c> reference (<c>Nothing</c> in Visual Basic).
        /// </exception>

        protected static TResult Invoke<TResult>(string serverName, string servicePath, Func<TService, TResult> function)
        {
            if (function == null) throw new ArgumentNullException(nameof(function));

            using (var client = new NamedPipeServiceClient<TService>(serverName, servicePath))
            {
                return function.Invoke(client.Service);
            }
        }

        #endregion methods
    }
}

// eof "NamedPipeServiceClient.cs"

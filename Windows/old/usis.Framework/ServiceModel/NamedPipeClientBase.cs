//
//  @(#) NamedPipeClientBase.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using System;
using System.Globalization;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace usis.Framework.ServiceModel
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

    [Obsolete("Use the type from usis.Platform.ServiceModel instead.")]
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

        public NamedPipeClientBase(string serverName, string servicePath) : base(new ServiceEndpoint(
            ContractDescription.GetContract(typeof(TChannel)),
            new NetNamedPipeBinding(),
            new EndpointAddress(string.Format(CultureInfo.InvariantCulture,
                "{0}://{1}/{2}", Uri.UriSchemeNetPipe, serverName, servicePath))))
        { }

        //  ----------------
        //  Service property
        //  ----------------

        /// <summary>
        /// Gets the service used to send messages.
        /// </summary>
        /// <value>
        /// The service used to send messages.
        /// </value>

        public TChannel Service
        {
            get
            {
                return Channel;
            }
        }
    }
}

// eof "NamedPipeClientBase.cs"

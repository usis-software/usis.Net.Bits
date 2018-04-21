//
//  @(#) NamedPipeServiceHostSnapIn.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace usis.Framework.ServiceModel
{
    //  --------------------------------
    //  NamedPipeServiceHostSnapIn class
    //  --------------------------------

    /// <summary>
    /// Provides a generic snap-in that hosts a WFC service with a named pipe endpoint.
    /// </summary>
    /// <typeparam name="TService">The type of the service.</typeparam>
    /// <typeparam name="TContract">The type of the contract.</typeparam>

    [Obsolete("Use type from usis.Framework.Windows assembly instead.")]
    public class NamedPipeServiceHostSnapIn<TService, TContract> :
        ServiceHostBaseSnapIn<NamedPipeServiceHostConfigurator<TService, TContract>>
        where TService : class
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedPipeServiceHostSnapIn{TService, TContract}"/> class.
        /// </summary>

        public NamedPipeServiceHostSnapIn() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedPipeServiceHostSnapIn{TService, TContract}"/> class.
        /// </summary>
        /// <param name="canPauseAndResume">if set to <c>true</c> the snap-in can pause and resume.</param>

        protected NamedPipeServiceHostSnapIn(bool canPauseAndResume) : base(canPauseAndResume) { }

        #endregion construction
    }

    #region NamedPipeServiceHostConfigurator<TService, TContract> class

    //  -----------------------------------------------------------
    //  NamedPipeServiceHostConfigurator<TService, TContract> class
    //  -----------------------------------------------------------

    /// <summary>
    /// Provides a class to configure an <see cref="ServiceHost"/> to use the named pipe protocol.
    /// </summary>
    /// <typeparam name="TService">The type of the service.</typeparam>
    /// <typeparam name="TContract">The type of the service contract.</typeparam>
    /// <seealso cref="ServiceModel.ServiceHostBaseConfigurator{TService}" />

    public class NamedPipeServiceHostConfigurator<TService, TContract> : ServiceHostBaseConfigurator<TService>
    {
        #region overrides

        //  ---------------
        //  Scheme property
        //  ---------------

        /// <summary>
        /// Gets the scheme name through which the service is accessed.
        /// </summary>
        /// <value>
        /// The scheme name through which the service is accessed.
        /// </value>

        protected override string Scheme { get { return Uri.UriSchemeNetPipe; } }

        //  -------------------------
        //  CreateServiceHostInstance
        //  -------------------------

        /// <summary>
        /// Creates an instance of a class that derives from <see cref="ServiceHostBase" />
        /// and hosts the service of this snap-in.
        /// </summary>
        /// <returns>
        /// An <b>ServiceHostBase</b> that hosts the service of this snap-in.
        /// </returns>

        public override ServiceHostBase CreateServiceHostInstance()
        {
            ServiceHost host = null;
            ServiceHost tmp = null;
            try
            {
                var serviceAddress = CreateServiceAddress(typeof(TService).Name);
                tmp = new ServiceHost(typeof(TService), serviceAddress);
                tmp.AddServiceEndpoint(typeof(TContract), CreateChannelBinding(), serviceAddress);
                host = tmp;
                tmp = null;
            }
            finally
            {
                if (tmp != null) ((IDisposable)tmp).Dispose();
            }
            return host;
        }

        //  ---------------------------
        //  CreateChannelBinding method
        //  ---------------------------

        /// <summary>
        /// Creates a binding for the service endpoint.
        /// </summary>
        /// <returns>
        /// A newly created binding for the service endpoint.
        /// </returns>

        public override Binding CreateChannelBinding()
        {
            return new NetNamedPipeBinding();
        }

        #endregion overrides
    }

    #endregion NamedPipeServiceHostConfigurator<TService, TContract> class
}

// eof "NamedPipeServiceHostSnapIn.cs"

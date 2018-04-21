//
//  @(#) NamedPipeServiceHostSnapIn.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

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

    public class NamedPipeServiceHostSnapIn<TService, TContract> : ServiceHostSnapIn<NamedPipeServiceHostFactory<TService, TContract>> where TService : class
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

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedPipeServiceHostSnapIn{TService, TContract}"/> class.
        /// </summary>
        /// <param name="canPauseAndResume">if set to <c>true</c> the snap-in can pause and resume.</param>
        /// <param name="openOnConnect">if set to <c>true</c> the service host is opened on connection.</param>

        protected NamedPipeServiceHostSnapIn(bool canPauseAndResume, bool openOnConnect) : base(canPauseAndResume, openOnConnect) { }

        #endregion construction
    }

    #region NamedPipeServiceHostFactory<TService, TContract> class

    //  ------------------------------------------------------
    //  NamedPipeServiceHostFactory<TService, TContract> class
    //  ------------------------------------------------------

    /// <summary>
    /// Provides a factory to create and configure a <see cref="ServiceHost"/> that uses the named pipe protocol.
    /// </summary>
    /// <typeparam name="TService">The type of the service.</typeparam>
    /// <typeparam name="TContract">The type of the service contract.</typeparam>
    /// <seealso cref="ServiceModel.ServiceHostFactory{TService}" />

    public class NamedPipeServiceHostFactory<TService, TContract> : ServiceHostFactory<TService>
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
        /// <remarks>
        /// This property returns <see cref="Uri.UriSchemeNetPipe"/>.
        /// </remarks>

        protected override string Scheme => Uri.UriSchemeNetPipe;

        //  ------------------------
        //  CreateServiceHost method
        //  ------------------------

        /// <summary>
        /// Creates an instance of a class that derives from <see cref="ServiceHostBase" />
        /// and hosts the.
        /// </summary>
        /// <returns>
        /// A <see cref="ServiceHostBase" /> object that hosts the service.
        /// </returns>

        public override ServiceHostBase CreateServiceHost()
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

    #endregion NamedPipeServiceHostFactory<TService, TContract> class
}

// eof "NamedPipeServiceHostSnapIn.cs"

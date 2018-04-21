//
//  @(#) WebServiceHostSnapIn.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;

namespace usis.Framework.ServiceModel.Web
{
    #region WebServiceHostSnapIn<TService> class

    //  ------------------------------------
    //  WebServiceHostSnapIn<TService> class
    //  ------------------------------------

    /// <summary>
    /// Provides a generic snap-in that hosts a web service.
    /// </summary>
    /// <typeparam name="TService">The type of the web service.</typeparam>
    /// <seealso cref="ServiceHostSnapIn" />
    /// <remarks>
    /// This snap-in allows you to configure the web service (endpoint, binding, ...)
    /// in your application configuration file.
    /// </remarks>

    public class WebServiceHostSnapIn<TService> : ServiceHostSnapIn
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="WebServiceHostSnapIn{TService}"/> class.
        /// </summary>

        public WebServiceHostSnapIn() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebServiceHostSnapIn{TService}"/> class.
        /// </summary>
        /// <param name="canPauseAndResume">if set to <c>true</c> the snap-in can pause and resume.</param>
        /// <param name="openOnConnect">if set to <c>true</c> the service host is opened on connection.</param>

        public WebServiceHostSnapIn(bool canPauseAndResume, bool openOnConnect) : base(canPauseAndResume, openOnConnect) { }

        #endregion construction

        //  ------------------------
        //  CreateServiceHost method
        //  ------------------------

        /// <summary>
        /// Creates an instance of a <see cref="ServiceHostBase" /> object.
        /// </summary>
        /// <returns>
        /// An newly created <see cref="ServiceHostBase" /> instance.
        /// </returns>

        protected override ServiceHostBase CreateServiceHost()
        {
            return new WebServiceHost(typeof(TService));
        }
    }

    #endregion WebServiceHostSnapIn<TService> class

    #region WebServiceHostSnapIn<TService, TContract> class

    //  -----------------------------------------------
    //  WebServiceHostSnapIn<TService, TContract> class
    //  -----------------------------------------------

    /// <summary>
    /// Provides a generic snap-in that hosts a web service.
    /// </summary>
    /// <typeparam name="TService">
    /// The type of the service to host as a web service.
    /// </typeparam>
    /// <typeparam name="TContract">The type of the contract.</typeparam>

    public class WebServiceHostSnapIn<TService, TContract> :
        ServiceHostSnapIn<WebServiceHostConfigurator<TService, TContract>>
        where TService : class
    { }

    #endregion WebServiceHostSnapIn<TService, TContract> class

    #region WebServiceHostConfigurator<TService, TContract> class

    //  -----------------------------------------------------
    //  WebServiceHostConfigurator<TService, TContract> class
    //  -----------------------------------------------------

    /// <summary>
    /// Provides a class to configure a <see cref="WebServiceHost" />.
    /// </summary>
    /// <typeparam name="TService">The type of the service.</typeparam>
    /// <typeparam name="TContract">The type of the contract.</typeparam>
    /// <seealso cref="ServiceModel.ServiceHostFactory{TService}" />

    public class WebServiceHostConfigurator<TService, TContract> : ServiceHostFactory<TService>
    {
        //  ---------------
        //  Scheme property
        //  ---------------

        /// <summary>
        /// Gets the scheme name through which the service is accessed.
        /// </summary>
        /// <value>
        /// The scheme name through which the service is accessed.
        /// </value>

        protected override string Scheme => Uri.UriSchemeHttp;

        //  --------------------------------
        //  CreateServiceHostInstance method
        //  --------------------------------

        /// <summary>
        /// Creates an instance of a class that derives from <see cref="ServiceHostBase" />
        /// and hosts the service of this snap-in.
        /// </summary>
        /// <returns>
        /// An <b>ServiceHostBase</b> that hosts the service of this snap-in.
        /// </returns>

        public override ServiceHostBase CreateServiceHost()
        {
            ServiceHost host = null;
            ServiceHost tmp = null;
            try
            {
                var serviceAddress = CreateServiceAddress(typeof(TService).Name);
                tmp = new WebServiceHost(typeof(TService), serviceAddress);
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

        public override Binding CreateChannelBinding() { return new WebHttpBinding(); }
    }

    #endregion WebServiceHostConfigurator<TService, TContract> class

    #region SecureWebServiceHostConfigurator<TService, TContract> class

    //  -----------------------------------------------------------
    //  SecureWebServiceHostConfigurator<TService, TContract> class
    //  -----------------------------------------------------------

    /// <summary>
    /// Provides a class to configure a <see cref="WebServiceHost" />
    /// with security enabled bindings.
    /// </summary>
    /// <typeparam name="TService">The type of the service.</typeparam>
    /// <typeparam name="TContract">The type of the contract.</typeparam>
    /// <seealso cref="WebServiceHostConfigurator{TService, TContract}" />

    public class SecureWebServiceHostConfigurator<TService, TContract> : WebServiceHostConfigurator<TService, TContract>
    {
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
            var binding = new WebHttpBinding();
            binding.Security.Mode = WebHttpSecurityMode.TransportCredentialOnly;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
            return binding;
        }
    }

    #endregion SecureWebServiceHostConfigurator<TService, TContract> class
}

// eof "WebServiceHostSnapIn.cs"

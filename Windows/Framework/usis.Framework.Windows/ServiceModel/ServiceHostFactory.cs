//
//  @(#) ServiceHostFactory.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

using System;
using System.Globalization;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace usis.Framework.ServiceModel
{
    #region ServiceHostFactory<TService> class

    //  -------------------------------------------
    //  ServiceHostFactory<TService> class
    //  -------------------------------------------

    /// <summary>
    /// Provides a generic base class to configure service hosts for a specified service type.
    /// </summary>
    /// <typeparam name="TService">The type of the service.</typeparam>

    public abstract class ServiceHostFactory<TService> : ServiceHostFactory
    {
        //  ------------------------
        //  ServiceFullName property
        //  ------------------------

        /// <summary>
        /// Gets the full type name of the service, including namespace.
        /// </summary>
        /// <value>
        /// The full type name of the service, including namespace.
        /// </value>

        public override string ServiceFullName => typeof(TService).FullName;
    }

    #endregion ServiceHostFactory<TService> class

    #region ServiceHostFactory class

    //  ------------------------
    //  ServiceHostFactory class
    //  ------------------------

    /// <summary>
    /// Provides a base class to create and configure service hosts (classes that derive from <see cref="ServiceHostBase"/>).
    /// </summary>

    public abstract class ServiceHostFactory
    {
        #region properties

        //  ---------------
        //  Scheme property
        //  ---------------

        /// <summary>
        /// Gets the scheme name through which the service is accessed.
        /// </summary>
        /// <value>
        /// The scheme name through which the service is accessed.
        /// </value>

        protected virtual string Scheme => null;

        //  -------------
        //  Port property
        //  -------------

        /// <summary>
        /// Gets the port number through which the service is accessed.
        /// </summary>
        /// <value>
        /// The port number through which the service is accessed.
        /// </value>

        protected virtual int? Port => null;

        //  ------------------------
        //  ServiceFullName property
        //  ------------------------

        /// <summary>
        /// Gets the full type name of the service, including namespace.
        /// </summary>
        /// <value>
        /// The full type name of the service, including namespace.
        /// </value>

        public virtual string ServiceFullName => null;

        #endregion properties

        #region methods

        //  ------------------------
        //  CreateServiceHost method
        //  ------------------------

        /// <summary>
        /// Creates an instance of a class that derives from <see cref="ServiceHostBase" />
        /// and hosts the.
        /// </summary>
        /// <returns>
        /// A <see cref="ServiceHostBase"/> object that hosts the service.
        /// </returns>

        public abstract ServiceHostBase CreateServiceHost();

        //  ------------------------
        //  CreateBaseAddress method
        //  ------------------------

        /// <summary>
        /// Creates the base address for the service.
        /// </summary>
        /// <returns>
        /// The base address for the service.
        /// </returns>

        public virtual Uri CreateBaseAddress()
        {
            if (string.IsNullOrWhiteSpace(Scheme)) return null;

            var host = System.Net.Dns.GetHostName();
            var port = Port.HasValue ? string.Format(CultureInfo.InvariantCulture, ":{0}", Port.Value) : string.Empty;
            return new Uri(string.Format(CultureInfo.InvariantCulture, "{0}://{1}{2}/", Scheme, host, port));
        }

        //  ---------------------------
        //  CreateServiceAddress method
        //  ---------------------------

        /// <summary>
        /// Creates the service address.
        /// </summary>
        /// <param name="servicePath">The path part of the service URL.</param>
        /// <returns>
        /// The full URL of the service.
        /// </returns>

        public virtual Uri CreateServiceAddress(string servicePath)
        {
            var baseAddress = CreateBaseAddress();
            if (baseAddress == null) return null;

            var relative = new Uri(servicePath, UriKind.Relative);
            return new Uri(baseAddress, relative);
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

        public virtual Binding CreateChannelBinding() { return null; }

        #endregion methods
    }

    #endregion ServiceHostFactory class
}

// eof "ServiceHostFactory.cs"

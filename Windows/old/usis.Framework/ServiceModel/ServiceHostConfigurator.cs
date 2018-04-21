//
//  @(#) ServiceHostConfigurator.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2016 usis GmbH. All rights reserved.

using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace usis.Framework.ServiceModel
{
    //  ---------------------------------------
    //  ServiceHostConfigurator<TService> class
    //  ---------------------------------------

    /// <summary>
    /// Provides a class to configure an <see cref="ServiceHost"/>.
    /// </summary>
    /// <typeparam name="TService">The type of the service.</typeparam>
    /// <seealso cref="ServiceModel.ServiceHostBaseConfigurator{TService}" />

    public class ServiceHostConfigurator<TService> : ServiceHostBaseConfigurator<TService>
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

        protected override string Scheme { get { return Uri.UriSchemeHttp; } }

        //  --------------------------------
        //  CreateServiceHostInstance method
        //  --------------------------------

        /// <summary>
        /// Creates an instance of a class that derives from <see cref="T:System.ServiceModel.ServiceHostBase" />
        /// and hosts the service to configure.
        /// </summary>
        /// <returns>
        /// An <b>ServiceHostBase</b> that hosts the service to configure.
        /// </returns>

        public override ServiceHostBase CreateServiceHostInstance()
        {
            ServiceHost host = null;
            ServiceHost tmp = null;
            try
            {
                var serviceAddress = CreateServiceAddress(typeof(TService).Name);
                tmp = new ServiceHost(typeof(TService), serviceAddress);
                tmp.Description.Behaviors.Add(new ServiceMetadataBehavior
                {
                    HttpGetEnabled = true
                });
                host = tmp;
                tmp = null;
            }
            finally
            {
                if (tmp != null) ((IDisposable)tmp).Dispose();
            }
            return host;
        }
    }
}

// eof "ServiceHostConfigurator.cs"

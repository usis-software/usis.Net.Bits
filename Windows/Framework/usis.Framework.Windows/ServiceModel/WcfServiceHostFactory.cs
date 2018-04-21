//
//  @(#) WcfServiceHostFactory.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

//using System;
using System.ServiceModel;
//using System.ServiceModel.Description;

namespace usis.Framework.ServiceModel
{
    //  -------------------------------------
    //  WcfServiceHostFactory<TService> class
    //  -------------------------------------

    /// <summary>
    /// Provides a class to configure an <see cref="ServiceHost"/>.
    /// </summary>
    /// <typeparam name="TService">The type of the service.</typeparam>
    /// <seealso cref="ServiceHostFactory{TService}" />

    public class WcfServiceHostFactory<TService> : ServiceHostFactory<TService>
    {
        ////  ---------------
        ////  Scheme property
        ////  ---------------

        ///// <summary>
        ///// Gets the scheme name through which the service is accessed.
        ///// </summary>
        ///// <value>
        ///// The scheme name through which the service is accessed.
        ///// </value>

        //protected override string Scheme => Uri.UriSchemeHttp;

        //  --------------------------------
        //  CreateServiceHostInstance method
        //  --------------------------------

        /// <summary>
        /// Creates an instance of a class that derives from <see cref="ServiceHostBase" />
        /// and hosts the.
        /// </summary>
        /// <returns>
        /// A <see cref="ServiceHostBase" /> object that hosts the service.
        /// </returns>

        public override ServiceHostBase CreateServiceHost()
        {
            //ServiceHost host = null;
            //ServiceHost tmp = null;
            //try
            //{
            //    var serviceAddress = CreateServiceAddress(typeof(TService).Name);
            //    tmp = new ServiceHost(typeof(TService), serviceAddress);
            //    tmp.Description.Behaviors.Add(new ServiceMetadataBehavior
            //    {
            //        HttpGetEnabled = true
            //    });
            //    host = tmp;
            //    tmp = null;
            //}
            //finally
            //{
            //    if (tmp != null) (tmp as IDisposable)?.Dispose();
            //}
            //return host;

            return new ServiceHost(typeof(TService));
        }
    }
}

// eof "WcfServiceHostFactory.cs"

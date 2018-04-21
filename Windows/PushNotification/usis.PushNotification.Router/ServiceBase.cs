//
//  @(#) ServiceBase.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2017 usis GmbH. All rights reserved.

using usis.Framework;
using usis.Platform;

namespace usis.PushNotification
{
    //  -----------------
    //  ServiceBase class
    //  -----------------

    /// <summary>
    /// Provides a base class for WCF services.
    /// </summary>
    /// <seealso cref="ContextInjectable{IApplication}" />

    public abstract class ServiceBase : ContextInjectable<IApplication>
    {
        //  --------------
        //  Model property
        //  --------------

        /// <summary>
        /// Gets the model application extension.
        /// </summary>
        /// <value>
        /// The model application extension.
        /// </value>

        internal protected Model Model => Context.Extensions.Find<Model>();

        //  ---------------
        //  Router property
        //  ---------------

        /// <summary>
        /// Gets the router application extension.
        /// </summary>
        /// <value>
        /// The router application extension.
        /// </value>

        internal protected Router Router => Context.Extensions.Find<Router>();
    }
}

// eof "ServiceBase.cs"

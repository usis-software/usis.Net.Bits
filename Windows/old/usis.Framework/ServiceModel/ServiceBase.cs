//
//  @(#) ServiceBase.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using System;
using usis.Framework.Portable;

namespace usis.Framework.ServiceModel
{
    //  -----------------
    //  ServiceBase class
    //  -----------------

    /// <summary>
    /// Provides a base class for a service that can access a model as an extension of the hosting application.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <seealso cref="ContextInjectable{IApplication}" />

    [Obsolete("Use type from usis.Framework.Windows assembly instead.")]
    public abstract class ServiceBase<TModel> : ContextInjectable<IApplication>
    {
        //  --------------
        //  Model property
        //  --------------

        /// <summary>
        /// Gets the model as an extension of the hosting application.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>

        internal protected TModel Model { get { return Context.Extensions.Find<TModel>(); } }
    }
}

// eof "ServiceBase.cs"

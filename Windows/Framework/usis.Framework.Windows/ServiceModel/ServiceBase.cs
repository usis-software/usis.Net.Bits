//
//  @(#) ServiceBase.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using usis.Platform;

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

        internal protected TModel Model => Context.Extensions.Find<TModel>();
    }
}

// eof "ServiceBase.cs"

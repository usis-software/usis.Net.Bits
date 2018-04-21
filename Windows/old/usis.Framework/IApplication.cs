//
//  @(#) IApplication.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2016 usis GmbH. All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
#if USE_SYSTEM_SERVICEMODEL
using System.ServiceModel;
#else
using usis.Framework.Portable;
#endif
using usis.Platform.Portable;

namespace usis.Framework
{
    #region IApplication interface

    //  ----------------------
    //  IApplication interface
    //  ----------------------

    /// <summary>
    /// Provides a frame to compose an application from serveral
    /// components, so called snap-ins.
    /// </summary>

    public interface IApplication : IExtensibleObject<IApplication>
    {
        //  -------------------
        //  Properties property
        //  -------------------

        /// <summary>
        /// Gets a collection of application-scope properties.
        /// </summary>
        /// <value>
        /// An <see cref="IDictionary"/> that contains the application-scope properties.
        /// </value>

        IDictionary Properties
        {
            get;
        }

        //  -------------------------
        //  ConnectedSnapIns property
        //  -------------------------

        /// <summary>
        /// Gets a collection of snap-ins that are connected by an application.
        /// </summary>
        /// <value>
        /// An enumeration of connected snap-ins.
        /// </value>

        IEnumerable<ISnapIn> ConnectedSnapIns
        {
            get;
        }

        //  ----------------------------
        //  ConnectRequiredSnapIn method
        //  ----------------------------

        /// <summary>
        /// Connects a required snap-in.
        /// </summary>
        /// <param name="snapInType">
        /// The type of the snap-in to connect.
        /// </param>
        /// <param name="instance">
        /// The snap-in that depends on the snap-in to connect.
        /// </param>
        /// <returns>
        /// <b>true</b> if the snap-in is connected; otherwise, <b>false</b>.
        /// </returns>

        bool ConnectRequiredSnapIn(Type snapInType, ISnapIn instance);

        //  ---------------------
        //  PauseOperation method
        //  ---------------------        
        /// <summary>
        /// Pauses all operations performed by the snap-ins of an application.
        /// </summary>

        void PauseOperation();

        //  ----------------------
        //  ResumeOperation method
        //  ----------------------        
        /// <summary>
        /// Resumes  all operations performed by the snap-ins of an application.
        /// </summary>

        void ResumeOperation();
    }

    #endregion IApplication interface

    #region IApplicationExtension interface

    //  -------------------------------
    //  IApplicationExtension interface
    //  -------------------------------

    /// <summary>
    /// Enables an object to extend an application through aggregation.
    /// </summary>

    public interface IApplicationExtension : IExtension<IApplication>
    {
        //  ------------
        //  Start method
        //  ------------

        /// <summary>
        /// Allows an extension to intialize on startup.
        /// Called after all snap-ins of an application are loaded and connected.
        /// </summary>
        /// <param name="owner">
        /// The application that owns the extension.
        /// </param>

        void Start(IApplication owner);
    }

    #endregion IApplicationExtension interface

    #region ContextInjectable<T> class

    //  --------------------------
    //  ContextInjectable<T> class
    //  --------------------------

    /// <summary>
    /// Provides a base class that allows to inject a context dependancy object
    /// of the specified type.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the context object to inject.
    /// </typeparam>

    public abstract class ContextInjectable<T> : IInjectable<T>
    {
        #region properties

        //  ----------------
        //  Context property
        //  ----------------

        /// <summary>
        /// Gets the injected context object.
        /// </summary>
        /// <value>
        /// The injected context object.
        /// </value>

        protected T Context
        {
            private set;
            get;
        }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextInjectable{T}"/> class.
        /// </summary>

        protected ContextInjectable() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextInjectable{T}"/> class
        /// with the specified context dependency object.
        /// </summary>
        /// <param name="dependency">
        /// The context dependency object.
        /// </param>

        protected ContextInjectable(T dependency)
        {
            Context = dependency;
        }

        #endregion construction

        #region methods

        //  -------------
        //  Inject method
        //  -------------
        /// <summary>
        /// Injects the specified dependency.
        /// </summary>
        /// <param name="dependency">
        /// The context dependency object.
        /// </param>

        public void Inject(T dependency)
        {
            Context = dependency;
        }

        #endregion methods
    }

    #endregion ContextInjectable<T> class
}

// eof "IApplication.cs"

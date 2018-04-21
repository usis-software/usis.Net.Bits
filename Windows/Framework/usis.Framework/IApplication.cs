//
//  @(#) IApplication.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using usis.Platform;

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
        /// An <b>IHierarchicalValueStore</b> that contains the application-scope properties.
        /// </value>

        IHierarchicalValueStorage Properties { get; }

        //  -------------------------
        //  ConnectedSnapIns property
        //  -------------------------

        /// <summary>
        /// Gets a collection of snap-ins that are connected by an application.
        /// </summary>
        /// <value>
        /// An enumeration of connected snap-ins.
        /// </value>

        IEnumerable<ISnapIn> ConnectedSnapIns { get; }

        //  -----------------------------
        //  ConnectRequiredSnapIns method
        //  -----------------------------

        /// <summary>
        /// Connects the specified required snap-ins.
        /// </summary>
        /// <param name="snapInTypes">
        /// The types of the snap-ins to connect.
        /// </param>
        /// <param name="instance">
        /// The snap-in that depends on the snap-ins to connect.
        /// </param>

        void ConnectRequiredSnapIns(ISnapIn instance, params Type[] snapInTypes);

        //  ----------------------
        //  ReportException method
        //  ----------------------

        /// <summary>
        /// Allows the application to receive notifications about exceptions that occurred.
        /// </summary>
        /// <param name="exception">The exception that occurred.</param>

        void ReportException(Exception exception);
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
}

// eof "IApplication.cs"

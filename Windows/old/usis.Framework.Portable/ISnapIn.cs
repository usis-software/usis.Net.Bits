//
//  @(#) ISnapIn.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2016 usis GmbH. All rights reserved.

using System;

namespace usis.Framework.Portable
{
    //  -----------------
    //  ISnapIn interface
    //  -----------------

    /// <summary>
    /// The <b>ISnapIn</b> interface allows an implementing class to be
    /// connected by an <see cref="IApplication"/> during startup.
    /// </summary>

    [Obsolete("Use type from usis.Framework namespace instead.")]
    public interface ISnapIn
    {
        //  -------------------
        //  OnConnection method
        //  -------------------

        /// <summary>
        /// An object that implements <see cref="IApplication"/> calls this method during startup -
        /// after an instance of the implementing class has been created.
        /// </summary>
        /// <param name="application">
        /// The application that connects the snap-in.
        /// </param>
        /// <returns>
        /// <b>true</b> if the snap-in is connected; otherwise, <b>false</b>.
        /// </returns>

        bool OnConnection(IApplication application);

        //  --------------------
        //  CanDisconnect method
        //  --------------------

        /// <summary>
        /// Called to determine whether the implementing snap-in
        /// is ready to be disconnnected.
        /// </summary>
        /// <returns>
        /// <b>true</b> when the snap-in is ready to be disconnected;
        /// otherwise, <b>false</b>.
        /// </returns>

        bool CanDisconnect();

        //  ----------------------
        //  OnDisconnection method
        //  ----------------------

        /// <summary>
        /// Called to disconnect the implementing snap-in.
        /// </summary>

        void OnDisconnection();
    }
}

// eof "ISnapIn.cs"

//
//  @(#) IExtension.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

using System;
using usis.Platform;

namespace usis.Framework
{
    //  --------------------------
    //  ApplicationExtension class
    //  --------------------------

    /// <summary>
    /// Provides a base class that enables a derived class to extend an application through aggregation.
    /// </summary>

    public abstract class ApplicationExtension : Extension<IApplication>, IApplicationExtension
    {
        #region IApplicationExtension implementation

        //  ------------
        //  Start method
        //  ------------

        /// <summary>
        /// Allows an extension to intialize on startup.
        /// </summary>
        /// <param name="owner">The application that owns the extension.</param>

        public void Start(IApplication owner)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));
            if (!owner.Equals(Owner)) throw new InvalidOperationException();
            OnStart();
        }

        #endregion IApplicationExtension implementation

        #region virtual methods

        //  --------------
        //  OnStart method
        //  --------------

        /// <summary>
        /// Called after all snap-ins of an application are loaded and connected.
        /// </summary>

        protected virtual void OnStart() { }

        #endregion virtual methods
    }
}

// eof "IExtension.cs"

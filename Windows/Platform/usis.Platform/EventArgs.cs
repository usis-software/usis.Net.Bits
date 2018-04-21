//
//  @(#) EventArgs.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015-2017 usis GmbH. All rights reserved.

using System;

namespace usis.Platform
{
    //  ------------------
    //  EventArgs<T> class
    //  ------------------

    /// <summary>
    /// Provides a base class for <see cref="EventArgs"/> that hold an typed argument.
    /// </summary>
    /// <typeparam name="T">The type of the argument.</typeparam>
    /// <seealso cref="EventArgs" />

    [Obsolete("Implement a property directly.")]
    public abstract class EventArgs<T> : EventArgs
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="EventArgs{T}"/> class
        /// with the specified argument.
        /// </summary>
        /// <param name="argument">The argument.</param>

        protected EventArgs(T argument) { Argument = argument; }

        #endregion construction

        #region properties

        //  -----------------
        //  Argument property
        //  -----------------

        /// <summary>
        /// Gets the argument.
        /// </summary>
        /// <value>
        /// The argument.
        /// </value>

        public T Argument { get; }

        #endregion properties
    }
}

// eof "EventArgs.cs"

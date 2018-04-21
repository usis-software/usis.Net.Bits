//
//  @(#) ExceptionEventArgs.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using System;

namespace usis.Platform
{
    //  ------------------------
    //  ExceptionEventArgs class
    //  ------------------------

    /// <summary>
    /// Represent data with exception informations for an event.
    /// </summary>
    /// <seealso cref="EventArgs" />

    public class ExceptionEventArgs : EventArgs
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionEventArgs"/> class
        /// with the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>

        public ExceptionEventArgs(Exception exception) { Exception = exception; }

        #endregion construction

        #region properties

        //  ------------------
        //  Exception property
        //  ------------------

        /// <summary>
        /// Gets the exception.
        /// </summary>
        /// <value>
        /// The exception.
        /// </value>

        public Exception Exception { get; }

        #endregion properties
    }
}

// eof "ExceptionEventArgs.cs"

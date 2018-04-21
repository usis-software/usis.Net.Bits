//
//  @(#) AsyncStatusEventArgs.cs
//
//  Project:    usis.ManagementConsole
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using Microsoft.ManagementConsole;
using System;

namespace usis.ManagementConsole
{
    //  --------------------------
    //  AsyncStatusEventArgs class
    //  --------------------------

    /// <summary>
    /// Provides the status information for asynchronous operation events.
    /// </summary>
    /// <seealso cref="EventArgs" />

    public class AsyncStatusEventArgs : EventArgs
    {
        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncStatusEventArgs"/> class
        /// with the specified status information for asynchronous operations.
        /// </summary>
        /// <param name="status">The status information for asynchronous operations.</param>

        public AsyncStatusEventArgs(AsyncStatus status) { Status = status; }

        //  ---------------
        //  Status property
        //  ---------------

        /// <summary>
        /// Gets the status information for an asynchronous operation.
        /// </summary>
        /// <value>
        /// The status information for an asynchronous operation.
        /// </value>

        public AsyncStatus Status { get; }
    }
}

// eof "AsyncStatusEventArgs.cs"

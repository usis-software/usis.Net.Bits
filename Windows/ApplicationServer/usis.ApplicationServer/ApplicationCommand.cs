//
//  @(#) ApplicationCommand.cs
//
//  Project:    usis.Server
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2016 usis GmbH. All rights reserved.

namespace usis.ApplicationServer
{
    //  ------------------------------
    //  ApplicationCommand enumeration
    //  ------------------------------

    /// <summary>
    /// Indicates the command to send to an application.
    /// </summary>

    public enum ApplicationCommand
    {
        /// <summary>
        /// Starts an application.
        /// </summary>

        Start,

        /// <summary>
        /// Stops an application.
        /// </summary>

        Stop,

        /// <summary>
        /// Tells the application and its snap-ins to pause all operations.
        /// </summary>

        Pause,

        /// <summary>
        /// Tells the application and its snap-ins to resume all operations.
        /// </summary>

        Resume
    };
}

// eof "ApplicationCommand.cs"

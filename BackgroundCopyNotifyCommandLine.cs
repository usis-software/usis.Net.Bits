//
//  @(#) BackgroundCopyNotifyCommandLine.cs
//
//  Project:    usis.Net.Bits
//  System:     Microsoft Visual Studio 2022
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017-2024 usis GmbH. All rights reserved.

namespace usis.Net.Bits
{
    //  -------------------------------------
    //  BackgroundCopyNotifyCommandLine class
    //  -------------------------------------

    /// <summary>
    /// Specifies a program to execute when the job enters the error or transferred state.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="BackgroundCopyNotifyCommandLine"/> class.
    /// </remarks>
    /// <param name="program">The program to execute.</param>
    /// <param name="parameters">The parameters of the program.</param>

    public sealed class BackgroundCopyNotifyCommandLine(string program, string? parameters)
    {
        #region properties

        //  ----------------
        //  Program property
        //  ----------------

        /// <summary>
        /// Gets the program to execute.
        /// </summary>
        /// <value>
        /// The program to execute.
        /// </value>

        public string? Program { get; } = program;

        //  -------------------
        //  Parameters property
        //  -------------------

        /// <summary>
        /// Gets the parameters of the program.
        /// </summary>
        /// <value>
        /// The parameters of the program.
        /// </value>

        public string? Parameters { get; } = parameters;

        #endregion

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundCopyNotifyCommandLine"/> class
        /// with the specified program to execute.
        /// </summary>
        /// <param name="program">The program to execute.</param>

        public BackgroundCopyNotifyCommandLine(string program) : this(program, null) { }

        #endregion
    }
}

// eof "BackgroundCopyNotifyCommandLine.cs"

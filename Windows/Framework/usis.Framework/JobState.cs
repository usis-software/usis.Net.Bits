//
//  @(#) JobState.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

namespace usis.Framework
{
    //  --------------------
    //  JobState enumeration
    //  --------------------

    /// <summary>
    /// Indicates the current state of a job.
    /// </summary>

    public enum JobState
    {
        /// <summary>
        /// The current job state is unknown.
        /// </summary>

        Unknown,

        /// <summary>
        /// The job was newly created but not yet started.
        /// </summary>

        New,

        /// <summary>
        /// The job is running.
        /// </summary>

        Running,

        /// <summary>
        /// The job was completed.
        /// </summary>

        Completed,

        /// <summary>
        /// The job failed because an exception occurred.
        /// </summary>

        Failed
    }
}

// eof "JobState.cs"

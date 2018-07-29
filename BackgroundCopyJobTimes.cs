//
//  @(#) BackgroundCopyJobProgress.cs
//
//  Project:    usis.Net.Bits
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017,2018 usis GmbH. All rights reserved.

using System;
using usis.Net.Bits.Interop;

namespace usis.Net.Bits
{
    //  ----------------------------
    //  BackgroundCopyJobTimes class
    //  ----------------------------

    /// <summary>
    /// Provides job-related time stamps.
    /// </summary>

    public class BackgroundCopyJobTimes
    {
        #region fields

        private BG_JOB_TIMES times;

        #endregion fields

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal BackgroundCopyJobTimes(BG_JOB_TIMES times) => this.times = times;

        #endregion construction

        #region properties

        //  ---------------------
        //  CreationTime property
        //  ---------------------

        /// <summary>
        /// Gets the time the job was created.
        /// </summary>
        /// <value>
        /// The time the job was created.
        /// </value>

        public DateTime CreationTime => times.CreationTime.ToDateTime();

        //  -------------------------
        //  ModificationTime property
        //  -------------------------

        /// <summary>
        /// Gets the time the job was last modified or bytes were transferred.
        /// </summary>
        /// <value>
        /// The time the job was last modified or bytes were transferred.
        /// </value>
        /// <remarks>
        /// Adding files or calling any of the set methods of the <see cref="BackgroundCopyJob"/> class changes this value.
        /// In addition, changes to the state of the job and calling the <c>Suspend</c>, <c>Resume</c>, <c>Cancel</c>,
        /// and <c>Complete</c> methods change this value
        /// </remarks>

        public DateTime ModificationTime => times.ModificationTime.ToDateTime();

        //  -------------------------------
        //  TransferCompletionTime property
        //  -------------------------------

        /// <summary>
        /// Gets the time the job entered the <see cref="BackgroundCopyJobState.Transferred"/>  state.
        /// </summary>
        /// <value>
        /// The transfer completion time.
        /// </value>

        public DateTime TransferCompletionTime => times.TransferCompletionTime.ToDateTime();

        #endregion properties
    }
}

// eof "BackgroundCopyJobProgress.cs"

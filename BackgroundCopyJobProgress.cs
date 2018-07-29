//
//  @(#) BackgroundCopyJobProgress.cs
//
//  Project:    usis.Net.Bits
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017,2018 usis GmbH. All rights reserved.

using usis.Net.Bits.Interop;

namespace usis.Net.Bits
{
    //  -------------------------------
    //  BackgroundCopyJobProgress class
    //  -------------------------------

    /// <summary>
    /// Provides job-related progress information,
    /// such as the number of bytes and files transferred.
    /// </summary>

    public sealed class BackgroundCopyJobProgress
    {
        #region fields

        private BG_JOB_PROGRESS progress;

        #endregion fields

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal BackgroundCopyJobProgress(BG_JOB_PROGRESS progress) => this.progress = progress;

        #endregion construction

        #region public properties

        //  -------------------
        //  BytesTotal property
        //  -------------------

        /// <summary>
        /// Total number of bytes to transfer for all files in the job.
        /// </summary>

        public long BytesTotal => progress.bytesTotal;

        //  -------------------------
        //  BytesTransferred property
        //  -------------------------

        /// <summary>
        /// Number of bytes transferred.
        /// </summary>

        public long BytesTransferred => progress.bytesTransferred;

        //  -------------------
        //  FilesTotal property
        //  -------------------

        /// <summary>
        /// Total number of files to transfer for this job.
        /// </summary>

        public int FilesTotal => progress.filesTotal;

        //  -------------------------
        //  FilesTransferred property
        //  -------------------------

        /// <summary>
        /// Number of files transferred.
        /// </summary>

        public int FilesTransferred => progress.filesTransferred;

        #endregion public properties
    }

    #region BackgroundCopyJobReplyProgress class

    //  ------------------------------------
    //  BackgroundCopyJobReplyProgress class
    //  ------------------------------------

    /// <summary>
    /// Provides progress information related to the reply portion of an upload-reply job.
    /// </summary>

    public sealed class BackgroundCopyJobReplyProgress
    {
        #region fields

        private BG_JOB_REPLY_PROGRESS progress;

        #endregion fields

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal BackgroundCopyJobReplyProgress(BG_JOB_REPLY_PROGRESS progress) => this.progress = progress;

        #endregion construction

        #region public properties

        //  -------------------
        //  BytesTotal property
        //  -------------------

        /// <summary>
        /// Total number of bytes to transfer.
        /// </summary>

        public long BytesTotal => (long)progress.bytesTotal;

        //  -------------------------
        //  BytesTransferred property
        //  -------------------------

        /// <summary>
        /// Number of bytes transferred.
        /// </summary>

        public long BytesTransferred => (long)progress.bytesTransferred;

        #endregion public properties
    }

    #endregion BackgroundCopyJobReplyProgress class
}

// eof "BackgroundCopyJobProgress.cs"

//
//  @(#) BackgroundCopyFileProgress.cs
//
//  Project:    usis.Net.Bits
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System.Globalization;
using usis.Net.Bits.Interop;

namespace usis.Net.Bits
{
    //  --------------------------------
    //  BackgroundCopyFileProgress class
    //  --------------------------------

    /// <summary>
    /// Provides file-related progress information,
    /// such as the number of bytes transferred.
    /// </summary>

    public class BackgroundCopyFileProgress
    {
        #region fields

        private BG_FILE_PROGRESS progress;

        #endregion fields

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal BackgroundCopyFileProgress(BG_FILE_PROGRESS progress) { this.progress = progress; }

        #endregion construction

        #region properties

        //  -------------------
        //  BytesTotal property
        //  -------------------

        /// <summary>
        /// Gets the size of the file in bytes.
        /// If the value is -1, the total size of the file has not been determined.
        /// BITS does not set this value if it cannot determine the size of the file.
        /// For example, if the specified file or server does not exist,
        /// BITS cannot determine the size of the file.
        /// </summary>

        public long BytesTotal => progress.bytesTotal;

        //  -------------------------
        //  BytesTransferred property
        //  -------------------------

        /// <summary>
        /// Gets the number of bytes transferred.
        /// </summary>

        public long BytesTransferred => progress.bytesTransferred;

        //  ------------------
        //  Completed property
        //  ------------------

        /// <summary>
        /// Gets a value indicating whether the file is available to the user.
        /// </summary>

        public bool Completed => progress.completed;

        #endregion properties

        #region methods

        //  ---------------
        //  ToString method
        //  ---------------

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture,
                "{0}: {1} bytes of {2}; {3}completed.",
                nameof(BackgroundCopyFileProgress), BytesTransferred, BytesTotal, Completed ? string.Empty : "not ");
        }

        #endregion methods
    }
}

// eof "BackgroundCopyFileProgress.cs"

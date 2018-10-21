//
//  @(#) BackgroundCopyFileRange.cs
//
//  Project:    usis.Net.Bits
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2018 usis GmbH. All rights reserved.

using System;
using usis.Net.Bits.Interop;

namespace usis.Net.Bits
{
    //  -----------------------------
    //  BackgroundCopyFileRange class
    //  -----------------------------

    /// <summary>
    /// Identifies a range of bytes to download from a file.
    /// </summary>

    public sealed class BackgroundCopyFileRange
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundCopyFileRange"/> class
        /// with the specified offset and length.
        /// </summary>
        /// <param name="offset">The offset to the beginning of the range of bytes to download from a file.</param>
        /// <param name="length">The length of the range, in bytes.</param>

        public BackgroundCopyFileRange(long offset, long length) { Offset = offset; Length = length; }

        #endregion construction

        #region properties

        //  ---------------
        //  Offset property
        //  ---------------

        /// <summary>
        /// Gets or sets the offset to the beginning of the range of bytes to download from a file.
        /// </summary>
        /// <value>
        /// The zero-based offset to the beginning of the range of bytes to download from a file.
        /// </value>

        public long Offset { get; set; }

        //  ---------------
        //  Length property
        //  ---------------

        /// <summary>
        /// Gets or sets the length of the range, in bytes.
        /// </summary>
        /// <value>
        /// The length of the range, in bytes.
        /// </value>

        public long Length { get; }

        #endregion properties

        #region methods

        //  ------------------
        //  ToFileRange method
        //  ------------------

        internal BG_FILE_RANGE ToFileRange()
        {
            return new BG_FILE_RANGE()
            {
                InitialOffset = Convert.ToUInt64(Offset),
                Length = Length == Constants.LengthToEndOfFile ? Interop.Constants.BG_LENGTH_TO_EOF : Convert.ToUInt64(Length)
            };
        }

        #endregion methods
    }
}

// eof "BackgroundCopyFileRange.cs"

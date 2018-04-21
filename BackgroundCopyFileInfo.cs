//
//  @(#) BackgroundCopyFileInfo.cs
//
//  Project:    usis.Net.Bits
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using usis.Net.Bits.Interop;

namespace usis.Net.Bits
{
    //  ----------------------------
    //  BackgroundCopyFileInfo class
    //  ----------------------------

    /// <summary>
    /// Provides the local and remote names of the file to transfer.
    /// </summary>

    public class BackgroundCopyFileInfo
    {
        #region fields

        internal BG_FILE_INFO fileInfo;

        #endregion fields

        #region properties

        //  ------------------
        //  LocalName property
        //  ------------------

        /// <summary>
        /// Gets or sets the local name of the file.
        /// </summary>
        /// <value>
        /// The local name of the file.
        /// </value>

        public string LocalName { get => fileInfo.LocalName; set => fileInfo.LocalName = value; }

        //  -------------------
        //  RemoteName property
        //  -------------------

        /// <summary>
        /// Gets or sets the remote name of the file.
        /// </summary>
        /// <value>
        /// The remote name of the file.
        /// </value>

        public string RemoteName { get => fileInfo.RemoteName; set => fileInfo.RemoteName = value; }

        #endregion properties
    }
}

// eof "BackgroundCopyFileInfo.cs"

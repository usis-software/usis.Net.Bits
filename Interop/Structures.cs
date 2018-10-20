//
//  @(#) Structures.cs
//
//  Project:    usis.Net.Bits
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017,2018 usis GmbH. All rights reserved.

using System.Runtime.InteropServices;

namespace usis.Net.Bits.Interop
{
    #region BG_JOB_PROGRESS structure

    //  -------------------------
    //  BG_JOB_PROGRESS structure
    //  -------------------------

    [StructLayout(LayoutKind.Sequential)]
    internal struct BG_JOB_PROGRESS
    {
        internal long bytesTotal;

        internal long bytesTransferred;

        internal int filesTotal;

        internal int filesTransferred;
    }

    #endregion BG_JOB_PROGRESS structure

    #region BG_JOB_REPLY_PROGRESS structure

    //  -------------------------------
    //  BG_JOB_REPLY_PROGRESS structure
    //  -------------------------------

    [StructLayout(LayoutKind.Sequential)]
    internal struct BG_JOB_REPLY_PROGRESS
    {
        internal ulong bytesTotal;

        internal ulong bytesTransferred;
    }

    #endregion BG_JOB_REPLY_PROGRESS structure

    #region BG_FILE_PROGRESS structure

    //  --------------------------
    //  BG_FILE_PROGRESS structure
    //  --------------------------

    [StructLayout(LayoutKind.Sequential)]
    internal struct BG_FILE_PROGRESS
    {
        internal long bytesTotal;

        internal long bytesTransferred;

        [MarshalAs(UnmanagedType.Bool)]
        internal bool completed;
    }

    #endregion BG_FILE_PROGRESS structure

    #region BG_JOB_TIMES structure

    //  ----------------------
    //  BG_JOB_TIMES structure
    //  ----------------------

    [StructLayout(LayoutKind.Sequential)]
    internal struct BG_JOB_TIMES
    {
        internal System.Runtime.InteropServices.ComTypes.FILETIME CreationTime;
        internal System.Runtime.InteropServices.ComTypes.FILETIME ModificationTime;
        internal System.Runtime.InteropServices.ComTypes.FILETIME TransferCompletionTime;
    }

    #endregion BG_JOB_TIMES structure

    #region BG_FILE_INFO structure

    //  ----------------------
    //  BG_FILE_INFO structure
    //  ----------------------

    [StructLayout(LayoutKind.Sequential)]
    internal struct BG_FILE_INFO
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        internal string RemoteName;

        [MarshalAs(UnmanagedType.LPWStr)]
        internal string LocalName;
    }

    #endregion BG_FILE_INFO structure

    #region BG_AUTH_CREDENTIALS structure

    //  -----------------------------
    //  BG_AUTH_CREDENTIALS structure
    //  -----------------------------

    [StructLayout(LayoutKind.Sequential)]
    internal struct BG_AUTH_CREDENTIALS
    {
        internal BackgroundCopyAuthenticationTarget Target;

        internal BackgroundCopyAuthenticationScheme Scheme;

        [MarshalAs(UnmanagedType.LPWStr)]
        internal string UserName;

        [MarshalAs(UnmanagedType.LPWStr)]
        internal string Password;
    }

    #endregion BG_AUTH_CREDENTIALS structure
}

// eof "Structures.cs"

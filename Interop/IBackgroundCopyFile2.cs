//
//  @(#) IBackgroundCopyFile2.cs
//
//  Project:    usis.Net.Bits
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017-2018 usis GmbH. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace usis.Net.Bits.Interop
{
    //  ------------------------------
    //  IBackgroundCopyFile2 interface
    //  ------------------------------

    [ComImport]
    [Guid(IID.IBackgroundCopyFile2)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IBackgroundCopyFile2
    {
        #region IBackgroundCopyFile methods

        //  --------------------
        //  GetRemoteName method
        //  --------------------

        [return: MarshalAs(UnmanagedType.LPWStr)]
        string GetRemoteName();

        //  -------------------
        //  GetLocalName method
        //  -------------------

        [return: MarshalAs(UnmanagedType.LPWStr)]
        string GetLocalName();

        //  ------------------
        //  GetProgress method
        //  ------------------

        BG_FILE_PROGRESS GetProgress();

        #endregion IBackgroundCopyFile methods

        //  --------------------
        //  GetFileRanges method
        //  --------------------

        void GetFileRanges(out uint rangeCount, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] out BG_FILE_RANGE[] ranges);

        //  --------------------
        //  SetRemoteName method
        //  --------------------

        void SetRemoteName([MarshalAs(UnmanagedType.LPWStr)] string url);
    }
}

// eof "IBackgroundCopyFile2.cs"

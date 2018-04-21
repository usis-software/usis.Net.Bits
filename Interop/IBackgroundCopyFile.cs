//
//  @(#) IBackgroundCopyFile.cs
//
//  Project:    usis.Net.Bits
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace usis.Net.Bits.Interop
{
    //  -----------------------------
    //  IBackgroundCopyFile interface
    //  -----------------------------

    [ComImport]
    [Guid(IID.IBackgroundCopyFile)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IBackgroundCopyFile
    {
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
    }
}

// eof "IBackgroundCopyFile.cs"

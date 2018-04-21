//
//  @(#) IBackgroundCopyError.cs
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
    //  ------------------------------
    //  IBackgroundCopyError interface
    //  ------------------------------

    [ComImport]
    [Guid(IID.IBackgroundCopyError)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IBackgroundCopyError
    {
        //  ---------------
        //  GetError method
        //  ---------------

        void GetError(out BackgroundCopyErrorContext context, out int code);

        //  --------------
        //  GetFile method
        //  --------------

        [return: MarshalAs(UnmanagedType.Interface)]
        IBackgroundCopyFile GetFile();

        //  --------------------------
        //  GetErrorDescription method
        //  --------------------------

        [PreserveSig]
        uint GetErrorDescription(int languageId, [MarshalAs(UnmanagedType.LPWStr)] out string description);

        //  ---------------------------------
        //  GetErrorContextDescription method
        //  ---------------------------------

        [PreserveSig]
        uint GetErrorContextDescription(int languageId, [MarshalAs(UnmanagedType.LPWStr)] out string description);

        //  ------------------
        //  GetProtocol method
        //  ------------------

        [return: MarshalAs(UnmanagedType.LPWStr)]
        string GetProtocol();
    }
}

// eof "IBackgroundCopyError.cs"

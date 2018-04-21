//
//  @(#) IEnumBackgroundCopyFiles.cs
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
    //  ----------------------------------
    //  IEnumBackgroundCopyFiles interface
    //  ----------------------------------

    [ComImport]
    [Guid(IID.IEnumBackgroundCopyFiles)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IEnumBackgroundCopyFiles
    {
        //  -----------
        //  Next method
        //  -----------

        [PreserveSig]
        int Next(uint count, [MarshalAs(UnmanagedType.Interface)] out IBackgroundCopyFile element, IntPtr fetched);

        //  -----------
        //  Skip method
        //  -----------

        [Obsolete]
        void Skip(uint count);

        //  ------------
        //  Reset method
        //  ------------

        [Obsolete]
        void Reset();

        //  ------------
        //  Clone method
        //  ------------

        [Obsolete]
        [return: MarshalAs(UnmanagedType.Interface)]
        IEnumBackgroundCopyFiles Clone();

        //  ---------------
        //  GetCount method
        //  ---------------

        [Obsolete]
        uint GetCount();
    }
}

// eof "IEnumBackgroundCopyFiles.cs"

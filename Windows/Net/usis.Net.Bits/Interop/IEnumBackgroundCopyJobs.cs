//
//  @(#) IEnumBackgroundCopyJobs.cs
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
    //  ---------------------------------
    //  IEnumBackgroundCopyJobs interface
    //  ---------------------------------

    [ComImport]
    [Guid(IID.IEnumBackgroundCopyJobs)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IEnumBackgroundCopyJobs
    {
        //  -----------
        //  Next method
        //  -----------

        [PreserveSig]
        int Next(uint count, [MarshalAs(UnmanagedType.Interface)] out IBackgroundCopyJob element, IntPtr fetched);

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
        IEnumBackgroundCopyJobs Clone();

        //  ---------------
        //  GetCount method
        //  ---------------

        [Obsolete]
        uint GetCount();
    }
}

// eof "IEnumBackgroundCopyJobs.cs"

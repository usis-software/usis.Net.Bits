//
//  @(#) IBackgroundCopyManager.cs
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
    //  --------------------------------
    //  IBackgroundCopyManager interface
    //  --------------------------------

    [ComImport]
    [Guid(IID.IBackgroundCopyManager)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IBackgroundCopyManager
    {
        //  ----------------
        //  CreateJob method
        //  ----------------

        [return: MarshalAs(UnmanagedType.Interface)]
        IBackgroundCopyJob CreateJob([MarshalAs(UnmanagedType.LPWStr)] string displayName, BackgroundCopyJobType type, out Guid jobId);

        //  -------------
        //  GetJob method
        //  -------------

        [PreserveSig]
        uint GetJob(ref Guid jobId, out IBackgroundCopyJob job);

        //  ---------------
        //  EnumJobs method
        //  ---------------

        [return: MarshalAs(UnmanagedType.Interface)]
        IEnumBackgroundCopyJobs EnumJobs(uint flags);

        //  --------------------------
        //  GetErrorDescription method
        //  --------------------------

        [PreserveSig]
        uint GetErrorDescription(uint hResult, int languageId, [MarshalAs(UnmanagedType.LPWStr)] out string description);
    }
}

// eof "IBackgroundCopyManager.cs"

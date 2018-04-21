//
//  @(#) IBackgroundCopyCallback.cs
//
//  Project:    usis.Net.Bits
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System.Runtime.InteropServices;

namespace usis.Net.Bits.Interop
{
    //  ---------------------------------
    //  IBackgroundCopyCallback interface
    //  ---------------------------------

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid(IID.IBackgroundCopyCallback)]
    internal interface IBackgroundCopyCallback
    {
        //  ---------------------
        //  JobTransferred method
        //  ---------------------

        [PreserveSig]
        uint JobTransferred([MarshalAs(UnmanagedType.Interface)] IBackgroundCopyJob job);

        //  ---------------
        //  JobError method
        //  ---------------

        [PreserveSig]
        uint JobError(
            [MarshalAs(UnmanagedType.Interface)] IBackgroundCopyJob job,
            [MarshalAs(UnmanagedType.Interface)] IBackgroundCopyError error);

        //  ----------------------
        //  JobModification method
        //  ----------------------

        [PreserveSig]
        uint JobModification([MarshalAs(UnmanagedType.Interface)] IBackgroundCopyJob job, int reserved);
    }
}

// eof "IBackgroundCopyCallback.cs"

//
//  @(#) IBackgroundCopyCallback.cs
//
//  Project:    usis.Net.Bits
//  System:     Microsoft Visual Studio 2019
//  Author:     Udo Schäfer
//
//  Copyright (c) 2019 usis GmbH. All rights reserved.

using System.Runtime.InteropServices;

namespace usis.Net.Bits.Interop
{
    //  ----------------------------------
    //  IBackgroundCopyCallback2 interface
    //  ----------------------------------

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid(IID.IBackgroundCopyCallback2)]
    internal interface IBackgroundCopyCallback2
    {
        #region IBackgroundCopyCallback methods

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

        #endregion IBackgroundCopyCallback methods

        //  ----------------------
        //  FileTransferred method
        //  ----------------------

        [PreserveSig]
        uint FileTransferred(
            [MarshalAs(UnmanagedType.Interface)] IBackgroundCopyJob job,
            [MarshalAs(UnmanagedType.Interface)] IBackgroundCopyFile file);
    }
}

// eof "IBackgroundCopyCallback.cs"

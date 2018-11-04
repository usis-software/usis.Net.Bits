//
//  @(#) IBackgroundCopyJobHttpOptions.cs
//
//  Project:    usis.Net.Bits
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2018 usis GmbH. All rights reserved.

using System.Runtime.InteropServices;

namespace usis.Net.Bits.Interop
{
    //  ---------------------------------------
    //  IBackgroundCopyJobHttpOptions interface
    //  ---------------------------------------

    [ComImport]
    [Guid(IID.IBackgroundCopyJobHttpOptions)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IBackgroundCopyJobHttpOptions
    {
        //  -------------------------------
        //  SetClientCertificateByID method
        //  -------------------------------

        void SetClientCertificateByID(
            BackgroundCopyCertificateStoreLocation storeLocation,
            [MarshalAs(UnmanagedType.LPWStr)] string storeName,
            [MarshalAs(UnmanagedType.LPArray)] byte[] certHashBlob);

        //  ---------------------------------
        //  SetClientCertificateByName method
        //  ---------------------------------

        void SetClientCertificateByName(
            BackgroundCopyCertificateStoreLocation storeLocation,
            [MarshalAs(UnmanagedType.LPWStr)] string storeName,
            [MarshalAs(UnmanagedType.LPWStr)] string subjectName);

        //  ------------------------------
        //  RemoveClientCertificate method
        //  ------------------------------

        void RemoveClientCertificate();

        //  ---------------------------
        //  GetClientCertificate method
        //  ---------------------------

        void GetClientCertificate(
            out BackgroundCopyCertificateStoreLocation storeLocation,
            [MarshalAs(UnmanagedType.LPWStr)] out string storeName,
            [MarshalAs(UnmanagedType.LPArray, SizeConst = 20)] out byte[] certHashBlob,
            [MarshalAs(UnmanagedType.LPWStr)] out string subjectName);

        //  -----------------------
        //  SetCustomHeaders method
        //  -----------------------

        void SetCustomHeaders([MarshalAs(UnmanagedType.LPWStr)] string requestHeaders);

        //  -----------------------
        //  GetCustomHeaders method
        //  -----------------------

        [return: MarshalAs(UnmanagedType.LPWStr)]
        string GetCustomHeaders();

        //  -----------------------
        //  SetSecurityFlags method
        //  -----------------------

        void SetSecurityFlags(BackgroundCopyJobHttpSecurityOptions flags);

        //  -----------------------
        //  GetSecurityFlags method
        //  -----------------------

        BackgroundCopyJobHttpSecurityOptions GetSecurityFlags();
    }
}

// eof "IBackgroundCopyJobHttpOptions.cs"

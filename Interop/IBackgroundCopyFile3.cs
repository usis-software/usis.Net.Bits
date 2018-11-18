//
//  @(#) IBackgroundCopyFile3.cs
//
//  Project:    usis.Net.Bits
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2018 usis GmbH. All rights reserved.

using System.Runtime.InteropServices;

namespace usis.Net.Bits.Interop
{
    //  ------------------------------
    //  IBackgroundCopyFile3 interface
    //  ------------------------------

    [ComImport]
    [Guid(IID.IBackgroundCopyFile3)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IBackgroundCopyFile3
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

        #region IBackgroundCopyFile2 methods

        //  --------------------
        //  GetFileRanges method
        //  --------------------

        void GetFileRanges(out uint rangeCount, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] out BG_FILE_RANGE[] ranges);

        //  --------------------
        //  SetRemoteName method
        //  --------------------

        void SetRemoteName([MarshalAs(UnmanagedType.LPWStr)] string url);

        #endregion IBackgroundCopyFile2 methods

        //virtual HRESULT STDMETHODCALLTYPE SetValidationState(
        //    /* [in] */ BOOL state) = 0;

        //virtual HRESULT STDMETHODCALLTYPE GetValidationState(
        //    /* [out] */ __RPC__out BOOL *pState) = 0;

        //virtual HRESULT STDMETHODCALLTYPE IsDownloadedFromPeer(
        //    /* [out] */ __RPC__out BOOL *pVal) = 0;

        [return: MarshalAs(UnmanagedType.LPWStr)]
        string GetTemporaryName();

        void SetValidationState([MarshalAs(UnmanagedType.Bool)] bool state);

        [return: MarshalAs(UnmanagedType.Bool)]
        bool GetValidationState();

        [return: MarshalAs(UnmanagedType.Bool)]
        bool IsDownloadedFromPeer();
    }
}

// eof "IBackgroundCopyFile3.cs"

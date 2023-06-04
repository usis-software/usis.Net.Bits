//
//  @(#) IBackgroundCopyJob.cs
//
//  Project:    usis.Net.Bits
//  System:     Microsoft Visual Studio 2022
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017-2023 usis GmbH. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace usis.Net.Bits.Interop
{
    //  ----------------------------
    //  IBackgroundCopyJob interface
    //  ----------------------------

    [ComImport]
    [Guid(IID.IBackgroundCopyJob)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IBackgroundCopyJob
    {
        //  -----------------
        //  AddFileSet method
        //  -----------------

        void AddFileSet(int cFileCount, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] BG_FILE_INFO[] pFileSet);

        //  --------------
        //  AddFile method
        //  --------------

        void AddFile([MarshalAs(UnmanagedType.LPWStr)] string remoteUrl, [MarshalAs(UnmanagedType.LPWStr)] string localName);

        //  ----------------
        //  EnumFiles method
        //  ----------------

        [return: MarshalAs(UnmanagedType.Interface)]
        IEnumBackgroundCopyFiles EnumFiles();

        //  --------------
        //  Suspend method
        //  --------------

        void Suspend();

        //  -------------
        //  Resume method
        //  -------------

        void Resume();

        //  -------------
        //  Cancel method
        //  -------------

        void Cancel();

        //  ---------------
        //  Complete method
        //  ---------------

        void Complete();

        //  ------------
        //  GetId method
        //  ------------

        Guid GetId();

        //  --------------
        //  GetType method
        //  --------------

        BackgroundCopyJobType GetType();

        //  ------------------
        //  GetProgress method
        //  ------------------

        BG_JOB_PROGRESS GetProgress();

        //  ---------------
        //  GetTimes method
        //  ---------------

        BG_JOB_TIMES GetTimes();

        //  ---------------
        //  GetState method
        //  ---------------

        BackgroundCopyJobState GetState();

        //  ---------------
        //  GetError method
        //  ---------------

        [PreserveSig]
        uint GetError([MarshalAs(UnmanagedType.Interface)] out IBackgroundCopyError error);

        //  ---------------
        //  GetOwner method
        //  ---------------

        [return: MarshalAs(UnmanagedType.LPWStr)]
        string GetOwner();

        //  ---------------------
        //  SetDisplayName method
        //  ---------------------

        void SetDisplayName([MarshalAs(UnmanagedType.LPWStr)] string displayName);

        //  ---------------------
        //  GetDisplayName method
        //  ---------------------

        [return: MarshalAs(UnmanagedType.LPWStr)]
        string GetDisplayName();

        //  ---------------------
        //  SetDescription method
        //  ---------------------

        void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string description);

        //  ---------------------
        //  GetDescription method
        //  ---------------------

        [return: MarshalAs(UnmanagedType.LPWStr)]
        string GetDescription();

        //  ------------------
        //  SetPriority method
        //  ------------------

        void SetPriority(BackgroundCopyJobPriority priority);

        //  ------------------
        //  GetPriority method
        //  ------------------

        BackgroundCopyJobPriority GetPriority();

        //  ---------------------
        //  SetNotifyFlags method
        //  ---------------------

        [PreserveSig]
        int SetNotifyFlags(BackgroundCopyJobNotifications notifyFlags);

        //  ---------------------
        //  GetNotifyFlags method
        //  ---------------------

        [PreserveSig]
        uint GetNotifyFlags(out BackgroundCopyJobNotifications notifyFlags);

        //  -------------------------
        //  SetNotifyInterface method
        //  -------------------------

        [PreserveSig]
        uint SetNotifyInterface([MarshalAs(UnmanagedType.Interface)] IBackgroundCopyCallback? callback);

        //  -------------------------
        //  GetNotifyInterface method
        //  -------------------------

        [return: MarshalAs(UnmanagedType.Interface)]
        IBackgroundCopyCallback GetNotifyInterface();

        //  ---------------------------
        //  SetMinimumRetryDelay method
        //  ---------------------------

        void SetMinimumRetryDelay(int seconds);

        //  ---------------------------
        //  GetMinimumRetryDelay method
        //  ---------------------------

        int GetMinimumRetryDelay();

        //  ---------------------------
        //  SetNoProgressTimeout method
        //  ---------------------------

        void SetNoProgressTimeout(int seconds);

        //  ---------------------------
        //  GetNoProgressTimeout method
        //  ---------------------------

        int GetNoProgressTimeout();

        //  --------------------
        //  GetErrorCount method
        //  --------------------

        int GetErrorCount();

        //  -----------------------
        //  SetProxySettings method
        //  -----------------------

        void SetProxySettings(
            BackgroundCopyJobProxyUsage ProxyUsage,
            [MarshalAs(UnmanagedType.LPWStr)] string ProxyList,
            [MarshalAs(UnmanagedType.LPWStr)] string ProxyBypassList);

        //  -----------------------
        //  GetProxySettings method
        //  -----------------------

        void GetProxySettings(
            out BackgroundCopyJobProxyUsage pProxyUsage,
            [MarshalAs(UnmanagedType.LPWStr)] out string pProxyList,
            [MarshalAs(UnmanagedType.LPWStr)] out string pProxyBypassList);

        //  --------------------
        //  TakeOwnership method
        //  --------------------

        void TakeOwnership();
    }
}

// eof "IBackgroundCopyJob.cs"

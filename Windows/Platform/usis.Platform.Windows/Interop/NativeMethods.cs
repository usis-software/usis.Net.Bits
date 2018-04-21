//
//  @(#) NativeMethods.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2008-2016 usis GmbH. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace usis.Platform.Interop
{
    //  -------------------
    //  NativeMethods class
    //  -------------------

    internal static partial class NativeMethods
    {
        #region ole32.dll

        [DllImport("ole32.dll")]
        internal static extern int PropStgNameToFmtId(
            [MarshalAs(UnmanagedType.LPWStr)]
            string name,
            ref Guid formatId);

        [DllImport("ole32.dll")]
        internal static extern int PropVariantClear(
            ref PROPVARIANT pvar);

        [DllImport("ole32.dll")]
        internal static extern int StgCreateStorageEx(
            [MarshalAs(UnmanagedType.LPWStr)]
            string name,
            int mode,
            uint stgfmt,
            uint grfAttrs,
            [MarshalAs(UnmanagedType.LPStruct)]
            STGOPTIONS options,
            IntPtr reserved2,
            [In] ref Guid riid,
            [MarshalAs(UnmanagedType.IUnknown)]
            out object ppObjectOpen);

        [DllImport("ole32.dll")]
        internal static extern int StgOpenStorageEx(
            [MarshalAs(UnmanagedType.LPWStr)]
            string name,
            int mode,
            uint stgfmt,
            uint grfAttrs,
            [MarshalAs(UnmanagedType.LPStruct)]
            STGOPTIONS options,
            IntPtr reserved2,
            [In] ref Guid riid,
            [MarshalAs(UnmanagedType.IUnknown)]
            out object ppObjectOpen);

        [DllImport("ole32.dll")]
        public static extern int StgIsStorageFile(
            [MarshalAs(UnmanagedType.LPWStr)]
            string name);

        #region CreateStreamOnHGlobal method

        //[DllImport("ole32.dll")]
        //public static extern int CreateStreamOnHGlobal(
        //    IntPtr hGlobal,
        //    [MarshalAs(UnmanagedType.Bool)]
        //    bool deleteOnRelease,
        //    [MarshalAs(UnmanagedType.Interface)]
        //    out IStream stream);

        #endregion CreateStreamOnHGlobal method

        #region ReleaseStgMedium method

        //[DllImport("ole32.dll")]
        //public static extern void ReleaseStgMedium(ref STGMEDIUM pmedium);

        #endregion ReleaseStgMedium method

        #endregion ole32.dll
    }
}

// eof "NativeMethods.cs"

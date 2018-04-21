//
//  @(#) NativeMethods.Ole32.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2013
//  Author:     Udo Schäfer
//
//  Copyright (c) 2008-2014 usis GmbH. All rights reserved.

using System.Runtime.InteropServices;

namespace usis.Platform.Interop
{
    //  +------------------------------------------------------------------+
    //  |                                                                  |
    //  |   NativeMethods class                                            |
    //  |                                                                  |
    //  +------------------------------------------------------------------+

    internal static partial class NativeMethods
    {
        //  +------------------------------------------------------------------+
        //  |                                                                  |
        //  |   Ole32 class                                                    |
        //  |                                                                  |
        //  +------------------------------------------------------------------+

        public static partial class Ole32
        {
            #region CreateStreamOnHGlobal method

            //[DllImport("ole32.dll")]
            //public static extern int CreateStreamOnHGlobal(
            //    IntPtr hGlobal,
            //    [MarshalAs(UnmanagedType.Bool)]
            //    bool deleteOnRelease,
            //    [MarshalAs(UnmanagedType.Interface)]
            //    out IStream stream);

            #endregion // CreateStreamOnHGlobal method

            #region ReleaseStgMedium method

            //  +------------------------------------------------------------------+
            //  |                                                                  |
            //  |   ReleaseStgMedium method                                        |
            //  |                                                                  |
            //  +------------------------------------------------------------------+

			//[DllImport("ole32.dll")]
			//public static extern void ReleaseStgMedium(ref STGMEDIUM pmedium);

            #endregion // ReleaseStgMedium method

            #region PropVariantClear method

            //  +------------------------------------------------------------------+
            //  |                                                                  |
            //  |   PropVariantClear method                                        |
            //  |                                                                  |
            //  +------------------------------------------------------------------+

            [DllImport("ole32.dll")]
            internal static extern int PropVariantClear(ref PROPVARIANT pvar);

            #endregion // PropVariantClear method

        } // Ole32 class

    } // NativeMethods class

} // usis.Platform.Interop namespace

// eof "NativeMethods.Ole32.cs"

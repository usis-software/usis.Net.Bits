//
//  @(#) NativeMethods.Ole32.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2013
//  Author:     Udo Schäfer
//
//  Copyright (c) 2008-2014 usis GmbH. All rights reserved.

using System;
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
            #region StgIsStorageFile method

            //  +------------------------------------------------------------------+
            //  |                                                                  |
            //  |   StgIsStorageFile method                                        |
            //  |                                                                  |
            //  +------------------------------------------------------------------+

            [DllImport("ole32.dll")]
            public static extern int StgIsStorageFile(
                [MarshalAs(UnmanagedType.LPWStr)]
                string name);

            #endregion // StgIsStorageFile method

            #region PropStgNameToFmtId method

            //  +------------------------------------------------------------------+
            //  |                                                                  |
            //  |   PropStgNameToFmtId method                                      |
            //  |                                                                  |
            //  +------------------------------------------------------------------+

            [DllImport("ole32.dll")]
            internal static extern int PropStgNameToFmtId(
                [MarshalAs(UnmanagedType.LPWStr)]
                string name,
                ref Guid formatId);

            #endregion // PropStgNameToFmtId method

        } // Ole32 class

    } // NativeMethods class

} // usis.Platform.Interop namespace

// eof "NativeMethods.Ole32.cs"

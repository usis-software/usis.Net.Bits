//
//  @(#) Extensions.cs
//
//  Project:    usis.Net.Bits
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017,2018 usis GmbH. All rights reserved.

using System;
using System.Runtime.InteropServices.ComTypes;

namespace usis.Net.Bits.Interop
{
    //  ----------------
    //  Extensions class
    //  ----------------

    internal static class Extensions
    {
        //  -----------------
        //  ToDateTime method
        //  -----------------

        public static DateTime ToDateTime(this FILETIME fileTime) => DateTimeFromFileTime(fileTime.dwHighDateTime, fileTime.dwLowDateTime);

        #region private methods

        //  ---------------------------
        //  DateTimeFromFileTime method
        //  ---------------------------

        private static DateTime DateTimeFromFileTime(int high, int low)
        {
            if (high == 0 && low == 0) return DateTime.MinValue;

            long fileTime = ((long)high << 32) + (uint)low;
            return DateTime.FromFileTime(fileTime);
        }

        #endregion private methods
    }
}

// eof "Extensions.cs"

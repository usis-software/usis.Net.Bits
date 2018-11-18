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
        #region FILETIME extension methods

        //  -----------------
        //  ToDateTime method
        //  -----------------

        internal static DateTime ToDateTime(this FILETIME fileTime) => DateTimeFromFileTime(fileTime.dwHighDateTime, fileTime.dwLowDateTime);

        #endregion FILETIME extension methods

        #region private methods

        //  ---------------------------
        //  DateTimeFromFileTime method
        //  ---------------------------

        private static DateTime DateTimeFromFileTime(int high, int low)
        {
            if (high == 0 && low == 0) return DateTime.MinValue;

            var fileTime = ((long)high << 32) + (uint)low;
            return DateTime.FromFileTime(fileTime);
        }

        #endregion private methods

        #region QueryInterface method

        //  ---------------------
        //  QueryInterface method
        //  ---------------------

        internal static TInterface QueryInterface<TInterface>(object i) where TInterface : class => (i as TInterface) ?? throw new NotSupportedException(Strings.NotSupported);

        #endregion QueryInterface method
    }
}

// eof "Extensions.cs"

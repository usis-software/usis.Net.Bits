//
//  @(#) Extensions.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2008-2016 usis GmbH. All rights reserved.

using System;
using System.Runtime.InteropServices.ComTypes;

namespace usis.Platform.StructuredStorage
{
    //  ----------------
    //  Extensions class
    //  ----------------

    internal static class Extensions
    {
        //  -----------------
        //  ToFILETIME method
        //  -----------------

        public static FILETIME ToFILETIME(this DateTime dateTime)
        {
            FILETIME fileTime = new FILETIME();
            long ticks = dateTime.ToFileTime();
            fileTime.dwHighDateTime = (int)(ticks >> 32);
            fileTime.dwLowDateTime = (int)(ticks & uint.MaxValue);
            return fileTime;
        }

        //  -----------------
        //  ToDateTime method
        //  -----------------

        public static DateTime ToDateTime(this FILETIME fileTime)
        {
            return DateTimeFromFileTime(fileTime.dwHighDateTime, fileTime.dwLowDateTime);
        }

        #region private methods

        //  ---------------------------
        //  DateTimeFromFileTime method
        //  ---------------------------

        private static DateTime DateTimeFromFileTime(int high, int low)
        {
            long fileTime = ((long)high << 32) + (uint)low;
            return DateTime.FromFileTime(fileTime);
        }

        #endregion private methods
    }
}

// eof "Extensions.cs"

//
//  @(#) StringExtensions.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015-2017 usis GmbH. All rights reserved.

using System;

namespace usis.Platform
{
    //  ----------------------
    //  StringExtensions class
    //  ----------------------

    internal static class StringExtensions
    {
        //  -------------
        //  CutOut method
        //  -------------

        internal static string CutOut(this string s, int startIndex, int length)
        {
            if (startIndex >= s.Length) return string.Empty;
            int l = length;
            if (startIndex + l > s.Length) l = s.Length - startIndex;
            return s.Substring(startIndex, l);
        }

        //  ------------
        //  Right method
        //  ------------

        internal static string Right(this string s, int length)
        {
            var p = Math.Max(s.Length - length, 0);
            var l = Math.Min(s.Length, length);
            return s.Substring(p, l);
        }
    }
}

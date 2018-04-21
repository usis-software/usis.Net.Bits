//
//  @(#) StringExtension.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015-2017 usis GmbH. All rights reserved.

using System;

namespace usis.Platform
{
    internal static class CharConstants
    {
        public const char Space = ' ';
    }

    //  ---------------------
    //  StringExtension class
    //  ---------------------

    internal static class StringExtension
    {
        private const char SPACE = ' ';

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

        //internal static string TrimEndSpaces(this string s)
        //{
        //    return s.TrimEnd(SPACE);
        //}
        //internal static string Left(this string s, int length)
        //{
        //    return s.Substring(0, Math.Min(length, s.Length));
        //}
        //internal static string Replicate(this string s, int count)
        //{
        //    if (count == 1) return s;
        //    else if (count > 1)
        //    {
        //        var sb = new StringBuilder();
        //        for (int i = 0; i < count; i++)
        //        {
        //            sb.Append(s);
        //        }
        //        return sb.ToString();
        //    }
        //    else throw new ArgumentOutOfRangeException(nameof(count));
        //}
    }
}

// eof "StringExtension.cs"

//
//  @(#) HexString.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015-2017 usis GmbH. All rights reserved.

using System;
using System.Linq;

namespace usis.Platform
{
    //  ---------------
    //  HexString class
    //  ---------------    

    /// <summary>
    /// Converts bytes to a hexadecimal string representation and vice versa.
    /// </summary>
    
    public static class HexString
    {
        //  ----------------
        //  FromBytes method
        //  ----------------

        /// <summary>
        /// Converts the numeric value of each element of a specified array of bytes
        /// to its equivalent hexadecimal string representation.
        /// </summary>
        /// <param name="value">
        /// An array of bytes.
        /// </param>
        /// <returns>
        /// A string of hexadecimal characters that represents the bytes in value;
        /// for example, "4D5A9000".
        /// </returns>

        public static string FromBytes(byte[] value) { return BitConverter.ToString(value).Replace("-", string.Empty); }

        //  --------------
        //  ToBytes method
        //  --------------

        /// <summary>
        /// Converts a specified hexadecimal string representation to its
        /// equivalent array of bytes.
        /// </summary>
        /// <param name="hex">
        /// A string with hexadecimal representation of data.
        /// </param>
        /// <returns>
        /// An array of bytes.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The <b>hex</b> parameter is a null reference (<c>Nothing</c> in Visual Basic).
        /// </exception>
        /// <exception cref="FormatException">
        /// The <b>hex</b> is not a valid hexadecimal string representation.
        /// </exception>

        public static byte[] ToBytes(string hex)
        {
            if (hex == null) throw new ArgumentNullException(nameof(hex));

            int length = hex.Length;
            if (length % 2 > 0) throw new FormatException();

            return Enumerable.Range(0, length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}

// eof "HexString.cs"

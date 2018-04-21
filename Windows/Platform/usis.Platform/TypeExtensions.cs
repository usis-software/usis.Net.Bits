//
//  @(#) TypeExtensions.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017,2018 usis GmbH. All rights reserved.

using System;

namespace usis.Platform
{
    //  --------------------
    //  TypeExtensions class
    //  --------------------

    /// <summary>
    /// Provides extension methods to the <see cref="Enum"/> class.
    /// </summary>

    public static class TypeExtensions
    {
        //  --------------
        //  IsOneOf method
        //  --------------

        /// <summary>
        /// Determines whether a value is equal to one of the specified values.
        /// </summary>
        /// <typeparam name="T">The type of the values.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="values">The values.</param>
        /// <returns>
        ///   <c>true</c> if <c>value</c> is equal to one of the specified <c>values</c>; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="values" /> is a <c>null</c> reference.</exception>

        public static bool IsOneOf<T>(this T value, params T[] values)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));

            foreach (var item in values)
            {
                if (Equals(item, value)) return true;
            }
            return false;
        }
    }
}

// eof "TypeExtensions.cs"

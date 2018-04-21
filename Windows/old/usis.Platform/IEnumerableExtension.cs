//
//  @(#) IEnumerableExtension.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015,2016 usis GmbH. All rights reserved.

using System.Collections.Generic;

namespace usis.Platform
{
    //  --------------------------
    //  IEnumerableExtension class
    //  --------------------------    
    /// <summary>
    /// Provides extension to classes in the context of the
    /// <see cref="IEnumerable{T}"/> interface.
    /// </summary>

    public static class IEnumerableExtension
    {
        /// <summary>
        /// Creates an enumeration for the specified object.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the object.
        /// </typeparam>
        /// <param name="element">
        /// The object to "iterate".
        /// </param>
        /// <returns>
        /// An enumeration with one element.
        /// </returns>

        public static IEnumerable<T> Yield<T>(this T element)
        {
            yield return element;
        }

        /// <summary>
        /// Creates an enumeration for the specified objects.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the objects.
        /// </typeparam>
        /// <param name="elements">
        /// The objects to "iterate".
        /// </param>
        /// <returns>
        /// An enumeration with the corresponding elements.
        /// </returns>

        public static IEnumerable<T> Yield<T>(params T[] elements)
        {
            foreach (var element in elements)
            {
                yield return element;
            }
        }
    }
}

// eof "IEnumerableExtension.cs"

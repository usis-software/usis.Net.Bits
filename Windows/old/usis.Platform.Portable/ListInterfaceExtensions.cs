//
//  @(#) ListInterfaceExtensions.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;

namespace usis.Platform.Portable
{
    //  -----------------------------
    //  ListInterfaceExtensions class
    //  -----------------------------

    /// <summary>
    /// Provides extension methods to the <see cref="IList{T}"/> interface.
    /// </summary>

    public static class ListInterfaceExtensions
    {
        //  --------------
        //  Replace method
        //  --------------

        /// <summary>
        /// Replaces the items in a list with the items of specified collection.
        /// </summary>
        /// <typeparam name="T">THe type of the items.</typeparam>
        /// <param name="list">The list with the old items.</param>
        /// <param name="collection">The collection with the new items.</param>
        /// <exception cref="ArgumentNullException"></exception>

        public static void Replace<T>(this IList<T> list, IEnumerable<T> collection)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));

            var oldCount = list.Count;
            var newCount = collection == null ? 0 : collection.Count();
            var maxCount = Math.Max(oldCount, newCount);

            for (int i = 0; i < maxCount; i++)
            {
                if (i < newCount && i < oldCount)
                {
                    list[i] = collection.ElementAt(i);
                }
                else if (i < oldCount)
                {
                    list.RemoveAt(newCount);
                }
                else if (i < newCount)
                {
                    list.Add(collection.ElementAt(i));
                }
            }
        }

        //public static void ReplaceRange<T>(this List<T> list, IEnumerable<T> collection)
        //{
        //    if (list == null) throw new ArgumentNullException(nameof(list));
        //    list.Clear();
        //    list.AddRange(collection);
        //}
    }
}

// eof "ListInterfaceExtensions.cs"

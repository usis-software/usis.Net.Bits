//
//  @(#) CollectionInterfaceExtensions.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015-2017 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;

namespace usis.Platform.Portable
{
    //  -----------------------------------
    //  CollectionInterfaceExtensions class
    //  -----------------------------------

    /// <summary>
    /// Provides extension methods for the <see cref="ICollection{T}"/> interface.
    /// </summary>

    [Obsolete("Use type from usis.Platform namespace instead.")]
    public static class CollectionInterfaceExtensions
    {
        //  ----------
        //  Add method
        //  ----------

        /// <summary>
        /// Adds the specified items to the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the collection items.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="items">The items to add.</param>

        internal static void Add<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items) { collection.Add(item); }
        }

        //  --------------
        //  Replace method
        //  --------------

        /// <summary>
        /// Clears the collection and then adds the specified items to it.
        /// </summary>
        /// <typeparam name="T">The type of the collection items.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="items">The items to add.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection"/> is a null reference.
        /// </exception>

        public static void Replace<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            collection.Clear();
            collection.Add(items);
        }
    }
}

// eof "CollectionInterfaceExtensions.cs"

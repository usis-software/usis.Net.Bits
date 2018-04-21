//
//  @(#) MmcListViewColumnCollectionExtensions.cs
//
//  Project:    usis.ManagementConsole
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2016 usis GmbH. All rights reserved.

using System;
using Microsoft.ManagementConsole;

namespace usis.ManagementConsole
{
    //  -------------------------------------------
    //  MmcListViewColumnCollectionExtensions class
    //  -------------------------------------------

    /// <summary>
    /// Provides extension methods for the <see cref="MmcListViewColumnCollection"/> class.
    /// </summary>

    public static class MmcListViewColumnCollectionExtensions
    {
        //  ----------------
        //  AddColumn method
        //  ----------------

        /// <summary>
        /// Adds a column with the specified title and width to the collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="title">The title of the column.</param>
        /// <param name="width">The width of the column.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection"/> is a null reference.
        /// </exception>

        [Obsolete("Use Add method instead.")]
        public static void AddColumn(this MmcListViewColumnCollection collection, string title, int width)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            collection.Add(new MmcListViewColumn(title, width));
        }

        //  ----------
        //  Add method
        //  ----------

        /// <summary>
        /// Adds a column with the specified title and width to the collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="title">The title of the column.</param>
        /// <param name="width">The width of the column.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection"/> is a null reference.
        /// </exception>

        public static void Add(this MmcListViewColumnCollection collection, string title, int width)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            collection.Add(new MmcListViewColumn(title, width));
        }

        //public static void Change(this MmcListViewColumnCollection collection, string title, int width)
        //{
        //}
    }
}

// eof "MmcListViewColumnCollectionExtensions"

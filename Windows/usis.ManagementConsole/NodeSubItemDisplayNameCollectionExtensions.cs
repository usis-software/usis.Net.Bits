//
//  @(#) NodeSubItemDisplayNameCollectionExtensions.cs
//
//  Project:    usis.ManagementConsole
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

using Microsoft.ManagementConsole;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace usis.ManagementConsole
{
    //  ------------------------------------------------
    //  NodeSubItemDisplayNameCollectionExtensions class
    //  ------------------------------------------------

    /// <summary>
    /// Provides extension methods for the <see cref="NodeSubItemDisplayNameCollection"/> class.
    /// </summary>

    [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "SubItem")]
    public static class NodeSubItemDisplayNameCollectionExtensions
    {
        #region Add method

        //  ----------
        //  Add method
        //  ----------

        /// <summary>
        /// Adds the specified node sub-item display names to the collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="names">The node sub-item display names.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection"/> is a null reference (<c>Nothing</c> in Visual Basic).
        /// or
        /// <paramref name="names"/> is a null reference (<c>Nothing</c> in Visual Basic).
        /// </exception>

        public static void Add(this NodeSubItemDisplayNameCollection collection, params string[] names)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (names == null) throw new ArgumentNullException(nameof(names));
            if (names.Length > 0) collection.AddRange(names);
        }

        #endregion Add method

        #region AddString method

        //  ----------------
        //  AddString method
        //  ----------------

        /// <summary>
        /// Adds a node sub-item display name to the collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="displayName">The display name.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection" />  is a null reference (<c>Nothing</c> in Visual Basic).
        /// </exception>
        /// <remarks>
        /// If <paramref name="displayName" /> is a null reference an empty string is added.
        /// </remarks>

        public static void AddString(this NodeSubItemDisplayNameCollection collection, string displayName)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            collection.Add(displayName ?? string.Empty);
        }

        #endregion AddString method

        #region AddFormat method

        //  ----------------
        //  AddFormat method
        //  ----------------

        /// <summary>
        /// Adds a node sub-item display name from the specified formatted string to the collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments.</param>

        public static void AddFormat(this NodeSubItemDisplayNameCollection collection, string format, params object[] args)
        {
            collection.AddFormat(CultureInfo.CurrentCulture, format, args);
        }

        /// <summary>
        /// Adds a node sub-item display name from the specified formatted string to the collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="cultureInfo">The culture information.</param>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection" />  is a null reference (<c>Nothing</c> in Visual Basic).
        /// </exception>

        public static void AddFormat(this NodeSubItemDisplayNameCollection collection,
            CultureInfo cultureInfo, string format, params object[] args)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            collection.Add(string.Format(cultureInfo, format, args));
        }

        #endregion AddFormat method
    }
}

// eof "NodeSubItemDisplayNameCollectionExtensions.cs"

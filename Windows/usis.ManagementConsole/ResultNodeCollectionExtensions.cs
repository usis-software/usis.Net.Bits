//
//  @(#) ResultNodeCollectionExtensions.cs
//
//  Project:    usis.ManagementConsole
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ManagementConsole;

namespace usis.ManagementConsole
{
    //  ------------------------------------
    //  ResultNodeCollectionExtensions class
    //  ------------------------------------

    /// <summary>
    /// Provides extension methods for the <see cref="ResultNodeCollection"/> class.
    /// </summary>

    public static class ResultNodeCollectionExtensions
    {
        //  ----------
        //  Add method
        //  ----------

        /// <summary>
        /// Adds the specified nodes to the collection.
        /// </summary>
        /// <param name="collection">The colletion.</param>
        /// <param name="nodes">The nodes.</param>
        /// <exception cref="ArgumentNullException"><c>collection</c> is a null reference.</exception>

        public static void Add(this ResultNodeCollection collection, params ResultNode[] nodes)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            collection.AddRange(nodes);
        }

        //  --------------
        //  Replace method
        //  --------------

        /// <summary>
        /// Clear the collection and then adds the specified nodes.
        /// </summary>
        /// <param name="collection">The colletion.</param>
        /// <param name="nodes">The nodes.</param>
        /// <exception cref="ArgumentNullException"><c>collection</c> is a null reference.</exception>

        public static void Replace(this ResultNodeCollection collection, params ResultNode[] nodes)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            collection.Clear();
            collection.AddRange(nodes);
        }

        /// <summary>
        /// Clear the collection and then adds the specified nodes.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="nodes">The nodes.</param>

        public static void Replace(this ResultNodeCollection collection, IEnumerable<ResultNode> nodes)
        {
            collection.Replace(nodes.ToArray());
        }
    }
}

// eof "ResultNodeCollectionExtensions.cs"

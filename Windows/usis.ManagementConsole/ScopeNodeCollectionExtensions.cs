//
//  @(#) ScopeNodeCollectionExtensions.cs
//
//  Project:    usis.ManagementConsole
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using Microsoft.ManagementConsole;
using System;

namespace usis.ManagementConsole
{
    //  -----------------------------------
    //  ScopeNodeCollectionExtensions class
    //  -----------------------------------

    /// <summary>
    /// Provides extension methods to the <see cref="ScopeNodeCollection"/> class.
    /// </summary>

    public static class ScopeNodeCollectionExtensions
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

        public static void Add(this ScopeNodeCollection collection, params Microsoft.ManagementConsole.ScopeNode[] nodes)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            collection.AddRange(nodes);
        }
    }
}

// eof "ScopeNodeCollectionExtensions.cs"

//
//  @(#) ListInterfaceExtension.cs
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
    //  ----------------------------
    //  ListInterfaceExtension class
    //  ----------------------------

    internal static class ListInterfaceExtension
    {
        //  --------------
        //  Replace method
        //  --------------

        public static void Replace<T>(this IList<T> collection, IEnumerable<T> newCollection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            var oldCount = collection.Count;
            var newCount = newCollection == null ? 0 : newCollection.Count();
            var maxCount = Math.Max(oldCount, newCount);

            for (int i = 0; i < maxCount; i++)
            {
                if (i < newCount && i < oldCount)
                {
                    collection[i] = newCollection.ElementAt(i);
                }
                else if (i < oldCount)
                {
                    collection.RemoveAt(newCount);
                }
                else if (i < newCount)
                {
                    collection.Add(newCollection.ElementAt(i));
                }
            }
        }
    }
}

// eof "ListInterfaceExtension.cs"

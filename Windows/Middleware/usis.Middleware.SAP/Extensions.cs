//
//  @(#) Extensions.cs
//
//  Project:    usis Middleware
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015-2017 usis GmbH. All rights reserved.

using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace usis.Middleware.SAP
{
    //  ----------------
    //  Extensions class
    //  ----------------

    /// <summary>
    /// Provides extension methods.
    /// </summary>

    internal static class Extensions
    {
        #region IEnumerable<T>.Serialize method

        //  ----------------
        //  Serialize method
        //  ----------------

        internal static void Serialize<T>(this IEnumerable<T> items, XmlWriter writer)
        {
            foreach (var item in items)
            {
                var serializer = new XmlSerializer(item.GetType());
                serializer.Serialize(writer, item);
            }
        }

        #endregion IEnumerable<T>.Serialize method
    }
}

// eof "Extensions.cs"

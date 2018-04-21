//
//  @(#) EnumerableInterfaceExtensions.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;

namespace usis.Platform
{
    //  -----------------------------------
    //  EnumerableInterfaceExtensions class
    //  -----------------------------------

    /// <summary>
    /// Provides extension methods to the <see cref="IEnumerable{T}"/> interface.
    /// </summary>

    public static class EnumerableInterfaceExtensions
    {
        //  --------------
        //  UseEach method
        //  --------------

        internal static void UseEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (var item in enumeration)
            {
                try { action.Invoke(item); }
                finally { if (item is IDisposable disposable) disposable.Dispose(); }
            }
        }
    }
}

// eof "EnumerableInterfaceExtensions.cs"

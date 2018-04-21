using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace usis.Net.Bits.Administration
{
    internal static class Enumerable
    {
        //  --------------
        //  ForEach method
        //  --------------

        internal static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (var item in enumeration) action.Invoke(item);
        }

        //  ------------------
        //  DisposeEach method
        //  ------------------

        internal static void DisposeEach(this IEnumerable enumeration)
        {
            enumeration.OfType<IDisposable>().ForEach(i => i.Dispose());
        }
    }
}

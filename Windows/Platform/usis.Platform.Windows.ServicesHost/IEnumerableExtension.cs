using System.Collections.Generic;

namespace usis.Platform
{
    public static class IEnumerableExtension
    {
        public static IEnumerable<T> Yield<T>(this T element)
        {
            yield return element;
        }
    }
}

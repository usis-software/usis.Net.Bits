using System.Diagnostics;
using System.Globalization;

namespace usis.Platform.Portable
{
    //  ---------
    //  Log class
    //  ---------

    /// <summary>
    /// Provides a set of methods to log the execution of your code.
    /// </summary>

    internal static class Log
    {
        //  ------------
        //  Print method
        //  ------------

        /// <summary>
        /// Writes a formatted string followed by a line terminator to the log.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero of more objects to format.</param>

        public static void Print(string format, params object[] args)
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, format, args));
        }
    }
}

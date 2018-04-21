using System;
using usis.Net.Bits;

namespace BitsTest
{
    internal static class Sample
    {
        internal static void Main()
        {
            using (var manager = BackgroundCopyManager.Connect())
            {
                Console.WriteLine("BITS Version {0}", manager.Version);
                Console.WriteLine();

                // list all BITS jobs of the current user
                foreach (var job in manager.EnumerateJobs(false))
                {
                    Console.WriteLine("{0} '{1}' {2}", job.Id.ToString("B"), job.DisplayName, job.State.ToString());
                    job.Dispose();
                }
            }
            Console.WriteLine();
            if (System.Diagnostics.Debugger.IsAttached) Console.ReadKey(true);
        }
    }
}

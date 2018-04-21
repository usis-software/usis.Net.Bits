using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using usis.Net.Bits;

//#pragma warning disable CS0618 // Type or member is obsolete

namespace BitsTest
{
    internal static class Program
    {
        [SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object)")]
        [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Console.WriteLine(System.String)")]
        internal static void Main()
        {
            using (var manager = BackgroundCopyManager.Connect())
            {
                Console.WriteLine($"BITS Version: {manager.Version}");
                Console.WriteLine();

                ListJobs(manager);

                using (var job = manager.GetJob(new Guid("{4a69f431-ed76-44aa-aeab-8f28ef624984}"), false))
                {
                }
                PressAnyKey();

                using (var job = manager.CreateJob("Test", BackgroundCopyJobType.UploadReply))
                {
                    job.MinimumRetryDelay = 30;
                    job.NoProgressTimeout = 60;
                    job.AddFiles(
                        //new BackgroundCopyFileInfo { RemoteName = "http://localhost/bits", LocalName = @"C:\tmp\test1.mp4" },
                        //new BackgroundCopyFileInfo { RemoteName = "http://localhost/bits", LocalName = @"C:\tmp\test2.mp4" },
                        new BackgroundCopyFileInfo { RemoteName = "http://localhost/bits", LocalName = @"C:\tmp\test.mp4" });
                    job.NotifyCommandLine = new BackgroundCopyNotifyCommandLine(@"C:\Windows\System32\notepad.exe", @"C:\Windows\System32\notepad.exe test.txt");
                    var cmdLine = job.NotifyCommandLine;
                    Console.WriteLine(cmdLine);
                    //job.ReplyFileName = @"C:\tmp\Reply.txt";
                    //var progress = job.RetrieveReplyProgress();
                    var fileName = job.ReplyFileName;
                }
                PressAnyKey();
            }
        }

        [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Console.WriteLine(System.String)")]
        internal static void ListJobs(BackgroundCopyManager manager)
        {
            Console.WriteLine("BITS jobs:");
            manager.EnumerateJobs(false).UseEach((job) =>
            {
                Console.WriteLine(job);
            });
        }

        internal static void UseEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (var item in enumeration)
            {
                try { action.Invoke(item); }
                finally { if (item is IDisposable disposable) disposable.Dispose(); }
            }
        }

        #region PressAnyKey method

        [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Console.Write(System.String)")]
        private static void PressAnyKey()
        {
            Console.WriteLine();
            Console.Write("Press any key to continue ... ");
            Console.ReadKey(true);
            Console.WriteLine();
            Console.WriteLine();
        }

        #endregion PressAnyKey method
    }
}

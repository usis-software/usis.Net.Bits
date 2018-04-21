using usis.Net.Bits;

namespace BitsTest
{
    internal static class Sample
    {
        internal static void Main()
        {
            using (var manager = BackgroundCopyManager.Connect())
            {
                using (var job = manager.CreateJob("Test", BackgroundCopyJobType.Download))
                {
                    job.AddFiles(
                        new BackgroundCopyFileInfo { RemoteName = "http://localhost/bits1", LocalName = @"C:\tmp\test1.dat" },
                        new BackgroundCopyFileInfo { RemoteName = "http://localhost/bits2", LocalName = @"C:\tmp\test2.dat" },
                        new BackgroundCopyFileInfo { RemoteName = "http://localhost/bits3", LocalName = @"C:\tmp\test3.dat" });
                    job.Resume();
                }
            }
        }
    }
}

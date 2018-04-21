using System;

namespace IZYTRON.IQ
{
    internal static class Program
    {
        internal static void Main(string[] args)
        {
            Console.WriteLine();
            Console.WriteLine("IZYTRON.IQ Test Client");
            Console.WriteLine();

            var databaseId = new Guid("cb2f651b-11db-4dc2-8ebc-8e927b3d6c05");

            using (var client = SyncServiceClient.ConnectToServer(new Uri("http://usis-svr07"), databaseId))
            {
                var state = client.State;
                Console.WriteLine("Status: {0}", state);

                client.Monitor.ProgressChanged += (sender, e) => { Console.WriteLine(e.Progress); };

                if (state != SyncState.ConnectionFailed)
                {
                    if (state == SyncState.ReadyToUpload)
                    {
                        PressAnyKeyTo("start synchronization");
                        client.StartSynchronization();
                        Console.WriteLine("Synchronization started.");
                    }
                    client.ResumeWait();
                }
                PressAnyKeyTo("end the test client");
            }

            #region BITS test

            //using (var bits = BackgroundCopyManager.Connect())
            //{
            //    using (var job = bits.EnumerateJobs(false).FirstOrDefault())
            //    {
            //        if (job != null)
            //        {
            //            var monitor = new ProgressMonitor();

            //            job.Modified += (sender, e) => { var p = job.RetrieveProgress(); monitor.UpdateProgress(0 , p.BytesTotal, p.BytesTransferred); };

            //            monitor.ProgressChanged += (sender, e) =>
            //            {
            //                Console.WriteLine(e.Progress);
            //            };

            //            PressAnyKey();
            //        }
            //    }
            //}

            #endregion BITS test
        }

        #region PressAnyKey method

        private static void PressAnyKeyTo(string description)
        {
            Console.WriteLine();
            Console.WriteLine(" [ Press any key to {0} ]", description);
            Console.WriteLine();
            Console.ReadKey(true);
        }

        #endregion PressAnyKey method
    }
}

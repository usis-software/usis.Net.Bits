using System;
using System.Security.Cryptography.X509Certificates;

namespace Playground
{
    internal class Certificates
    {
        public static void Main()
        {
            //var data = File.ReadAllBytes("de.usis.Demo.Push.Dev.p12");
            //X509Certificate2 certificate = new X509Certificate2(data, "master");

            //var store = new X509Store(StoreName.TrustedPublisher, StoreLocation.LocalMachine);
            using (var store = new X509Store(StoreName.Root))
            {
                // usisSolutionSvc\Personal
                store.Open(OpenFlags.OpenExistingOnly);
                foreach (var item in store.Certificates)
                {
                    Console.WriteLine(item.FriendlyName);
                }
            }
            Console.WriteLine();
            Console.Write("Press any key to continue ... ");
            Console.ReadKey(true);
        }
    }
}

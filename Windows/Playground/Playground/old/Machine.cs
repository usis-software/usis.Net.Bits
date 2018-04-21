using System;
using System.ServiceProcess;
using System.Web;

namespace Playground
{
    internal static class Machine
    {
        internal static void Main()
        {
            //foreach (var service in ServiceController.GetServices())
            //{
            //    Console.WriteLine(service.DisplayName);
            //}
            Console.WriteLine(Environment.MachineName);
            //Console.WriteLine(HttpContext.Current.Server.MachineName);
            Console.WriteLine(System.Net.Dns.GetHostName());
            usis.Platform.ConsoleExtension.PressAnyKey();
        }
    }
}

namespace usis.Platform
{
    internal static class ConsoleExtension
    {
        internal static void PressAnyKey()
        {
            Console.WriteLine();
            Console.Write("Press any key to continue ... ");
            Console.ReadKey(true);
            Console.WriteLine();
        }
    }
}
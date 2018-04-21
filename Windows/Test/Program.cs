using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var engine = new usis.PushNotification.JobEngine();
            engine.Start(() => TestAction());

            Console.WriteLine("Press any key to continue ... ");
            Console.ReadKey(true);
        }

        static void TestAction()
        {
            Console.WriteLine("Starting job...");
            System.Threading.Thread.Sleep(5000);
            Console.WriteLine("Job done.");
        }
    }
}

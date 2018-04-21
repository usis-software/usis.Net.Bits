using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playground
{
    internal static class RFC
    {
        internal static void Main()
        {
            var destination = RfcDestinationManager.GetDestination("Gateway");
            var function = destination.Repository.CreateFunction("BAPI_USER_GETLIST");
            function.Invoke(destination);
            var table = function.GetTable("USERLIST");
            foreach (var user in table)
            {
                var field = user["USERNAME"];
                Console.WriteLine(field.GetString());
            }
            Console.WriteLine();

            //Type[] handlers = new Type[1] { typeof(IDocServer) };
            //RfcServer server = RfcServerManager.GetServer("adis", handlers);
            //server.Start();

            destination = RfcDestinationManager.GetDestination("Local");
            function = destination.Repository.CreateFunction("Z_TEST_FM");

            PressAnyKey();
        }
        private static void PressAnyKey()
        {
            Console.WriteLine();
            Console.Write("Press any key to continue ... ");
            Console.ReadKey(true);
            Console.WriteLine();
        }
    }

    //public class IDocServer
    //{
    //    [RfcServerFunction(Name = "IDOC_INBOUND_ASYNCHRONOUS")]
    //    public static void InboundAsynchronous(
    //        RfcServerContext context,
    //        IRfcFunction function)
    //    {
    //        if (context == null) throw new ArgumentNullException(nameof(context));
    //        if (function == null) throw new ArgumentNullException(nameof(function));
    //    }
    //}
}

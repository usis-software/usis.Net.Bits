using System;
//using System.Collections.Generic;
//using usis.Middleware.SAP.Bapi;

namespace RfcTest
{
    internal static class Program
    {
        //private const string destinationName = "audius.SAP.Gateway";
        private const string destinationName = "SAP.RAC.E16";

        internal static void Main()
        {
            var destination = SAP.Middleware.Connector.RfcDestinationManager.GetDestination(destinationName);
            Console.Write($"Connecting to {destination.Name} ... ");
            destination.Ping();
            Console.WriteLine();

            #region old fashioned

            //var function = destination.Repository.CreateFunction("BAPI_USER_GETLIST");
            //function.Invoke(destination);
            //Console.WriteLine();
            //Console.WriteLine("--- Users: ---");
            //var table = function.GetTable("USERLIST");
            //foreach (var user in table)
            //{
            //    var field = user["USERNAME"];
            //    Console.WriteLine(field.GetString());
            //}

            #endregion old fashioned

            Console.WriteLine();
            Console.WriteLine("- Loading users:");
            Console.WriteLine();

            //var model = new BasisModel(destination);
            //foreach (var user in model.GetList())
            //{
            //    Console.WriteLine($"{user.UserName}, {user.FullName};");
            //}

            #region press any key...

            Console.WriteLine();
            Console.Write("Press any key to continue ... ");
            Console.ReadKey(true);

            #endregion press any key...
        }
    }

    //internal static class Extensions
    //{
    //    internal static IEnumerable<User> GetList(this IUser model)
    //    {
    //        return model.GetList(0, true);
    //    }
    //}
}

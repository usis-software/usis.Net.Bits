using System;
using System.Data;
using System.Diagnostics;
using System.Globalization;

namespace Playground
{
    internal class DataAccess
    {
        internal static void Main()
        {
            var factories = System.Data.Common.DbProviderFactories.GetFactoryClasses();
            foreach (var item in factories.Rows)
            {
                var row = item as DataRow;
                var factory = System.Data.Common.DbProviderFactories.GetFactory(row);
                Debug.WriteLine(factory.GetType().FullName);
                Debug.WriteLine(row[2]);
                Console.WriteLine(string.Format(CultureInfo.CurrentCulture, "{0}: {1}", row[0], row[1]));
            }
            var dataSource = new usis.Data.DataSource("MsSql")
            {
                ConnectionString = "Server=.;Database=audius;Integrated Security=true"
            };
            Debug.Print(dataSource.ProviderFactory.GetType().FullName);
            //var registry = new usis.Data.DbRegistry(dataSource);
            //foreach (var item in registry.OpenLocalDataSource().Stores)
            //{
            //    Debug.Print(item.Name);
            //}
            Console.WriteLine();
            Console.Write("Press any key to continue ... ");
            Console.ReadKey(true);
        }
    }
}

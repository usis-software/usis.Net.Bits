using usis.Framework;

namespace usis.Server.ObjectBroker
{
    internal class Broker : ApplicationExtension
    {
        private Repository Repository
        {
            get; set;
        }

        protected override void OnAttach()
        {
            Repository = Owner.Extensions.Find<Repository>();
            base.OnAttach();
        }

        protected override void OnStart()
        {
            foreach (var store in Repository.Storages)
            {
                foreach (var item in store.Entities)
                {
                    System.Diagnostics.Debug.WriteLine(item);
                }
            }
            base.OnStart();
        }

        //private static void ListTables()
        //{
        //    var dataSource = new DataSource("MySql.Data.MySqlClient", "server=localhost;user id=root;password=Ej5u4u3g9W;database=sakila");

        //    using (var command = DataSourceCommand.ExecuteReader(dataSource, CommandType.Text, "SHOW TABLES;"))
        //    {
        //        while (command.Reader.Read())
        //        {
        //            Console.WriteLine(command.Reader[0]);
        //        }
        //    }
        //    Console.WriteLine();
        //}
    }
}

using System;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations.History;

namespace usis.Data.Entity
{
    [CLSCompliant(false)]
    public class MySqlHistoryContext : MySql.Data.Entity.MySqlHistoryContext
    {
        public MySqlHistoryContext(System.Data.Common.DbConnection connection, string schema) : base(connection, schema) { }
    }

    //public class DbConfiguration : System.Data.Entity.DbConfiguration
    //{
    //    #region construction

    //    //  ------------
    //    //  construction
    //    //  ------------

    //    public DbConfiguration(DataSource dataSource)
    //    {
    //        SetProviderFactory(dataSource.ProviderInvariantName, dataSource.ProviderFactory);
    //        if (!string.IsNullOrWhiteSpace(dataSource.EntityConnectionFactoryTypeName))
    //        {
    //            SetDefaultConnectionFactory(Create<IDbConnectionFactory>(dataSource.EntityConnectionFactoryTypeName));
    //        }
    //        if (!string.IsNullOrWhiteSpace(dataSource.EntityProviderServicesTypeName))
    //        {
    //            SetProviderServices(dataSource.ProviderInvariantName,
    //                Create<DbProviderServices>(dataSource.EntityProviderServicesTypeName));
    //        }
    //        if (!string.IsNullOrWhiteSpace(dataSource.HistoryContextTypeName))
    //        {
    //            SetHistoryContext(dataSource.ProviderInvariantName, (connection, schema) =>
    //            {
    //                var type = Type.GetType(dataSource.HistoryContextTypeName);
    //                var historyContext = Activator.CreateInstance(type, new object[] { connection, schema }) as HistoryContext;
    //                return historyContext;
    //            });
    //        }
    //    }

    //    #endregion construction

    //    #region public methods

    //    //  ---------------------------------
    //    //  SetDataSourceConfiguration method
    //    //  ---------------------------------

    //    public static void SetDataSourceConfiguration(DataSource dataSource)
    //    {
    //        SetConfiguration(new DbConfiguration(dataSource));
    //    }

    //    #endregion public methods

    //    #region private methods

    //    //  -------------
    //    //  Create method
    //    //  -------------

    //    private static T Create<T>(string typeName) where T : class
    //    {
    //        return Activator.CreateInstance(Type.GetType(typeName, true)) as T;
    //    }

    //    #endregion private methods
    //}
}

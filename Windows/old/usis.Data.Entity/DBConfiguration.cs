//
//  @(#) DBConfiguration.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using System;
using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations.History;
using System.Data.Entity.Migrations.Sql;

namespace usis.Data.Entity
{
    //  ---------------------
    //  DBConfiguration class
    //  ---------------------

    /// <summary>
    /// Defines Entity Framework configuration for an application.
    /// </summary>
    /// <remarks>
    /// Base class documentation: https://msdn.microsoft.com/en-us/library/system.data.entity.dbconfiguration.aspx
    /// </remarks>
    /// <seealso cref="DbConfiguration" />

    public class DBConfiguration : DbConfiguration
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="DBConfiguration"/> class
        /// with the specified data source.
        /// </summary>
        /// <param name="dataSource">A data source that provides the configuration information.</param>

        public DBConfiguration(DataSource dataSource)
        {
            if (dataSource == null) throw new ArgumentNullException(nameof(dataSource));
 
            SetProviderFactory(dataSource.ProviderInvariantName, dataSource.ProviderFactory);

            var entityConnectionFactory = dataSource.EntityConnectionFactoryTypeName;
            var entityProviderServices = dataSource.EntityProviderServicesTypeName;

            #region configure MySql

            if ("MySql.Data.MySqlClient".Equals(dataSource.ProviderInvariantName))
            {
                if (string.IsNullOrWhiteSpace(entityConnectionFactory)) entityConnectionFactory = "MySql.Data.Entity.MySqlConnectionFactory, MySql.Data.Entity.EF6";
                if (string.IsNullOrWhiteSpace(entityProviderServices)) entityProviderServices = "MySql.Data.MySqlClient.MySqlProviderServices, MySql.Data.Entity.EF6";
            }

            #endregion configure MySql

            // set configuration

            if (!string.IsNullOrWhiteSpace(entityConnectionFactory))
            {
                SetDefaultConnectionFactory(Create<IDbConnectionFactory>(entityConnectionFactory));
            }
            if (!string.IsNullOrWhiteSpace(entityProviderServices))
            {
                SetProviderServices(dataSource.ProviderInvariantName, Create<DbProviderServices>(entityProviderServices));
            }
            if (!string.IsNullOrWhiteSpace(dataSource.HistoryContextTypeName))
            {
                SetHistoryContext(dataSource.ProviderInvariantName, (connection, schema) =>
                {
                    var type = Type.GetType(dataSource.HistoryContextTypeName);
                    var historyContext = Activator.CreateInstance(type, new object[] { connection, schema }) as HistoryContext;
                    return historyContext;
                });
            }
            if (!string.IsNullOrWhiteSpace(dataSource.MigrationSqlGeneratorTypeName))
            {
                SetMigrationSqlGenerator(dataSource.ProviderInvariantName, () => Create<MigrationSqlGenerator>(dataSource.MigrationSqlGeneratorTypeName));
            }
        }

        #endregion construction

        #region public methods

        //  ---------------------------------
        //  SetDataSourceConfiguration method
        //  ---------------------------------

        /// <summary>
        /// Sets the Entity Framework configuration singleton instance
        /// with the specified data source.
        /// </summary>
        /// <param name="dataSource">A data source that provides the configuration information.</param>

        public static void SetDataSourceConfiguration(DataSource dataSource)
        {
            SetConfiguration(new DBConfiguration(dataSource));
        }

        #endregion public methods

        #region private methods

        //  -------------
        //  Create method
        //  -------------

        private static T Create<T>(string typeName) where T : class
        {
            return Activator.CreateInstance(Type.GetType(typeName, true)) as T;
        }

        #endregion private methods
    }
}

// eof "DBConfiguration.cs"

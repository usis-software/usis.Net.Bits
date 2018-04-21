//
//  @(#) DbConfiguration.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) usis GmbH. All rights reserved.

using System;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations.History;

namespace usis.Data.Entity
{
    //  ---------------------
    //  DbConfiguration class
    //  ---------------------

    /// <summary>
    /// Defines Entity Framework configuration for an application.
    /// </summary>
    /// <seealso cref="System.Data.Entity.DbConfiguration" />

    public class DbConfiguration : System.Data.Entity.DbConfiguration
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="DbConfiguration"/> class
        /// with the specified data source.
        /// </summary>
        /// <param name="dataSource">A data source that provides the configuration information.</param>

        public DbConfiguration(DataSource dataSource)
        {
            SetProviderFactory(dataSource.ProviderInvariantName, dataSource.ProviderFactory);
            if (!string.IsNullOrWhiteSpace(dataSource.EntityConnectionFactoryTypeName))
            {
                SetDefaultConnectionFactory(Create<IDbConnectionFactory>(dataSource.EntityConnectionFactoryTypeName));
            }
            if (!string.IsNullOrWhiteSpace(dataSource.EntityProviderServicesTypeName))
            {
                SetProviderServices(dataSource.ProviderInvariantName,
                    Create<DbProviderServices>(dataSource.EntityProviderServicesTypeName));
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
            SetConfiguration(new DbConfiguration(dataSource));
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

// eof "DbConfiguration.cs"

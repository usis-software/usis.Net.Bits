//
//  @(#) DataSource.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015,2016 usis GmbH. All rights reserved.

using System;
using System.Data.Common;
using System.Globalization;
using System.Runtime.Serialization;
using usis.Platform.Portable;

namespace usis.Data
{
    //  ----------------
    //  DataSource class
    //  ----------------

    /// <summary>
    /// Defines a data source for .NET Framework data providers.
    /// </summary>

    [DataContract]
    public class DataSource
    {
        #region properties

        //  -------------------------
        //  ConnectionString property
        //  -------------------------

        /// <summary>
        /// Gets or sets the string used to open a connection.
        /// </summary>
        /// <value>
        /// The connection string used to establish the initial connection.
        /// The exact contents of the connection string depend on the specific data source for this connection.
        /// </value>

        [DataMember]
        public string ConnectionString { get; set; }

        //  -----------------------
        //  CommandTimeout property
        //  -----------------------

        /// <summary>
        /// Gets or sets the wait time before terminating the attempt to execute a command and generating an error.
        /// </summary>
        /// <value>
        /// The time in seconds to wait for the command to execute.
        /// </value>

        [DataMember]
        public int CommandTimeout { get; set; }

        //  ------------------------
        //  ProviderFactory property
        //  ------------------------

        private DbProviderFactory factory;

        /// <summary>
        /// Gets the provider factory for the data source.
        /// </summary>
        /// <value>
        /// The provider factory.
        /// </value>

        public DbProviderFactory ProviderFactory
        {
            get
            {
                if (factory == null)
                {
                    factory = CreateFactory(ProviderInvariantName);
                }
                return factory;
            }
        }

        //  ------------------------------
        //  ProviderInvariantName property
        //  ------------------------------

        /// <summary>
        /// Gets the invariant name of the provider.
        /// </summary>
        /// <value>
        /// The invariant name of the provider.
        /// </value>

        [DataMember]
        public string ProviderInvariantName { get; set; }

        //  ----------------------------------------
        //  EntityConnectionFactoryTypeName property
        //  ----------------------------------------

        /// <summary>
        /// Gets the type name of the entity framework default connection factory.
        /// </summary>
        /// <value>
        /// The type name of the entity framework default connection factory.
        /// </value>

        public string EntityConnectionFactoryTypeName { get; }

        //  ---------------------------------------
        //  EntityProviderServicesTypeName property
        //  ---------------------------------------

        /// <summary>
        /// Gets the type name of the entity framework provider.
        /// </summary>
        /// <value>
        /// The type name of the entity framework provider.
        /// </value>

        public string EntityProviderServicesTypeName { get; }

        //  --------------------------------
        //  ProviderFactoryTypeName property
        //  --------------------------------

        /// <summary>
        /// Gets the type name of the ADO.NET provider.
        /// </summary>
        /// <value>
        /// The type name of the ADO.NET provider.
        /// </value>

        public string ProviderFactoryTypeName { get; }

        //  -------------------------------
        //  HistoryContextTypeName property
        //  -------------------------------

        /// <summary>
        /// Gets the type name of the history context.
        /// </summary>
        /// <value>
        /// The type name of the history context.
        /// </value>

        public string HistoryContextTypeName { get; }

        //  --------------------------------------
        //  MigrationSqlGeneratorTypeName property
        //  --------------------------------------

        /// <summary>
        /// Gets the typw name of the migration SQL generator.
        /// </summary>
        /// <value>
        /// The type name of the migration SQL generator.
        /// </value>

        public string MigrationSqlGeneratorTypeName { get; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSource"/> class
        /// with the specified provider invariant name.
        /// </summary>
        /// <param name="providerInvariantName">
        /// Invariant name of a provider.
        /// </param>

        public DataSource(string providerInvariantName)
        {
            ProviderInvariantName = providerInvariantName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSource"/> class
        /// with the specified provider invariant name and connection string.
        /// </summary>
        /// <param name="providerInvariantName">
        /// Invariant name of a provider.
        /// </param>
        /// <param name="connectionString">The connection string.</param>

        public DataSource(string providerInvariantName, string connectionString) : this(providerInvariantName)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSource"/> class
        /// with the specified value store.
        /// </summary>
        /// <param name="valueStore">The value store that provides the data sources properties.</param>

        public DataSource(IValueStore valueStore)
        {
            if (valueStore == null) throw new ArgumentNullException(nameof(valueStore));

            ProviderInvariantName = valueStore.Get<string>("Provider");
            ConnectionString = valueStore.Get<string>("ConnectionString");

            ProviderFactoryTypeName = valueStore.Get<string>("ProviderFactory");
            EntityConnectionFactoryTypeName = valueStore.Get<string>("EntityConnectionFactory");
            EntityProviderServicesTypeName = valueStore.Get<string>("EntityProviderServices");
            HistoryContextTypeName = valueStore.Get<string>("HistoryContext");
            MigrationSqlGeneratorTypeName = valueStore.Get<string>("MigrationSqlGenerator");
        }

        #endregion construction

        #region public mehods

        //  ---------------------
        //  OpenConnection method
        //  ---------------------

        /// <summary>
        /// Creates and opens a database connection
        /// with the settings specified by the <see cref="ConnectionString"/>.
        /// </summary>
        /// <returns>
        /// An opened database connection.
        /// </returns>

        public DbConnection OpenConnection()
        {
            DbConnection connection = ProviderFactory.CreateConnection();
            connection.ConnectionString = ConnectionString;
            connection.Open();
            return connection;
        }

        #region CheckConnection method

        //public void CheckConnection()
        //{
        //    using (var connection = OpenConnection())
        //    {
        //        System.Diagnostics.Trace.WriteLine(string.Format(
        //            CultureInfo.CurrentCulture, "...checking data source '{0}' connection.", connection));
        //    }
        //}

        #endregion CheckConnection method

        //  ---------------
        //  ToString method
        //  ---------------

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture,
                "Provider={0}; Connection string='{1}'",
                ProviderInvariantName, ConnectionString);
        }
        
        #endregion public mehods

        #region private mehods

        //  --------------------
        //  CreateFactory method
        //  --------------------

        private DbProviderFactory CreateFactory(string providerInvariantName)
        {
            if (!string.IsNullOrWhiteSpace(ProviderFactoryTypeName))
            {
                return Activator.CreateInstance(Type.GetType(ProviderFactoryTypeName, true)) as DbProviderFactory;
            }
            if (string.IsNullOrWhiteSpace(providerInvariantName))
            {
                throw new ArgumentNullOrWhiteSpaceException(nameof(providerInvariantName));
            }
            string name = null;
            if (providerInvariantName.Equals("MsOleDb", StringComparison.OrdinalIgnoreCase))
            {
                name = "System.Data.OleDb";
            }
            else if (providerInvariantName.Equals("MsSql", StringComparison.OrdinalIgnoreCase))
            {
                name = "System.Data.SqlClient";
            }
            else if (providerInvariantName.Equals("MsOdbc", StringComparison.OrdinalIgnoreCase))
            {
                name = "System.Data.Odbc";
            }
            else if (providerInvariantName.Equals("WebDbService", StringComparison.OrdinalIgnoreCase))
            {
                throw new NotImplementedException();
            }
            else name = providerInvariantName;
            return DbProviderFactories.GetFactory(name);
        }

        #endregion private mehods
    }
}

// eof "DataSource.cs"

//
//  @(#) DataSource.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 12
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015 usis GmbH. All rights reserved.

#pragma warning disable 1591

using System;

namespace usis.Data
{
    public sealed class DbRegistry
    {
        #region properties

        //  ----------------
        //  Service property
        //  ----------------

        private DbRegistryService Service
        {
            get; set;
        }

        //  ------------------------
        //  LocalDataSource property
        //  ------------------------

        private DbRegistryKey localDataSource;

        [Obsolete("Use OpenLocalDataSource method instead.")]
        public DbRegistryKey LocalDataSource
        {
            get
            {
                if (localDataSource == null)
                {
                    localDataSource = new DbRegistryKey(Service, null, nameof(LocalDataSource));
                }
                return localDataSource;
            }
        }

        #endregion properties

        public DbRegistryKey OpenLocalDataSource()
        {
            return new DbRegistryKey(Service, null, "LocalDataSource");
        }

        #region construction

        //  -----------
        //  constructor
        //  -----------

        /// <summary>
        /// Initializes a new instance of the <see cref="DbRegistry"/> class.
        /// </summary>
        /// <param name="dataSource">
        /// The data source.
        /// </param>

        public DbRegistry(DataSource dataSource)
        {
            Service = new DbRegistryService(dataSource);
        }

        #endregion construction
    }
}

// eof "DataSource.cs"

//
//  @(#) ApplicationInterfaceExtensions.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

using System;
using usis.Framework.Windows;
using usis.Platform;
using usis.Platform.Data;

namespace usis.Framework.Data
{
    //  ------------------------------------
    //  ApplicationInterfaceExtensions class
    //  ------------------------------------

    /// <summary>
    /// Provides extension methods to the <see cref="IApplication"/> interface.
    /// </summary>

    public static class ApplicationInterfaceExtensions
    {
        #region AddDataSourceExtensionFromRegistrySettings method

        //  -------------------------------------------------
        //  AddDataSourceExtensionFromRegistrySettings method
        //  -------------------------------------------------

        /// <summary>
        /// Adds a <see cref="DataSourceApplicationExtension" /> to the application
        /// with a data source that is initialized from the specified registry settings.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="subkeyName">Name of the registry subkey.</param>
        /// <returns>
        /// A newly created <see cref="DataSourceApplicationExtension" />.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="application" /> is a null reference.</exception>

        public static DataSourceApplicationExtension AddDataSourceExtensionFromRegistrySettings(this IExtensibleObject<IApplication> application, string subkeyName)
        {
            if (application == null) throw new ArgumentNullException(nameof(application));
            var store = application.With<RegistrySettings>()?.LocalMachine.CreateSubStorage(subkeyName, true);
            if (store == null) return null;
            var dataSource = new DataSource(store);
            if (string.IsNullOrWhiteSpace(dataSource.ProviderInvariantName)) return null;
            var extension = new DataSourceApplicationExtension(dataSource);
            application.Extensions.Add(extension);
            return extension;
        }

        #endregion AddDataSourceExtensionFromRegistrySettings method

        #region AddDataSourceExtensionFromConnectionStrings method

        //  --------------------------------------------------
        //  AddDataSourceExtensionFromConnectionStrings method
        //  --------------------------------------------------

        /// <summary>
        /// Adds a <see cref="DataSourceApplicationExtension" /> to the application
        /// with a data source that is is initialized from a connection string setting with the specified name.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="connectionStringName">Name of the connection string.</param>
        /// <returns>
        /// A newly created <see cref="DataSourceApplicationExtension" />.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="application" /> is a null reference.</exception>
        /// <remarks>
        /// The connection string setting is retrieved from the application configuration file.
        /// </remarks>

        public static DataSourceApplicationExtension AddDataSourceExtensionFromConnectionStrings(this IExtensibleObject<IApplication> application, string connectionStringName)
        {
            if (application == null) throw new ArgumentNullException(nameof(application));
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringName];
            if (connectionString == null) return null;
            var dataSource = new DataSource(connectionString.ProviderName, connectionString.ConnectionString);
            var extension = new DataSourceApplicationExtension(dataSource);
            application.Extensions.Add(extension);
            return extension;
        }

        #endregion AddDataSourceExtensionFromConnectionStrings method
    }
}

// eof "ApplicationInterfaceExtensions.cs"

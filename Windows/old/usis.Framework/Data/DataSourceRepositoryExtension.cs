//
//  @(#) DataSourceRepositoryExtension.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using System;
using System.Configuration;
using System.Linq;
using usis.Data;

namespace usis.Framework.Data
{
    //  -----------------------------------
    //  DataSourceRepositoryExtension class
    //  -----------------------------------

    /// <summary>
    /// Provides extension methods to the <see cref="DataSourceRepository"/> class.
    /// </summary>

    public static class DataSourceRepositoryExtension
    {
        //  ---------------------------------------
        //  ReadApplicationConfigurationFile method
        //  ---------------------------------------

        /// <summary>
        /// Reads connection strings from the application configuration file
        /// and adds them to the repository.
        /// </summary>
        /// <param name="repository">The data source repository.</param>
        /// <exception cref="ArgumentNullException">
        /// <i>repository</i> is a null reference (<b>Nothing</b> in Visual Basic).
        /// </exception>

        public static void ReadApplicationConfigurationFile(this DataSourceRepository repository)
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));
            foreach (var item in ConfigurationManager.ConnectionStrings.Cast<ConnectionStringSettings>())
            {
                repository[item.Name] = DataSourceApplicationExtension.FromConnectionString(item);
            }
        }
    }
}

// eof "DataSourceRepositoryExtension.cs"

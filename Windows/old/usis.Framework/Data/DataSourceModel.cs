//
//  @(#) DataSourceModel.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using usis.Data;
using usis.Framework.Portable;

namespace usis.Framework.Data
{
    //  ---------------------
    //  DataSourceModel class
    //  ---------------------

    /// <summary>
    /// Provides a base class for models that access a single data source.
    /// </summary>
    /// <seealso cref="ApplicationExtension" />

    public class DataSourceModel : ApplicationExtension
    {
        #region fields

        private DataSource dataSource;

        #endregion fields

        #region properties

        //  -------------------
        //  DataSource property
        //  -------------------

        /// <summary>
        /// Gets the data source.
        /// </summary>
        /// <value>
        /// The data source.
        /// </value>

        public DataSource DataSource
        {
            get
            {
                if (dataSource == null) dataSource = Owner.FindExtension<DataSourceApplicationExtension>()?.DataSource;
                return dataSource;
            }
        }

        #endregion properties
    }
}

// eof "DataSourceModel.cs"

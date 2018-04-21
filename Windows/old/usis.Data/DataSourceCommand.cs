//
//  @(#) DataSourceCommand.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

namespace usis.Data
{
    //  -----------------------
    //  DataSourceCommand class
    //  -----------------------

    /// <summary>
    /// Represents a command to execute againts a data source.
    /// </summary>
    /// <seealso cref="IDisposable"/>

    public sealed class DataSourceCommand : IDisposable
    {
        #region fields

        private DbConnection connection;
        private DbCommand command;
        private DbDataReader reader;

        #endregion fields

        #region construction

        //  ------------
        //  construction
        //  ------------

        private DataSourceCommand() { }

        #endregion construction

        #region properties

        //  ---------------
        //  Reader property
        //  ---------------

        /// <summary>
        /// Gets a reader to read the results of the command.
        /// </summary>
        /// <value>
        /// A reader to read the results of the command.
        /// </value>

        public DbDataReader Reader { get { return reader; } }

        #endregion properties

        #region methods

        //  --------------------
        //  ExecuteReader method
        //  --------------------

        /// <summary>
        /// Executes a command against the specified data source.
        /// </summary>
        /// <param name="dataSource">The data source.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="commandText">The command text.</param>
        /// <returns>
        /// A <see cref="DataSourceCommand"/> object that represents the command.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <i>dataSource</i> is a null reference (<b>Nothing</b> in Visual Basic).
        /// </exception>

        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public static DataSourceCommand ExecuteReader(DataSource dataSource, CommandType commandType, string commandText)
        {
            if (dataSource == null) throw new ArgumentNullException(nameof(dataSource));

            DataSourceCommand dataSourceCommand = null;
            DataSourceCommand tmp = null;
            try
            {
                tmp = new DataSourceCommand();
                tmp.connection = dataSource.OpenConnection();
                tmp.command = tmp.connection.CreateCommand();
                tmp.command.CommandType = commandType;
                tmp.command.CommandText = commandText;
                tmp.command.CommandTimeout = dataSource.CommandTimeout;
                tmp.reader = tmp.command.ExecuteReader();

                dataSourceCommand = tmp;
                tmp = null;
            }
            finally
            {
                if (tmp != null) tmp.Dispose();
            }
            return dataSourceCommand;
        }

        #endregion methods

        #region IDisposable implementation

        //  --------------
        //  Dispose method
        //  --------------

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>

        public void Dispose()
        {
            if (reader != null)
            {
                reader.Close();
                reader = null;
            }
            if (command != null)
            {
                command.Dispose();
                command = null;
            }
            if (connection != null)
            {
                connection.Close();
                connection = null;
            }
        }

        #endregion IDisposable implementation
    }
}

// eof "DataSourceCommand.cs"

//
//  @(#) DataSourceRepository.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015-2017 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;

namespace usis.Platform.Data
{
    //  --------------------------
    //  DataSourceRepository class
    //  --------------------------

    /// <summary>
    /// Provides a container to access data sources by name.
    /// </summary>

    public class DataSourceRepository
    {
        #region fields

        //private IHierarchicalValueStore store;
        private Dictionary<string, DataSource> dataSources = new Dictionary<string, DataSource>(StringComparer.OrdinalIgnoreCase);

        #endregion fields

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSourceRepository"/> class.
        /// </summary>

        public DataSourceRepository() { }

        ///// <summary>
        ///// Initializes a new instance of the <see cref="DataSourceRepository"/> class.
        ///// with the specified hierarchical value store as backing store.
        ///// </summary>
        ///// <param name="store">The store for the data source repository.</param>

        //public DataSourceRepository(IHierarchicalValueStore store)
        //{
        //    this.store = store;
        //}

        #endregion construction

        #region events

        //  --------------
        //  Reloaded event
        //  --------------

        //public event EventHandler<EventArgs> Reloaded;

        #endregion events

        #region properties

        //  -------
        //  Indexer
        //  -------

        /// <summary>
        /// Gets or sets the <see cref="DataSource" /> with the specified name.
        /// </summary>
        /// <value>
        /// The <see cref="DataSource"/>.
        /// </value>
        /// <param name="name">The name of the data source.</param>
        /// <returns>
        /// The <see cref="DataSource"/> with the specified name.
        /// </returns>

        public DataSource this[string name]
        {
            get => dataSources[name]; set => Set(name, value);
        }

        //  ----------------------
        //  DataSourceNames method
        //  ----------------------

        /// <summary>
        /// Gets an enumerator to iterate the names of the data sources in the repository.
        /// </summary>
        /// <value>
        /// An enumerator to iterate the names of the data sources in the repository.
        /// </value>

        public IEnumerable<string> DataSourceNames => dataSources.Keys;

        #endregion properties

        #region methods

        //  -------------
        //  Reload method
        //  -------------

        ///// <summary>
        ///// Reloads all data sources from the backing store
        ///// and raises the <see cref="Reloaded"/> event.
        ///// </summary>
        ///// <remarks>
        ///// If no backing store was specified when the repository was created
        ///// this method does nothing.
        ///// </remarks>

        //public void Reload()
        //{
        //    if (store != null)
        //    {
        //        dataSources.Clear();
        //        Load(store);
        //        if (Reloaded != null) Reloaded(this, EventArgs.Empty);
        //    }
        //}

        //  ----------
        //  Set method
        //  ----------

        /// <summary>
        /// Adds a data source with the specified name to the repository.
        /// </summary>
        /// <param name="name">The name of the data source.</param>
        /// <param name="dataSource">The data source.</param>
        /// <exception cref="ArgumentNullException">
        /// <i>dataSource</i> is a null reference (<b>Nothing</b> in Visual Basic).
        /// </exception>
        /// <remarks>
        /// If a data source with the specified name already exists
        /// this data source is replaced with <paramref name="dataSource"/>.
        /// </remarks>

        internal void Set(string name, DataSource dataSource)
        {
            dataSources[name] = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
        }

        #region private methods


        //  -----------
        //  Load method
        //  -----------

        //private void Load(IHierarchicalValueStore valueStore)
        //{
        //    if (valueStore == null) throw new ArgumentNullException(nameof(valueStore));
        //    foreach (var item in valueStore.EnumerateStores(false))
        //    {
        //        var providerName = item.Get<string>("Provider");
        //        if (!string.IsNullOrWhiteSpace(providerName))
        //        {
        //            Add(item.Name, new DataSource(providerName)
        //            {
        //                ConnectionString = item.Get<string>("ConnectionString"),
        //                CommandTimeout = item.Get<int>("CommandTimeout")
        //            });
        //        }
        //    }
        //}

        #endregion private methods

        #endregion methods
    }
}

// eof "DataSourceRepository.cs"

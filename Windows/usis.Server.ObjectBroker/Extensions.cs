using System;
using usis.Framework.Windows;
using usis.Platform;
using usis.Platform.Data;

namespace usis.Framework.Data
{
    //  ----------------
    //  Extensions class
    //  ----------------

    internal static class Extensions
    {
        #region ReadDataSources method

        //  ----------------------
        //  ReadDataSources method
        //  ----------------------

        public static void ReadDataSources(this DataSourceRepository repository, IHierarchicalValueStorage store)
        {
            if (store == null) return;
            foreach (var subStore in store.Storages)
            {
                var dataSource = new DataSource(subStore);
                repository[subStore.Name] = dataSource;
            }
        }

        #endregion ReadDataSources method

        #region DataSourceRepository methods

        //  ------------------------------
        //  AddDataSourceRepository method
        //  ------------------------------

        public static void CreateDataSourceRepository(this IExtensibleObject<IApplication> application)
        {
            if (application == null) throw new ArgumentNullException(nameof(application));
            if (application.GetDataSourceRepository() != null) return;
            application.Extensions.Add(new DataSourceRepositoryApplicationExtension());
        }

        public static void CreateDataSourceRepository(this IExtensibleObject<IApplication> application, IHierarchicalValueStorage store)
        {
            application.CreateDataSourceRepository();
            application.GetDataSourceRepository().ReadDataSources(store);
        }

        //  ------------------------------
        //  GetDataSourceRepository method
        //  ------------------------------

        public static DataSourceRepository GetDataSourceRepository(this IExtensibleObject<IApplication> application)
        {
            return application.With<DataSourceRepositoryApplicationExtension>()?.Repository;
        }

        #endregion DataSourceRepository methods

        #region RegistrySettings methods

        //  -----------------------------
        //  CreateRegistrySettings method
        //  -----------------------------

        public static void CreateRegistrySettings(this IExtensibleObject<IApplication> application)
        {
            if (application == null) throw new ArgumentNullException(nameof(application));
            if (application.GetRegistrySettings() != null) return;
            application.Extensions.Add(new RegistrySettings());
        }

        //  --------------------------
        //  GetRegistrySettings method
        //  --------------------------

        public static RegistrySettings GetRegistrySettings(this IExtensibleObject<IApplication> application)
        {
            if (application == null) throw new ArgumentNullException(nameof(application));
            return application.With<RegistrySettings>();
        }

        #endregion RegistrySettings methods
    }

    #region DataSourceRepositoryApplicationExtension class

    //  ----------------------------------------------
    //  DataSourceRepositoryApplicationExtension class
    //  ----------------------------------------------

    internal class DataSourceRepositoryApplicationExtension : Extension<IApplication>
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSourceRepositoryApplicationExtension"/> class.
        /// </summary>

        public DataSourceRepositoryApplicationExtension()
        {
            Repository = new DataSourceRepository();
        }

        #endregion construction

        #region properties

        //  -------------------
        //  Repository property
        //  -------------------

        public DataSourceRepository Repository
        {
            get; private set;
        }

        #endregion properties
    }

    #endregion DataSourceRepositoryApplicationExtension class
}

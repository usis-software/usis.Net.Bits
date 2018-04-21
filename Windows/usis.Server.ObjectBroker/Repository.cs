using System.Collections.Generic;
using usis.Framework;
using usis.Framework.Data;
using usis.Platform;

namespace usis.Server.ObjectBroker
{
    public class Repository : Extension<IApplication>
    {
        private List<IStorage> storages = new List<IStorage>();

        public IEnumerable<IStorage> Storages
        {
            get { return storages; }
        }

        protected override void OnAttach()
        {
            var dataSources = Owner.Extensions.Find<DataSourceRepositoryApplicationExtension>()?.Repository;
            if (dataSources == null) return;

            foreach (var dataSourceName in dataSources.DataSourceNames)
            {
                if (dataSourceName.StartsWith("Local", System.StringComparison.OrdinalIgnoreCase)) continue;

                var dataSource = dataSources[dataSourceName];
                if (dataSource == null) continue;

                storages.Add(new DataSourceStorage(dataSource));
            }
        }
    }
}

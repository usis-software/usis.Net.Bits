using System.Diagnostics.CodeAnalysis;
using usis.Platform.Data;

namespace usis.Data.Registry
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class RootNode : ContainerNode
    {
        public RootNode()
        {
            //Store = usis.Platform.RegistryValueStore.OpenLocalMachine();
        }

        private DataSource dataSource;

        public DataSource DataSource
        {
            get
            {
                return dataSource;
            }
            set
            {
                dataSource = value;
                if (dataSource != null)
                {
                    //var registry = new DbRegistry(dataSource);
                    //Store = registry.OpenLocalDataSource();
                }
                else Store = null;
            }
        }
    }
}

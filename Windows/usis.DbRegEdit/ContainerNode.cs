using Microsoft.ManagementConsole;
using System.Collections.Generic;
using usis.Platform;

namespace usis.Data.Registry
{
    internal class ContainerNode : ScopeNode
    {
        private IHierarchicalValueStorage parent;
        private string storeName;

        private IHierarchicalValueStorage store;

        public IHierarchicalValueStorage Store
        {
            get
            {
                if (store == null && parent != null)
                {
                    Store = parent.OpenStorage(storeName, false);
                }
                return store;
            }
            internal protected set
            {
                store = value;
                DisplayName = store == null ? null : store.Name;
            }
        }
        protected ContainerNode()
        {
            ViewDescriptions.Add(ValueListView.Description);
        }
        private ContainerNode(IHierarchicalValueStorage parent, string name) : this()
        {
            this.parent = parent;
            storeName = name;

            DisplayName = name;
        }
        //private ContainerNode(IHierarchicalValueStore store) : this()
        //{
        //    Store = store;
        //}
        protected override void OnExpand(AsyncStatus status)
        {
            if (Store == null) return;
            var nodes = new List<ContainerNode>();
            foreach (var name in Store.StorageNames)
            {
                nodes.Add(new ContainerNode(Store, name));
            }
            nodes.Sort((x, y) => string.Compare(x.DisplayName, y.DisplayName, System.StringComparison.OrdinalIgnoreCase));
            Children.AddRange(nodes.ToArray());
        }
    }
}

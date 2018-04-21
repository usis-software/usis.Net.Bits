using Microsoft.ManagementConsole;
using System.Collections.Generic;
using System.Globalization;
using usis.Platform;

namespace usis.Data.Registry
{
    internal sealed class ValueListView : MmcListView
    {
        internal static MmcListViewDescription Description
        {
            get
            {
                return new MmcListViewDescription()
                {
                    DisplayName = "Values",
                    ViewType = typeof(ValueListView),
                    Options = MmcListViewOptions.ExcludeScopeNodes
                };
            }
        }
        protected override void OnInitialize(AsyncStatus status)
        {
            base.OnInitialize(status);

            Columns[0].SetWidth(150);

            Columns.Add(new MmcListViewColumn("Type", 100));
            Columns.Add(new MmcListViewColumn("Data", 300));

            Refresh();
        }
        private IHierarchicalValueStorage Store
        {
            get
            {
                var node = ScopeNode as ContainerNode;
                return node == null ? null : node.Store;
            }
        }
        private void Refresh()
        {
            ResultNodes.Clear();
            var nodes = new List<ResultNode>();
            foreach (var item in Store.Values)
            {
                var node = new ResultNode()
                {
                    DisplayName = item.Name
                };
                node.SubItemDisplayNames.Add(item.Value == null ? string.Empty : item.Value.GetType().Name);
                node.SubItemDisplayNames.Add(string.Format(CultureInfo.CurrentCulture, "{0}", item.Value));
                //ResultNodes.Add(node);
                nodes.Add(node);
            }
            ResultNodes.AddRange(nodes.ToArray());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Foundation;
using UIKit;

namespace usis.iOS.PNRouter
{
    internal class TableViewSource<T> : UITableViewSource
    {
        protected IEnumerable<T> Objects
        {
            get; private set;
        }

        public TableViewSource(IEnumerable<T> objects)
        {
            if (objects == null) throw new ArgumentNullException(nameof(objects));
            Objects = objects;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return Objects.Count();
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            Debug.Assert(indexPath.Section == 0);

            T item = Objects.ElementAt(indexPath.Row);
            Selected?.Invoke(this, new TableViewSourceItemEventArgs<T>(item));
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var item = Objects.ElementAt(indexPath.Row) as ITableViewObject;
            if (item == null) throw new NotImplementedException();
            else return item.GetCell(tableView, indexPath);
        }

        public event EventHandler<TableViewSourceItemEventArgs<T>> Selected;
    }

    internal class TableViewSourceItemEventArgs<T> : EventArgs
    {
        public TableViewSourceItemEventArgs(T item)
        {
            Item = item;
        }

        public T Item
        {
            get; private set;
        }
    }

    internal interface ITableViewObject
    {
        UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath);
    }
}

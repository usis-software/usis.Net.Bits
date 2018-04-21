//
//  @(#) TableViewSource.cs
//
//  Project:    usis Cocoa
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using Foundation;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UIKit;
using usis.Platform;

#pragma warning disable 1591

namespace usis.Cocoa.UIKit
{
    //  ------------------------
    //  TableViewSource<T> class
    //  ------------------------

    public class TableViewSource<TItem> : UITableViewSource, INotifyTableViewSourceChanged
    {
        #region properties

        //  --------------
        //  Items property
        //  --------------

        protected IEnumerable<TItem> Items { get; private set; }

        //  ----------------------------------
        //  DefaultTableViewCellStyle property
        //  ----------------------------------

        protected UITableViewCellStyle DefaultTableViewCellStyle { get; set; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        public TableViewSource(IEnumerable<TItem> items) : this(items, null) { }

        public TableViewSource(IEnumerable<TItem> items, IValueStorage attributes)
        {
            if (items == null) Items = new System.Collections.ObjectModel.ObservableCollection<TItem>();
            else Items = items;

            if (items is INotifyCollectionChanged notify)
            {
                notify.CollectionChanged += Notify_CollectionChanged;
            }

            if (attributes == null)
            {
                DefaultTableViewCellStyle = UITableViewCellStyle.Default;
            }
            else
            {
                DefaultTableViewCellStyle = attributes.Get(nameof(DefaultTableViewCellStyle), UITableViewCellStyle.Default);
            }
        }

        #endregion construction

        #region methods

        //  -----------------
        //  CreateCell method
        //  -----------------

        protected virtual UITableViewCell CreateCell(UITableViewCellStyle style, string reuseIdentifier)
        {
            return new TableViewCell(style, reuseIdentifier);
        }

        //  --------------------
        //  CustomizeCell method
        //  --------------------

        protected virtual void CustomizeCell(UITableView tableView, UITableViewCell cell, TItem item)
        {
            if (cell == null) throw new ArgumentNullException(nameof(cell));
            cell.TextLabel.Text = item.ToString();
        }

        //  ----------------
        //  CellStyle method
        //  ----------------

        protected virtual UITableViewCellStyle CellStyle(NSIndexPath indexPath) { return DefaultTableViewCellStyle; }

        //  ---------------------
        //  CellIdentifier method
        //  ---------------------

        protected virtual string CellIdentifier(NSIndexPath indexPath) { return typeof(TItem).Name; }

        //  -------------
        //  ItemAt method
        //  -------------

        protected TItem ItemAt(NSIndexPath indexPath)
        {
            if (indexPath == null) throw new ArgumentNullException(nameof(indexPath));
            return Items.ElementAt(indexPath.Row);
        }

        #endregion methods

        #region overrides

        //  --------------------
        //  RowsInSection method
        //  --------------------

        public override nint RowsInSection(UITableView tableview, nint section) { return Items.Count(); }

        //  --------------
        //  GetCell method
        //  --------------

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            if (tableView == null) throw new ArgumentNullException(nameof(tableView));
            if (indexPath == null) throw new ArgumentNullException(nameof(indexPath));

            var item = ItemAt(indexPath);
            var tableViewItem = item as ITableViewItem;
            if (tableViewItem == null)
            {
                UITableViewCell cell = null;
                UITableViewCell tmp = null;
                try
                {
                    var identifier = CellIdentifier(indexPath);
                    tmp = tableView.DequeueReusableCell(identifier);
                    if (tmp == null) tmp = CreateCell(CellStyle(indexPath), CellIdentifier(indexPath));
                    CustomizeCell(tableView, tmp, item);
                    cell = tmp; tmp = null;
                    return cell;
                }
                finally
                {
                    if (tmp != null) tmp.Dispose();
                }
            }
            else return tableViewItem.GetCell(tableView, indexPath);
        }

        //  ------------------
        //  RowSelected method
        //  ------------------

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (indexPath == null) throw new ArgumentNullException(nameof(indexPath));
            TItem item = ItemAt(indexPath);
            ItemSelected?.Invoke(this, new TableViewSourceItemEventArgs<TItem>(item));
        }

        #endregion overrides

        #region events

        //  ------------------
        //  ItemSelected event
        //  ------------------

        public event EventHandler<TableViewSourceItemEventArgs<TItem>> ItemSelected;

        //  -----------------
        //  ItemChanged event
        //  -----------------

        public event EventHandler<TableViewItemChangedEventArgs> ItemChanged;

        #endregion events

        #region private methods

        //  -------------------------------
        //  Notify_CollectionChanged method
        //  -------------------------------

        private void Notify_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (ItemChanged == null) return;

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        var args = new TableViewItemChangedEventArgs(TableViewItemChangeType.Insert, e.OldStartingIndex, e.NewStartingIndex);
                        for (int i = 0; i < e.NewItems.Count; i++)
                        {
                            ItemChanged(this, args);
                            args.NewIndexPath.Row++;
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    {
                        var args = new TableViewItemChangedEventArgs(TableViewItemChangeType.Delete, e.OldStartingIndex, e.NewStartingIndex);
                        for (int i = 0; i < e.OldItems.Count; i++)
                        {
                            ItemChanged(this, args);
                            args.OldIndexPath.Row++;
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    {
                        var args = new TableViewItemChangedEventArgs(TableViewItemChangeType.Update, e.OldStartingIndex, e.NewStartingIndex);
                        for (int i = 0; i < e.OldItems.Count; i++)
                        {
                            ItemChanged(this, args);
                            args.OldIndexPath.Row++;
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    throw new NotImplementedException();
                case NotifyCollectionChangedAction.Reset:
                    throw new NotImplementedException();
                default:
                    break;
            }
        }

        #endregion private methods
    }

    #region TableViewCell class

    //  -------------------
    //  TableViewCell class
    //  -------------------

    [SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface")]
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public class TableViewCell : UITableViewCell
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        public TableViewCell(UITableViewCellStyle style, string reuseIdentifier) : base(style, reuseIdentifier) { }

        #endregion construction

        #region overrides

        //  ------------------
        //  SetSelected method
        //  ------------------

        public override void SetSelected(bool selected, bool animated)
        {
            if (selected)
            {
                var style = UIDevice.CurrentDevice.CheckSystemVersion(7, 0) ?
                    UIActivityIndicatorViewStyle.Gray :
                    UIActivityIndicatorViewStyle.White;
                var accessoryView = new UIActivityIndicatorView(style);
                AccessoryView = accessoryView;
                accessoryView.StartAnimating();
            }
            else
            {
                AccessoryView = null;
            }
            base.SetSelected(selected, animated);
        }

        #endregion overrides
    }

    #endregion TableViewCell class

    #region INotifyTableViewSourceChanged interface

    //  ---------------------------------------
    //  INotifyTableViewSourceChanged interface
    //  ---------------------------------------

    internal interface INotifyTableViewSourceChanged
    {
        //  -----------------
        //  ItemChanged event
        //  -----------------

        event EventHandler<TableViewItemChangedEventArgs> ItemChanged;
    }

    #endregion INotifyTableViewSourceChanged interface

    #region TableViewItemChangeType enumeration

    //  -----------------------------------
    //  TableViewItemChangeType enumeration
    //  -----------------------------------

    public enum TableViewItemChangeType
    {
        Insert,
        Delete,
        Move,
        Update
    }

    #endregion TableViewItemChangeType enumeration

    #region TableViewItemChangedEventArgs class

    //  -----------------------------------
    //  TableViewItemChangedEventArgs class
    //  -----------------------------------

    public class TableViewItemChangedEventArgs : EventArgs
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        public TableViewItemChangedEventArgs(TableViewItemChangeType changeType, int oldIndex, int newIndex)
        {
            ChangeType = changeType;
            OldIndexPath = new IndexPath(0, oldIndex);
            NewIndexPath = new IndexPath(0, newIndex);
        }

        #endregion construction

        #region properties

        //  -------------------
        //  ChangeType property
        //  -------------------

        public TableViewItemChangeType ChangeType
        {
            get; private set;
        }

        //  ---------------------
        //  OldIndexPath property
        //  ---------------------

        public IndexPath OldIndexPath
        {
            get; private set;
        }

        //  ---------------------
        //  NewIndexPath property
        //  ---------------------

        public IndexPath NewIndexPath
        {
            get; private set;
        }

        #endregion properties
    }

    #endregion TableViewItemChangedEventArgs class

    #region TableViewSourceItemEventArgs<T> class

    //  -------------------------------------
    //  TableViewSourceItemEventArgs<T> class
    //  -------------------------------------

    public class TableViewSourceItemEventArgs<T> : EventArgs
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        public TableViewSourceItemEventArgs(T item)
        {
            Item = item;
        }

        #endregion construction

        #region properties

        //  -------------
        //  Item property
        //  -------------

        public T Item
        {
            get; private set;
        }

        #endregion properties
    }

    #endregion TableViewSourceItemEventArgs<T> class

    #region ITableViewItem interface

    //  ------------------------
    //  ITableViewItem interface
    //  ------------------------

    public interface ITableViewItem
    {
        //  --------------
        //  GetCell method
        //  --------------

        UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath);
    }

    #endregion ITableViewItem interface

    #region IndexPath class

    //  ---------------
    //  IndexPath class
    //  ---------------

    public class IndexPath
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        public IndexPath(int section, int row)
        {
            Section = section;
            Row = row;
        }

        #endregion construction

        #region properties

        //  ----------------
        //  Section property
        //  ----------------

        public int Section { get; set; }

        //  ------------
        //  Row property
        //  ------------

        public int Row { get; set; }

        #endregion properties

        #region methods

        //  --------------------
        //  ToNSIndexPath method
        //  --------------------

        public NSIndexPath ToNSIndexPath() { return NSIndexPath.FromRowSection(Row, Section); }

        #endregion methods
    }

    #endregion IndexPath class
}

// eof "TableViewSource.cs"

//
//  @(#) TableViewController.cs
//
//  Project:    usis Cocoa
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using Foundation;
using System;
using System.Diagnostics.CodeAnalysis;
using UIKit;
using usis.Framework;
using usis.Mobile;
using usis.Platform;

#pragma warning disable 1591

namespace usis.Cocoa.UIKit
{
    //  -------------------------
    //  TableViewController class
    //  -------------------------

    [SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface")]
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public class TableViewController : UITableViewController, IContextInjectable<IApplication>, IAppLayoutAttributes
    {
        #region fields

        private bool refreshEnabled;
        private bool refreshInBackground;

        #endregion fields

        #region properties

        //  -----------------------
        //  RefreshEnabled property
        //  -----------------------

        public bool RefreshEnabled
        {
            get => refreshEnabled;
            set
            {
                if (value == refreshEnabled) return;
                if (IsViewLoaded)
                {
                    if (value) CreateRefreshControl();
                    else RefreshControl = null;
                }
                refreshEnabled = value;
            }
        }

        //  ----------------------------
        //  RefreshInBackground property
        //  ----------------------------

        public bool RefreshInBackground
        {
            get => refreshInBackground;
            set
            {
                if (value == refreshInBackground) return;
                if (RefreshControl != null && RefreshControl.Refreshing)
                {
                    throw new InvalidOperationException();
                }
                refreshInBackground = value;
            }
        }

        //  -------------------
        //  Attributes property
        //  -------------------

        protected IValueStorage Attributes { get; private set; }

        #endregion properties

        #region methods

        //  ---------------------
        //  RefreshContent method
        //  ---------------------

        protected virtual void RefreshContent() { CompleteRefresh(); }

        //  ----------------------
        //  CompleteRefresh method
        //  ----------------------

        protected void CompleteRefresh() { InvokeOnMainThread(EndRefreshing); }

        //  --------------------------------
        //  AttachSourceChangeHandler method
        //  --------------------------------

        protected void AttachSourceChangeHandler()
        {
            if (TableView?.Source is INotifyTableViewSourceChanged notify)
            {
                notify.ItemChanged += Notify_ItemChanged;
            }
        }

        //  --------------------------------
        //  DetachSourceChangeHandler method
        //  --------------------------------

        protected void DetachSourceChangeHandler()
        {
            if (TableView?.Source is INotifyTableViewSourceChanged notify) notify.ItemChanged -= Notify_ItemChanged;
        }

        //  -------------------
        //  StartRefresh method
        //  -------------------

        protected void StartRefresh()
        {
            if (RefreshControl == null) return;
            if (RefreshControl.Refreshing) return;

            RefreshControl.BeginRefreshing();
            TableView.SetContentOffset(new CoreGraphics.CGPoint(0, TableView.ContentOffset.Y - RefreshControl.Frame.Size.Height), true);
            BeginRefresh();
        }

        #endregion methods

        #region overrides

        //  ------------------
        //  ViewDidLoad method
        //  ------------------

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
            {
                TableView.CellLayoutMarginsFollowReadableWidth = false;
            }

            if (refreshEnabled) CreateRefreshControl();

            AttachSourceChangeHandler();
        }

        //  --------------------
        //  ViewDidUnload method
        //  --------------------

        public override void ViewDidUnload()
        {
            DetachSourceChangeHandler();

            base.ViewDidUnload();
        }

        //  --------------------
        //  ViewDidAppear method
        //  --------------------

        private bool viewDidAppear;

        public override void ViewDidAppear(bool animated)
        {
            if (!viewDidAppear)
            {
                viewDidAppear = true;
                ViewDidFirstAppear(animated);
            }
            base.ViewDidAppear(animated);
        }

        //  -------------------------
        //  ViewDidFirstAppear method
        //  -------------------------

        protected virtual void ViewDidFirstAppear(bool animated) { }

        #endregion overrides

        #region private methods

        //  -------------------------
        //  Notify_ItemChanged method
        //  -------------------------

        private void Notify_ItemChanged(object sender, TableViewItemChangedEventArgs e)
        {
            InvokeOnMainThread(() => { Update(e); });
        }

        //  -------------
        //  Update method
        //  -------------

        private void Update(TableViewItemChangedEventArgs e)
        {
            switch (e.ChangeType)
            {
                case TableViewItemChangeType.Insert:
                    TableView.InsertRows(UITableViewRowAnimation.Fade, e.NewIndexPath.ToNSIndexPath());
                    break;
                case TableViewItemChangeType.Delete:
                    TableView.DeleteRows(UITableViewRowAnimation.Fade, e.OldIndexPath.ToNSIndexPath());
                    break;
                case TableViewItemChangeType.Move:
                    break;
                case TableViewItemChangeType.Update:
                    TableView.ReloadRows(UITableViewRowAnimation.None, e.OldIndexPath.ToNSIndexPath());
                    break;
                default:
                    break;
            }
        }

        //  ---------------------------
        //  CreateRefreshControl method
        //  ---------------------------

        private void CreateRefreshControl()
        {
            if (RefreshControl == null)
            {
                RefreshControl = new UIRefreshControl();
                RefreshControl.ValueChanged += (sender, e) => { BeginRefresh(); };
            }
        }

        //  -------------------
        //  BeginRefresh method
        //  -------------------

        private void BeginRefresh()
        {
            if (RefreshInBackground) InvokeInBackground(RefreshContent);
            else RefreshContent();
        }

        //  --------------------
        //  EndRefreshing method
        //  --------------------

        private void EndRefreshing() { RefreshControl.EndRefreshing(); }

        #endregion private methods

        #region IContextInjectable implementation

        //  ----------------
        //  Context property
        //  ----------------

        public IApplication Context { get; private set; }

        //  -------------
        //  Inject method
        //  -------------

        public void Inject(IApplication dependency)
        {
            if (Context != dependency)
            {
                Context = dependency;
                OnInject(dependency);
            }
        }

        //  ---------------
        //  OnInject method
        //  ---------------

        protected virtual void OnInject(IApplication application) { }

        #endregion IContextInjectable implementation

        #region IAppLayoutAttributes implementation

        //  --------------------
        //  SetAttributes method
        //  --------------------

        public virtual void SetAttributes(IValueStorage attributes)
        {
            Attributes = attributes;
            Title = attributes.GetString(nameof(Title));
            RefreshEnabled = attributes.Get(nameof(RefreshEnabled), false);
            RefreshInBackground = attributes.Get(nameof(RefreshInBackground), false);
        }

        #endregion IAppLayoutAttributes implementation
    }

    #region UITableViewExtension class

    //  --------------------------
    //  UITableViewExtension class
    //  --------------------------

    public static class UITableViewExtension
    {
        //  ----------------
        //  InsertRow method
        //  ----------------

        internal static void InsertRows(
            this UITableView tableView,
            UITableViewRowAnimation withRowAnimation,
            params NSIndexPath[] indexPath)
        {
            tableView.InsertRows(indexPath, withRowAnimation);
        }

        //  -----------------
        //  DeleteRows method
        //  -----------------

        internal static void DeleteRows(
            this UITableView tableView,
            UITableViewRowAnimation withRowAnimation,
            params NSIndexPath[] indexPath)
        {
            tableView.DeleteRows(indexPath, withRowAnimation);
        }

        //  -----------------
        //  ReloadRows method
        //  -----------------

        internal static void ReloadRows(
            this UITableView tableView,
            UITableViewRowAnimation withRowAnimation,
            params NSIndexPath[] indexPath)
        {
            tableView.ReloadRows(indexPath, withRowAnimation);
        }

        //  ---------------------
        //  ClearSelection method
        //  ---------------------

        public static void ClearSelection(this UITableView tableView, bool animated)
        {
            if (tableView == null) throw new ArgumentNullException(nameof(tableView));
            foreach (var indexPath in tableView.IndexPathsForSelectedRows)
            {
                tableView.DeselectRow(indexPath, animated);
            }
        }
    }

    #endregion UITableViewExtension class
}

// eof "TableViewController.cs"

//
//  @(#) ChannelListViewController.cs
//
//  Project:    PNRouter iOS App
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using System.Collections.Generic;
using Foundation;
using UIKit;
using usis.Cocoa.UIKit;
using usis.Platform;
using usis.PushNotification;

namespace usis.Cocoa.PNRouter
{
    //  -------------------------------
    //  ChannelListViewController class
    //  -------------------------------

    internal class ChannelListViewController : ListViewController<ApnsChannelInfo>
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        public ChannelListViewController()
        {
            // configuration button
            var infoButton = new UIButton(UIButtonType.InfoLight);
            infoButton.TouchUpInside += (sender, e) =>
            {
                var viewController = new ConfigurationViewController();
                this.PresentModalNavigationController(viewController, true);
            };
            NavigationItem.LeftBarButtonItem = new UIBarButtonItem(infoButton);

            // add channel button
            NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Add, (sender, e) =>
            {
                var viewController = new NewChannelDetailViewController();
                viewController.NavigationItem.LeftBarButtonItem = this.BarButtonItemCancelModalView();
                this.PresentModalNavigationController(viewController, true);
            });
        }

        #endregion construction

        #region overrides

        //  -------------------
        //  CreateSource method
        //  -------------------

        protected override UITableViewSource CreateSource()
        {
            var source = new Source(Items, Attributes);
            source.ItemSelected += (sender, e) =>
            {
                // item selected handler
                ItemSelectedCommand?.Execute(e.Item);
            };
            return source;
        }

        //  -------------------------
        //  ViewDidFirstAppear method
        //  -------------------------

        protected override void ViewDidFirstAppear(bool animated) { StartRefresh(); }

        //  --------------------
        //  SetAttributes method
        //  --------------------

        public override void SetAttributes(IValueStorage attributes)
        {
            base.SetAttributes(attributes);
            ItemSelectedCommand = attributes.GetCommand(nameof(ItemSelectedCommand), this);
        }

        #endregion overrides

        #region Source class

        //  ------------
        //  Source class
        //  ------------

        private class Source : TableViewSource<ApnsChannelInfo>
        {
            #region construction

            //  ------------
            //  construction
            //  ------------

            public Source(IEnumerable<ApnsChannelInfo> items, IValueStorage attributes) : base(items, attributes) { }

            #endregion construction

            #region overrides

            //  --------------------
            //  CustomizeCell method
            //  --------------------

            protected override void CustomizeCell(UITableView tableView, UITableViewCell cell, ApnsChannelInfo item)
            {
                cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
                cell.TextLabel.Text = item.Key.BundleId;
                cell.DetailTextLabel.Text = item.Key.Environment.ToString();
                if (!string.IsNullOrWhiteSpace(item.Description)) cell.DetailTextLabel.Text += $" - {item.Description}";
            }

            //  -----------------
            //  CanEditRow method
            //  -----------------

            public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath) { return true; }

            //  -------------------------
            //  CommitEditingStyle method
            //  -------------------------

            public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
            {
                var items = Items as IList<ApnsChannelInfo>;
                items?.Remove(ItemAt(indexPath));
            }
            
            #endregion overrides
        }

        #endregion Source class
    }
}

// eof "ChannelListViewController.cs"

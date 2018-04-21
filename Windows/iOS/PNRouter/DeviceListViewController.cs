//
//  @(#) DeviceListViewController.cs
//
//  Project:    PNRouter iOS App
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using System.Collections.Generic;
using UIKit;
using usis.Cocoa.UIKit;
using usis.PushNotification;

namespace usis.Cocoa.PNRouter
{
    //  ------------------------------
    //  DeviceListViewController class
    //  ------------------------------

    internal class DeviceListViewController : ListViewController<ApnsReceiverInfo>
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        public DeviceListViewController(IEnumerable<ApnsReceiverInfo> devices) : base(devices)
        {
            Title = Strings.DevicesListViewTitle;
        }

        #endregion construction

        #region overrides

        //  ------------------
        //  ViewDidLoad method
        //  ------------------

        public override void ViewDidLoad()
        {
            TableView.AllowsSelection = true;

            var source = new Source(Items);
            source.ItemSelected += (sender, e) =>
            {
                NavigationController.PushViewController(new DeviceDetailViewController(e.Item), true);
            };
            TableView.Source = source;
            base.ViewDidLoad();
        }

        #endregion overrides

        #region Source class

        //  ------------
        //  Source class
        //  ------------

        private class Source : TableViewSource<ApnsReceiverInfo>
        {
            #region construction

            //  ------------
            //  construction
            //  ------------

            public Source(IEnumerable<ApnsReceiverInfo> devices) : base(devices)
            {
                DefaultTableViewCellStyle = UITableViewCellStyle.Subtitle;
            }

            #endregion construction

            #region overrides

            //  --------------------
            //  CustomizeCell method
            //  --------------------

            protected override void CustomizeCell(UITableView tableView, UITableViewCell cell, ApnsReceiverInfo item)
            {
                cell.TextLabel.Text = item.Name;
                cell.DetailTextLabel.Text = item.DeviceToken.HexString;
                cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
            }

            #endregion overrides
        }

        #endregion Source class
    }
}

// eof "DeviceListViewController.cs"

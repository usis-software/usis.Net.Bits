//
//  @(#) ChannelDetailViewController.cs
//
//  Project:    PNRouter iOS App
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using UIKit;
using usis.Cocoa.UIKit;
using usis.Framework;
using usis.Platform.Data;
using usis.PushNotification;

namespace usis.Cocoa.PNRouter
{
    //  ---------------------------------
    //  ChannelDetailViewController class
    //  ---------------------------------

    internal class ChannelDetailViewController : DetailViewController
    {
        //  ------------
        //  construction
        //  ------------

        public ChannelDetailViewController(ApnsChannelInfo channel)
        {
            Title = channel.Key.BundleId;

            DetailViewTextRow devicesRow;

            Layout.AddSections(
                new DetailViewSection().AddRows(
                    new DetailViewTextRow(UITableViewCellStyle.Value1, "Bundle ID")
                    .SetBinding(nameof(DetailViewTextRow.DetailText), channel.Key, nameof(channel.Key.BundleId)),
                    new DetailViewTextRow(UITableViewCellStyle.Value1, "Environment")
                    .SetBinding(nameof(DetailViewTextRow.DetailText), channel.Key, nameof(channel.Key.Environment)))
                    .WithHeader("Channel"),
                new DetailViewSection().AddRows(
                    new DetailViewTextRow(UITableViewCellStyle.Value2, "Description")
                    .SetBinding(nameof(DetailViewTextRow.DetailText), channel, nameof(channel.Description)),
                    new DetailViewTextRow(UITableViewCellStyle.Value2, "Certificate")
                    .SetBinding(nameof(DetailViewTextRow.DetailText), channel, nameof(channel.CertificateThumbprint))),
                new DetailViewSection().AddRows(
                    devicesRow = new DetailViewTextRow(UITableViewCellStyle.Default, "Devices")));

            devicesRow.Accessory = UITableViewCellAccessory.DisclosureIndicator;
            devicesRow.SelectionStyle = UITableViewCellSelectionStyle.Blue;
            devicesRow.Selected += (sender, e) =>
            {
                Context.With<Model>().LoadDevices(channel.Key, (devices) =>
                {
                    InvokeOnMainThread(() =>
                    {
                        if (devices == null) TableView.ClearSelection(true);
                        else NavigationController.PushViewController(new DeviceListViewController(devices), true);
                    });
                });
            };
        }
    }

    internal class NewChannelDetailViewController : DetailViewController
    {
        public NewChannelDetailViewController()
        {
            Title = Strings.NewChannelDetailViewTitle;
        }
    }
}

// eof "ChannelDetailViewController.cs"

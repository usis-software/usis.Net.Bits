//
//  @(#) DeviceListViewController.cs
//
//  Project:    PNRouter iOS App
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using UIKit;
using usis.Cocoa.UIKit;
using usis.Platform.Data;
using usis.PushNotification;

namespace usis.Cocoa.PNRouter
{
    //  --------------------------------
    //  DeviceDetailViewController class
    //  --------------------------------

    internal class DeviceDetailViewController : DetailViewController
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        public DeviceDetailViewController(ApnsReceiverInfo device)
        {
            Title = device.Name;

            Layout.AddSections(
                new DetailViewSection().AddRows(
                    new DetailViewTextRow(UITableViewCellStyle.Value1, "Name")
                    .SetBinding("DetailText", device, nameof(ApnsReceiverInfo.Name)),
                    new DetailViewTextRow(UITableViewCellStyle.Value1, "Device Token")
                    .SetDetailTextBinding(device.DeviceToken, nameof(ApnsDeviceToken.HexString))),
                new DetailViewSection().AddRows(
                    new DetailViewTextRow(UITableViewCellStyle.Value1, "First Registration")
                    .SetDetailTextBinding(device, nameof(ApnsReceiverInfo.FirstRegistration)),
                    new DetailViewTextRow(UITableViewCellStyle.Value1, "Last Registration")
                    .SetDetailTextBinding(device, nameof(ApnsReceiverInfo.LastRegistration))),
                new DetailViewSection().AddRows(
                    new DetailViewTextRow(UITableViewCellStyle.Value1, "Account")
                    .SetDetailTextBinding(device, nameof(ApnsReceiverInfo.Account)),
                    new DetailViewTextRow(UITableViewCellStyle.Value1, "Groups")
                    .SetDetailTextBinding(device, nameof(ApnsReceiverInfo.Groups)),
                    new DetailViewTextRow(UITableViewCellStyle.Value1, "Info")
                    .SetDetailTextBinding(device, nameof(ApnsReceiverInfo.Info))));
        }

        #endregion construction
    }
}

// eof "DeviceListViewController.cs"

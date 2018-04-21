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

namespace usis.Cocoa.PNRouter
{
    //  ---------------------------------
    //  ConfigurationViewController class
    //  ---------------------------------

    internal sealed class ConfigurationViewController : DetailViewController
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        public ConfigurationViewController()
        {
            Layout.AddSections(
                new DetailViewSection().AddRows(
                    new DetailViewTextRow(UITableViewCellStyle.Value1, "Server"),
                    new DetailViewTextRow(UITableViewCellStyle.Value1, "Port"),
                    new DetailViewTextRow(UITableViewCellStyle.Value1, "Path")));
        }

        #endregion construction

        #region overrides

        //  ------------------
        //  ViewDidLoad method
        //  ------------------

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (sender, e) =>
            {
                DismissModalViewController(true);
            });
        }

        #endregion overrides
    }
}

// eof "ChannelDetailViewController.cs"

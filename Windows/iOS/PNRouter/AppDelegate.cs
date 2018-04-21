//
//  @(#) AppDelegate.cs
//
//  Project:    PNRouter iOS App
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using System.Diagnostics;
using Foundation;
using Newtonsoft.Json.Linq;
using UIKit;
using usis.Cocoa.Net;
using usis.Cocoa.PushNotification;
using usis.Cocoa.UIKit;

namespace usis.Cocoa.PNRouter
{
    //  -----------------
    //  AppDelegate class
    //  -----------------

    public class AppDelegate : Framework.USApplicationDelegate
    {
        #region overrides

        //  ------------------------
        //  FinishedLaunching method
        //  ------------------------

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            application.RegisterForRemoteNotification(NotificationTypes.Badge | NotificationTypes.Sound | NotificationTypes.Alert);

            return base.FinishedLaunching(application, launchOptions);
        }

        //  ---------------------------------------
        //  RegisteredForRemoteNotifications method
        //  ---------------------------------------

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            application.PerformNetworkOperation(new RegisterForRemoteNotificationNetworkOperation(deviceToken)
            {
                Name = UIDevice.CurrentDevice.Name,
                ReportException = false
            });
        }

        //  ---------------------------------
        //  ReceivedRemoteNotification method
        //  ---------------------------------

        public override void ReceivedRemoteNotification(UIApplication application, JObject userInfo)
        {
            var aps = userInfo["aps"];
            var alert = aps["alert"];

            Debug.Print(userInfo.ToString());

            UIWindow window = Window;
            UIViewController viewController = window.RootViewController;
            viewController.ShowAlert("Notification", alert.ToString());
        }

        #endregion overrides
    }
}

// eof "AppDelegate.cs"

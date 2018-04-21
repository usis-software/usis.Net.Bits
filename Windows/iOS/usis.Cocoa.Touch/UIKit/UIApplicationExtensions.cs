//
//  @(#) UIApplicationExtensions.cs
//
//  Project:    usis Cocoa
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using System;
using UIKit;

#pragma warning disable 1591

namespace usis.Cocoa.UIKit
{
    #region NotificationType enumeration

    //  ----------------------------
    //  NotificationType enumeration
    //  ----------------------------

    [Flags]
    public enum NotificationTypes
    {
        None = 0,
        Badge = 1,
        Sound = 2,
        Alert = 4
    }

    #endregion NotificationType enumeration

    //  -----------------------------
    //  UIApplicationExtensions class
    //  -----------------------------

    public static class UIApplicationExtensions
    {
        #region GetWindow method

        //  ----------------
        //  GetWindow method
        //  ----------------

        public static UIWindow GetWindow(this UIApplication application)
        {
            if (application == null) throw new ArgumentNullException(nameof(application));
            return application.Delegate.GetWindow();
        }

        #endregion GetWindow method

        #region RegisterForRemoteNotification method

        //  ------------------------------------
        //  RegisterForRemoteNotification method
        //  ------------------------------------

        public static void RegisterForRemoteNotification(this UIApplication application, NotificationTypes notificationType)
        {
            if (application == null) throw new ArgumentNullException(nameof(application));

            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                UIUserNotificationType type = (UIUserNotificationType)notificationType;
                var notificationSettings = UIUserNotificationSettings.GetSettingsForTypes(type, null);
                application.RegisterUserNotificationSettings(notificationSettings);
                application.RegisterForRemoteNotifications();
            }
            else
            {
                UIRemoteNotificationType type = (UIRemoteNotificationType)notificationType;
                application.RegisterForRemoteNotificationTypes(type);
            }
        }

        #endregion RegisterForRemoteNotification method
    }
}

// eof "UIApplicationExtensions.cs"

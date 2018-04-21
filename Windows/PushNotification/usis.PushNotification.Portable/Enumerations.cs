//
//  @(#) Enumerations.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

namespace usis.PushNotification
{
    #region ChannelType enumeration

    //  -----------------------
    //  ChannelType enumeration
    //  -----------------------    
    /// <summary>
    /// Specifies the type of the push notification channel.
    /// </summary>

    public enum ChannelType
    {
        /// <summary>
        /// Specifies an APNs channel.
        /// </summary>

        ApplePushNotificationService,

        /// <summary>
        /// Specifies a WNS channel.
        /// </summary>

        WindowsNotificationService,

        /// <summary>
        /// Specifies a GCM channel.
        /// </summary>

        GoogleCloudMessaging
    }

    #endregion ChannelType enumeration

    #region Environment enumeration

    //  -----------------------
    //  Environment enumeration
    //  -----------------------

    /// <summary>
    /// Specifies the environment in which to send push notifications.
    /// </summary>

    public enum Environment
    {
        /// <summary>
        /// Specifies the development environment.
        /// </summary>

        Development,

        /// <summary>
        /// Specifies the production environment.
        /// </summary>

        Production
    }

    #endregion Environment enumeration

    #region NotificationState enumeration

    //  -----------------------------
    //  NotificationState enumeration
    //  -----------------------------    

    /// <summary>
    /// Specifies the state of a push notification.
    /// </summary>

    public enum NotificationState
    {
        /// <summary>
        /// The notification was not yet sent to the notification service.
        /// </summary>

        Unsent,

        /// <summary>
        /// The notification is being send to the notification service.
        /// </summary>

        Pending,

        /// <summary>
        /// The notification was successfully sent to the notification service.
        /// </summary>

        Sent,

        /// <summary>
        /// An error occurred, sending the notification to the notification service.
        /// </summary>

        Failed
    }

    #endregion NotificationState enumeration
}

// eof "Enumerations.cs"

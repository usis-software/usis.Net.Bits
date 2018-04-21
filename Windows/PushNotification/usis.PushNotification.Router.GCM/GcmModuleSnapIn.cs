//
//  @(#) GcmModuleSnapIn.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2017 usis GmbH. All rights reserved.

using System.ComponentModel;

namespace usis.PushNotification
{
    //  ---------------------
    //  GcmModuleSnapIn class
    //  ---------------------

    /// <summary>
    /// Provides a snap-in to use Google Cloud Messaging
    /// to send notifications.
    /// </summary>
    /// <seealso cref="Framework.SnapIn" />

    public class GcmModuleSnapIn : Framework.SnapIn
    {
        //  -------------------
        //  OnConnecting method
        //  -------------------

        /// <summary>
        /// Raises the <see cref="Framework.SnapIn.Connecting" /> event.
        /// </summary>
        /// <param name="e">A <see cref="CancelEventArgs" /> object that contains the event data.</param>

        protected override void OnConnecting(CancelEventArgs e)
        {
            Model.RegisterPushServiceModel(ChannelType.GoogleCloudMessaging, new GcmModel());

            base.OnConnecting(e);
        }
    }
}

// eof "GcmModuleSnapIn.cs"

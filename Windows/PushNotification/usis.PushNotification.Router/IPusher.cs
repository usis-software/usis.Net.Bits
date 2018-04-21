//
//  @(#) IPusher.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2016 usis GmbH. All rights reserved.

using System;
using PushSharp.Core;

namespace usis.PushNotification
{
    //  -----------------
    //  IPusher interface
    //  -----------------

    /// <summary>
    /// Defines the members that a component must implement
    /// and that are used to send notifications via a push notification service.
    /// </summary>
    /// <seealso cref="IDisposable" />

    public interface IPusher : IDisposable
    {
        //  -----------------------
        //  SendNotification method
        //  -----------------------

        /// <summary>
        /// Sends a notification to the push service.
        /// </summary>
        /// <param name="context">The notification context.</param>

        void SendNotification(NotificationContext context);

        #region events

        //  ---------------------------
        //  NotificationSucceeded event
        //  ---------------------------

        /// <summary>
        /// Occurs when a notification was sent successfully.
        /// </summary>

        event EventHandler<NotificationEventArgs> NotificationSucceeded;

        //  ------------------------
        //  NotificationFailed event
        //  ------------------------

        /// <summary>
        /// Occurs when sending a notification failed.
        /// </summary>

        event EventHandler<NotificationExceptionEventArgs> NotificationFailed;

        //  ----------------------
        //  ConnectionFailed event
        //  ----------------------

        /// <summary>
        /// Occurs when establishing a connection to the push notification service failed.
        /// </summary>

        event EventHandler<NotificationExceptionEventArgs> ConnectionFailed;

        #endregion events
    }

    #region NotificationEventArgs class

    //  ---------------------------
    //  NotificationEventArgs class
    //  ---------------------------

    /// <summary>
    /// Contains push notification event data.
    /// </summary>
    /// <seealso cref="EventArgs" />

    public class NotificationEventArgs : EventArgs
    {
        //  -----------------------
        //  NotificationId property
        //  -----------------------

        /// <summary>
        /// Gets the notification identifier.
        /// </summary>
        /// <value>
        /// The notification identifier.
        /// </value>

        public Guid NotificationId { get; }

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationEventArgs" /> class
        /// with the specified notification.
        /// </summary>
        /// <param name="notification">The notification.</param>
        /// <exception cref="ArgumentNullException"><paramref name="notification" /> is a null reference.</exception>

        [CLSCompliant(false)]
        public NotificationEventArgs(INotification notification)
        {
            if (notification == null) throw new ArgumentNullException(nameof(notification));

            var id = notification.Tag as Guid?;
            if (id.HasValue) NotificationId = id.Value;
        }
    }

    #endregion NotificationEventArgs class

    #region NotificationExceptionEventArgs class

    //  ------------------------------------
    //  NotificationExceptionEventArgs class
    //  ------------------------------------

    /// <summary>
    /// Contains push notification event and exception data.
    /// </summary>
    /// <seealso cref="NotificationEventArgs" />

    public class NotificationExceptionEventArgs : NotificationEventArgs
    {
        //  ------------------
        //  Exception property
        //  ------------------

        /// <summary>
        /// Gets the exception.
        /// </summary>
        /// <value>
        /// The exception.
        /// </value>

        public Exception Exception { get; }

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationExceptionEventArgs"/> class
        /// with the specified notification and exception.
        /// </summary>
        /// <param name="notification">The notification.</param>
        /// <param name="exception">The exception.</param>

        [CLSCompliant(false)]
        public NotificationExceptionEventArgs(INotification notification, Exception exception) : base(notification)
        {
            Exception = exception;
        }
    }

    #endregion NotificationExceptionEventArgs class
}

// eof "IPusher.cs"

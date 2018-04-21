//
//  @(#) Router.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2017 usis GmbH. All rights reserved.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using usis.Framework;
using usis.Framework.Windows;
using usis.Platform;

namespace usis.PushNotification
{
    //  ------------
    //  Router class
    //  ------------

    /// <summary>
    /// Provides the usis Push Notification Router kernel functionality.
    /// </summary>
    /// <seealso cref="ApplicationExtension" />
    /// <seealso cref="IDisposable" />

    public sealed class Router : ApplicationExtension, IDisposable
    {
        #region fields and properties

        private Dictionary<Guid, IPusher> pushers = new Dictionary<Guid, IPusher>();
        private ConcurrentQueue<Guid> notificationQueue = new ConcurrentQueue<Guid>();

        private int checkQueueInterval = 10;
        private int keepChannelOpenInterval = 60 * 5;
        private int cleanupInterval = 60 * 60;
        private int notificationArchiveAge = 30;

        private EventWaitHandle stop = new EventWaitHandle(false, EventResetMode.ManualReset);
        private EventWaitHandle dequeue = new EventWaitHandle(false, EventResetMode.AutoReset);
        private Task dequeueTask;

        private System.Timers.Timer checkQueueTimer;
        private System.Timers.Timer cleanupTimer;

        private string archiveDirectory;

        private bool paused;

        //  --------------
        //  Model property
        //  --------------

        private Model Model { get; set; }

        #endregion fields and properties

        #region overrides

        //  ---------------
        //  OnAttach method
        //  ---------------

        /// <summary>
        /// Called when the extension is added to the
        /// <see cref="IExtensibleObject{TObject}.Extensions" /> property.
        /// </summary>

        protected override void OnAttach()
        {
            Model = Owner.Extensions.Find<Model>();

            var settings = Owner.Extensions.Find<RegistrySettings>();
            var interval = settings.LocalMachine.Get("CheckQueueInterval", checkQueueInterval);
            checkQueueInterval = interval == 0 ? checkQueueInterval : interval;
            keepChannelOpenInterval = settings.LocalMachine.Get("KeepChannelOpenInterval", keepChannelOpenInterval);
            cleanupInterval = settings.LocalMachine.Get("CleanupInterval", cleanupInterval);
            archiveDirectory = settings.LocalMachine.GetString("ArchiveDirectory");
            notificationArchiveAge = settings.LocalMachine.Get("NotificationArchiveAge", notificationArchiveAge);
        }

        //  --------------
        //  OnStart method
        //  --------------

        /// <summary>
        /// Called after all snap-ins of an application are loaded and connected.
        /// </summary>

        protected override void OnStart()
        {
            // start thread to dequeue notifications
            dequeueTask = Task.Factory.StartNew(Dequeue, TaskCreationOptions.LongRunning);

            // start "check queue" timer
            checkQueueTimer = new System.Timers.Timer(Convert.ToDouble(new TimeSpan(0, 0, checkQueueInterval).TotalMilliseconds));
            checkQueueTimer.Elapsed += CheckQueue;
            checkQueueTimer.AutoReset = false;
            checkQueueTimer.Start();

            // start "cleanup" timer
            if (cleanupInterval > 0)
            {
                cleanupTimer = new System.Timers.Timer(Convert.ToDouble(new TimeSpan(0, 0, cleanupInterval).TotalMilliseconds));
                cleanupTimer.Elapsed += Cleanup;
                cleanupTimer.AutoReset = false;
                cleanupTimer.Start();
            }
        }

        //  ---------------
        //  OnDetach method
        //  ---------------

        /// <summary>
        /// Called when an extension is removed from the
        /// <see cref="IExtensibleObject{TObject}.Extensions" /> property.
        /// </summary>

        protected override void OnDetach()
        {
            stop.Set();

            dequeueTask?.Wait();

            #region terminate cleanup

            if (cleanupInterval > 0)
            {
                Debug.WriteLineIf(!cleanupTimer.Enabled, "Cleanup in progress!");
                while (!cleanupTimer.Enabled)
                {
                    Debug.WriteLine("... waiting for cleanup to complete.");
                    Thread.Sleep(1000);
                }
                cleanupTimer.Stop();
            }

            #endregion terminate cleanup

            ClosePushers();
        }

        #endregion overrides

        #region IDisposable implementation

        //  --------------
        //  Dispose method
        //  --------------

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>

        public void Dispose()
        {
            if (checkQueueTimer != null) { checkQueueTimer.Dispose(); checkQueueTimer = null; }
            if (cleanupTimer != null) { cleanupTimer.Dispose(); cleanupTimer = null; }
            if (stop != null) { stop.Dispose(); stop = null; }
            if (dequeue != null) { dequeue.Dispose(); dequeue = null; }

            foreach (var sender in pushers)
            {
                if (sender.Value is IDisposable disposable) disposable.Dispose();
            }
        }

        #endregion IDisposable implementation

        #region EnqueueNotification method

        //  --------------------------
        //  EnqueueNotification method
        //  --------------------------

        /// <summary>
        /// Add the notification with the specified identifier to the send queue.
        /// </summary>
        /// <param name="notificationId">The notification identifier.</param>
        /// <returns>The notification identifier.</returns>
        /// <remarks>A call to this method also informs the dequeue thread that there's a notification pending.</remarks>

        public Guid EnqueueNotification(Guid notificationId)
        {
            EnqueueNotifications(notificationId);
            return notificationId;
        }

        #endregion EnqueueNotification method

        #region EnqueueNotifications method

        //  ---------------------------
        //  EnqueueNotifications method
        //  ---------------------------

        private void EnqueueNotifications(params Guid[] notifications)
        {
            foreach (var id in notifications) notificationQueue.Enqueue(id);

            lock (this)
            {
                if (!paused) dequeue?.Set();
            }
        }

        #endregion EnqueueNotifications method

        #region Dequeue / PushNotification

        //  --------------
        //  Dequeue method
        //  --------------

        private void Dequeue()
        {
            while (WaitHandle.WaitAny(new WaitHandle[] { stop, dequeue }) != 0)
            {
                Debug.WriteLine("Checking notification queue...");
                while (notificationQueue.TryDequeue(out Guid id))
                {
                    Debug.Print("Notification '{0}' dequeued.", id);
                    PushNotification(id);
                }
            }
            Debug.WriteLine("... dequeue thread stopped.");
        }

        //  -----------------------
        //  PushNotification method
        //  -----------------------

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void PushNotification(Guid notificationId)
        {
            var context = Model.RetrieveNotificationContext(notificationId);
            if (context != null && context.ChannelId.HasValue)
            {
                // find pusher; mark notification as failed, if not found
                var pusher = FindOrCreatePusher(context.ChannelId.Value);
                if (pusher == null) Model.MarkNotificationAsFailed(notificationId);
                else
                {
                    // try to send notification
                    try { pusher.SendNotification(context); }
                    catch (Exception exception)
                    {
                        Owner.ReportException(exception);
                        Model.MarkNotificationAsFailed(context.Notification.NotificationId);
                    }
                }
            }
        }

        #endregion Dequeue / PushNotification

        #region FindPusher method

        //  -------------------------
        //  FindOrCreatePusher method
        //  -------------------------

        private IPusher FindOrCreatePusher(Guid channelId)
        {
            lock (pushers)
            {
                // get pusher from dictionary
                if (!pushers.TryGetValue(channelId, out IPusher pusher))
                {
                    // no pusher active => create one
                    pusher = CreatePusher(channelId);
                }
                return pusher;
            }
        }

        #endregion FindPusher method

        #region CreatePusher method

        //  -------------------
        //  CreatePusher method
        //  -------------------

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private IPusher CreatePusher(Guid channelId)
        {
            var channel = Model.GetChannel(channelId);
            if (channel != null)
            {
                IPusher pusher;
                try
                {
                    //pusher = channel.CreatePusher(keepChannelOpenInterval);
                    pusher = Model.CreatePusher(channel, keepChannelOpenInterval);
                }
                catch (Exception exception)
                {
                    Owner.ReportException(exception);
                    return null;
                }
                pushers.Add(channelId, pusher);
                pusher.NotificationSucceeded += (object sender, NotificationEventArgs e) =>
                {
                    Model.SaveNotificationState(e.NotificationId, NotificationState.Sent);
                    Debug.WriteLine("Notification '{0}' sent.", e.NotificationId);
                };
                pusher.NotificationFailed += (object sender, NotificationExceptionEventArgs e) =>
                {
                    Model.MarkNotificationAsFailed(e.NotificationId);
                    Debug.Print("Notification '{0}' failed to send:\n - {1}", e.NotificationId, e.Exception);
                };
                pusher.ConnectionFailed += (object sender, NotificationExceptionEventArgs e) =>
                {
                    Model.SaveNotificationState(e.NotificationId, NotificationState.Unsent);
                    Owner.ReportException(new RouterException(string.Format(CultureInfo.CurrentCulture,
                        "Connection for notification '{0}' failed:\n - {1}", e.NotificationId, e.Exception),
                        e.Exception));
                };
                return pusher;
            }
            else return null;
        }

        #endregion CreatePusher method

        #region CheckQueue method

        //  -----------------
        //  CheckQueue method
        //  -----------------

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void CheckQueue(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (Model.IsDatabaseInitialized)
            {
                Trace.WriteLine("Looking for unsent notifications...");
                try
                {
                    foreach (var item in Model.UnsentNotifications()) EnqueueNotifications(item);
                }
                catch (Exception exception) { Owner.ReportException(exception); }
            }
            checkQueueTimer.Start();
        }

        #endregion CheckQueue method

        #region Cleanup method

        //  --------------
        //  Cleanup method
        //  --------------

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void Cleanup(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (Model.IsDatabaseInitialized)
            {
                Trace.WriteLine("Cleanup notifications...");
                try
                {
                    string path = null;
                    if (!string.IsNullOrWhiteSpace(archiveDirectory))
                    {
                        path = Path.Combine(archiveDirectory, string.Format(CultureInfo.InvariantCulture,
                            "Notifications.sent.{0:yyyy-MM-dd-hh-mm-ss-fffffff}.csv", DateTime.UtcNow));
                    }
                    int i = Model.ArchiveNotifications(NotificationState.Sent, notificationArchiveAge, path);

                    Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "{0} sent notifications archived.", i));
                }
                catch (Exception exception) { Owner.ReportException(exception); }
            }
            cleanupTimer.Start();
        }

        #endregion Cleanup method

        #region Pause method

        //  ------------
        //  Pause method
        //  ------------

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        internal void Pause()
        {
            Debug.WriteLine("Pausing router...");
            lock (this)
            {
                paused = true;
            }
            checkQueueTimer.Stop();
            try
            {
                ClosePushers();
            }
            catch (Exception exception)
            {
                Owner.ReportException(exception);
            }
        }

        #endregion Pause method

        #region ClosePushers method

        //  -------------------
        //  ClosePushers method
        //  -------------------

        private void ClosePushers()
        {
            lock (pushers)
            {
                var channelIds = new List<Guid>(pushers.Keys);
                foreach (var id in channelIds) ClosePusher(id);
            }
        }

        #endregion ClosePushers method

        #region ClosePusher method

        //  ------------------
        //  ClosePusher method
        //  ------------------

        private void ClosePusher(Guid channelId)
        {
            lock (pushers)
            {
                if (pushers.TryGetValue(channelId, out IPusher pusher))
                {
                    Debug.Print("Closing pusher '{0}'...", channelId);
                    pusher.Dispose();
                    pushers.Remove(channelId);
                }
            }
        }

        /// <summary>
        /// Closes the pusher with the speciified identifier.
        /// </summary>
        /// <param name="channelId">The channel identifier.</param>
        /// <returns>
        /// A boolean that indicates whether the channel was closed.
        /// </returns>

        public bool ClosePusher(Guid? channelId)
        {
            if (channelId.HasValue) ClosePusher(channelId.Value);
            return channelId.HasValue;
        }

        #endregion ClosePusher method

        #region Resume method

        //  -------------
        //  Resume method
        //  -------------

        internal void Resume()
        {
            Debug.WriteLine("Resuming router...");
            lock (this) { paused = false; }
            dequeue.Set();
            checkQueueTimer.Start();
        }

        #endregion Resume method

        #region Channel management methods

        #region UpdateChannel method

        //  --------------------
        //  UpdateChannel method
        //  --------------------

        /// <summary>
        /// Updates a channel with the specified channel information.
        /// </summary>
        /// <param name="channelInfo">The channel information.</param>

        public void UpdateChannel(IChannelInfo channelInfo)
        {
            ClosePusher(Model.UpdateChannel(channelInfo));
        }

        #endregion UpdateChannel method

        #region DeleteChannel method

        //  --------------------
        //  DeleteChannel method
        //  --------------------

        /// <summary>
        /// Deletes the channel with the specified key.
        /// </summary>
        /// <param name="channelKey">The channel key.</param>

        public void DeleteChannel(ChannelKey channelKey)
        {
            ClosePusher(Model.DeleteChannel(channelKey));
        }

        #endregion DeleteChannel method

        #endregion Channel management methods
    }
}

// eof "Router.cs"

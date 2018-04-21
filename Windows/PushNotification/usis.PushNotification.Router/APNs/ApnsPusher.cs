//
//  @(#) ApnsPusher.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2016 usis GmbH. All rights reserved.

using System;
using System.Diagnostics;
using System.Timers;
using Newtonsoft.Json.Linq;
using PushSharp.Apple;

namespace usis.PushNotification
{
    //  ----------------
    //  ApnsPusher class
    //  ----------------

    internal sealed class ApnsPusher : IPusher
    {
        #region fields

        private ApnsConfiguration configuration;
        private object lockObject = new object();
        private ApnsServiceBroker broker;

        private Timer timer = new Timer();

        #endregion fields

        #region construction/destruction

        //  ------------
        //  construction
        //  ------------

        internal ApnsPusher(ApnsChannel channel, int keepOpenInterval)
        {
            configuration = ApnsModel.CreateConfiguration(channel);

            timer.Interval = Convert.ToDouble(new TimeSpan(0, 0, keepOpenInterval).TotalMilliseconds);
            timer.AutoReset = false;
            timer.Elapsed += (sender, e) => { Close(); };
        }

        //  ---------
        //  finalizer
        //  ---------

        ~ApnsPusher() { Close(); }

        #endregion construction/destruction

        #region methods

        //  -----------
        //  Open method
        //  -----------

        private void Open()
        {
            lock (lockObject)
            {
                if (broker != null) return;
                if (configuration != null) broker = new ApnsServiceBroker(configuration);
                if (broker == null) return;
                broker.OnNotificationSucceeded += Broker_OnNotificationSucceeded;
                broker.OnNotificationFailed += Broker_OnNotificationFailed;
                broker.Start();
            }
        }

        //  ------------
        //  Close method
        //  ------------

        private void Close()
        {
            timer.Stop();
            lock (lockObject)
            {
                if (broker != null)
                {
                    broker.Stop();
                    while (!broker.IsCompleted)
                    {
                        Debug.Print("Waiting for broker to complete...");
                        System.Threading.Thread.Sleep(100);
                    }
                    broker.OnNotificationSucceeded -= Broker_OnNotificationSucceeded;
                    broker.OnNotificationFailed -= Broker_OnNotificationFailed;
                    broker = null;
                }
            }
        }

        #endregion methods

        #region event handlers

        //  -------------------------------------
        //  Broker_OnNotificationSucceeded method
        //  -------------------------------------

        private void Broker_OnNotificationSucceeded(PushSharp.Apple.ApnsNotification notification)
        {
            OnNotificationSucceeded(new NotificationEventArgs(notification));
        }

        //  ----------------------------------
        //  Broker_OnNotificationFailed method
        //  ----------------------------------

        private void Broker_OnNotificationFailed(PushSharp.Apple.ApnsNotification notification, AggregateException aggregateException)
        {
            aggregateException.Handle(exception =>
            {
                if (exception is ApnsConnectionException || exception.InnerException is ApnsConnectionException)
                {
                    OnConnectionFailed(new NotificationExceptionEventArgs(notification, exception));
                }
                else
                {
                    OnNotificationFailed(new NotificationExceptionEventArgs(notification, exception));
                }
                return true;
            });
        }

        #endregion event handlers

        #region events

        //  ---------------------------
        //  NotificationSucceeded event
        //  ---------------------------

        public event EventHandler<NotificationEventArgs> NotificationSucceeded;

        private void OnNotificationSucceeded(NotificationEventArgs e)
        {
            NotificationSucceeded?.Invoke(this, e);
        }

        //  ------------------------
        //  NotificationFailed event
        //  ------------------------

        public event EventHandler<NotificationExceptionEventArgs> NotificationFailed;

        private void OnNotificationFailed(NotificationExceptionEventArgs e)
        {
            NotificationFailed?.Invoke(this, e);
        }

        //  ----------------------
        //  ConnectionFailed event
        //  ----------------------

        public event EventHandler<NotificationExceptionEventArgs> ConnectionFailed;

        private void OnConnectionFailed(NotificationExceptionEventArgs e)
        {
            ConnectionFailed?.Invoke(this, e);
        }

        #endregion events

        #region SendNotification method

        //  -----------------------
        //  SendNotification method
        //  -----------------------

        void IPusher.SendNotification(NotificationContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            var receiver = context.Receiver as ApnsReceiver;
            if (receiver == null) throw new InvalidOperationException();

            SendAPNsNotification(context.Notification.NotificationId, receiver.DeviceToken, context.Notification.Payload);
        }

        //  ---------------------------
        //  SendAPNsNotification method
        //  ---------------------------

        private void SendAPNsNotification(Guid notificationId, string deviceToken, string payload)
        {
            timer.Stop();
            lock (lockObject)
            {
                Open(); if (broker == null) return;

                // Queue notification to send
                var notification = new PushSharp.Apple.ApnsNotification(deviceToken, JObject.Parse(payload));
                notification.Tag = notificationId;
                broker.QueueNotification(notification);
                Debug.WriteLine("Notification queued: deviceToken='{0}', payload='{1}'", deviceToken, payload);
            }
            timer.Start();
        }

        #endregion SendNotification method

        #region IDisposable implementation

        //  --------------
        //  Dispose method
        //  --------------

        void IDisposable.Dispose()
        {
            Close();
            if (timer != null) { timer.Dispose(); timer = null; }
            Debug.WriteLine("ApnsPusher disposed.");
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable implementation
    }
}

// eof "ApnsPusher.cs"

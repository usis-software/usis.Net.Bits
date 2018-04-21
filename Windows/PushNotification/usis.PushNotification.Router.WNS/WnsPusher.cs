//
//  @(#) WnsPusher.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2016 usis GmbH. All rights reserved.

using System;
using System.Diagnostics;
using System.Net.Http;
using System.Xml.Linq;
using PushSharp.Windows;

namespace usis.PushNotification
{
    //  ---------------
    //  WnsPusher class
    //  ---------------

    internal sealed class WnsPusher : IPusher
    {
        #region fields

        private WnsConfiguration configuration;
        private WnsServiceBroker serviceBroker;

        #endregion fields

        #region construction/destruction

        //  -----------
        //  constructor
        //  -----------

        internal WnsPusher(WnsChannel channel)
        {
            configuration = WnsModel.CreateConfiguration(channel);
        }

        //  ---------
        //  finalizer
        //  ---------

        ~WnsPusher()
        {
            Close();
            Debug.WriteLine("WnsPusher finalized.");
        }

        #endregion construction/destruction

        #region IPusher implementation

        //  -----------------------
        //  SendNotification method
        //  -----------------------

        void IPusher.SendNotification(NotificationContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            var receiver = context.Receiver as WnsReceiver;
            if (receiver == null) throw new InvalidOperationException();

            var broker = Open();
            if (broker == null) return;

            WnsNotification notification = null;
            var payload = XElement.Parse(context.Notification.Payload);
            var payloadType = payload?.Name.LocalName;
            if (string.Equals("toast", payloadType, StringComparison.Ordinal))
            {
                notification = new WnsToastNotification();
            }
            else if (string.Equals("badge", payloadType, StringComparison.Ordinal))
            {
                notification = new WnsBadgeNotification();
            }
            else if (string.Equals("tile", payloadType, StringComparison.Ordinal))
            {
                notification = new WnsTileNotification();
            }
            else throw new RouterException("Unknown notification payload format.");

            notification.Tag = context.Notification.NotificationId;
            notification.ChannelUri = receiver.ChannelUri;
            notification.Payload = payload;

            broker.QueueNotification(notification);
        }

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

        #endregion IPusher implementation

        #region private methods

        //  -----------
        //  Open method
        //  -----------

        private WnsServiceBroker Open()
        {
            lock (this)
            {
                if (serviceBroker == null && configuration != null)
                {
                    serviceBroker = new WnsServiceBroker(configuration);
                    serviceBroker.OnNotificationSucceeded += (notification) =>
                    {
                        OnNotificationSucceeded(new NotificationEventArgs(notification));
                    };
                    serviceBroker.OnNotificationFailed += (notification, aggregateException) =>
                    {
                        aggregateException.Handle(exception =>
                        {
                            if (exception is HttpRequestException)
                            {
                                OnConnectionFailed(new NotificationExceptionEventArgs(notification, exception));
                            }
                            else
                            {
                                OnNotificationFailed(new NotificationExceptionEventArgs(notification, exception));
                            }
                            return true;
                        });
                    };
                    serviceBroker.Start();
                }
                return serviceBroker;
            }
        }

        //  ------------
        //  Close method
        //  ------------

        private void Close()
        {
            lock (this)
            {
                if (serviceBroker != null)
                {
                    serviceBroker.Stop();
                    while (!serviceBroker.IsCompleted)
                    {
                        Debug.Print("Waiting for broker to complete...");
                        System.Threading.Thread.Sleep(100);
                    }
                    serviceBroker = null;
                }
            }
        }

        #endregion private methods

        #region IDisposable implementation

        //  --------------
        //  Dispose method
        //  --------------

        void IDisposable.Dispose()
        {
            Close();
            Debug.WriteLine("WNSPusher disposed.");
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable implementation
    }
}

// eof "WnsPusher.cs"

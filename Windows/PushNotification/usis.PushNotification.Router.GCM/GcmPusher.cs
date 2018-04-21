using System;
using System.Diagnostics;
using System.Net.Http;
using PushSharp.Google;

namespace usis.PushNotification
{
    //  ---------------
    //  GcmPusher class
    //  ---------------

    internal sealed class GcmPusher : IPusher
    {
        #region fields

        private GcmConfiguration configuration;
        private GcmServiceBroker serviceBroker;

        #endregion fields

        #region construction/destruction

        //  ------------
        //  construction
        //  ------------

        internal GcmPusher(GcmChannel channel)
        {
            configuration = GcmModel.CreateConfiguration(channel);
        }

        //  -----------
        //  destruction
        //  -----------

        ~GcmPusher()
        {
            Close();
            Debug.WriteLine("GcmPusher finalized.");
        }

        #endregion construction/destruction

        #region IPusher implementation

        void IPusher.SendNotification(NotificationContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            var receiver = context.Receiver as GcmReceiver;
            if (receiver == null) throw new InvalidOperationException();

            var broker = Open();
            if (broker == null) return;

            GcmNotification notification = null;
            //var payload = XElement.Parse(context.Notification.Payload);
            //var payloadType = payload?.Name.LocalName;
            //if (string.Equals("toast", payloadType, StringComparison.Ordinal))
            //{
            //    notification = new WnsToastNotification();
            //}
            //else if (string.Equals("badge", payloadType, StringComparison.Ordinal))
            //{
            //    notification = new WnsBadgeNotification();
            //}
            //else if (string.Equals("tile", payloadType, StringComparison.Ordinal))
            //{
            //    notification = new WnsTileNotification();
            //}
            //else throw new RouterException("Unknown notification payload format.");

            notification.Tag = context.Notification.NotificationId;
            //notification.ChannelUri = receiver.ChannelUri;
            //notification.Payload = payload;

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

        private GcmServiceBroker Open()
        {
            lock (this)
            {
                if (serviceBroker == null && configuration != null)
                {
                    serviceBroker = new GcmServiceBroker(configuration);
                    serviceBroker.OnNotificationSucceeded += (notification) =>
                    {
                        OnNotificationSucceeded(new NotificationEventArgs(notification));
                    };
                    serviceBroker.OnNotificationFailed += (notification, aggregateException) =>
                    {
                        aggregateException.Handle(exception =>
                        {
                            Debugger.Break();
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
            Debug.WriteLine("GcmPusher disposed.");
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable implementation
    }
}

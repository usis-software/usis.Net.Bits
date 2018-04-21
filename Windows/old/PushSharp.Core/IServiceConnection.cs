using System;
using System.Threading.Tasks;

namespace PushSharp.Core
{
    public interface IServiceConnection<TNotification> : IDisposable where TNotification : INotification
	{
		Task Send (TNotification notification);
	}

    public class NotificationEventArgs<TNotification> : EventArgs where TNotification : INotification
    {
        public TNotification Notification
        {
            get; set;
        }
        public NotificationEventArgs(TNotification notification)
        {
            Notification = notification;
        }
    }
    public class NotificationExceptionEventArgs<TNotification> : NotificationEventArgs<TNotification> where TNotification : INotification
    {
        public AggregateException Exception
        {
            get; set;
        }
        public NotificationExceptionEventArgs(TNotification notification, AggregateException exception) : base(notification)
        {
            Exception = exception;
        }
    }
}

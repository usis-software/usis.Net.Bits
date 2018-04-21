using System;

namespace PushSharp.Core
{
	public interface IServiceBroker<TNotification> where TNotification : INotification
	{
        event EventHandler<NotificationEventArgs<TNotification>> OnNotificationSucceeded;
        event EventHandler<NotificationExceptionEventArgs<TNotification>> OnNotificationFailed;

        System.Collections.Generic.IEnumerable<TNotification> TakeMany ();
		bool IsCompleted { get; }

        void NotificationSucceeded (TNotification notification);
        void NotificationFailed (TNotification notification, AggregateException exception);
	}
}


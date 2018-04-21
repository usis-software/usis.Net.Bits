using System;
using System.Runtime.Serialization;

namespace PushSharp.Core
{
    [Serializable]
    public class DeviceSubscriptionExpiredException : Exception
    {
        public DeviceSubscriptionExpiredException() : base("Device Subscription has Expired")
        {
            ExpiredAt = DateTime.UtcNow;
        }

        public string OldSubscriptionId { get; set; }
        public string NewSubscriptionId { get; set; }
        public DateTime ExpiredAt { get; set; }

        public DeviceSubscriptionExpiredException(string message) : this(message, null) { }

        public DeviceSubscriptionExpiredException(string message, Exception innerException) : base(message, innerException)
        {
            ExpiredAt = DateTime.UtcNow;
        }

        protected DeviceSubscriptionExpiredException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            OldSubscriptionId = info.GetString(nameof(OldSubscriptionId));
            NewSubscriptionId = info.GetString(nameof(NewSubscriptionId));
            ExpiredAt = info.GetDateTime(nameof(ExpiredAt));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null) throw new ArgumentNullException(nameof(info));

            info.AddValue(nameof(OldSubscriptionId), OldSubscriptionId);
            info.AddValue(nameof(NewSubscriptionId), NewSubscriptionId);
            info.AddValue(nameof(ExpiredAt), ExpiredAt);

            base.GetObjectData(info, context);
        }
    }

    [Serializable]
    public class NotificationException : Exception
    {
        public NotificationException() { }

        public NotificationException(string message) : base(message, null) { }

        public NotificationException(string message, Exception innerException) : base(message, innerException) { }

        public NotificationException(string message, INotification notification) : base(message)
        {
            Notification = notification;
        }

        public NotificationException(string message, INotification notification, Exception innerException)
            : base(message, innerException)
        {
            Notification = notification;
        }

        public INotification Notification { get; set; }

        protected NotificationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }

    [Serializable]
    public class RetryAfterException : Exception
    {
        public RetryAfterException() { }

        public RetryAfterException(string message) : base(message, null) { }

        public RetryAfterException(string message, Exception innerException) : base(message, innerException) { }

        public RetryAfterException(string message, DateTime retryAfterUtc) : base(message)
        {
            RetryAfterUtc = retryAfterUtc;
        }

        public DateTime RetryAfterUtc { get; set; }

        protected RetryAfterException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            RetryAfterUtc = info.GetDateTime(nameof(RetryAfterUtc));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null) throw new ArgumentNullException(nameof(info));

            info.AddValue(nameof(RetryAfterUtc), RetryAfterUtc);

            base.GetObjectData(info, context);
        }
    }
}


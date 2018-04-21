using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace PushSharp.Apple
{
    public enum ApnsNotificationErrorStatusCode
    {
        NoErrors = 0,
        ProcessingError = 1,
        MissingDeviceToken = 2,
        MissingTopic = 3,
        MissingPayload = 4,
        InvalidTokenSize = 5,
        InvalidTopicSize = 6,
        InvalidPayloadSize = 7,
        InvalidToken = 8,
        Shutdown = 10,
        Unknown = 255
    }

    [Serializable]
    public class ApnsNotificationException : Exception
    {
        public ApnsNotificationException() { }

        public ApnsNotificationException(string message) : base(message) { }

        public ApnsNotificationException(string message, Exception innerException) : base(message, innerException) { }

        public ApnsNotificationException(byte errorStatusCode, ApnsNotification notification)
            : this(ToErrorStatusCode(errorStatusCode), notification)
        { }

        public ApnsNotificationException (ApnsNotificationErrorStatusCode errorStatusCode, ApnsNotification notification)
            : base (string.Format(CultureInfo.CurrentCulture, "Apns notification error: '{0}'", errorStatusCode))
        {
            Notification = notification;
            ErrorStatusCode = errorStatusCode;
        }
        protected ApnsNotificationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            ErrorStatusCode = (ApnsNotificationErrorStatusCode)info.GetInt32(nameof(ErrorStatusCode));
            Notification = (ApnsNotification)info.GetValue(nameof(Notification), typeof(ApnsNotification));
        }
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null) throw new ArgumentNullException(nameof(info));

            info.AddValue(nameof(ErrorStatusCode), ErrorStatusCode);
            info.AddValue(nameof(Notification), Notification);

            base.GetObjectData(info, context);
        }

        public ApnsNotification Notification { get; set; }

        public ApnsNotificationErrorStatusCode ErrorStatusCode { get; private set; }
        
        private static ApnsNotificationErrorStatusCode ToErrorStatusCode(byte errorStatusCode)
        {
            var s = ApnsNotificationErrorStatusCode.Unknown;
            if (!Enum.TryParse<ApnsNotificationErrorStatusCode>(errorStatusCode.ToString(CultureInfo.InvariantCulture), out s)) return ApnsNotificationErrorStatusCode.Unknown;
            return s;
        }
    }

    [Serializable]
    public class ApnsConnectionException : Exception
    {
        public ApnsConnectionException() : base() { }

        public ApnsConnectionException (string message) : base (message)
        {
        }

        public ApnsConnectionException (string message, Exception innerException) : base (message, innerException)
        {
        }

        protected ApnsConnectionException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}

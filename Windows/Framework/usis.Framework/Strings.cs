using System;
using System.Collections.Generic;
using System.Text;

namespace usis.Framework
{
    internal static class Strings
    {
        internal const string FailedToConnectSnapIn = "Failed to connect snap-in '{0}': {1}";
        internal const string FailedToDisconnectSnapIn = "Failed to disconnect snap-in '{0}': {1}";
        internal const string FailedToLoadSnapIn = "Failed to load snap-in '{0}': {1}";
        internal const string FailedToPauseSnapIn = "Failed to pause snap-in '{0}': {1}";
        internal const string FailedToResumeSnapIn = "Failed to resume snap-in '{0}': {1}";
        internal const string HttpError = "HTTP Error {0} - {1}";
        internal const string SnapInInterfaceNotImplemented = "The type '{0}' does not implement the interface '{1}'.";
    }
}

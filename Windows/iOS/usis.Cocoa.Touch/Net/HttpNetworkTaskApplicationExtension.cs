using usis.Framework;

#pragma warning disable 1591

namespace usis.Cocoa.Net
{
    public class HttpNetworkTaskApplicationExtension : ApplicationExtension
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public HttpNetworkTask CreateNetworkTask()
        {
            return new HttpNetworkTask();
        }
    }
}

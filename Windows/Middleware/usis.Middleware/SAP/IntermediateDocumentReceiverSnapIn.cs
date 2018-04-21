#pragma warning disable 1591

using SAP.Middleware.Connector;
using System;
using System.ComponentModel;
using System.Diagnostics;
using usis.Framework;

namespace usis.Middleware.SAP
{
    public class IntermediateDocumentReceiverSnapIn : ServiceSnapIn
    {
        protected override void OnConnecting(CancelEventArgs e)
        {
            var extension = Application.UseExtension<RfcServerApplicationExtension>();
            extension.RegisterHandler("adis", typeof(IntermediateDocumentServer));
            base.OnConnecting(e);
        }
    }

    internal static class IntermediateDocumentServer
    {
        //[CLSCompliant(false)]
        [RfcServerFunction(Name = "IDOC_INBOUND_ASYNCHRONOUS")]
        public static void InboundAsynchronous(
            RfcServerContext context,
            IRfcFunction function)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (function == null) throw new ArgumentNullException(nameof(function));

            Debug.WriteLine("IDOC_INBOUND_ASYNCHRONOUS called...");
            System.Threading.Thread.Sleep(5000);
            Debug.WriteLine("IDOC_INBOUND_ASYNCHRONOUS returned.");
        }
    }
}

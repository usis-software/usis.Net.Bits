using SAP.Middleware.Connector;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using usis.Framework;

namespace usis.Middleware.SAP
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class TestSnapIn : ServiceSnapIn
    {
        protected override void OnConnecting(CancelEventArgs e)
        {
            Application.With<RfcServerApplicationExtension>(true).RegisterHandler("usis.RFC.Server", typeof(TestHandler));

            base.OnConnecting(e);
        }

        internal static class TestHandler
        {
            [RfcServerFunction(Name = "Z_TEST")]
            public static void Z_TEST(RfcServerContext context, IRfcFunction function)
            {
                if (context == null) throw new ArgumentNullException(nameof(context));
                if (function == null) throw new ArgumentNullException(nameof(function));

                Debug.WriteLine("Z_TEST called.");
            }
        }
    }
}

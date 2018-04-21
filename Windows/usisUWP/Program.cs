using System;
using System.ComponentModel;
using usis.Framework.Configuration;

namespace usis.Universal
{
    internal static class Program
    {
        internal static void Main()
        {
            Application.Run<App>(new ApplicationConfiguration(typeof(TestSnapIn)));

            System.Diagnostics.Debug.WriteLine("Exit.");
        }
    }

    internal class TestSnapIn : usis.Framework.SnapIn
    {
        protected override void OnConnecting(CancelEventArgs e)
        {
            base.OnConnecting(e);
        }
        protected override void OnConnected(EventArgs e)
        {
            base.OnConnected(e);
        }
        protected override void OnDisconnecting(CancelEventArgs e)
        {
            base.OnDisconnecting(e);
        }
        protected override void OnDisconnected(EventArgs e)
        {
            base.OnDisconnected(e);
        }
    }
}

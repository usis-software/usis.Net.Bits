using System;
using System.ComponentModel;
using usis.Platform.Windows;

namespace BitsTest
{
    //  -------------
    //  Service class
    //  -------------

    internal class Service : usis.Platform.Windows.Service, IDisposable
    {
        #region fields

        private BitsServer server = new BitsServer();

        #endregion fields

        #region construction

        /// <summary>
        /// Initializes a new instance of the <see cref="Service"/> class.
        /// </summary>

        public Service() : base("BITSServer", false) { }

        #endregion construction

        #region IDisposable implementation

        //  --------------
        //  Dispose method
        //  --------------

        public void Dispose()
        {
            if (server != null) { server.Dispose(); server = null; }
        }

        #endregion IDisposable implementation

        //  -----------
        //  Main method
        //  -----------

        internal static void Main(string[] args)
        {
            using (var service = new Service())
            {
                ServicesHost.StartServicesOrConsole(args, service);
            }
        }

        #region overrides

        //  -----------------
        //  OnStarting method
        //  -----------------

        protected override void OnStarting(CancelEventArgs e)
        {
            server.AddPrefix("http://*/bits/");
            server.Start();

            base.OnStarting(e);
        }

        //  -----------------
        //  OnStopping method
        //  -----------------

        protected override void OnStopping(CancelEventArgs e)
        {
            server.Stop();

            base.OnStopping(e);
        }

        #endregion overrides
    }
}

using System;
using System.ComponentModel;
using System.ServiceProcess;

namespace usis.Platform.Windows
{
    //  -------------
    //  Service class
    //  -------------

    /// <summary>
    /// Provides a base class for services that can be hosted by a <see cref="ServicesHost"/>
    /// by implementing the <see cref="IService"/> interface.
    /// </summary>
    /// <seealso cref="IService" />
    /// <seealso cref="ServicesHost"/>

    internal abstract class Service : IService
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="Service" /> class with a specified service name
        /// and a value that indicates whether the service can be paused and resumed.
        /// </summary>
        /// <param name="name">The name of the service.</param>
        /// <param name="canPauseAndContinue">if set to <c>true</c> the service can by paused and resumed.</param>

        protected Service(string name, bool canPauseAndContinue) { Name = name; CanPauseAndContinue = canPauseAndContinue; }

        #endregion construction

        #region properties

        //  -------------
        //  Name property
        //  -------------

        /// <summary>
        /// Gets the short name used to identify the service to the system.
        /// </summary>
        /// <value>
        /// The short name used to identify the service to the system.
        /// </value>

        public string Name { get; }

        //  ----------------------------
        //  CanPauseAndContinue property
        //  ----------------------------

        /// <summary>
        /// Gets a value indicating whether the service can be paused and resumed.
        /// </summary>
        /// <value>
        /// <c>true</c> if the service can pause and continue; otherwise, <c>false</c>.
        /// </value>

        public bool CanPauseAndContinue { get; protected set; }

        #endregion properties

        #region IService implementation

        //  -------------------------
        //  ConfigureInstaller method
        //  -------------------------

        /// <summary>
        /// Configures the specified installer.
        /// </summary>
        /// <param name="installer">The installer that is configured to install the service.</param>
        /// <exception cref="ArgumentNullException"><paramref name="installer" /> is a <c>null</c> reference.</exception>

        void IService.ConfigureInstaller(ServiceInstaller installer)
        {
            if (installer == null) throw new ArgumentNullException(nameof(installer));

            installer.ServiceName = Name;
        }

        //  --------------
        //  OnStart method
        //  --------------

        /// <summary>
        /// Called when the service starts.
        /// </summary>

        void IService.OnStart()
        {
            var e = new CancelEventArgs();
            OnStarting(e);
            if (e.Cancel) return;
            OnStarted(EventArgs.Empty);
        }

        //  --------------
        //  OnPause method
        //  --------------

        void IService.OnPause()
        {
            throw new NotImplementedException();
        }

        //  -----------------
        //  OnContinue method
        //  -----------------

        void IService.OnContinue()
        {
            throw new NotImplementedException();
        }

        //  -------------
        //  OnStop method
        //  -------------

        /// <summary>
        /// Called when service is stopped.
        /// </summary>

        void IService.OnStop()
        {
            var e = new CancelEventArgs();
            OnStopping(e);
            if (e.Cancel) return;
            OnStopped(EventArgs.Empty);
        }

        //  -----------------
        //  OnShutdown method
        //  -----------------

        void IService.OnShutdown()
        {
            throw new NotImplementedException();
        }

        #endregion IService implementation

        #region methods

        protected virtual void OnStarting(CancelEventArgs e) { }

        protected virtual void OnStarted(EventArgs e) { }

        protected virtual void OnStopping(CancelEventArgs e) { }

        protected virtual void OnStopped(EventArgs e) { }

        #endregion methods
    }
}

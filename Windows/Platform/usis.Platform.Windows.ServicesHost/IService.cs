//
//  @(#) IService.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

using System.Diagnostics;
using System.Globalization;
using System.ServiceProcess;

namespace usis.Platform.Windows
{
    //  ------------------
    //  IService interface
    //  ------------------

    /// <summary>
    /// Defines properties and methods so that an implementing class
    /// can be used as a Windows service.
    /// </summary>

    public interface IService
    {
        //  -------------
        //  Name property
        //  -------------

        /// <summary>
        /// Gets the short name used to identify the service to the system.
        /// </summary>
        /// <value>
        /// The short name used to identify the service to the system.
        /// </value>

        string Name { get; }

        //  ----------------------------
        //  CanPauseAndContinue property
        //  ----------------------------

        /// <summary>
        /// Gets a value indicating whether the service can be paused and resumed.
        /// </summary>
        /// <value>
        /// <c>true</c> if the service can pause and continue; otherwise, <c>false</c>.
        /// </value>

        bool CanPauseAndContinue { get; }

        //  --------------
        //  OnStart method
        //  --------------

        /// <summary>
        /// Called when the service starts.
        /// </summary>

        void OnStart();

        //  -------------
        //  OnStop method
        //  -------------

        /// <summary>
        /// Called when service is stopped.
        /// </summary>

        void OnStop();

        //  --------------
        //  OnPause method
        //  --------------

        /// <summary>
        /// Called when service receives a Pause command.
        /// </summary>

        void OnPause();

        //  -----------------
        //  OnContinue method
        //  -----------------

        /// <summary>
        /// Called when service receives a Continue command.
        /// </summary>

        void OnContinue();

        //  -----------------
        //  OnShutdown method
        //  -----------------

        /// <summary>
        /// Called when service receives a Shutdown command.
        /// </summary>

        void OnShutdown();

        //  -------------------------
        //  ConfigureInstaller method
        //  -------------------------

        /// <summary>
        /// Configures the specified installer.
        /// </summary>
        /// <param name="installer">
        /// The installer that is configured to install the service.
        /// </param>

        void ConfigureInstaller(ServiceInstaller installer);
    }

    #region WindowsService class

    //  --------------------
    //  WindowsService class
    //  --------------------

    internal class WindowsService
    {
        #region properties

        //  ----------------
        //  Service property
        //  ----------------

        public IService Service { get; private set; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        public WindowsService(IService service) { Service = service; }

        #endregion construction

        #region methods

        //  ------------
        //  Start method
        //  ------------

        public void Start()
        {
            Log.Print("Service '{0}' starting ...", Service.Name);
            Service.OnStart();
            Log.Print("... service '{0}' started.", Service.Name);
        }

        //  -----------
        //  Stop method
        //  -----------

        public void Stop()
        {
            Log.Print("Service '{0}' stopping ...", Service.Name);
            Service.OnStop();
            Log.Print("... service '{0}' stopped.", Service.Name);

            Trace.Flush();
        }

        //  ---------------
        //  Shutdown method
        //  ---------------

        public void Shutdown()
        {
            Log.Print("Service '{0}' shutting down ...", Service.Name);
            Service.OnShutdown();
            Log.Print("... service '{0}' shutted down.", Service.Name);

            Trace.Flush();
        }

        //  ------------
        //  Pause method
        //  ------------

        public void Pause()
        {
            Log.Print("Service '{0}' pausing ...", Service.Name);
            Service.OnPause();
            Log.Print("... service '{0}' paused.", Service.Name);

            Trace.Flush();
        }

        //  ---------------
        //  Continue method
        //  ---------------

        public void Continue()
        {
            Log.Print("Service '{0}' resuming ...", Service.Name);
            Service.OnContinue();
            Log.Print("... service '{0}' resumed.", Service.Name);

            Trace.Flush();
        }

        #endregion methods
    }

    #endregion WindowsService class

    #region Log class

    //  ---------
    //  Log class
    //  ---------

    internal static class Log
    {
        //  ------------
        //  Print method
        //  ------------

        public static void Print(string format, params object[] args)
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, format, args));
        }
    }

    #endregion Log class
}

// eof "IService.cs"

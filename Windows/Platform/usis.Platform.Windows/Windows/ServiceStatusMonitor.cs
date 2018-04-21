//
//  @(#) ServiceStatusMonitor.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using System;
using System.Linq;
using System.ServiceProcess;
using System.Timers;

namespace usis.Platform.Windows
{
    #region ServiceStatus enumeration

    //  -------------------------
    //  ServiceStatus enumeration
    //  -------------------------

    /// <summary>
    /// Indicates the current state of a Windows service.
    /// </summary>

    public enum ServiceStatus
    {
        /// <summary>
        /// An error occurred when accessing a system API.
        /// </summary>

        Error = -1,

        /// <summary>
        /// The service is not installed.
        /// </summary>

        NotInstalled = 0,

        /// <summary>
        /// The service is not running. This corresponds to the Win32 SERVICE_STOPPED constant, which is defined as 0x00000001.
        /// </summary>

        Stopped = 1,

        /// <summary>
        /// The service is starting. This corresponds to the Win32 SERVICE_START_PENDING constant, which is defined as 0x00000002.
        /// </summary>

        StartPending = 2,

        /// <summary>
        /// The service is stopping. This corresponds to the Win32 SERVICE_STOP_PENDING constant, which is defined as 0x00000003.
        /// </summary>

        StopPending = 3,

        /// <summary>
        /// The service is running. This corresponds to the Win32 SERVICE_RUNNING constant, which is defined as 0x00000004.
        /// </summary>

        Running = 4,

        /// <summary>
        /// The service continue is pending. This corresponds to the Win32 SERVICE_CONTINUE_PENDING constant, which is defined as 0x00000005.
        /// </summary>

        ContinuePending = 5,

        /// <summary>
        /// The service pause is pending. This corresponds to the Win32 SERVICE_PAUSE_PENDING constant, which is defined as 0x00000006.
        /// </summary>

        PausePending = 6,

        /// <summary>
        /// The service is paused. This corresponds to the Win32 SERVICE_PAUSED constant, which is defined as 0x00000007.
        /// </summary>

        Paused = 7
    }

    #endregion ServiceStatus enumeration

    //  --------------------------
    //  ServiceStatusMonitor class
    //  --------------------------

    /// <summary>
    /// Provides functionality to monitor state changes of a Windows service
    /// </summary>
    /// <seealso cref="IDisposable" />

    public sealed class ServiceStatusMonitor : IDisposable
    {
        #region fields

        private Timer timer;

        #endregion fields

        #region properties

        //  -------------------
        //  ServerName property
        //  -------------------

        /// <summary>
        /// Gets the name of the server.
        /// </summary>
        /// <value>
        /// The name of the server.
        /// </value>

        public string ServerName { get; }

        //  ------------------
        //  ServiceName method
        //  ------------------

        /// <summary>
        /// Gets the name of the service.
        /// </summary>
        /// <value>
        /// The name of the service.
        /// </value>

        public string ServiceName { get; }

        //  ---------------
        //  Status property
        //  ---------------

        /// <summary>
        /// Gets the status of the service.
        /// </summary>
        /// <value>
        /// The status of the service.
        /// </value>

        public ServiceStatus Status { get; private set; }

        #endregion properties

        #region events

        //  -------------------
        //  StatusChanged event
        //  -------------------

        /// <summary>
        /// Occurs when a change in the service's status has been detected.
        /// </summary>

        public event EventHandler<ServiceStatusEventArgs> StatusChanged;

        #endregion events

        #region construction/destruction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceStatusMonitor"/> class.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="interval">The interval in milliseconds between status update checks.</param>

        public ServiceStatusMonitor(string serviceName, int interval)
        {
            timer = new Timer(interval);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();

            ServerName = "localhost";
            ServiceName = serviceName;

            DeterminServiceStatus(false);
        }

        //  --------------
        //  Dispose method
        //  --------------

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>

        public void Dispose()
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
                timer = null;
            }
        }

        #endregion construction/destruction

        #region private methods

        //  --------------------
        //  Timer_Elapsed method
        //  --------------------

        private void Timer_Elapsed(object sender, ElapsedEventArgs e) { DeterminServiceStatus(true); }

        //  ----------------------------
        //  DeterminServiceStatus method
        //  ----------------------------

        private void DeterminServiceStatus(bool raiseEvent)
        {
            var status = Status;
            try
            {
                if (status == ServiceStatus.NotInstalled)
                {
                    if (ServiceController.GetServices(ServerName).FirstOrDefault(
                        (controller) => controller.ServiceName.Equals(
                            ServiceName, StringComparison.OrdinalIgnoreCase)) == null) return;
                }
                using (var controller = new ServiceController(ServiceName, ServerName))
                {
                    Status = (ServiceStatus)controller.Status;
                }
            }
            catch (InvalidOperationException)
            {
                Status = ServiceStatus.NotInstalled;
            }
            catch (System.ComponentModel.Win32Exception)
            {
                Status = ServiceStatus.Error;
            }
            if (raiseEvent)
            {
                var tmp = StatusChanged;
                if (status != Status && tmp != null) tmp(this, new ServiceStatusEventArgs(Status));
            }
        }

        #endregion private methods
    }

    #region ServiceStatusEventArgs class

    //  ----------------------------
    //  ServiceStatusEventArgs class
    //  ----------------------------

    /// <summary>
    /// Event arguments for the <see cref="ServiceStatusMonitor.StatusChanged"/> event;
    /// </summary>
    /// <seealso cref="EventArgs" />

    public class ServiceStatusEventArgs : EventArgs
    {
        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceStatusEventArgs"/> class
        /// with the specified service status.
        /// </summary>
        /// <param name="status">The status of the service.</param>

        public ServiceStatusEventArgs(ServiceStatus status) { Status = status; }

        //  ---------------
        //  Status property
        //  ---------------

        /// <summary>
        /// Gets the status of the service.
        /// </summary>
        /// <value>
        /// The status of the service.
        /// </value>

        public ServiceStatus Status { get; }
    }

    #endregion ServiceStatusEventArgs class
}

// eof "ServiceStatusMonitor.cs"

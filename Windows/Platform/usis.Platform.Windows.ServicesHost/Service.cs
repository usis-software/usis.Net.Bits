//
//  @(#) Service.cs
//
//  Project:    usis.Server
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2016 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.ServiceProcess;
using usis.Framework;
using usis.Framework.Configuration;
using usis.Framework.Portable;
using usis.Server;

namespace usis.ApplicationServer
{
    #region ApplicationCommand enumeration

    //  ------------------------------
    //  ApplicationCommand enumeration
    //  ------------------------------

    /// <summary>
    /// Indicates the command to send to an application.
    /// </summary>

    public enum ApplicationCommand
    {
        /// <summary>
        /// Starts an application.
        /// </summary>

        Start,

        /// <summary>
        /// Stops an application.
        /// </summary>

        Stop,

        /// <summary>
        /// Tells the application and its snap-ins to pause all operations.
        /// </summary>

        Pause,

        /// <summary>
        /// Tells the application and its snap-ins to resume all operations.
        /// </summary>

        Resume
    };

    #endregion ApplicationCommand enumeration

    //  -------------
    //  Service class
    //  -------------

    /// <summary>
    /// Implements a Windows service that provides the ability
    /// to host multiple applications.
    /// </summary>

    public class Service : IWindowsService
    {
        #region fields

        private List<HostedApplicationContext> applications = new List<HostedApplicationContext>();
        private Dictionary<Guid, HostedApplicationContext> instances = new Dictionary<Guid, HostedApplicationContext>();

        #endregion fields

        #region properties

        //  ---------------------
        //  Applications property
        //  ---------------------

        /// <summary>
        /// Gets an enumerator to iterate the applications that are hosted by the service.
        /// </summary>
        /// <value>
        /// An enumerator to iterate the applications that are hosted by the service.
        /// </value>

        public IEnumerable<ApplicationInstanceInfo> Applications
        {
            get
            {
                lock (instances)
                {
                    foreach (var application in applications)
                    {
                        UpdateApplicationInstanceInfo(application);

                        yield return application.Info;
                    }
                }
            }
        }

        #endregion properties

        #region protected methods

        //  ----------------
        //  Configure method
        //  ----------------

        /// <summary>
        /// Configures the service with the specified configuration.
        /// </summary>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <i>configuration</i> is a null reference (<b>Nothing</b> in Visual Basic).
        /// </exception>

        protected void Configure(ConfigurationRoot configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            lock (instances)
            {
                foreach (var appConfiguration in configuration.Applications)
                {
                    if (appConfiguration.Disabled) continue;

                    appConfiguration.ReadAppConfigFile();

                    applications.Add(new HostedApplicationContext()
                    {
                        Configuration = appConfiguration
                    });
                }

                #region debug code
#if DEBUG
                if (applications.Count > 1)
                {
                    Debug.Print("{0}: {1} applications configured.", Name, applications.Count);
                }
#endif
                #endregion debug code
            }
        }

        //  -----------------------------------
        //  ReadApplicationConfiguration method
        //  -----------------------------------

        /// <summary>
        /// Reads the application configuration.
        /// </summary>

        protected void ReadApplicationConfiguration()
        {
            var section = ConfigurationManager.GetSection("usis.Server.ApplicationService") as ApplicationServiceConfigurationSection;
            if (section == null) return;

            Configure(new ConfigurationRoot(section));
        }

        #endregion protected methods

        #region public methods

        //  ---------------------
        //  ExecuteCommand method
        //  ---------------------

        /// <summary>
        /// Send the specified command to an application.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <param name="instanceId">The instance identifier of an application that should execute the command.</param>
        /// <returns>
        /// An <b>OperationResult</b> with information about the execution.
        /// </returns>

        public OperationResult ExecuteCommand(ApplicationCommand command, Guid instanceId)
        {
            var result = new OperationResult(true);
            lock (instances)
            {
                HostedApplicationContext application = null;
                if (instances.TryGetValue(instanceId, out application))
                {
                    switch (command)
                    {
                        case ApplicationCommand.Start:
                            StartApplication(application);
                            break;
                        case ApplicationCommand.Stop:
                            StopApplication(application);
                            break;
                        case ApplicationCommand.Pause:
                            application.Proxy?.PauseOperation();
                            break;
                        case ApplicationCommand.Resume:
                            application.Proxy?.ResumeOperation();
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
                else result.ReportWarning(string.Format(CultureInfo.CurrentCulture, Strings.ApplicationNotFound, instanceId), 1);
            }
            return result;
        }

        //  ---------------------------------
        //  GetApplicationInstanceInfo method
        //  ---------------------------------

        /// <summary>
        /// Gets the instance information for the application with the specified identifier.
        /// </summary>
        /// <param name="instanceId">The instance identifier.</param>
        /// <returns>
        /// The instance information for the application with the specified identifier,
        /// or if there is no application with this identifier.
        /// </returns>

        public ApplicationInstanceInfo GetApplicationInstanceInfo(Guid instanceId)
        {
            lock (instances)
            {
                HostedApplicationContext application = null;
                if (instances.TryGetValue(instanceId, out application))
                {
                    UpdateApplicationInstanceInfo(application);
                }
                return application?.Info;
            }
        }

        #endregion public methods

        #region private method

        //  ------------------------------------
        //  UpdateApplicationInstanceInfo method
        //  ------------------------------------

        private void UpdateApplicationInstanceInfo(HostedApplicationContext application)
        {
            if (application.Info == null)
            {
                // create an instance information
                application.Info = new ApplicationInstanceInfo()
                {
                    Name = application.Configuration.Name
                };
                if (string.IsNullOrWhiteSpace(application.Info.Name))
                {
                    application.Info.Name = string.Format(
                        CultureInfo.InvariantCulture,
                        Strings.ApplicationName,
                        applications.IndexOf(application) + 1);
                }
                application.Info.Id = Guid.NewGuid();
                instances.Add(application.Info.Id, application);
            }
            // determine the current state of the application instance
            application.Info.State = DetermineApplicationState(application);
        }

        //  -----------------------
        //  StartApplication method
        //  -----------------------

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private static void StartApplication(HostedApplicationContext application)
        {
            if (application.Proxy != null) return;

            Debug.Assert(application.Configuration != null);
            Debug.Assert(application.AppDomain == null);

            try
            {
                if (string.IsNullOrWhiteSpace(application.Configuration.AppDomainName))
                {
                    application.Proxy = new HostedApplicationProxy();
                }
                else
                {
                    var setup = new AppDomainSetup();
                    setup.ApplicationBase = application.Configuration.AppBasePath;
                    //setup.ShadowCopyFiles = "true";
                    setup.ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
                    application.AppDomain = AppDomain.CreateDomain(application.Configuration.AppDomainName, null, setup);
                    application.Proxy = application.AppDomain.CreateInstanceAndUnwrap(
                        typeof(HostedApplicationProxy).Assembly.FullName,
                        typeof(HostedApplicationProxy).FullName) as HostedApplicationProxy;

                    Debug.Print("AppDomain '{0}' created.", application.AppDomain.FriendlyName);
                }
                application.Proxy.CreateApplication(application.Configuration);
                application.Proxy.Start();
            }
            catch (Exception exception)
            {
                Debug.Print("Failed to start application '{0}: {1}", application.Configuration.Name, exception.Message);
                StopApplication(application);
                application.Exception = exception;
            }
        }

        //  ----------------------
        //  StopApplication method
        //  ----------------------

        private static void StopApplication(HostedApplicationContext application)
        {
            if (application.Proxy != null)
            {
                application.Proxy.Stop();
                application.Proxy = null;
            }
            if (application.AppDomain != null)
            {
                var name = application.AppDomain.FriendlyName;
                AppDomain.Unload(application.AppDomain);
                Debug.Print("AppDomain '{0}' unloaded.", name);
                application.AppDomain = null;
            }
        }

        //  --------------------------------
        //  DetermineApplicationState method
        //  --------------------------------

        private static ApplicationInstanceState DetermineApplicationState(HostedApplicationContext application)
        {
            if (application.Exception != null) return ApplicationInstanceState.Failed;
            if (application.Proxy == null)
            {
                return ApplicationInstanceState.Stopped;
            }
            else if (application.Proxy.IsPaused)
            {
                return ApplicationInstanceState.Paused;
            }
            else return ApplicationInstanceState.Running;
        }

        #endregion private method

        #region IWindowsService methods

        #region Name property

        //  -------------
        //  Name property
        //  -------------

        /// <summary>
        /// Gets the short name used to identify the service to the system.
        /// </summary>
        /// <value>
        /// The short name used to identify the service to the system.
        /// </value>
        /// <remarks>
        /// Override this property in derived classes to provide a different
        /// serive name than the default name <c>usisAppSvc</c>.
        /// </remarks>

        public virtual string Name { get { return "usisAppSvc"; } }

        #endregion Name property

        #region CanPauseAndContinue property

        //  ----------------------------
        //  CanPauseAndContinue property
        //  ----------------------------

        /// <summary>
        /// Gets a value indicating whether the service can be paused and resumed.
        /// </summary>
        /// <value>
        /// <b>true</b> if the service can pause and continue; otherwise, <b>false</b>.
        /// </value>
        /// <remarks>
        /// An <see cref="Service"/> allways returns <b>true</b>.
        /// </remarks>

        public bool CanPauseAndContinue { get { return true; } }

        #endregion CanPauseAndContinue property

        #region OnStart method

        //  --------------
        //  OnStart method
        //  --------------

        /// <summary>
        /// Called when the service starts.
        /// </summary>

        public virtual void OnStart()
        {
            lock (instances) foreach (var application in applications) StartApplication(application);
        }

        #endregion OnStart method

        #region OnStop method

        //  -------------
        //  OnStop method
        //  -------------

        /// <summary>
        /// Called when service is stopped.
        /// </summary>

        public virtual void OnStop()
        {
            lock (instances)
            {
                foreach (var application in applications)
                {
                    StopApplication(application);
                }
                applications.Clear();
            }
        }

        #endregion OnStop method

        #region OnPause method

        //  --------------
        //  OnPause method
        //  --------------

        /// <summary>
        /// Called when service receives a Pause command.
        /// </summary>

        public void OnPause()
        {
            lock (instances)
            {
                foreach (var application in applications)
                {
                    application.Proxy.PauseOperation();
                }
            }
        }

        #endregion OnPause method

        #region OnContinue method

        //  -----------------
        //  OnContinue method
        //  -----------------

        /// <summary>
        /// Called when service receives a Continue command.
        /// </summary>

        public void OnContinue()
        {
            lock (instances)
            {
                foreach (var application in applications)
                {
                    application.Proxy.ResumeOperation();
                }
            }
        }

        #endregion OnContinue method

        #region OnShutdown method

        //  -----------------
        //  OnShutdown method
        //  -----------------

        /// <summary>
        /// Called when service receives a Shutdown command.
        /// </summary>

        public void OnShutdown() { OnStop(); }

        #endregion OnShutdown method

        #region ConfigureInstaller method

        //  -------------------------
        //  ConfigureInstaller method
        //  -------------------------

        /// <summary>
        /// Configures the specified installer.
        /// </summary>
        /// <param name="installer">
        /// The installer that is configured to install the service.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <i>installer</i> is a null reference (<b>Nothing</b> in Visual Basic).
        /// </exception>

        public virtual void ConfigureInstaller(ServiceInstaller installer)
        {
            if (installer == null) throw new ArgumentNullException(nameof(installer));

            installer.ServiceName = Name;
            installer.DisplayName = "usis Application Service";
        }

        #endregion ConfigureInstaller method

        #endregion IWindowsService methods

        #region HostedApplicationContext class

        //  ------------------------------
        //  HostedApplicationContext class
        //  ------------------------------

        private class HostedApplicationContext
        {
            internal Exception Exception { get; set; }

            internal ApplicationInstanceInfo Info { get; set; }

            internal ApplicationConfiguration Configuration { get; set; }

            internal HostedApplicationProxy Proxy { get; set; }

            internal AppDomain AppDomain { get; set; }
        }

        #endregion HostedApplicationContext class
    }

    #region ApplicationServiceConfigurationSection class

    //  --------------------------------------------
    //  ApplicationServiceConfigurationSection class
    //  --------------------------------------------

    internal class ApplicationServiceConfigurationSection : ConfigurationRootSection { }

    #endregion ApplicationServiceConfigurationSection class
}

// eof "Service.cs"

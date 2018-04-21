//
//  @(#) Service.cs
//
//  Project:    usis Application Server
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2018 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Xml.Serialization;
using usis.Framework;
using usis.Framework.Configuration;
using usis.Platform;
using usis.Platform.Windows;

namespace usis.ApplicationServer
{
    //  -------------
    //  Service class
    //  -------------

    /// <summary>
    /// Implements a Windows service that provides the ability
    /// to host multiple applications.
    /// </summary>
    /// <seealso cref="IService" />

    public class Service : IService
    {
        #region fields

        private Dictionary<Guid, HostedApplicationContext> instances = new Dictionary<Guid, HostedApplicationContext>();
        private Dictionary<Guid, HostedApplicationContext> pausedApplications = new Dictionary<Guid, HostedApplicationContext>();
        private HashSet<object> configFileObjects = new HashSet<object>();

        #endregion fields

        #region properties

        //  ---------------------
        //  Applications property
        //  ---------------------

        /// <summary>
        /// Gets an enumerator to iterate through the applications that are hosted by the service.
        /// </summary>
        /// <value>
        /// An enumerator to iterate through the applications that are hosted by the service.
        /// </value>

        public IEnumerable<ApplicationInstanceInfo> Applications
        {
            get
            {
                lock (instances)
                {
                    foreach (var application in instances.Values) yield return UpdateApplicationInstanceInfo(application);
                }
            }
        }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="Service" /> class.
        /// </summary>

        public Service() { Name = Assembly.GetEntryAssembly().GetName().Name; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Service" /> class
        /// with the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="configuration" /> is a <c>null</c> reference (<c>Nothing</c> in Visual Basic).
        /// </exception>

        public Service(ConfigurationRoot configuration) : this()
        {
            Configure(configuration);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Service" /> class
        /// with the specified service name and configuration.
        /// </summary>
        /// <param name="name">The short name used to identify the service to the system.</param>
        /// <param name="configuration">The configuration.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="configuration" /> is a <c>null</c> reference (<c>Nothing</c> in Visual Basic).
        /// </exception>

        public Service(string name, ConfigurationRoot configuration)
        {
            Name = name;
            Configure(configuration);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Service" /> class
        /// with the specified snap-in types.
        /// </summary>
        /// <param name="snapInTypes">The snap-in types.</param>

        public Service(params Type[] snapInTypes) : this(new ConfigurationRoot(snapInTypes)) { }

        #endregion construction

        #region protected methods

        //  ----------------
        //  Configure method
        //  ----------------

        /// <summary>
        /// Configures the service with the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="configuration" /> is a <c>null</c> reference (<c>Nothing</c> in Visual Basic).
        /// </exception>

        protected void Configure(ConfigurationRoot configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            lock (instances)
            {
                foreach (var appConfiguration in configuration.Applications)
                {
                    foreach (var snapIn in appConfiguration.SnapIns) configFileObjects.Add(snapIn);
                    foreach (var property in appConfiguration.Properties) configFileObjects.Add(property);

                    ReadAppConfigFile(appConfiguration);

                    var context = new HostedApplicationContext(appConfiguration);
                    instances.Add(context.Configuration.Id, context);
                }
            }
        }

        /// <summary>
        /// Configures the service with the specified application configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>

        protected void Configure(ApplicationConfiguration configuration)
        {
            var root = new ConfigurationRoot();
            root.Applications.Add(configuration);
            Configure(root);
        }

        //  --------------------------
        //  CreateConfiguration method
        //  --------------------------

        /// <summary>
        /// Creates a root configuration from the hosted application configurations.
        /// </summary>
        /// <returns>
        /// A newly created root configuration from the hosted application configurations.
        /// </returns>

        protected ConfigurationRoot CreateConfiguration()
        {
            lock (instances)
            {
                var configuration = new ConfigurationRoot();
                foreach (var application in instances.Values)
                {
                    var appConfig = new ApplicationConfiguration()
                    {
                        // copy properties
                        Name = application.Configuration.Name,
                        Id = application.Configuration.Id,
                        AppDomainName = application.Configuration.AppDomainName,
                        ApplicationBase = application.Configuration.ApplicationBase,
                        ConfigurationFile = application.Configuration.ConfigurationFile,
                        StartType = application.Configuration.StartType,
                        AppConfigFile = application.Configuration.AppConfigFile
                    };
                    foreach (var snapIn in application.Configuration.SnapIns.Cast<SnapInConfiguration>())
                    {
                        if (configFileObjects.Contains(snapIn)) appConfig.SnapInsInternal.Add(snapIn);
                    }
                    foreach (var property in application.Configuration.Properties.Cast<Framework.Configuration.ConfigurationProperty>())
                    {
                        if (configFileObjects.Contains(property)) appConfig.PropertiesInternal.Add(property);
                    }
                    configuration.Applications.Add(appConfig);
                }
                return configuration;
            }
        }

        //  -----------------------------------
        //  ReadApplicationConfiguration method
        //  -----------------------------------

        /// <summary>
        /// Reads the configuration from the application configuration file.
        /// </summary>

        protected void ReadApplicationConfiguration()
        {
            var section = ConfigurationManager.GetSection(Constants.ApplicationConfigurationFileSection) as ApplicationServiceConfigurationSection;
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
        /// <param name="instanceId">The instance identifier of an application that should execute the command.</param>
        /// <param name="command">The command to execute.</param>
        /// <exception cref="NotImplementedException">The specified command is not implemented.</exception>

        public void ExecuteCommand(Guid instanceId, ApplicationCommand command)
        {
            lock (instances)
            {
                if (instances.TryGetValue(instanceId, out HostedApplicationContext context))
                {
                    switch (command)
                    {
                        case ApplicationCommand.Start:
                            StartApplication(context);
                            break;
                        case ApplicationCommand.Stop:
                            StopApplication(context);
                            break;
                        case ApplicationCommand.Pause:
                            context.Proxy?.PauseOperation();
                            break;
                        case ApplicationCommand.Resume:
                            context.Proxy?.ResumeOperation();
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
                else throw CreateInstanceNotFoundException(instanceId);
            }
        }

        //  ---------------------------------
        //  GetApplicationInstanceInfo method
        //  ---------------------------------

        /// <summary>
        /// Gets the instance information for the application with the specified identifier.
        /// </summary>
        /// <param name="instanceId">The instance identifier.</param>
        /// <returns>
        /// The instance information for the application with the specified identifier.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// An application with the specified instance identifier was not found on the server.
        /// </exception>

        public ApplicationInstanceInfo GetApplicationInstanceInfo(Guid instanceId)
        {
            lock (instances)
            {
                if (instances.TryGetValue(instanceId, out HostedApplicationContext context))
                {
                    return UpdateApplicationInstanceInfo(context);
                }
                else throw CreateInstanceNotFoundException(instanceId);
            }
        }

        //  -----------------
        //  FromSnapIn method
        //  -----------------

        /// <summary>
        /// Creates an application server instance with the specified snap-in.
        /// </summary>
        /// <typeparam name="TSnapIn">The type of the snap-in.</typeparam>
        /// <returns>
        /// A newly created application server instance.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This methods creates an application service instance that is initialized with one application configuration
        /// which connects the snap-in that is specified by the type parameter.
        /// </para>
        /// <para>
        /// This is a convenience method, that is implemented as follows:
        /// </para>
        /// <code>
        /// public static Service FromSnapIn&lt;TSnapIn&gt;() where TSnapIn : ISnapIn
        /// {
        ///     return new Service(new ConfigurationRoot(typeof(TSnapIn)));
        /// }
        /// </code>
        /// </remarks>

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static Service FromSnapIn<TSnapIn>() where TSnapIn : ISnapIn
        {
            return new Service(new ConfigurationRoot(typeof(TSnapIn)));
        }

        #endregion public methods

        #region private methods

        //  ------------------------
        //  ReadAppConfigFile method
        //  ------------------------

        private static void ReadAppConfigFile(ApplicationConfiguration config)
        {
            if (string.IsNullOrWhiteSpace(config.AppConfigFile)) return;

            var filePath = config.AppConfigFile;
            if (!string.IsNullOrWhiteSpace(config.ApplicationBase)) filePath = Path.Combine(config.ApplicationBase, filePath);
            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    var serializer = new XmlSerializer(typeof(ApplicationConfiguration));
                    if (serializer.Deserialize(reader) is ApplicationConfiguration configuration)
                    {
                        // replace properties (attributes)
                        if (configuration.Name != null) config.Name = configuration.Name;
                        if (configuration.AppDomainName != null) config.AppDomainName = configuration.AppDomainName;

                        // merge property dictionary and snap-ins
                        foreach (var property in configuration.PropertiesInternal) config.PropertiesInternal.Add(property);
                        foreach (var snapIn in configuration.SnapInsInternal) config.SnapInsInternal.Add(snapIn);
                    }
                }
            }
        }

        //  --------------------------------------
        //  CreateInstanceNotFoundException method
        //  --------------------------------------

        private static Exception CreateInstanceNotFoundException(Guid instanceId)
        {
            return new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.ApplicationNotFound, instanceId));
        }

        //  ------------------------------------
        //  UpdateApplicationInstanceInfo method
        //  ------------------------------------

        private static ApplicationInstanceInfo UpdateApplicationInstanceInfo(HostedApplicationContext context)
        {
            if (context.Info == null)
            {
                // create an instance information
                context.Info = new ApplicationInstanceInfo()
                {
                    Name = context.Configuration.Name,
                    Id = context.Configuration.Id
                };
                if (string.IsNullOrWhiteSpace(context.Info.Name))
                {
                    context.Info.Name = string.Format(
                        CultureInfo.InvariantCulture,
                        Strings.ApplicationName,
                        context.Info.Id.ToString());
                }
            }
            // determine the current state of the application instance
            context.Info.State = DetermineApplicationState(context);

            return context.Info;
        }

        //  -----------------------
        //  StartApplication method
        //  -----------------------

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private static void StartApplication(HostedApplicationContext context)
        {
            if (context.Proxy != null) return;

            Debug.Assert(context.Configuration != null);
            Debug.Assert(context.AppDomain == null);

            try
            {
                if (string.IsNullOrWhiteSpace(context.Configuration.AppDomainName))
                {
                    context.Proxy = new ApplicationProxy();
                }
                else
                {
                    var setup = new AppDomainSetup()
                    {
                        ApplicationBase = context.Configuration.ApplicationBase,
                        ConfigurationFile = context.Configuration.ConfigurationFile,
                        DisallowCodeDownload = true,
                        //ShadowCopyFiles = "true",
                    };
                    context.AppDomain = AppDomain.CreateDomain(context.Configuration.AppDomainName, null, setup);

                    Debug.Print("AppDomain '{0}' created; configuration file: {1}",
                        context.AppDomain.FriendlyName,
                        context.AppDomain.SetupInformation.ConfigurationFile);

                    context.Proxy = context.AppDomain.CreateInstanceAndUnwrap(
                        typeof(ApplicationProxy).Assembly.FullName,
                        typeof(ApplicationProxy).FullName) as ApplicationProxy;
                }
                context.Proxy.CreateApplication(context.Configuration.ToXml());
                context.Proxy.Start();
            }
            catch (Exception exception)
            {
                Debug.Print("Failed to start application '{0}: {1}", context.Configuration.Name, exception.Message);
                StopApplication(context);
                context.Exception = exception;
            }
        }

        //  ----------------------
        //  StopApplication method
        //  ----------------------

        private static void StopApplication(HostedApplicationContext context)
        {
            if (context.Proxy != null)
            {
                context.Proxy.Stop();
                context.Proxy = null;
            }
            if (context.AppDomain != null)
            {
                var name = context.AppDomain.FriendlyName;
                AppDomain.Unload(context.AppDomain);
                Debug.Print("AppDomain '{0}' unloaded.", name);
                context.AppDomain = null;
            }
        }

        //  --------------------------------
        //  DetermineApplicationState method
        //  --------------------------------

        private static ApplicationInstanceState DetermineApplicationState(HostedApplicationContext context)
        {
            if (context.Configuration.StartType == ApplicationStartMode.Disabled) return ApplicationInstanceState.Disabled;
            if (context.Exception != null) return ApplicationInstanceState.Failed;
            if (context.Proxy == null)
            {
                return ApplicationInstanceState.Stopped;
            }
            else if (context.Proxy.IsPaused)
            {
                return ApplicationInstanceState.Paused;
            }
            else return ApplicationInstanceState.Running;
        }

        #endregion private methods

        #region IWindowsService implementation

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
        /// Override this property in derived classes to provide a different serive name than the default name.
        /// The default name is the simple name of the service's entry assembly.
        /// </remarks>

        public virtual string Name { get; }

        #endregion Name property

        #region CanPauseAndContinue property

        //  ----------------------------
        //  CanPauseAndContinue property
        //  ----------------------------

        /// <summary>
        /// Gets a value indicating whether the service can be paused and resumed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the service can pause and continue; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// An <see cref="Service" /> allways returns <c>true</c>.
        /// </remarks>

        public bool CanPauseAndContinue => true;

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
            lock (instances)
            {
                #region debug code
#if DEBUG
                if (instances.Values.Count > 1)
                {
                    Debug.Print("{0}: {1} applications configured.", Name, instances.Values.Count);
                }
#endif
                #endregion debug code

                Parallel.ForEach(instances.Values.Where(
                    (context) => context.Configuration.StartType == ApplicationStartMode.Automatic),
                    (context) => StartApplication(context));
            }
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
                Parallel.ForEach(instances.Values.Where(
                    (context) => context.Configuration.StartType != ApplicationStartMode.Disabled),
                    (context) => StopApplication(context));
                instances.Clear();
                configFileObjects.Clear();
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
                Parallel.ForEach(instances.Values.Where(
                    (context) => DetermineApplicationState(context) == ApplicationInstanceState.Running),
                    (context) =>
                    {
                        lock (pausedApplications)
                        {
                            context.Proxy.PauseOperation();
                            pausedApplications.Add(context.Configuration.Id, context);
                        }
                    });
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
                Parallel.ForEach(pausedApplications.Values, (context) =>
                {
                    lock (pausedApplications)
                    {
                        context.Proxy.ResumeOperation();
                        pausedApplications.Remove(context.Configuration.Id);
                    }
                });
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
        /// <paramref name="installer"/> is a null reference (<b>Nothing</b> in Visual Basic).
        /// </exception>

        public virtual void ConfigureInstaller(ServiceInstaller installer)
        {
            if (installer == null) throw new ArgumentNullException(nameof(installer));

            installer.ServiceName = Name;
            installer.DisplayName = Strings.ApplicationServiceDisplayName;
        }

        #endregion ConfigureInstaller method

        #endregion IWindowsService implementation

        #region HostedApplicationContext class

        //  ------------------------------
        //  HostedApplicationContext class
        //  ------------------------------

        private class HostedApplicationContext
        {
            #region construction

            //  ------------
            //  construction
            //  ------------

            internal HostedApplicationContext(ApplicationConfiguration configuration)
            {
                Configuration = configuration;

                if (Configuration.Id == Guid.Empty) Configuration.Id = Guid.NewGuid();
            }

            #endregion construction

            #region properties

            internal ApplicationConfiguration Configuration { get; }

            internal Exception Exception { get; set; }

            internal ApplicationInstanceInfo Info { get; set; }

            internal ApplicationProxy Proxy { get; set; }

            internal AppDomain AppDomain { get; set; }

            #endregion properties
        }

        #endregion HostedApplicationContext class
    }
}

// eof "Service.cs"

//
//  @(#) PNRouterService.cs
//
//  Project:    usisPNRouter
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

using System;
using System.Reflection;
using System.ServiceProcess;
using usis.ApplicationServer;
using usis.Framework;
using usis.Framework.Configuration;
using usis.Platform;

namespace usis.PushNotification
{
    //  ---------------------
    //  PNRouterService class
    //  ---------------------

    /// <summary>
    /// Represents the usis Push Notification Router Windows service.
    /// </summary>
    /// <seealso cref="Service" />

    internal class PNRouterService : Service
    {
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
        /// <remarks>
        /// The name of the usis Push Notification Router service is <c>usisPNRouter</c>.
        /// </remarks>

        public override string Name => Constants.ServiceName;

        #endregion properties

        #region OnStart method

        //  --------------
        //  OnStart method
        //  --------------

        /// <summary>
        /// Called when the service starts.
        /// </summary>

        public override void OnStart()
        {
            var configuration = new ApplicationConfiguration(typeof(SnapIn));
            Application.ReadConfigurationFile(configuration, "usis.PushNotification.Router");
            Configure(configuration);

            base.OnStart();
        }

        #endregion OnStart method

        #region ConfigureInstaller method

        //  -------------------------
        //  ConfigureInstaller method
        //  -------------------------

        public override void ConfigureInstaller(ServiceInstaller installer)
        {
            if (installer == null) throw new ArgumentNullException(nameof(installer));

            var assembly = Assembly.GetExecutingAssembly();

            installer.ServiceName = Name;
            installer.DisplayName = assembly.GetTitle();
            installer.Description = assembly.GetDescription();
        }

        #endregion ConfigureInstaller method
    }
}

// eof "PNRouterService.cs"

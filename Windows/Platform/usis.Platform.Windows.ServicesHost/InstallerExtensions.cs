//
//  @(#) InstallerExtensions.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015-2017 usis GmbH. All rights reserved.

using System;
using System.Configuration.Install;
using System.ServiceProcess;

namespace usis.Platform.Windows
{
    //  ------------------------
    //  InstallerExtensions class
    //  ------------------------

    /// <summary>
    /// Provides extension methods to the <see cref="Installer"/> class
    /// and derived classes.
    /// </summary>

    public static class InstallerExtensions
    {
        //  ------------------------------------
        //  ConfigureServiceHostInstaller method
        //  ------------------------------------

        /// <summary>
        /// Created a <see cref="ServiceProcessInstaller" /> and
        /// for each specified <see cref="IService" /> a <see cref="ServiceInstaller" />
        /// that is used to install the service.
        /// </summary>
        /// <param name="installer">The installer to configure.</param>
        /// <param name="account">The account under which to run the service process.</param>
        /// <param name="services">A list of services to install in the context of the service host.</param>
        /// <exception cref="ArgumentNullException">
        /// <c>installer</c> or <c>services</c> is a null reference (<c>Nothing</c> in Visual Basic).
        /// </exception>

        public static void ConfigureServiceHostInstaller(
            this Installer installer,
            ServiceAccount account,
            params IService[] services)
        {
            if (installer == null) throw new ArgumentNullException(nameof(installer));
            if (services == null) throw new ArgumentNullException(nameof(services));

            installer.Context = new InstallContext();
            var assemblypath = installer.GetType().Assembly.Location;
            installer.Context.Parameters[Platform.InstallerExtensions.AssemblyPathParameterName] = assemblypath;

            using (var processInstaller = new ServiceProcessInstaller())
            {
                processInstaller.Account = account;
                foreach (var service in services)
                {
                    using (var serviceInstaller = new ServiceInstaller())
                    {
                        service.ConfigureInstaller(serviceInstaller);
                        processInstaller.Installers.Add(serviceInstaller);
                    }
                }
                if (processInstaller.Installers.Count > 0)
                {
                    installer.Installers.Add(processInstaller);
                }
            }
        }
    }
}

// eof "InstallerExtensions.cs"

//
//  @(#) InstallerExtension.cs
//
//  Project:    usis.Server
//  System:     Microsoft Visual Studio 12
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015 usis GmbH. All rights reserved.

using System.Collections.Generic;
using System.Configuration.Install;
using System.ServiceProcess;

namespace usis.Server
{
    //  ------------------------
    //  InstallerExtension class
    //  ------------------------

    internal static class InstallerExtension
    {
        //  -------------------------------------
        //  ConfigureServerEngineInstaller method
        //  -------------------------------------

        public static void ConfigureServerEngineInstaller(
            this Installer installer,
            IEnumerable<IWindowsService> services)
        {
            installer.Context = new InstallContext();
            installer.Context.Parameters["assemblypath"] = installer.GetType().Assembly.Location;

            using (var processInstaller = new ServiceProcessInstaller())
            {
                processInstaller.Account = ServiceAccount.LocalService;
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

            } // using processInstaller

        } // ConfigureServerEngineInstaller method

    } // InstallerExtension class

} // namespace usis.Server

// eof "InstallerExtension.cs"

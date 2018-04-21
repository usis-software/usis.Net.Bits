//
//  @(#) Installer.cs
//
//  Project:    usisSvr
//  System:     Microsoft Visual Studio 12
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014,2015 usis GmbH. All rights reserved.

using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace usis.Server
{
    //  ---------------
    //  Installer class
    //  ---------------

    [RunInstaller(true)]
    public class Installer : System.Configuration.Install.Installer
    {
        //  -----------
        //  constructor
        //  -----------

        public Installer()
        {
            this.Context = new InstallContext();
            this.Context.Parameters["assemblypath"] = this.GetType().Assembly.Location;

            using (var processInstaller = new ServiceProcessInstaller())
            {
                processInstaller.Account = ServiceAccount.NetworkService;
                //foreach (var service in ServerEngine.LoadServicesConfiguration())
                {
                    using (var serviceInstaller = new ServiceInstaller())
                    {
                        //service.ConfigureInstaller(serviceInstaller);
                        serviceInstaller.ServiceName = "usisSolutionSvc";
                        processInstaller.Installers.Add(serviceInstaller);
                    }
                }
                if (processInstaller.Installers.Count > 0)
                {
                    this.Installers.Add(processInstaller);
                }

            } // using processInstaller

        } // constructor

    } // Installer class

} // usis.Server namespace

// eof "Installer.cs"

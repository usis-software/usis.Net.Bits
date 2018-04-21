//
//  @(#) Installer.cs
//
//  Project:    usisPNRouter
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2016 usis GmbH. All rights reserved.

using System.Collections;
using System.ComponentModel;
using System.ServiceProcess;
using usis.Platform;
using usis.Platform.Windows;

namespace usis.PushNotification
{
    //  ---------------
    //  Installer class
    //  ---------------

    [RunInstaller(true)]
    public class Installer : System.Configuration.Install.Installer
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        public Installer()
        {
            this.ConfigureServiceHostInstaller(ServiceAccount.LocalSystem, new PNRouterService());
        }

        #endregion construction

        #region overrides

        //  ----------------------
        //  OnBeforeInstall method
        //  ----------------------

        protected override void OnBeforeInstall(IDictionary savedState)
        {
            this.AppendAssemblyPathParameters("-service");

            base.OnBeforeInstall(savedState);
        }

        #endregion overrides
    }
}

// eof "Installer.cs"

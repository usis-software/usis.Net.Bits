//
//  @(#) Installer.cs
//
//  Project:    usisAppSvr
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015,2016 usis GmbH. All rights reserved.

using System.Collections;
using System.ComponentModel;
using System.ServiceProcess;
using usis.Platform;
using usis.Platform.Windows;

namespace usis.ApplicationServer
{
    //  ---------------
    //  Installer class
    //  ---------------    
    /// <summary>
    /// Provides an installer for the usis Application Server.
    /// </summary>

    [RunInstaller(true)]
    public class Installer : System.Configuration.Install.Installer
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="Installer"/> class.
        /// </summary>

        public Installer()
        {
            this.ConfigureServiceHostInstaller(ServiceAccount.LocalSystem, Program.CreateServices());
        }

        #endregion construction

        #region overrides

        //  ----------------------
        //  OnBeforeInstall method
        //  ----------------------

        /// <summary>
        /// Raises the <see cref="System.Configuration.Install.Installer.BeforeInstall" /> event.
        /// </summary>
        /// <param name="savedState">An <see cref="IDictionary" /> that contains the state of the computer before
        /// the installers in the <see cref="System.Configuration.Install.Installer.Installers" /> property are installed.
        /// This <see cref="IDictionary" /> object should be empty at this point.</param>

        protected override void OnBeforeInstall(IDictionary savedState)
        {
            this.AppendAssemblyPathParameters("-service");

            base.OnBeforeInstall(savedState);
        }

        #endregion overrides
    }
}

// eof "Installer.cs"

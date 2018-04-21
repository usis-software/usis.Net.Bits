//
//  @(#) Program.cs
//
//  Project:    IZYTRON.IQ.SyncSvc
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 audius GmbH. All rights reserved.

using System;
using usis.Platform;
using WixSharp;
using WixSharp.CommonTasks;
using WixSharp.Controls;

namespace IZYTRON.IQ.SyncSvc.WixSharp.Setup
{
    //  -------------
    //  Program class
    //  -------------

    internal class Program
    {
        #region configuration constant

#if DEBUG
        private const string configuration = "Debug";
#else
        private const string configuration = "Release";
#endif

        #endregion configuration constant

        //  -----------
        //  Main method
        //  -----------

        internal static void Main()
        {
            File service;
            var project = new Project("IZYTRON.IQ Sync Service",
                             new Dir($@"%ProgramFiles%\{Constants.CompanyDirectory}\{Constants.ProductDirectory}",
                                 service = new File("IZYTRON.IQ.SyncSvc.exe"),
                                 new File("IZYTRON.IQ.SyncSvc.exe.config"),
                                 new DirFiles("IZYTRON.IQ*.dll"),
                                 new DirFiles("EntityFramework*.dll"),
                                 new DirFiles("usis*.dll")));


            #region install service

            // install service
            service.ServiceInstaller = new ServiceInstaller()
            {
                Name = "IZYTRON.IQ.SyncSvc",
                DisplayName = "IZYTRON.IQ Sync Service",
                //Description = "IZYTRON.IQ Sync Service",
                StartOn = SvcEvent.Install,
                StopOn = SvcEvent.InstallUninstall_Wait,
                RemoveOn = SvcEvent.Uninstall_Wait,
                StartType = SvcStartType.demand,
                DelayedAutoStart = true,
                Arguments = "-service"
            };

            #endregion install service

            project.Platform = Platform.x64;

            // GUIDs - Project, Upgrade, Product; get version from assembly
            project.GUID = new Guid("6fe30b47-2577-43ad-9095-1861ba25889b");
            project.UpgradeCode = new Guid("CA364F41-79BA-402B-99B8-6CD45DE80441");
            project.Version = System.Reflection.Assembly.GetExecutingAssembly().GetFileVersion();
            project.ProductId = Guid.NewGuid();

            // upgrade strategy
            project.MajorUpgradeStrategy = MajorUpgradeStrategy.Default;
            project.MajorUpgradeStrategy.RemoveExistingProductAfter = Step.InstallInitialize;

            #region configure UI

            // remove EULA dialog
            project.UI = WUI.WixUI_InstallDir;
            project.RemoveDialogsBetween(NativeDialogs.WelcomeDlg, NativeDialogs.InstallDirDlg);

            #endregion configure UI

            project.ControlPanelInfo.Manufacturer = "audius GmbH";

            var directory = Environment.CurrentDirectory;
            project.SourceBaseDir = $@"{directory}\..\IZYTRON.IQ.SyncSvc\bin\{configuration}";

            project.PreserveTempFiles = true;
            project.BuildMsi($@"{directory}\IZYTRON.IQ.SyncSvc.{project.Version}.msi");
        }
    }
}
// eof "Program.cs"

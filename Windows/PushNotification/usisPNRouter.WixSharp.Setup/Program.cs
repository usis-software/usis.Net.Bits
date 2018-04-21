//
//  @(#) Program.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using System;
using System.Diagnostics;
using System.Windows.Forms;
using usis.Platform;
using usis.Platform.Windows;
using WixSharp;
using WixSharp.CommonTasks;
using WixSharp.Controls;

namespace usisPNRouter.WixSharp.Setup
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

        static void Main()
        {
            File service;

            var router = new Feature("Push Notification Router", "usis Push Notification Router");
            var mySql = new Feature("MySQL support", "MySQL support");
            router.Add(mySql);

            var project = new ManagedProject("usis Push Notification Router",

                new Dir(@"%ProgramFiles%\usis\usisPNRouter\",
                    service = new File(router, "usisPNRouter.exe"),
                    new File(router, "usisPNRouter.exe.config"),
                    new DirFiles(router, "usis*.dll"),
                    new DirFiles(router, "PushSharp*.dll"),
                    new File(router, "Newtonsoft.Json.dll"),
                    new DirFiles(mySql, "MySql*.dll"),
                    new File(router, "Microsoft.ManagementConsole.dll"),
                    new DirFiles(router, "EntityFramework*.dll"),
                    new File(router, "CsvHelper.dll"),
                    new File(router, "usisPNRouter.msc",
                        new FileShortcut("usis Push Notification Router", @"%ProgramMenu%\usis")),
                    new File(router, @"..\usis.PushNotification.Documentation\Help\usisPNRouter.chm")),
                
                // property to get installation directory in custom actions
                new SetPropertyAction(
                    "ARPINSTALLLOCATION",
                    "[INSTALLDIR]",
                    Return.check,
                    When.After,
                    Step.InstallValidate,
                    Condition.NOT_Installed));

            // custom action to run installer
            project.DefaultRefAssemblies.Add($@"bin\{configuration}\usis.Platform.Windows.dll");
            project.BeforeInstall += (e) => { if (e.IsUninstalling) RunAdminInstaller(e); };
            project.AfterInstall += (e) => { if (e.IsInstalling) RunAdminInstaller(e); };

            #region install service

            // install service
            service.ServiceInstaller = new ServiceInstaller()
            {
                Name = "usisPNRouter",
                DisplayName = "usis Push Notification Router",
                Description = "Provides services to send push notifications to mobile devices.",
                StartOn = SvcEvent.Install,
                StopOn = SvcEvent.InstallUninstall_Wait,
                RemoveOn = SvcEvent.Uninstall_Wait,
                StartType = SvcStartType.auto,
                //DelayedAutoStart = true,
                Arguments = "-service"
            };

            #endregion install service

            project.Platform = Platform.x64;

            // GUIDs - Project, Upgrade, Product; get version from assembly
            project.GUID = new Guid("687834EB-62C4-4131-9093-A02D16C66C6A");
            project.UpgradeCode = new Guid("C8EF682C-189B-4A8C-91A7-7C4758DE4E2F");
            project.Version = System.Reflection.Assembly.GetExecutingAssembly().GetFileVersion();
            project.ProductId = Guid.NewGuid();

            // upgrade strategy
            project.MajorUpgradeStrategy = MajorUpgradeStrategy.Default;
            project.MajorUpgradeStrategy.RemoveExistingProductAfter = Step.InstallInitialize;

            #region configure UI

            // remove EULA dialog
            //project.UI = WUI.WixUI_InstallDir;
            //project.UI = WUI.WixUI_FeatureTree;
            project.UI = WUI.WixUI_Mondo;
            //project.UI = WUI.WixUI_Advanced;
            //project.RemoveDialogsBetween(NativeDialogs.WelcomeDlg, NativeDialogs.InstallDirDlg);
            project.RemoveDialogsBetween(NativeDialogs.WelcomeDlg, "SetupTypeDlg");

            #endregion configure UI

            project.ControlPanelInfo.Manufacturer = "usis GmbH";
            project.ControlPanelInfo.ProductIcon = @"usis.icon.64x64.ico";

            var directory = Environment.CurrentDirectory;
            project.SourceBaseDir = $@"{directory}\..\Build\bin\{configuration}";

            project.DefaultFeature = router;

            project.PreserveTempFiles = false;
            project.BuildMsi($@"{directory}\usisPNRouter.{project.Version}.msi");
        }

        #region RunAdminInstaller method

        //  ------------------------
        //  RunAdminInstaller method
        //  ------------------------

        private static void RunAdminInstaller(SetupEventArgs e)
        {
            try
            {
                var assemblyFile = Utils.PathCombine(e.InstallDir, "usis.PushNotification.Administration.dll");
                var assembly = System.Reflection.Assembly.LoadFrom(assemblyFile);
                assembly.InvokeInstaller(e.IsInstalling, "/LogFile=");
                if (e.IsInstalling) ClearMuiCache();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Exception");
            }
        }

        #endregion RunAdminInstaller method

        #region ClearMuiCache method

        //  --------------------
        //  ClearMuiCache method
        //  --------------------

        private static void ClearMuiCache()
        {
            var registryPath = @"Local Settings\MuiCache";
            var key = RegistryValueStorage.OpenClassesRoot().OpenSubStorage(registryPath, false);

            Debug.Print($"Cleaning MuiCache ({key})...");
            foreach (var pair in key.EnumerateValuesInSubStorages())
            {
                Debug.Print($" - store {pair.Storage}");
                Debug.Print($" - value {pair.Value}");

                if (pair.Value.Name.IndexOf("usis.PushNotification.Administration.About.dll", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    Debug.WriteLine("- Found!");

                    pair.Storage.DeleteValue(pair.Value.Name);
                }
            }
        }

        #endregion ClearMuiCache method
    }
}

// eof "Program.cs"

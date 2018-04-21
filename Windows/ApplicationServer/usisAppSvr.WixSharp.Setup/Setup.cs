//
//  @(#) Setup.cs
//
//  Project:    usis Application Server
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using System;
using usis.Platform;
using WixSharp;
using WixSharp.CommonTasks;
using WixSharp.Controls;

namespace usis.ApplicationServer
{
    //  -----------
    //  Setup class
    //  -----------

    internal class Setup
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
            #region define project

            var directory = Environment.CurrentDirectory;

            var project = new ManagedProject("usis Application Server",
                new Dir(@"%ProgramFiles%\usis\usisAppSvr",
                    new File("usisAppSvr.exe").AddXmlInclude($@"{directory}\InstallServices.wxi", parentElement: "Component"),
                    new File("usisAppSvr.exe.config"),
                    new DirFiles("*.dll")))
            {
                UI = WUI.WixUI_InstallDir
            };

            // remove EULA dialog
            project.RemoveDialogsBetween(NativeDialogs.WelcomeDlg, NativeDialogs.InstallDirDlg);

            #endregion define project

            #region configure project

            // install as x64
            project.Platform = WixSharp.Platform.x64;

            // GUIDs - Project, Upgrade, Product; get version from assembly
            project.GUID = new Guid("AD3FA17C-F052-47D6-98E8-7714C3640356");
            project.UpgradeCode = new Guid("6D4D4DC2-C5D1-4D15-B351-299BC55BF3AC");
            project.Version = System.Reflection.Assembly.GetExecutingAssembly().GetFileVersion();
            project.ProductId = Guid.NewGuid();

            // upgrade strategy
            project.MajorUpgradeStrategy = MajorUpgradeStrategy.Default;
            project.MajorUpgradeStrategy.RemoveExistingProductAfter = Step.InstallInitialize;

            project.ControlPanelInfo.Manufacturer = "usis GmbH";

            project.SourceBaseDir = $@"{directory}\..\usisAppSvr\bin\{configuration}";

            project.PreserveTempFiles = false;
            project.BuildMsi($@"{directory}\usisAppSvr.{project.Version}.msi");

            #endregion configure project
        }
    }
}

// eof "Setup.cs"

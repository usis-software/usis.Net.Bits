//
//  @(#) Program.cs
//
//  Project:    usis BITS Administration
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;
using usis.Platform;
using WixSharp;
using WixSharp.CommonTasks;
using WixSharp.Controls;

namespace usisBITSAdmin.WixSharp.Setup
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

        [SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object,System.Object)")]
        [SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object)")]
        [STAThread]
        internal static void Main()
        {
            var project = new ManagedProject("usis BITS Administration",
                             new Dir(@"%ProgramFiles%\usis\BITSAdmin",
                                 new File("usisBITSAdmin.msc", 
                                    new FileShortcut("usis BITS Administration", @"%ProgramMenu%\usis")),
                                 new DirFiles("usis.*.dll"),
                                 new DirFiles("Microsoft.ManagementConsole.dll")))
            {
                Platform = Platform.x64,

                // GUIDs - Project, Upgrade, Product; get version from assembly
                GUID = new Guid("6fe30b47-2577-43ad-9095-1861ba25889b"),
                UpgradeCode = new Guid("3c9ecd8c-7c29-48a3-b859-af3252b5e2a2"),
                ProductId = Guid.NewGuid(),
                Version = System.Reflection.Assembly.GetExecutingAssembly().GetFileVersion(),

                // upgrade strategy
                MajorUpgradeStrategy = MajorUpgradeStrategy.Default
            };
            project.MajorUpgradeStrategy.RemoveExistingProductAfter = Step.InstallInitialize;

            // custom action to run installer
            project.DefaultRefAssemblies.Add($@"bin\{configuration}\usis.Platform.Windows.dll");
            project.BeforeInstall += (e) => { if (e.IsUninstalling) RunAdminInstaller(e); };
            project.AfterInstall += (e) => { if (e.IsInstalling) RunAdminInstaller(e); };

            #region configure UI

            // remove EULA dialog
            project.UI = WUI.WixUI_InstallDir;
            project.RemoveDialogsBetween(NativeDialogs.WelcomeDlg, NativeDialogs.InstallDirDlg);

            #endregion configure UI

            project.ControlPanelInfo.Manufacturer = /*System.Reflection.Assembly.GetExecutingAssembly().*/"audius GmbH";

            var directory = Environment.CurrentDirectory;
            project.SourceBaseDir = $@"{directory}\..\usis.Net.Bits.Administration\bin\{configuration}";

            project.PreserveTempFiles = false;
            project.BuildMsi($@"{directory}\usisBITSAdmin.{project.Version}.msi");
        }

        #region RunAdminInstaller method

        //  ------------------------
        //  RunAdminInstaller method
        //  ------------------------

        [SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.Reflection.Assembly.LoadFrom")]
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Windows.Forms.MessageBox.Show(System.String,System.String,System.Windows.Forms.MessageBoxButtons,System.Windows.Forms.MessageBoxIcon,System.Windows.Forms.MessageBoxDefaultButton,System.Windows.Forms.MessageBoxOptions)")]
        private static void RunAdminInstaller(SetupEventArgs e)
        {
            try
            {
                var assemblyFile = Utils.PathCombine(e.InstallDir, "usis.Net.Bits.Administration.dll");
                var assembly = System.Reflection.Assembly.LoadFrom(assemblyFile);
                assembly.InvokeInstaller(e.IsInstalling, "/LogFile=");
                //if (e.IsInstalling) ClearMuiCache();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0);
            }
        }

        #endregion RunAdminInstaller method
    }
}

// eof "Program.cs"

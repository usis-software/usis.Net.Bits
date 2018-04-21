using System;
using usis.Platform;
using WixSharp;
using WixSharp.CommonTasks;
using WixSharp.Controls;

namespace IDocRouter.WixSharp.Setup
{
    class Program
    {
        #region configuration constant

#if DEBUG
        private const string configuration = "Debug";
#else
        private const string configuration = "Release";
#endif

        #endregion configuration constant

        static void Main()
        {
            File service;
            var project = new Project("IDoc Router",
                             new Dir(@"%ProgramFiles%\audius\IDocRouter",
                                 service = new File("IDocRouter.exe"),
                                 new File("IDocRouter.exe.template.config"),
                                 new DirFiles("usis.*.dll"),
                                 new DirFiles("sapnco*.dll")));


            #region install service

            // install service
            service.ServiceInstaller = new ServiceInstaller()
            {
                Name = "IDocRouter",
                DisplayName = "IDoc Router",
                Description = "Sends and receives IDocs from a SAP system.",
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
            project.UpgradeCode = new Guid("73841209-613D-4FDA-8FF4-C8B1EA9053EA");
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
            project.SourceBaseDir = $@"{directory}\..\usisIDocRouter\bin\{configuration}";

            project.PreserveTempFiles = false;
            project.BuildMsi($@"{directory}\IDocRouter.{project.Version}.msi");
        }
    }
}
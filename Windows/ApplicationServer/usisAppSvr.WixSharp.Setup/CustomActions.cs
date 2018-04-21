//
//  @(#) CustomActions.cs
//
//  Project:    usis Application Server
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using Microsoft.Deployment.WindowsInstaller;
using WixSharp;
using WixSharp.CommonTasks;

namespace usis.ApplicationServer
{
    //  -------------------
    //  CustomActions class
    //  -------------------

    public static class CustomActions
    {
        //  ---------------------
        //  InstallService method
        //  ---------------------

        [CustomAction]
        public static ActionResult InstallService(Session session)
        {
            return session.HandleErrors(() =>
            {
                // install services and start them
                Tasks.InstallService(session.Property("INSTALLDIR") + "usisAppSvr.exe", true);
                Tasks.StartService("usisAppSvr", false);
                Tasks.StartService("usisAppSvrAdmin", false);

                // install management console snap-in
                Tasks.InstallService(session.Property("INSTALLDIR") + "usis.ApplicationServer.Administration.dll", true);
            });
        }

        //  -----------------------
        //  UninstallService method
        //  -----------------------

        [CustomAction]
        public static ActionResult UninstallService(Session session)
        {
            return session.HandleErrors(() =>
            {
                // uninstall management console snap-in
                Tasks.InstallService(session.Property("INSTALLDIR") + "usis.ApplicationServer.Administration.dll", false);

                // stop services and uninstall them
                Tasks.StopService("usisAppSvrAdmin", false);
                Tasks.StopService("usisAppSvr", false);
                Tasks.InstallService(session.Property("INSTALLDIR") + "usisAppSvr.exe", false);
            });
        }
    }
}

// eof "CustomActions.cs"

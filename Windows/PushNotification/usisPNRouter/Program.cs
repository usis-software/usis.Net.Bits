//
//  @(#) Program.cs
//
//  Project:    usisPNRouter
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

using System;
using System.Reflection;
using usis.Platform;
using usis.Platform.Windows;
using usis.PushNotification.Administration;

namespace usis.PushNotification
{
    //  -------------
    //  Program class
    //  -------------

    /// <summary>
    /// Contains the entry point of the usis Push Notification Router Windows service application.
    /// </summary>

    internal static class Program
    {
        //  -----------
        //  Main method
        //  -----------

        /// <summary>
        /// The entry point of the usis Push Notification Router Windows service application.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <returns>
        /// The return value.
        /// </returns>

        internal static int Main(string[] args)
        {
            var assembly = Assembly.GetExecutingAssembly();

            Console.WriteLine();
            Console.WriteLine(assembly.GetTitle());
            Console.WriteLine(assembly.GetCopyright());
            Console.WriteLine();

            var commandLine = new CommandLine(args);

            if (commandLine.HasOption("i") || commandLine.HasOption("install"))
            {
                return typeof(Installer).Assembly.Install();
            }
            else if (commandLine.HasOption("u") || commandLine.HasOption("uninstall"))
            {
                return typeof(Installer).Assembly.Uninstall();
            }
            else if (commandLine.HasOption("install-admin"))
            {
                return typeof(SnapInInstaller).Assembly.Install();
            }
            else if (commandLine.HasOption("uninstall-admin"))
            {
                return typeof(SnapInInstaller).Assembly.Uninstall();
            }
            else return ServicesHost.StartServicesOrConsole(args, new PNRouterService());
        }
    }
}

// eof "Program.cs"

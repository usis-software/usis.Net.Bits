//
//  @(#) Program.cs
//
//  Project:    usis Job Engine
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2018 usis GmbH. All rights reserved.

using System;
using System.Reflection;
using usis.ApplicationServer;
using usis.Platform;
using usis.Platform.Windows;

namespace usis.JobEngine
{
    //  -------------
    //  Program class
    //  -------------

    internal class Program
    {
        //  -----------
        //  Main method
        //  -----------

        internal static void Main(string[] args)
        {
            DisplayTitle();

            // start the job engine Windows service
            ServicesHost.StartServicesOrConsole(args, Service.FromSnapIn<JobEngineSnapIn>());
        }

        #region private methods

        //  -------------------
        //  DisplayTitle method
        //  -------------------

        private static void DisplayTitle()
        {
            var assembly = Assembly.GetExecutingAssembly();

            Console.WriteLine();
            Console.WriteLine(assembly.GetTitle());
            Console.WriteLine(assembly.GetCopyright());
            Console.WriteLine();
        }

        #endregion private methods
    }
}

// eof "Program.cs"

//
//  @(#) Program.cs
//
//  Project:    usisAppSvr
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015,2016 usis GmbH. All rights reserved.

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using usis.Platform;
using usis.Platform.Windows;

namespace usis.ApplicationServer
{
    //  -------------
    //  Program class
    //  -------------

    internal static class Program
    {
        //  -----------
        //  Main method
        //  -----------

        internal static int Main(string[] args)
        {
            //Trace.Listeners.Add(new TextWriterTraceListener("usisAppSvr.log"));
            //Trace.AutoFlush = true;
            
            var assembly = Assembly.GetExecutingAssembly();

            Console.WriteLine();
            Console.WriteLine(assembly.GetTitle());
            Console.WriteLine(assembly.GetCopyright());
            Console.WriteLine();

            Debug.WriteLine(string.Empty);
            Debug.Print("{0} process started:", assembly.GetTitle());

            Environment.CurrentDirectory = Path.GetDirectoryName(assembly.Location);
            Debug.Print("Current directory: {0}", Environment.CurrentDirectory);

            return ServicesHost.StartServicesOrConsole(args, CreateServices());
        }

        //  ---------------------
        //  CreateServices method
        //  ---------------------

        internal static IService[] CreateServices()
        {
            Server server = new Server();
            return new IService[] { server, new AdminService(server) };
        }
    }
}

// eof "Program.cs"

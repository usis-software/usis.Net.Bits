//
//  @(#) Program.cs
//
//  Project:    usis.FinTS.Server
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System;
using System.Reflection;
using usis.ApplicationServer;
using usis.Platform.Windows;
using usis.Platform;

namespace usis.FinTS.Server
{
    //  -------------
    //  Program class
    //  -------------

    internal static class Program
    {
        //  -----------
        //  Main method
        //  -----------

        internal static void Main(string[] args)
        {
            var assembly = Assembly.GetExecutingAssembly();

            Console.WriteLine();
            Console.WriteLine(assembly.GetDescription());
            Console.WriteLine(assembly.GetCopyright());
            Console.WriteLine();

            ServicesHost.StartServicesOrConsole(args, Service.FromSnapIn<TcpSnapIn>());
        }
    }
}

// eof "Program.cs"

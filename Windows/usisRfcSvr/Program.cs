//
//  @(#) Program.cs
//
//  Project:    usis RFC Server
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using SAP.Middleware.Connector;
using System;
using System.Reflection;
using usis.Platform;
using usis.Platform.Windows;

namespace usis.Server.SAP
{
    internal class Program
    {
        internal static int Main(string[] args)
        {
            Console.WriteLine();
            Console.WriteLine(ExecutingAssembly.Title);
            Console.WriteLine(ExecutingAssembly.Copyright);
            Console.WriteLine();

            var destination = RfcDestinationManager.GetDestination("audius.SAP.RAC");
            var function = destination.Repository.CreateFunction("BAPI_USER_GETLIST");
            function.Invoke(destination);
            var table = function.GetTable("USERLIST");
            foreach (var user in table)
            {
                var field = user["USERNAME"];
                Console.WriteLine(field.GetString());
            }
            Console.WriteLine();

            return ServicesHost.StartServicesOrConsole(args, new Service());
        }
    }

    internal class Service : ApplicationServer.Service
    {
        public Service() : base(new Framework.Configuration.ConfigurationRoot(typeof(Middleware.SAP.IntermediateDocumentReceiverSnapIn)))
        {
            ReadApplicationConfiguration();
        }
    }


    internal static class ExecutingAssembly
    {
        public static string Title => Assembly.GetExecutingAssembly().GetTitle();

        public static string Copyright => Assembly.GetExecutingAssembly().GetCopyright();
    }
}

// eof "Program.cs"

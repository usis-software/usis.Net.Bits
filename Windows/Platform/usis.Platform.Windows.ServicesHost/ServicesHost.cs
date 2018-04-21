//
//  @(#) ServicesHost.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;

namespace usis.Platform.Windows
{
    #region ServicesHost class

    //  ------------------
    //  ServicesHost class
    //  ------------------

    /// <summary>
    /// Provides the ability to host one or more Windows services.
    /// </summary>

    public static class ServicesHost
    {
        #region public methods

        //  --------------------
        //  StartServices method
        //  --------------------

        /// <summary>
        /// Starts the service host with a specified list of Windows services.
        /// </summary>
        /// <param name="args">An array of strings as command-line arguments.</param>
        /// <param name="services">An array of Windows services to host.</param>
        /// <returns>
        /// A service host return code.
        /// </returns>

        public static int StartServices(string[] args, params IService[] services)
        {
            return StartServices(false, args, services);
        }

        //  -----------------------------
        //  StartServicesOrConsole method
        //  -----------------------------

        /// <summary>
        /// Starts the service host with a specified list of Windows services
        /// in a console application. If the command line parameter <c>-service</c> is specified,
        /// the executable is registered for a service with the Service Control Manager (SCM).
        /// </summary>
        /// <param name="args">An array of strings as command-line arguments.</param>
        /// <param name="services">An array of Windows services to host.</param>
        /// <returns>
        /// A service host return code.
        /// </returns>

        public static int StartServicesOrConsole(string[] args, params IService[] services)
        {
            return StartServices(true, args, services);
        }

        #endregion public methods

        #region private methods

        //  --------------------
        //  StartServices method
        //  --------------------

        /// <summary>
        /// Starts the server engine with a specified list of Windows services.
        /// </summary>
        /// <param name="consoleIsDefault">if set to <c>true</c> the console server engine is the default.</param>
        /// <param name="args">An array of strings as command-line arguments.</param>
        /// <param name="services">An array of Windows services to host in the server process.</param>
        /// <returns>
        /// A server engine return code.
        /// </returns>

        private static int StartServices(bool consoleIsDefault, string[] args, params IService[] services)
        {
            using (IServicesHost engine = CreateServerEngine(consoleIsDefault, args, services))
            {
                return engine.Run();
            }
        }

        //  -------------------------
        //  CreateServerEngine method
        //  -------------------------

        private static IServicesHost CreateServerEngine(bool consoleIsDefault, string[] args, params IService[] services)
        {
            var commandLine = new CommandLine(args);

            if (commandLine.HasOption("c") || commandLine.HasOption("console"))
            {
                return new ConsoleServicesHost(services);
            }
            else if (commandLine.HasOption("s") || commandLine.HasOption("service"))
            {
                return new WindowsServicesHost(services);
            }
            else
            {
                if (consoleIsDefault) return new ConsoleServicesHost(services);
                else return new WindowsServicesHost(services);
            }
        }

        #endregion private methods
    }

    #endregion ServicesHost class

    #region IServicesHost interface

    //  -----------------------
    //  IServicesHost interface
    //  -----------------------

    internal interface IServicesHost : IDisposable
    {
        //  ----------
        //  Run method
        //  ----------

        int Run();
    }

    #endregion IServicesHost interface

    #region ServicesHostBase class

    //  ----------------------
    //  ServicesHostBase class
    //  ----------------------

    internal abstract class ServicesHostBase
    {
        #region properties

        //  -----------------
        //  Services property
        //  -----------------

        internal protected IEnumerable<WindowsService> Services { get; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal ServicesHostBase(params IService[] services)
        {
            Services = services.Select((s) => new WindowsService(s));
        }

        #endregion construction
    }

    #endregion ServicesHostBase class
}

// eof "ServicesHost.cs"

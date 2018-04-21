//
//  @(#) Constants.cs
//
//  Project:    usis.Server
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

namespace usis.ApplicationServer
{
    //  ---------------
    //  Constants class
    //  ---------------

    /// <summary>
    /// Contains contant values for the usis Application Server API.
    /// </summary>

    public static class Constants
    {
        /// <summary>
        /// The usis Application Server service name.
        /// </summary>

        public const string ApplicationServerServiceName = "usisAppSvr";

        /// <summary>
        /// The usis Application Server Administration service name.
        /// </summary>

        public const string ApplicationServerAdministrationServiceName = "usisAppSvrAdmin";

        /// <summary>
        /// The name of application configuration file section.
        /// </summary>

        public const string ApplicationConfigurationFileSection = "usis.ApplicationService";
    }
}

// eof "Constants.cs"

//
//  @(#) IApplicationConfiguration.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

using System.Collections.Generic;

namespace usis.Framework.Configuration
{
    //  -----------------------------------
    //  IApplicationConfiguration interface
    //  -----------------------------------

    /// <summary>
    /// Provides information about the configuration of an application.
    /// </summary>

    public interface IApplicationConfiguration
    {
        //  -------------
        //  Name property
        //  -------------

        /// <summary>
        /// Gets or sets the name of the application.
        /// </summary>
        /// <value>
        /// The name of the application.
        /// </value>

        string Name { get; set; }

        //  ----------------------
        //  AppDomainName property
        //  ----------------------

        /// <summary>
        /// Gets or sets the name of the application domain.
        /// </summary>
        /// <value>
        /// The name of the application domain.
        /// </value>

        string AppDomainName { get; set; }

        //  -------------------
        //  Properties property
        //  -------------------        

        /// <summary>
        /// Gets an enumerator to iterate all properties of an application configuration.
        /// </summary>
        /// <value>
        /// An enumerator to iterate all properties of an application configuration.
        /// </value>

        IEnumerable<IConfigurationProperty> Properties { get; }

        //  ----------------
        //  SnapIns property
        //  ----------------

        /// <summary>
        /// Gets an enumerator to iterate all snap-ins of an application configuration.
        /// </summary>
        /// <value>
        /// An enumerator to iterate all snap-ins of an application configuration.
        /// </value>

        IEnumerable<ISnapInConfiguration> SnapIns { get; }
    }
}

// eof "IApplicationConfiguration.cs"

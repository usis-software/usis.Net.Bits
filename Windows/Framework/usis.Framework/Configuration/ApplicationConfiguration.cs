//
//  @(#) ApplicationConfiguration.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;

namespace usis.Framework.Configuration
{
    #region ApplicationStartMode enumeration

    //  --------------------------------
    //  ApplicationStartMode enumeration
    //  --------------------------------

    /// <summary>
    /// Indicates the start mode of an application.
    /// </summary>

    public enum ApplicationStartMode
    {
        /// <summary>
        /// Indicates that the application is to be started by the application server, at start-up.
        /// </summary>

        Automatic,

        /// <summary>
        /// Indicates that the application is disabled, so that it cannot be started by a user or the application server.
        /// </summary>

        Disabled,

        /// <summary>
        /// Indicates that the application is started only manually, by a user (using the Application Service Manager) or by another application.
        /// </summary>

        Manual
    }

    #endregion ApplicationStartMode enumeration

    //  ------------------------------
    //  ApplicationConfiguration class
    //  ------------------------------

    /// <summary>
    /// Holds information about the configuration of an application.
    /// </summary>

    [XmlRoot("application")]
    public class ApplicationConfiguration : IApplicationConfiguration
    {
        #region properties

        //  -------------
        //  Name property
        //  -------------

        /// <summary>
        /// Gets or sets the name of the application.
        /// </summary>
        /// <value>
        /// The name of the application.
        /// </value>

        [XmlAttribute("name")]
        public string Name { get; set; }

        //  -----------------
        //  Disabled property
        //  -----------------

        /// <summary>
        /// Gets or sets a value indicating whether the application is disabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if disabled; otherwise, <c>false</c>.
        /// </value>

        [Obsolete("Use the StartType property instead.")]
        [XmlAttribute("disabled")]
        public bool Disabled { get; set; }

        //  -----------
        //  Id property
        //  -----------

        /// <summary>
        /// Gets or sets the unique identifier for the application.
        /// </summary>
        /// <value>
        /// The unique identifier for the application.
        /// </value>

        [XmlAttribute("id")]
        public Guid Id { get; set; }

        //  ----------------------
        //  AppDomainName property
        //  ----------------------

        /// <summary>
        /// Gets or sets the name of the application domain.
        /// </summary>
        /// <value>
        /// The name of the application domain.
        /// </value>

        [XmlAttribute("domain")]
        public string AppDomainName { get; set; }

        //  --------------------------
        //  ConfigurationFile property
        //  --------------------------

        /// <summary>
        /// Gets or sets the name of the configuration file for the application domain.
        /// </summary>
        /// <value>
        /// The name of the configuration file for the application domain.
        /// </value>

        [XmlAttribute("configurationFile")]
        public string ConfigurationFile { get; set; }

        //  ------------------------
        //  ApplicationBase property
        //  ------------------------

        /// <summary>
        /// Gets or sets the name of the directory containing the application.
        /// </summary>
        /// <value>
        /// The name of the application base directory.
        /// </value>

        [XmlAttribute("applicationBase")]
        public string ApplicationBase { get; set; }

        //  --------------------
        //  AppBasePath property
        //  --------------------

        /// <summary>
        /// Gets or sets the base directory that the assembly resolver uses to probe for assemblies.
        /// </summary>
        /// <value>
        /// The base directory that the assembly resolver uses to probe for assemblies.
        /// </value>

        [Obsolete("Use the ApplicationBase property instead.")]
        [XmlAttribute("appBasePath")]
        public string AppBasePath { get; set; }

        //  ----------------------
        //  AppConfigFile property
        //  ----------------------

        /// <summary>
        /// Gets or sets the name of the application configuration file.
        /// </summary>
        /// <value>
        /// The name of the application configuration file.
        /// </value>

        [XmlAttribute("appConfigFile")]
        public string AppConfigFile { get; set; }

        //  ------------------
        //  StartType property
        //  ------------------

        /// <summary>
        /// Gets or sets a value that indicates how the application starts. 
        /// </summary>
        /// <value>
        /// The start type.
        /// </value>

        [XmlAttribute("startType")]
        public ApplicationStartMode StartType { get; set; }

        //  -------------------
        //  Properties property
        //  -------------------        

        /// <summary>
        /// Gets an enumerator to iterate all properties of an application configuration.
        /// </summary>
        /// <value>
        /// An enumerator to iterate all properties of an application configuration.
        /// </value>

        [XmlIgnore]
        public IEnumerable<IConfigurationProperty> Properties => PropertiesInternal.Cast<IConfigurationProperty>();

        /// <summary>
        /// Gets a collection of user-defined properties for the application.
        /// </summary>
        /// <value>
        /// The collection of user-defined properties for the application.
        /// </value>

        [XmlElement("property")]
        public Collection<ConfigurationProperty> PropertiesInternal { get; private set; }

        //  ----------------
        //  SnapIns property
        //  ----------------

        /// <summary>
        /// Gets an enumerator to iterate all snap-ins of an application configuration.
        /// </summary>
        /// <value>
        /// An enumerator to iterate all snap-ins of an application configuration.
        /// </value>

        [XmlIgnore]
        public IEnumerable<ISnapInConfiguration> SnapIns => SnapInsInternal.Cast<ISnapInConfiguration>();

        /// <summary>
        /// Gets a collection of <see cref="SnapInConfiguration"/> objects
        /// </summary>
        /// <value>
        /// A collection of <b>SnapInConfiguration</b> objects.
        /// </value>

        [XmlElement("snapIn")]
        public Collection<SnapInConfiguration> SnapInsInternal { get; private set; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationConfiguration"/> class.
        /// </summary>

        public ApplicationConfiguration()
        {
            PropertiesInternal = new Collection<ConfigurationProperty>();
            SnapInsInternal = new Collection<SnapInConfiguration>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationConfiguration" /> class
        /// with the specified snap-in types.
        /// </summary>
        /// <param name="snapInTypes">The snap-in types.</param>
        /// <exception cref="ArgumentNullException">
        /// <c>snapInTypes</c> is a null reference.
        /// </exception>

        public ApplicationConfiguration(params Type[] snapInTypes) : this()
        {
            if (snapInTypes == null) throw new ArgumentNullException(nameof(snapInTypes));
            foreach (var snapInType in snapInTypes)
            {
                SnapInsInternal.Add(new SnapInConfiguration(snapInType));
            }
        }

        #endregion construction
    }
}

// eof "ApplicationConfiguration.cs"

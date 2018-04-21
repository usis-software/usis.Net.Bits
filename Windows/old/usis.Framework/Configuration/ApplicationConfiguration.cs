//
//  @(#) ApplicationConfiguration.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2016 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using usis.Framework.Portable;

namespace usis.Framework.Configuration
{
    //  ------------------------------
    //  ApplicationConfiguration class
    //  ------------------------------

    /// <summary>
    /// Holds information about the configuration of an application.
    /// </summary>

    [Obsolete("Use classes from the usis.Framework.Portable.Configuration namespace instead.")]
    [XmlRoot("application")]
    [Serializable]
    public class ApplicationConfiguration : Portable.Configuration.IApplicationConfiguration
    {
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
        /// Initializes a new instance of the <see cref="ApplicationConfiguration"/> class
        /// with the specified snap-ins.
        /// </summary>
        /// <param name="snapInTypes">A list of snap-in types.</param>

        public ApplicationConfiguration(params Type[] snapInTypes) : this()
        {
            if (snapInTypes == null) throw new ArgumentNullException(nameof(snapInTypes));

            foreach (var type in snapInTypes)
            {
                SnapInsInternal.Add(new SnapInConfiguration(type));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationConfiguration"/> class
        /// with the specified application.
        /// </summary>
        /// <param name="application">
        /// An <see cref="IApplication"/> object used to initialize the configuration.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <i>application</i> is a null reference (<b>Nothing</b> in Visual Basic).
        /// </exception>

        public ApplicationConfiguration(IApplication application) : this()
        {
            if (application == null) throw new ArgumentNullException(nameof(application));

            foreach (var item in application.Properties.ValueNames)
            {
                PropertiesInternal.Add(new ConfigurationProperty()
                {
                    Name = item.ToString(),
                    Value = application.Properties.GetValue(item)?.Value
                });
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationConfiguration"/> class
        /// with the specified configuration element.
        /// </summary>
        /// <param name="element">
        /// The configuration element.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <i>element</i> is a null reference (<b>Nothing</b> in Visual Basic).
        /// </exception>

        internal ApplicationConfiguration(ApplicationConfigurationElement element) : this()
        {
            foreach (var item in element.ApplicationProperties.Cast<PropertyConfigurationElement>())
            {
                PropertiesInternal.Add(new ConfigurationProperty()
                {
                    Name = item.Name,
                    Value = item.RawValue
                });
            }
            ReadSnapIns(element.SnapIns);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationConfiguration"/> class
        /// with the specified configuration section.
        /// </summary>
        /// <param name="section">The configuration section.</param>

        internal ApplicationConfiguration(ApplicationConfigurationSection section) : this()
        {
            ReadSnapIns(section.SnapIns);
        }

        #endregion construction

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

        [XmlAttribute("appDomain")]
        public string AppDomainName { get; set; }

        //  --------------------
        //  AppBasePath property
        //  --------------------

        /// <summary>
        /// Gets or sets the base directory that the assembly resolver uses to probe for assemblies.
        /// </summary>
        /// <value>
        /// The base directory that the assembly resolver uses to probe for assemblies.
        /// </value>

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
        public IEnumerable<Portable.Configuration.IConfigurationProperty> Properties
        {
            get { return PropertiesInternal.Cast<Portable.Configuration.IConfigurationProperty>(); }
        }

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
        public IEnumerable<Portable.Configuration.ISnapInConfiguration> SnapIns
        {
            get { return SnapInsInternal.Cast<Portable.Configuration.ISnapInConfiguration>(); }
        }

        /// <summary>
        /// Gets a collection of <see cref="SnapInConfiguration"/> objects
        /// </summary>
        /// <value>
        /// A collection of <b>SnapInConfiguration</b> objects.
        /// </value>

        [XmlElement("snapIn")]
        public Collection<SnapInConfiguration> SnapInsInternal { get; private set; }

        #endregion properties

        #region methods

        //  ------------------------
        //  ReadConfiguration method
        //  ------------------------

        /// <summary>
        /// Reads a configuration from the specified section of the application configuration file.
        /// </summary>
        /// <param name="sectionName">Name of the section.</param>
        /// <returns>
        /// The configuration that was read from the application configuration file.
        /// </returns>
        /// <exception cref="Platform.ArgumentNullOrWhiteSpaceException">
        /// <i>sectionName</i> is <b>null</b>, empty, or consists only of white-space characters.
        /// </exception>

        public static ApplicationConfiguration ReadConfiguration(string sectionName)
        {
            if (string.IsNullOrWhiteSpace(sectionName)) throw new Platform.ArgumentNullOrWhiteSpaceException(nameof(sectionName));

            var section = ConfigurationManager.GetSection(sectionName) as ApplicationConfigurationSection;
            if (section != null)
            {
                // convert to an application configuration
                return new ApplicationConfiguration(section);
            }
            return null;
        }

        //  ------------------
        //  ReadSnapIns method
        //  ------------------

        private void ReadSnapIns(SnapInConfigurationElementCollection snapIns)
        {
            foreach (var item in snapIns.Cast<SnapInConfigurationElement>())
            {
                SnapInsInternal.Add(new SnapInConfiguration(item));
            }
        }

        //  ------------------------
        //  ReadAppConfigFile method
        //  ------------------------

        /// <summary>
        /// Reads the application configuration file defined by the
        /// <see cref="AppConfigFile"/> property.
        /// </summary>

        public void ReadAppConfigFile()
        {
            if (string.IsNullOrWhiteSpace(AppConfigFile)) return;

            var filePath = AppConfigFile;
            if (!string.IsNullOrWhiteSpace(AppBasePath)) filePath = Path.Combine(AppBasePath, filePath);
            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    var serializer = new XmlSerializer(typeof(ApplicationConfiguration));
                    var configuration = serializer.Deserialize(reader) as ApplicationConfiguration;
                    if (configuration != null)
                    {
                        // replace properties (attributes)
                        if (configuration.Name != null) Name = configuration.Name;
                        if (configuration.AppDomainName != null) AppDomainName = configuration.AppDomainName;

                        // merge property dictionary and snap-ins
                        foreach (var property in configuration.PropertiesInternal) PropertiesInternal.Add(property);
                        foreach (var snapIn in configuration.SnapInsInternal) SnapInsInternal.Add(snapIn);
                    }
                }
            }
        }

        #endregion methods
    }
}

// eof "ApplicationConfiguration.cs"

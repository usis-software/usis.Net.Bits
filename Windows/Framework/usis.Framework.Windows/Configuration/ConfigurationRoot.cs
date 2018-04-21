//
//  @(#) ConfigurationRoot.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace usis.Framework.Configuration
{
    //  -----------------------
    //  ConfigurationRoot class
    //  -----------------------

    /// <summary>
    /// Holds the information about the configuration of an
    /// application service
    /// </summary>

    [XmlRoot("usis")]
    public class ConfigurationRoot
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationRoot"/> class.
        /// </summary>

        public ConfigurationRoot() { Applications = new Collection<ApplicationConfiguration>(); }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationRoot"/> class
        /// with the specified application configuration section.
        /// </summary>
        /// <param name="section">The application configuration section.</param>

        public ConfigurationRoot(ConfigurationRootSection section) : this()
        {
            if (section == null) throw new ArgumentNullException(nameof(section));
            foreach (var item in section.Applications.Cast<ApplicationConfigurationElement>())
            {
                Applications.Add(item.ToApplicationConfiguration());
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationRoot"/> class
        /// with the specified snap-in types.
        /// </summary>
        /// <param name="snapInTypes">The snap-in types.</param>

        public ConfigurationRoot(params Type[] snapInTypes) : this()
        {
            Applications.Add(new ApplicationConfiguration(snapInTypes));
        }

        #endregion construction

        #region properties

        //  ---------------------
        //  Applications property
        //  ---------------------

        /// <summary>
        /// Gets a collection of <see cref="ApplicationConfiguration"/> objects.
        /// contained in this configuration.
        /// </summary>
        /// <value>
        /// A collection of <b>ApplicationConfiguration</b> objects.
        /// </value>

        [XmlElement("application")]
        public Collection<ApplicationConfiguration> Applications { get; private set; }

        #endregion properties

        #region methods

        //  -----------
        //  Load method
        //  -----------

        /// <summary>
        /// Loads a XML configuration document from the specified reader.
        /// </summary>
        /// <param name="reader">
        /// The <b>TextReader</b> that provides access to the XML document
        /// with the configuration. 
        /// </param>
        /// <returns>
        /// The configuration loaded from the XML document.
        /// </returns>

        public static ConfigurationRoot Load(TextReader reader)
        {
            var serializer = new XmlSerializer(typeof(ConfigurationRoot));
            return serializer.Deserialize(reader) as ConfigurationRoot;
        }

        //  -----------
        //  Save method
        //  -----------

        /// <summary>
        /// Saves the configuration to the specified writer as a XML document.
        /// </summary>
        /// <param name="writer">
        /// The <b>TextWriter</b> used to write the XML document.
        /// </param>

        public void Save(TextWriter writer)
        {
            var serializer = new XmlSerializer(typeof(ConfigurationRoot));
            var settings = new XmlWriterSettings()
            {
                Indent = true,
                NewLineOnAttributes = true
            };
            using (var xml = XmlWriter.Create(writer, settings))
            {
                serializer.Serialize(xml, this);
            }
        }

        //  -------------------------------------------
        //  CreateSingleApplicationConfiguration method
        //  -------------------------------------------

        /// <summary>
        /// Creates a configuration with a single application that contains
        /// the specified snap-ins.
        /// </summary>
        /// <param name="snapInTypes">A list of snap-in types.</param>
        /// <returns>
        /// A configuration with one application.
        /// </returns>

        public static ConfigurationRoot CreateSingleApplicationConfiguration(params Type[] snapInTypes)
        {
            var configuration = new ConfigurationRoot();
            configuration.Applications.Add(new ApplicationConfiguration(snapInTypes));
            return configuration;
        }

        #endregion methods
    }

    #region ConfigurationRootSection class

    //  ------------------------------
    //  ConfigurationRootSection class
    //  ------------------------------

    /// <summary>
    /// Represents the root configuration section of an application service.
    /// </summary>

    public class ConfigurationRootSection : ConfigurationSection
    {
        #region Applications collections

        private const string applications = "applications";
        private const string application = "application";

        [ConfigurationProperty(applications)]
        [ConfigurationCollection(typeof(ApplicationConfigurationElement), AddItemName = application)]
        internal ApplicationConfigurationElementCollection Applications => this[applications] as ApplicationConfigurationElementCollection;

        #endregion Applications collections
    }

    #endregion ConfigurationRootSection class
}

// eof "ConfigurationRoot.cs"

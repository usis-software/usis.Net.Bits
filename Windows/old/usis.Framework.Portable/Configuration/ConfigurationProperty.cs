//
//  @(#) ConfigurationProperty.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2016 usis GmbH. All rights reserved.

using System.Xml.Serialization;

namespace usis.Framework.Portable.Configuration
{
    //  ---------------------------
    //  ConfigurationProperty class
    //  ---------------------------

    /// <summary>
    /// Represents a property of an application configuration or a snap-in configuration.
    /// </summary>

    public class ConfigurationProperty : IConfigurationProperty
    {
        //  -------------
        //  Name property
        //  -------------

        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>

        [XmlAttribute("name")]
        public string Name { get; set; }

        //  --------------
        //  Value property
        //  --------------

        /// <summary>
        /// Gets or sets the value of the property as an object.
        /// </summary>
        /// <value>
        /// The value of the property as an object.
        /// </value>

        [XmlElement("value")]
        public object Value { get; set; }
    }
}

// eof "ConfigurationProperty.cs"

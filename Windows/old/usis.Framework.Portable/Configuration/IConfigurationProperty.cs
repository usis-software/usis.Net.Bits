//
//  @(#) IConfigurationProperty.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2016 usis GmbH. All rights reserved.

namespace usis.Framework.Portable.Configuration
{
    /// <summary>
    /// Represents a property of an application configuration or a snap-in configuration.
    /// </summary>

    public interface IConfigurationProperty
    {
        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>

        string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of the property as an object.
        /// </summary>
        /// <value>
        /// The value of the property as an object.
        /// </value>

        object Value { get; set; }
    }
}

// eof "IConfigurationProperty.cs"

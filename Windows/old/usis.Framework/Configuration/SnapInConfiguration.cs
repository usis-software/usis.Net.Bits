//
//  @(#) SnapInConfiguration.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2016 usis GmbH. All rights reserved.

using System;
using System.Xml.Serialization;
using usis.Framework.Portable.Configuration;

namespace usis.Framework.Configuration
{
    //  -------------------------
    //  SnapInConfiguration class
    //  -------------------------

    /// <summary>
    /// Holds information about the configuration of an snap-in. 
    /// </summary>

    [Obsolete("Use classes from the usis.Framework.Portable.Configuration namespace instead.")]
    [Serializable]
    public class SnapInConfiguration : ISnapInConfiguration
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="SnapInConfiguration"/> class.
        /// </summary>

        public SnapInConfiguration() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SnapInConfiguration"/> class.
        /// </summary>
        /// <param name="type">
        /// The type of the snap-in.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <i>type</i> is a null reference (<b>Nothing</b> in Visual Basic).
        /// </exception>

        public SnapInConfiguration(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            TypeName = type.AssemblyQualifiedName;
        }

        //internal SnapInConfiguration(SnapInConfigurationElement element)
        //{
        //    TypeName = element.TypeName;
        //    AssemblyFile = element.AssemblyFile;
        //}

        #endregion construction

        #region properties

        //  -----------------
        //  TypeName property
        //  -----------------

        /// <summary>
        /// Gets or sets the assembly-qualified name of the snap-in,
        /// which includes the name of the assembly from which the snap-in can be loaded.
        /// </summary>
        /// <value>
        /// The assembly-qualified name of the snap-in,
        /// which includes the name of the assembly from which the snap-in can be loaded.
        /// </value>

        [XmlAttribute("type")]
        public string TypeName { get; set; }

        ////  ---------------------
        ////  AssemblyFile property
        ////  ---------------------

        /// <summary>
        /// Gets or sets the name, including the path,
        /// of a file that contains an assembly that defines the requested type.
        /// </summary>
        /// <value>
        /// The name, including the path,
        /// of a file that contains an assembly that defines the requested type.
        /// </value>

        [XmlAttribute("assemblyFile")]
        public string AssemblyFile { get; set; }

        #endregion properties
    }
}

// eof "SnapInConfiguration.cs"

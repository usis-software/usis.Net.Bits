//
//  @(#) SnapInConfiguration.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

using System;
using System.Globalization;
using System.Xml.Serialization;

namespace usis.Framework.Configuration
{
    //  -------------------------
    //  SnapInConfiguration class
    //  -------------------------

    /// <summary>
    /// Holds information about the configuration of an snap-in. 
    /// </summary>

    public class SnapInConfiguration : ISnapInConfiguration//, IXmlSerializable
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
        /// <c>type</c> is a null reference (<c>Nothing</c> in Visual Basic).
        /// </exception>

        public SnapInConfiguration(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            TypeName = type.AssemblyQualifiedName;
        }

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

        //  ---------------------
        //  AssemblyFile property
        //  ---------------------

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

        #region IXmlSerializable implementation

        //  ----------------
        //  GetSchema method
        //  ----------------

        //XmlSchema IXmlSerializable.GetSchema() { return null; }

        //void IXmlSerializable.ReadXml(XmlReader reader)
        //{
        //    TypeName = reader.GetAttribute("type");
        //    AssemblyFile = reader.GetAttribute("assemblyFile");
        //}

        //void IXmlSerializable.WriteXml(XmlWriter writer)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion IXmlSerializable implementation

        #endregion properties

        #region overrides

        //  ---------------
        //  ToString method
        //  ---------------

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "Snap-in type='{0}'", TypeName);
        }

        #endregion overrides
    }
}

// eof "SnapInConfiguration.cs"

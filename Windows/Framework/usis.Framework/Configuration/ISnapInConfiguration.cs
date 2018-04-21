//
//  @(#) ISnapInConfiguration.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

namespace usis.Framework.Configuration
{
    //  ------------------------------
    //  ISnapInConfiguration interface
    //  ------------------------------

    /// <summary>
    /// Represents the configuration of an snap-in. 
    /// </summary>

    public interface ISnapInConfiguration
    {
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

        string TypeName { get; set; }

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

        string AssemblyFile { get; set; }
    }
}

// eof "ISnapInConfiguration.cs"

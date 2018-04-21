//
//  @(#) InstallerExtensions.cs
//
//  Project:    usis.Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2017 usis GmbH. All rights reserved.

using System;
using System.Configuration.Install;
using System.Globalization;

namespace usis.Platform
{
    //  -------------------------
    //  InstallerExtensions class
    //  -------------------------

    /// <summary>
    /// Provides extension methods to the <see cref="Installer"/> class.
    /// </summary>

    public static class InstallerExtensions
    {
        /// <summary>
        /// The name of the <b>assemblypath</b> parameter.
        /// </summary>

        public const string AssemblyPathParameterName = "assemblypath";

        //  -----------------------------------
        //  AppendAssemblyPathParameters method
        //  -----------------------------------

        /// <summary>
        /// Appends the specified parameter string to the <b>assemblypath</b> parameter in the <see cref="Installer.Context"/>.
        /// </summary>
        /// <param name="installer">The installer.</param>
        /// <param name="parameters">The parameter string.</param>

        public static void AppendAssemblyPathParameters(this Installer installer, string parameters)
        {
            if (installer == null) throw new ArgumentNullException(nameof(installer));

            var assemblypath = installer.Context.Parameters[AssemblyPathParameterName];
            assemblypath = string.Format(CultureInfo.InvariantCulture, "\"{0}\" {1}", assemblypath, parameters);
            installer.Context.Parameters[AssemblyPathParameterName] = assemblypath;
        }
    }
}

// eof "InstallerExtensions.cs"

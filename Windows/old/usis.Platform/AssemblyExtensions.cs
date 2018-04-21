//
//  @(#) AssemblyExtensions.cs
//
//  Project:    usis.Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2016 usis GmbH. All rights reserved.

using System;
using System.Collections;
using System.Configuration.Install;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace usis.Platform
{
    //  ------------------------
    //  AssemblyExtensions class
    //  ------------------------

    /// <summary>
    /// Provides extension method to the <see cref="Assembly"/> class.
    /// </summary>

    public static class AssemblyExtensions
    {
        //  ---------------
        //  GetTitle method
        //  ---------------

        /// <summary>
        /// Gets the title of the assembly.
        /// </summary>
        /// <param name="assembly">
        /// The assembly.
        /// </param>
        /// <returns>
        /// The title of the assembly specified by an <see cref="AssemblyTitleAttribute"/>.
        /// </returns>

        public static string GetTitle(this Assembly assembly)
        {
            AssemblyTitleAttribute attribute = assembly.GetCustomAttribute<AssemblyTitleAttribute>();
            return attribute == null ? null : attribute.Title;
        }

        //  -------------------
        //  GetCopyright method
        //  -------------------

        /// <summary>
        /// Gets the copyright of the assembly.
        /// </summary>
        /// <param name="assembly">
        /// The assembly.
        /// </param>
        /// <returns>
        /// The copyright of the assembly specified by an <see cref="AssemblyCopyrightAttribute"/>.
        /// </returns>

        public static string GetCopyright(this Assembly assembly)
        {
            AssemblyCopyrightAttribute attribute = assembly.GetCustomAttribute<AssemblyCopyrightAttribute>();
            return attribute == null ? null : attribute.Copyright;
        }

        //  ---------------------
        //  GetDescription method
        //  ---------------------

        /// <summary>
        /// Gets the text description of the assembly.
        /// </summary>
        /// <param name="assembly">
        /// The assembly.
        /// </param>
        /// <returns>
        /// The text description of the assembly specified by an <see cref="AssemblyDescriptionAttribute"/>.
        /// </returns>

        public static string GetDescription(this Assembly assembly)
        {
            AssemblyDescriptionAttribute attribute = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>();
            return attribute == null ? null : attribute.Description;
        }

        //  -----------------
        //  GetVersion method
        //  -----------------

        /// <summary>
        /// Gets the version of the assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>
        /// The version of the assembly specified by an <see cref="AssemblyVersionAttribute"/>.
        /// </returns>

        public static Version GetVersion(this Assembly assembly)
        {
            AssemblyVersionAttribute attribute = assembly.GetCustomAttribute<AssemblyVersionAttribute>();
            return attribute == null ? null : new Version(attribute.Version);
        }

        //  ---------------------
        //  GetFileVersion method
        //  ---------------------

        /// <summary>
        /// Gets the file version of the assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>
        /// The file version of the assembly specified by an <see cref="AssemblyFileVersionAttribute"/>.
        /// </returns>

        public static Version GetFileVersion(this Assembly assembly)
        {
            AssemblyFileVersionAttribute attribute = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>();
            return attribute == null ? null : new Version(attribute.Version);
        }

        //  --------------
        //  Install method
        //  --------------

        /// <summary>
        /// Performs the installation on the installers in the assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>
        /// The HResult of an exception that occurred during installation,
        /// or 0 if the installtion succeeded.
        /// </returns>

        public static int Install(this Assembly assembly)
        {
            return assembly.InvokeInstaller(true);
        }

        //  ----------------
        //  Uninstall method
        //  ----------------

        /// <summary>
        /// Performs the uninstallation on the installers in the assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>
        /// The HResult of an exception that occurred during uninstallation,
        /// or 0 if the uninstalltion succeeded.
        /// </returns>

        public static int Uninstall(this Assembly assembly)
        {
            return assembly.InvokeInstaller(false);
        }

        //  ----------------------
        //  InvokeInstaller method
        //  ----------------------

        /// <summary>
        /// Invokes the installers in the assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="install">if set to <c>true</c> performs an installation, otherwise an uninstallation.</param>
        /// <param name="commandLineOptions">The command line options.</param>
        /// <returns>
        /// The HResult of an exception that occurred during uninstallation,
        /// or 0 if the uninstalltion succeeded.
        /// </returns>

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static int InvokeInstaller(this Assembly assembly, bool install, params string[] commandLineOptions)
        {
            try
            {
                using (var installer = new AssemblyInstaller(assembly, null))
                {
                    installer.CommandLine = commandLineOptions;
                    installer.UseNewContext = true;
                    IDictionary state = new Hashtable();
                    try
                    {
                        if (install)
                        {
                            installer.Install(state);
                            installer.Commit(state);
                        }
                        else installer.Uninstall(state);
                    }
                    catch
                    {
                        if (install)
                        {
                            try
                            {
                                installer.Rollback(state);
                            }
                            catch { }
                            throw;
                        }
                        throw;
                    }
                }
            }
            catch (Exception exception)
            {
                return exception.HResult;
            }
            return 0;
        }
    }
}

// eof "AssemblyExtensions.cs"

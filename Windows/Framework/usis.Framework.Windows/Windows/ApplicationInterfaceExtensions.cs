//
//  @(#) ApplicationInterfaceExtensions.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2016 usis GmbH. All rights reserved.

using System;
using usis.Platform;

namespace usis.Framework.Windows
{
    //  ------------------------------------
    //  ApplicationInterfaceExtensions class
    //  ------------------------------------

    /// <summary>
    /// Provides extension methods to the <see cref="IApplication"/> interface.
    /// </summary>

    public static class ApplicationInterfaceExtensions
    {
        #region constants

        internal const string SettingsPathPropertyKey = "usis.Framework.SettingsPath";
        internal const string EventLogSourcePropertyKey = "usis.Framework.EventLogSource";

        #endregion constants

        #region SetEventLogSource method

        //  ------------------------
        //  SetEventLogSource method
        //  ------------------------

        /// <summary>
        /// Sets the name of the event log source.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="name">The name of the event log source.</param>

        public static void SetEventLogSource(this IApplication application, string name)
        {
            if (application == null) throw new ArgumentNullException(nameof(application));
            application.Properties.SetValue(EventLogSourcePropertyKey, name);
        }

        #endregion SetEventLogSource method

        #region GetEventLogSource method

        //  ------------------------
        //  GetEventLogSource method
        //  ------------------------

        internal static string GetEventLogSource(this IApplication application)
        {
            var source = application.Properties.GetString(EventLogSourcePropertyKey);
            return string.IsNullOrWhiteSpace(source) ? "usis.Framework" : source;
        }

        #endregion GetEventLogSource method

        #region AddRegistrySettings method

        //  --------------------------
        //  AddRegistrySettings method
        //  --------------------------

        /// <summary>
        /// Adds a registry settings extension with the specified path to the application.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="path">The path for the <see cref="RegistrySettings" /> extension.</param>
        /// <returns>The registry settings extension created.</returns>

        public static RegistrySettings AddRegistrySettings(this IApplication application, string path)
        {
            application.SetSettingsPath(path);
            return AddRegistrySettings(application);
        }

        /// <summary>
        /// Adds a registry settings extension to the application.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <returns>The registry settings extension created.</returns>

        public static RegistrySettings AddRegistrySettings(this IExtensibleObject<IApplication> application)
        {
            if (application == null) throw new ArgumentNullException(nameof(application));

            var settings = new RegistrySettings();
            application.Extensions.Add(settings);
            return settings;
        }

        #endregion AddRegistrySettings method
    }
}

// eof "ApplicationInterfaceExtensions.cs"

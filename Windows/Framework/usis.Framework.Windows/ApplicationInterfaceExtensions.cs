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

namespace usis.Framework
{
    //  ------------------------------------
    //  ApplicationInterfaceExtensions class
    //  ------------------------------------

    /// <summary>
    /// Provides extension methods to the <see cref="IApplication"/> interface.
    /// </summary>

    [Obsolete("Use type from usis.Framework.Windows assembly instead.")]
    public static class ApplicationInterfaceExtensions
    {
#if USIS_PLATFORM
        #region constants

        internal const string SettingsPathPropertyKey = "usis.Framework.SettingsPath";
        internal const string EventLogSourcePropertyKey = "usis.Framework.EventLogSource";

        #endregion constants

        #region GetSettingsPath method

        //  ----------------------
        //  GetSettingsPath method
        //  ----------------------

        /// <summary>
        /// Gets the path where the application stores settings.
        /// </summary>
        /// <param name="application">
        /// The application.
        /// </param>
        /// <returns>
        /// The path where the application stores settings.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <c>application</c> is a null reference (<b>Nothing</b> in Visual Basic).
        /// </exception>

        public static string GetSettingsPath(this IApplication application)
        {
            if (application == null) throw new ArgumentNullException(nameof(application));
            return application.Properties.GetString(SettingsPathPropertyKey);
        }

        #endregion GetSettingsPath method

        #region SetSettingsPath method

        //  ----------------------
        //  SetSettingsPath method
        //  ----------------------

        /// <summary>
        /// Sets the path where the application stores settings.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="path">The path where the application stores settings.</param>

        public static void SetSettingsPath(this IApplication application, string path)
        {
            if (application == null) throw new ArgumentNullException(nameof(application));
            application.Properties.SetValue(SettingsPathPropertyKey, path);
        }

        #endregion SetSettingsPath method
#endif
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

#if USIS_DATA

        #region AddDataSourceExtensionFromRegistrySettings method

        //  -------------------------------------------------
        //  AddDataSourceExtensionFromRegistrySettings method
        //  -------------------------------------------------

        /// <summary>
        /// Adds a <see cref="DataSourceApplicationExtension" /> to the application
        /// with a data source that is initialized from the specified registry settings.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="subkeyName">Name of the registry subkey.</param>
        /// <returns>A newly created <see cref="DataSourceApplicationExtension"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>

        public static DataSourceApplicationExtension AddDataSourceExtensionFromRegistrySettings(this IExtensibleObject<IApplication> application, string subkeyName)
        {
            if (application == null) throw new ArgumentNullException(nameof(application));
            var store = application.FindExtension<RegistrySettings>()?.LocalMachine.CreateSubStore(subkeyName, true);
            if (store == null) return null;
            var extension = new DataSourceApplicationExtension(new usis.Data.DataSource(store));
            application.Extensions.Add(extension);
            return extension;
        }

        #endregion AddDataSourceExtensionFromRegistrySettings method

#endif

    }
}

// eof "ApplicationInterfaceExtensions.cs"

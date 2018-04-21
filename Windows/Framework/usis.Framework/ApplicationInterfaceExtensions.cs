//
//  @(#) ApplicationInterfaceExtensions.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

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

    public static class ApplicationInterfaceExtensions
    {
        #region constants

        internal const string SettingsPathPropertyKey = "usis.Framework.SettingsPath";
        internal const string EventLogSourcePropertyKey = "usis.Framework.EventLogSource";

        #endregion constants

        #region Use method

        //  ----------
        //  Use method
        //  ----------

        /// <summary>
        /// Finds an extension of the specified type
        /// or creates the extension if it does not exists.
        /// </summary>
        /// <typeparam name="TExtension">
        /// The type of the extension.
        /// </typeparam>
        /// <param name="application">
        /// The application which is extened.
        /// </param>
        /// <returns>
        /// An existing or new created application extension.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="application"/> is a <c>null</c> reference (<c>Nothing</c> in Visual Basic).
        /// </exception>

        [Obsolete("Use the With(bool) extension method instead.")]
        public static TExtension Use<TExtension>(this IExtensibleObject<IApplication> application)
            where TExtension : IExtension<IApplication>, new()
        {
            if (application == null) throw new ArgumentNullException(nameof(application));
            TExtension extension = application.Extensions.Find<TExtension>();
            if (extension == null)
            {
                extension = new TExtension();
                application.Extensions.Add(extension);
            }
            return extension;
        }

        #endregion Use method

        #region Find method

        //  -----------
        //  Find method
        //  -----------

        /// <summary>
        /// Finds the specified extension object in the <see cref="IExtensibleObject{TObject}.Extensions"/> collection.
        /// </summary>
        /// <typeparam name="TExtension">The type of the extension.</typeparam>
        /// <param name="application">The application.</param>
        /// <returns>
        /// The extension object of the specified type.
        /// </returns>

        [Obsolete("Use the With() extension method instead.")]
        public static TExtension Find<TExtension>(this IExtensibleObject<IApplication> application)
            where TExtension : IExtension<IApplication>
        {
            if (application == null) throw new ArgumentNullException(nameof(application));
            return application.Extensions.Find<TExtension>();
        }

        #endregion Find method

        //  -----------
        //  With method
        //  -----------

        /// <summary>
        /// Gets the extension object of the specified type from the <see cref="IExtensibleObject{TObject}.Extensions" /> collection.
        /// </summary>
        /// <typeparam name="TExtension">The type of the extension object.</typeparam>
        /// <param name="application">The application.</param>
        /// <returns>
        /// The extension object of the specified type.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="application" /> is a <c>null</c> reference (<c>Nothing</c> in Visual Basic).
        /// </exception>

        public static TExtension With<TExtension>(this IExtensibleObject<IApplication> application) where TExtension : IExtension<IApplication>
        {
            if (application == null) throw new ArgumentNullException(nameof(application));
            return application.Extensions.Find<TExtension>();
        }

        /// <summary>
        /// Gets the extension object of the specified type and optionally creates the extension object if it does not exists.
        /// </summary>
        /// <typeparam name="TExtension">The type of the extension object.</typeparam>
        /// <param name="application">The application which is extened.</param>
        /// <param name="create">if set to <c>true</c> the extension object is created if it does not exist.</param>
        /// <returns>
        /// An existing or new created extension object.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="application" /> is a <c>null</c> reference (<c>Nothing</c> in Visual Basic).
        /// </exception>

        public static TExtension With<TExtension>(this IExtensibleObject<IApplication> application, bool create) where TExtension : IExtension<IApplication>, new()
        {
            if (application == null) throw new ArgumentNullException(nameof(application));
            TExtension extension = application.Extensions.Find<TExtension>();
            if (extension == null && create)
            {
                extension = new TExtension();
                application.Extensions.Add(extension);
            }
            return extension;
        }

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

        #region GetCommandLine method

        //  ---------------------
        //  GetCommandLine method
        //  ---------------------

        /// <summary>
        /// Gets the command line that was supplied when the application was started.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <returns>
        /// The command line that was supplied when the application was started.
        /// </returns>

        public static CommandLine GetCommandLine(this IExtensibleObject<IApplication> application)
        {
            return application.With<CommandLineApplicationExtension>()?.CommandLine;
        }

        #endregion GetCommandLine method
    }

    #region CommandLineApplicationExtension class

    //  -------------------------------------
    //  CommandLineApplicationExtension class
    //  -------------------------------------

    internal sealed class CommandLineApplicationExtension : Extension<IApplication>
    {
        //  --------------------
        //  CommandLine property
        //  --------------------

        internal CommandLine CommandLine { get; }

        //  ------------
        //  construction
        //  ------------

        internal CommandLineApplicationExtension(CommandLine commandLine) { CommandLine = commandLine; }
    }

    #endregion CommandLineApplicationExtension class

    #region IApplicationInjectable interface

    //  --------------------------------
    //  IApplicationInjectable interface
    //  --------------------------------

    /// <summary>
    /// Defines an interface to inject a <see cref="IApplication"/> dependency.
    /// </summary>
    /// <seealso cref="IInjectable{IApplication}" />

    [Obsolete("TODO: Check usage.")]
    public interface IApplicationInjectable : IInjectable<IApplication>
    {
        /// <summary>
        /// Gets the application.
        /// </summary>
        /// <value>
        /// The application.
        /// </value>

        IApplication Application { get; }
    }

    #endregion IApplicationInjectable interface
}

// eof "ApplicationInterfaceExtensions.cs"

﻿//
//  @(#) ApplicationInterfaceExtension.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2016 usis GmbH. All rights reserved.

using System;
using usis.Platform.Portable;

namespace usis.Framework.Portable
{
    //  -----------------------------------
    //  ApplicationInterfaceExtension class
    //  -----------------------------------

    /// <summary>
    /// Provides extension methods to the <see cref="IApplication"/> interface.
    /// </summary>

    [Obsolete("Use type from usis.Framework namespace instead.")]
    public static class ApplicationInterfaceExtension
    {
        #region UseExtension method

        //  -------------------
        //  UseExtension method
        //  -------------------

        /// <summary>
        /// Finds an extension of the specified type
        /// and creates the extension if it does not exists.
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
        /// <i>application</i> is a null reference (<b>Nothing</b> in Visual Basic).
        /// </exception>

        public static TExtension UseExtension<TExtension>(this IExtensibleObject<IApplication> application)
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

        #endregion UseExtension method
        
        #region FindExtension method

        //  --------------------
        //  FindExtension method
        //  --------------------

        /// <summary>
        /// Finds the specified extension object in the <see cref="IExtensibleObject{TObject}.Extensions"/> collection.
        /// </summary>
        /// <typeparam name="TExtension">The type of the extension.</typeparam>
        /// <param name="application">The application.</param>
        /// <returns>
        /// The extension object of the specified type.
        /// </returns>

        public static TExtension FindExtension<TExtension>(this IExtensibleObject<IApplication> application)
            where TExtension : IExtension<IApplication>
        {
            if (application == null) throw new ArgumentNullException(nameof(application));
            return application.Extensions.Find<TExtension>();
        }

        #endregion FindExtension method
    }

    #region IApplicationInjectable interface

    //  --------------------------------
    //  IApplicationInjectable interface
    //  --------------------------------

    /// <summary>
    /// Defines an interface to inject a <see cref="IApplication"/> dependency.
    /// </summary>
    /// <seealso cref="Platform.Portable.IInjectable{IApplication}" />

    [Obsolete("Use type from usis.Framework instead.")]
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

// eof "ApplicationInterfaceExtension.cs"
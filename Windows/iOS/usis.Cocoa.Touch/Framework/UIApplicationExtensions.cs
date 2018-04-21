//
//  @(#) UIApplicationExtensions.cs
//
//  Project:    usis Cocoa
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using System;
using UIKit;
using usis.Framework;
using usis.Platform;

namespace usis.Cocoa.Framework
{
    //  ----------------------------
    //  UIApplicationExtension class
    //  ----------------------------

    /// <summary>
    /// Provides extension methods to the <see cref="UIApplication"/> interface.
    /// </summary>

    public static class UIApplicationExtensions
    {
        //  --------------------
        //  FindExtension method
        //  --------------------

        /// <summary>
        /// Finds the specified application extension.
        /// </summary>
        /// <typeparam name="TExtension">The type of the extension.</typeparam>
        /// <param name="application">The application.</param>
        /// <returns>
        /// An instance of the extension, or <b>null</b> if no extension of the specified type is registered.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <i>application</i> is a null reference (<b>Nothing</b> in Visual Basic).
        /// </exception>

        public static TExtension FindExtension<TExtension>(this UIApplication application)
        {
            if (application == null) throw new ArgumentNullException(nameof(application));

            return Application.RunningApplication.Extensions.Find<TExtension>();
        }

        //  -------------------
        //  UseExtension method
        //  -------------------

        /// <summary>
        /// Finds an extension of the specified type
        /// and creates the extension if it does not exists.
        /// </summary>
        /// <typeparam name="TExtension">The type of the extension.</typeparam>
        /// <param name="application">The application.</param>
        /// <returns>
        /// An existing or new created application extension.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <i>application</i> is a null reference (<b>Nothing</b> in Visual Basic).
        /// </exception>

        public static TExtension UseExtension<TExtension>(this UIApplication application)
            where TExtension : IExtension<IApplication>, new()
        {
            if (application == null) throw new ArgumentNullException(nameof(application));

            return Application.RunningApplication.With<TExtension>(true);
        }
    }
}

// eof "UIApplicationExtensions.cs"

//
//  @(#) RegistrySettings.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using System;
using usis.Framework.Portable;
using usis.Platform.Portable;
using usis.Platform.Windows;

namespace usis.Framework.Windows
{
    //  ----------------------
    //  RegistrySettings class
    //  ----------------------

    /// <summary>
    /// Provides an application extension to access settings store in the Windows registry.
    /// </summary>
    /// <seealso cref="ApplicationExtension" />

    [Obsolete("Use type from usis.Framework.Windows assembly instead.")]
    public class RegistrySettings : ApplicationExtension
    {
        #region overrides

        //  ---------------
        //  OnAttach method
        //  ---------------

        /// <summary>
        /// Called when the extension is added to the
        /// <see cref="IExtensibleObject{TObject}.Extensions"/> property.
        /// </summary>
        ///// <param name="owner">The extensible object that aggregates this extension.</param>

        protected override void OnAttach()
        {
            var path = Owner.GetSettingsPath();
            if (string.IsNullOrWhiteSpace(path)) path = @"Software\usis\Framework";
            LocalMachine = RegistryValueStore.OpenLocalMachine().CreateSubStore(path, true);
        }

        #endregion overrides

        #region properties

        //  ---------------------
        //  LocalMachine property
        //  ---------------------

        /// <summary>
        /// Gets a value store to access the <see cref="Microsoft.Win32.Registry.LocalMachine"/> key.
        /// </summary>
        /// <value>
        /// A value store to access the <see cref="Microsoft.Win32.Registry.LocalMachine"/> key.
        /// </value>

        public IHierarchicalValueStore LocalMachine { get; private set; }

        #endregion properties
    }
}

// eof "RegistrySettings.cs"

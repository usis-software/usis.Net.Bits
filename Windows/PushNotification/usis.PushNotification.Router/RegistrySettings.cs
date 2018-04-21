//
//  @(#) RegistrySettings.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using usis.Framework.Portable;
using usis.Platform.Portable;
using usis.Platform.Windows;

namespace usis.Framework.Windows
{
    //  ----------------------
    //  RegistrySettings class
    //  ----------------------

    internal class RegistrySettings : ApplicationExtension
    {
        #region overrides

        //  ---------------
        //  OnAttach method
        //  ---------------

        protected override void OnAttach(IApplication owner)
        {
            var path = owner.GetSettingsPath();
            if (string.IsNullOrWhiteSpace(path)) path = @"Software\usis\Framework";
            LocalMachine = RegistryValueStore.OpenLocalMachine().CreateSubStore(path, true);
        }

        #endregion overrides

        #region properties

        //  ---------------------
        //  LocalMachine property
        //  ---------------------

        public IHierarchicalValueStore LocalMachine
        {
            get; private set;
        }

        #endregion properties
    }
}

// eof "RegistrySettings.cs"

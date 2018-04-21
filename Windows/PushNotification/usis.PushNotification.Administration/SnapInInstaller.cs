//
//  @(#) SnapInInstaller.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using usis.Platform;
using usis.Platform.Windows;

namespace usis.PushNotification.Administration
{
    //  ---------------------
    //  SnapInInstaller class
    //  ---------------------

    /// <summary>
    /// Allows the .NET framework to install the snap-in.
    /// </summary>

    [RunInstaller(true)]
    public sealed class SnapInInstaller : Microsoft.ManagementConsole.SnapInInstaller
    {
        //  ---------------------
        //  OnAfterInstall method
        //  ---------------------

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected override void OnAfterInstall(IDictionary savedState)
        {
            base.OnAfterInstall(savedState);

            try { CleanMuiCache(); }
            catch (Exception exception) { Debug.Print("Failed to clean MUI cache: {0}.", exception.Message); }
        }

        //  --------------------
        //  CleanMuiCache method
        //  --------------------

        private static void CleanMuiCache()
        {
            var registryPath = @"Local Settings\MuiCache";
            var key = RegistryValueStorage.OpenClassesRoot().OpenSubStorage(registryPath, false);

            foreach (var pair in key.EnumerateValuesInSubStorages())
            {
                if (pair.Value.Name.Contains("usis.PushNotification.Administration.About.dll"))
                {
                    pair.Storage.DeleteValue(pair.Value.Name);
                }
            }
        }
    }
}

// eof "SnapInInstaller.cs"

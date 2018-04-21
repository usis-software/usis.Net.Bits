//
//@(#) ApplicationExtension.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2016 usis GmbH. All rights reserved.

using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace usis.Windows
{
    //  --------------------------
    //  ApplicationExtension class
    //  --------------------------

    internal static class ApplicationExtension
    {
        #region constants

        private const string settingsPathPropertyName = "usis.Framework.SettingsPath";

        #endregion constants

        #region InitializeSettings method

        //  -------------------------
        //  InitializeSettings method
        //  -------------------------

        //public static void InitializeSettings(this Application application, string settingsPath)
        //{
        //    if (application == null) throw new ArgumentNullException("application");

        //    application.Properties[settingsPathPropertyName] = settingsPath;

        //} // InitializeSettings method

        #endregion InitializeSettings method

        #region UseSettingsUserRegistryKey method

        //  ---------------------------------
        //  UseSettingsUserRegistryKey method
        //  ---------------------------------

        public static RegistryKey UseSettingsUserRegistryKey(this Application application)
        {
            return application.UseSettingsUserRegistryKey(null);
        }

        public static RegistryKey UseSettingsUserRegistryKey(this Application application, string path)
        {
            if (application == null) throw new ArgumentNullException(nameof(application));

            string s = application.Properties[settingsPathPropertyName] as string;
            if (string.IsNullOrWhiteSpace(s)) return null;

            if (!string.IsNullOrWhiteSpace(path))
            {
                s = Path.Combine(s, path);
            }
            return Registry.CurrentUser.CreateSubKey(s);
        }

        #endregion UseSettingsUserRegistryKey method

        #region get/set settings

        //  -----------------
        //  GetSetting method
        //  -----------------

        //public static int GetSetting(this Application application, string name, int defaultValue)
        //{
        //    using (var registryKey = application.UseSettingsUserRegistryKey())
        //    {
        //        return registryKey.GetInt32(name, defaultValue);
        //    }

        //} // GetSetting method

        //public static double GetSetting(this Application application, string name, double defaultValue)
        //{
        //    using (var registryKey = application.UseSettingsUserRegistryKey())
        //    {
        //        return registryKey.GetDouble(name, defaultValue);
        //    }

        //} // GetSetting method

        //  -----------------
        //  SetSetting method
        //  -----------------

        //public static void SetSetting(this Application application, string name, int value)
        //{
        //    using (var registryKey = application.UseSettingsUserRegistryKey())
        //    {
        //        registryKey.SetValue(name, value);
        //    }

        //} // SetSetting method

        #endregion get/set settings

        #region ShowErrorDialog method

        //  ----------------------
        //  ShowErrorDialog method
        //  ----------------------

        //public static void ShowErrorDialog(this Application application, Exception exception)
        //{
        //    string caption = StringResources.ErrorDialogCaption;
        //    application.ShowErrorDialog(exception, caption);

        //} // ShowErrorDialog method

        //public static void ShowErrorDialog(this Application application, Exception exception, string caption)
        //{
        //    if (application == null) throw new ArgumentNullException("application");
        //    if (exception == null) throw new ArgumentNullException("exception");

        //    string message = exception.Message;
        //    application.ShowErrorDialog(message, caption);

        //} // ShowErrorDialog method

        //public static void ShowErrorDialog(this Application application, string message)
        //{
        //    string caption = StringResources.ErrorDialogCaption;
        //    application.ShowErrorDialog(message, caption);
        //}

        //public static void ShowErrorDialog(this Application application, string message, string caption)
        //{
        //    if (application == null) throw new ArgumentNullException("application");

        //    MessageBox.Show(
        //        application.MainWindow,
        //        message,
        //        caption,
        //        MessageBoxButton.OK,
        //        MessageBoxImage.Error);
        //}

        #endregion ShowErrorDialog method

        #region internal methods

        //  ---------------------
        //  GetProperty<T> method
        //  ---------------------

        //internal static T GetProperty<T>(this Application application, string propertyKey) where T : class, new()
        //{
        //    if (application == null) throw new ArgumentNullException("application");

        //    if (!application.Properties.Contains(propertyKey))
        //    {
        //        application.Properties.Add(propertyKey, new T());
        //    }
        //    return application.Properties[propertyKey] as T;

        //} // GetProperty<T> method

        #endregion internal methods

        #region public methods

        //  --------------------------
        //  WaitForUIToComplete method
        //  --------------------------

        //public static void WaitForUIToComplete(this System.Windows.Threading.DispatcherObject application)
        //{
        //    if (application == null)
        //        throw new ArgumentNullException("application");

        //    application.Dispatcher.BeginInvoke(
        //        System.Windows.Threading.DispatcherPriority.ApplicationIdle,
        //        new Action(() =>
        //        {
        //        })
        //    ).Wait();

        //} // WaitForUIToComplete method

        #endregion public methods
    }
}

// eof "ApplicationExtension.cs"

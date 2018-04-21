//
//  @(#) WindowExtension.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2016 usis GmbH. All rights reserved.

using System;
using System.Globalization;
using System.Windows;
using Microsoft.Win32;
using usis.Platform.Windows;

namespace usis.Windows
{
    //  ---------------------
    //  WindowExtension class
    //  ---------------------

    /// <summary>
    /// Provides extensions methods to the <see cref="Window"/> class.
    /// </summary>

    public static class WindowExtension
    {
        #region constants

        private const string windowsSubKeyName = "Windows";

        //private const int GWL_STYLE = -16;
        //private const int GWL_EXSTYLE = -20;

        //private const int WS_MAXIMIZEBOX = 0x00010000;
        //private const int WS_MINIMIZEBOX = 0x00020000;
        //private const int WS_SYSMENU = 0x00080000;

        //private const int WS_EX_DLGMODALFRAME = 0x00000001;

        #endregion constants

        #region SaveWindowState methods

        //  ----------------------
        //  SaveWindowState method
        //  ----------------------

        /// <summary>
        /// Saves the specified windows position, size and state to the Windows Registry.
        /// </summary>
        /// <param name="window">
        /// The window.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <i>window</i> is a null reference (<b>Nothing</b> in Visual Basic).
        /// </exception>

        public static void SaveWindowState(this Window window)
        {
            if (window == null) throw new ArgumentNullException(nameof(window));

            string keyName = window.GetType().Name;
            window.SaveWindowState(keyName);
        }

        private static void SaveWindowState(this Window window, string keyName)
        {
            using (var settingsKey = Application.Current.UseSettingsUserRegistryKey())
            {
                if (settingsKey == null) return;
                using (var windowsKey = settingsKey.CreateSubKey(windowsSubKeyName))
                {
                    using (var windowKey = windowsKey.CreateSubKey(keyName))
                    {
                        window.SaveWindowState(windowKey);
                    }
                }
            }
        }

        private static void SaveWindowState(this Window window, RegistryKey registryKey)
        {
            registryKey.SetValue(nameof(window.Top), window.Top, RegistryValueKind.DWord);
            registryKey.SetValue(nameof(window.Left), window.Left, RegistryValueKind.DWord);
            registryKey.SetValue(nameof(window.Width), window.Width, RegistryValueKind.DWord);
            registryKey.SetValue(nameof(window.Height), window.Height, RegistryValueKind.DWord);
            registryKey.SetValue(nameof(window.WindowState), window.WindowState, RegistryValueKind.DWord);
        }

        //private static void SaveWindowState(this Window window, RegistryKey registryKey, string path)
        //{
        //    if (registryKey == null) throw new ArgumentNullException("registryKey");

        //    using (var key = registryKey.CreateSubKey(path))
        //    {
        //        window.SaveWindowState(key);
        //    }

        //} // SaveWindowState method

        #endregion SaveWindowState methods

        #region RestoreWindowState methods

        //  -------------------------
        //  RestoreWindowState method
        //  -------------------------

        /// <summary>
        /// Restores the specified windows position, size and state from the Windows Registry.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <exception cref="ArgumentNullException">
        /// <i>window</i> is a null reference (<b>Nothing</b> in Visual Basic).
        /// </exception>

        public static void RestoreWindowState(this Window window)
        {
            if (window == null) throw new ArgumentNullException(nameof(window));

            string keyName = window.GetType().Name;
            window.RestoreWindowState(keyName);
        }

        private static void RestoreWindowState(this Window window, string keyName)
        {
            using (var settingsKey = Application.Current.UseSettingsUserRegistryKey())
            {
                if (settingsKey == null) return;
                using (var windowsKey = settingsKey.OpenSubKey(windowsSubKeyName))
                {
                    if (windowsKey == null) return;
                    using (var windowKey = windowsKey.OpenSubKey(keyName))
                    {
                        if (windowKey == null) return;
                        window.RestoreWindowState(windowKey);
                    }
                }
            }
        }

        private static void RestoreWindowState(this Window window, RegistryKey registryKey)
        {
            window.Top = registryKey.GetDouble(nameof(window.Top), window.Top);
            window.Left = registryKey.GetDouble(nameof(window.Left), window.Left);
            window.Width = registryKey.GetDouble(nameof(window.Width), window.Width);
            window.Height = registryKey.GetDouble(nameof(window.Height), window.Height);
            WindowState state = (WindowState)Convert.ToInt32(registryKey.GetValue(nameof(window.WindowState), window.WindowState), CultureInfo.InvariantCulture);
            window.WindowState = state == WindowState.Minimized ? WindowState.Normal : state;
        }

        //public static void RestoreWindowState(this Window window, RegistryKey registryKey, string path)
        //{
        //    if (registryKey == null) throw new ArgumentNullException("registryKey");

        //    using (var key = registryKey.CreateSubKey(path))
        //    {
        //        window.RestoreWindowState(key);
        //    }

        //} // RestoreWindowState method

        #endregion RestoreWindowState methods

        #region public methods

        //public static void HideMinimizeBox(this Window window)
        //{
        //    window.ClearWindowStyle(WS_MINIMIZEBOX);
        //}

        //public static void HideMaximizeBox(this Window window)
        //{
        //    window.ClearWindowStyle(WS_MAXIMIZEBOX);
        //}

        //public static void HideSysMenu(this Window window)
        //{
        //    window.ClearWindowStyle(WS_SYSMENU);
        //}

        //public static void SetModalDialogFrame(this Window window)
        //{
        //    window.SetWindowAttribute(GWL_EXSTYLE, WS_EX_DLGMODALFRAME);
        //}

        #endregion public methods

        #region private methods

        //private static void ClearWindowStyle(this Window window, int newStyle)
        //{
        //    window.ClearWindowAttribute(GWL_STYLE, newStyle);
        //}

        //private static void ClearWindowAttribute(this Window window, int index, int newStyle)
        //{
        //    var hwnd = new System.Windows.Interop.WindowInteropHelper(window).EnsureHandle();
        //    if (hwnd != IntPtr.Zero)
        //    {
        //        var style = NativeMethods.GetWindowLong(hwnd, index);
        //        if (style != 0)
        //        {
        //            style = NativeMethods.SetWindowLong(hwnd, index, style & ~newStyle);
        //        }
        //    }
        //}

        //private static void SetWindowAttribute(this Window window, int index, int newStyle)
        //{
        //    var hwnd = new System.Windows.Interop.WindowInteropHelper(window).EnsureHandle();
        //    if (hwnd != IntPtr.Zero)
        //    {
        //        var style = NativeMethods.GetWindowLong(hwnd, index);
        //        if (style != 0)
        //        {
        //            style = NativeMethods.SetWindowLong(hwnd, index, style | newStyle);
        //        }
        //    }
        //}

        #endregion private methods
    }

    #region NativeMethods class

    //  -------------------
    //  NativeMethods class
    //  -------------------

    //internal static class NativeMethods
    //{
    //    [DllImport("user32.dll")]
    //    extern internal static int GetWindowLong(IntPtr hwnd, int index);

    //    [DllImport("user32.dll")]
    //    extern internal static int SetWindowLong(IntPtr hwnd, int index, int value);

    //} // NativeMethods class

    #endregion NativeMethods class
}

// eof "WindowExtension.cs"

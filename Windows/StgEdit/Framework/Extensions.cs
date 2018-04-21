//
//  @(#) Extensions.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

using System.Collections.Generic;
using System.Windows.Forms;

//namespace usis.Windows.Forms.Framework
//{
//    internal static class Extensions
//    {
//        //  -----------------------
//        //  RestoreFormState method
//        //  -----------------------

//        //[Obsolete("TODO")]
//        //internal static void RestoreFormState(this Form form)
//        //{
//        //    using (var registryKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\usis\Storage Editor"))
//        //    {
//        //        if (registryKey == null) return;
//        //        form.RestoreFormState(RegistryValueStorage.FromRegistryKey(registryKey));
//        //    }
//        //}

//        //internal static void RestoreFormState(this Form form, IValueStorage storage)
//        //{
//        //    if (storage.ValueNames.Count() > 0)
//        //    {
//        //        form.StartPosition = FormStartPosition.Manual;
//        //        form.Top = storage.GetInt32(nameof(form.Top), form.Top, CultureInfo.InvariantCulture);
//        //        form.Left = storage.GetInt32(nameof(form.Left), form.Left, CultureInfo.InvariantCulture);
//        //        form.Width = storage.GetInt32(nameof(form.Width), form.Width, CultureInfo.InvariantCulture);
//        //        form.Height = storage.GetInt32(nameof(form.Height), form.Height, CultureInfo.InvariantCulture);
//        //        var state = (FormWindowState)storage.GetInt32(nameof(form.WindowState), (int)form.WindowState, CultureInfo.InvariantCulture);
//        //        form.WindowState = state == FormWindowState.Minimized ? FormWindowState.Normal : state;
//        //    }
//        //    if (form is IWindow window) window.RestoreSettings(storage);
//        //}

//        //  --------------------
//        //  SaveFormState method
//        //  --------------------

//        //[Obsolete("TODO")]
//        //internal static void SaveFormState(this Form form)
//        //{
//        //    using (var registryKey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\usis\Storage Editor"))
//        //    {
//        //        form.SaveFormState(RegistryValueStorage.FromRegistryKey(registryKey));
//        //    }
//        //}

//        //internal static void SaveFormState(this Form form, IValueStorage storage)
//        //{
//        //    storage.SetValue(nameof(form.WindowState), (int)(form.WindowState == FormWindowState.Minimized ? FormWindowState.Normal : form.WindowState));
//        //    if (form is IWindow window)
//        //    {
//        //        window.LastLocation.SaveFormLocation(storage);
//        //        window.LastSize.SaveFormSize(storage);
//        //        window.SaveSettings(storage);
//        //    }
//        //    else if (form.WindowState == FormWindowState.Normal)
//        //    {
//        //        form.Location.SaveFormLocation(storage);
//        //        form.Size.SaveFormSize(storage);
//        //    }
//        //}

//        //private static void SaveFormLocation(this Point location, IValueStorage storage)
//        //{
//        //    storage.SetValue(nameof(Form.Top), location.Y);
//        //    storage.SetValue(nameof(Form.Left), location.X);
//        //}

//        //private static void SaveFormSize(this Size size, IValueStorage registryKey)
//        //{
//        //    registryKey.SetValue(nameof(Form.Width), size.Width);
//        //    registryKey.SetValue(nameof(Form.Height), size.Height);
//        //}
//    }
//}

namespace usis.Windows.Forms
{
    internal static class TreeNodeExtensions
    {
        internal static IEnumerable<TreeNode> Children(this TreeNode node)
        {
            foreach (var item in node.Nodes)
            {
                yield return item as TreeNode;
            }
        }
    }
}

// eof "Extensions.cs"

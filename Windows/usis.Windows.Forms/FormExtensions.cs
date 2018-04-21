//
//  @(#) FormExtensions.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using usis.Platform;

namespace usis.Windows.Forms
{
    //  ---------------------
    //  FormExtensions  class
    //  ---------------------

    /// <summary>
    /// Provides extension methods to the <see cref="Form"/> class.
    /// </summary>

    public static class FormExtensions
    {
        //  -----------------------
        //  RestoreFormState method
        //  -----------------------

        /// <summary>
        /// Restores the state of a form from the specified value storage.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="storage">The value storage.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="form"/>
        /// or
        /// <paramref name="storage"/>
        /// is a <c>null</c> reference.
        /// </exception>

        public static void RestoreFormState(this Form form, IValueStorage storage)
        {
            if (form == null) throw new ArgumentNullException(nameof(form));
            if (storage == null) throw new ArgumentNullException(nameof(storage));

            if (IsStateSaved(storage))
            {
                form.StartPosition = FormStartPosition.Manual;
                form.Top = storage.GetInt32(nameof(form.Top), form.Top, CultureInfo.InvariantCulture);
                form.Left = storage.GetInt32(nameof(form.Left), form.Left, CultureInfo.InvariantCulture);
                form.Width = storage.GetInt32(nameof(form.Width), form.Width, CultureInfo.InvariantCulture);
                form.Height = storage.GetInt32(nameof(form.Height), form.Height, CultureInfo.InvariantCulture);
                var state = (FormWindowState)storage.GetInt32(nameof(form.WindowState), (int)form.WindowState, CultureInfo.InvariantCulture);
                form.WindowState = state == FormWindowState.Minimized ? FormWindowState.Normal : state;
            }
            else form.Shown += (sender, e) => { if (!IsStateSaved(storage)) form.SaveFormState(storage); };
            if (form is IWindow window) window.RestoreSettings(storage);
        }

        //  --------------------
        //  SaveFormState method
        //  --------------------

        /// <summary>
        /// Saves the state of a form to the specified value storage.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="storage">The value storage.</param>
        /// <exception cref="ArgumentNullException"><paramref name="storage" /> is a <c>null</c> reference.</exception>

        public static void SaveFormState(this Form form, IValueStorage storage)
        {
            if (form == null) throw new ArgumentNullException(nameof(form));

            storage.SetValue(nameof(form.WindowState), (int)(form.WindowState == FormWindowState.Minimized ? FormWindowState.Normal : form.WindowState));
            if (form is IWindow window)
            {
                window.LastLocation.SaveFormLocation(storage);
                window.LastSize.SaveFormSize(storage);
                window.SaveSettings(storage);
            }
            else if (form.WindowState == FormWindowState.Normal)
            {
                form.Location.SaveFormLocation(storage);
                form.Size.SaveFormSize(storage);
            }
        }

        #region private methods

        //  -------------------
        //  IsStateSaved method
        //  -------------------

        private static bool IsStateSaved(IValueStorage storage)
        {
            var names = new HashSet<string>(storage.ValueNames);
            return
                names.Contains(nameof(Form.WindowState)) ||
                names.Contains(nameof(Form.Top)) ||
                names.Contains(nameof(Form.Left)) ||
                names.Contains(nameof(Form.Width)) ||
                names.Contains(nameof(Form.Height));
        }

        //  -----------------------
        //  SaveFormLocation method
        //  -----------------------

        private static void SaveFormLocation(this Point location, IValueStorage storage)
        {
            storage.SetValue(nameof(Form.Top), location.Y);
            storage.SetValue(nameof(Form.Left), location.X);
        }

        //  -------------------
        //  SaveFormSize method
        //  -------------------

        private static void SaveFormSize(this Size size, IValueStorage registryKey)
        {
            registryKey.SetValue(nameof(Form.Width), size.Width);
            registryKey.SetValue(nameof(Form.Height), size.Height);
        }

        #endregion private methods
    }
}

// eof "FormExtensions.cs"

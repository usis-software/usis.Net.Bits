//
//  @(#) IReloadable.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

#pragma warning disable 1591

namespace usis.Platform
{
    #region IReloadable interface

    //  ---------------------
    //  IReloadable interface
    //  ---------------------

    public interface IReloadable
    {
        //  -------------
        //  Reload method
        //  -------------

        void Reload(bool throwException/*Action completionHandler*/);
    }

    #endregion IReloadable interface

    #region ReloadableInterfaceExtensions class

    //  -----------------------------------
    //  ReloadableInterfaceExtensions class
    //  -----------------------------------

    public static class ReloadableInterfaceExtensions
    {
        //  -------------
        //  Reload method
        //  -------------

        public static void Reload(this IReloadable reloadable)
        {
            if (reloadable == null) throw new ArgumentNullException(nameof(reloadable));
            reloadable.Reload(true);
        }
    }

    #endregion ReloadableInterfaceExtensions class

    #region ReloadableCollection class

    //  --------------------------
    //  ReloadableCollection class
    //  --------------------------

    public class ReloadableCollection<T> : ObservableCollection<T>, IReloadable
    {
        #region Reload method

        //  -------------
        //  Reload method
        //  -------------

        public void Reload(bool throwException)
        {
            var e = new CancelEventArgs();
            OnReloading(e);
            if (e.Cancel) return;
            try
            {
                OnReload(EventArgs.Empty);
            }
            catch (Exception exception)
            {
                if (!throwException) OnReloadFailed(new ExceptionEventArgs(exception));
                else throw;
            }
            OnReloaded(EventArgs.Empty);
        }

        #endregion Reload method

        #region virtual methods

        //  ---------------
        //  OnReload method
        //  ---------------

        protected virtual void OnReload(EventArgs e) { PerformReload?.Invoke(this, e); }

        //  ------------------
        //  OnReloading method
        //  ------------------

        protected virtual void OnReloading(CancelEventArgs e) { Reloading?.Invoke(this, e); }

        //  -----------------
        //  OnReloaded method
        //  -----------------

        protected virtual void OnReloaded(EventArgs e) { Reloaded?.Invoke(this, e); }

        //  ---------------------
        //  OnReloadFailed method
        //  ---------------------

        protected virtual void OnReloadFailed(ExceptionEventArgs e) { ReloadFailed?.Invoke(this, e); }

        #endregion virtual methods

        #region events

        //  ---------------
        //  Reloading event
        //  ---------------

        public event CancelEventHandler Reloading;

        //  -------------------
        //  PerformReload event
        //  -------------------

        public event EventHandler PerformReload;

        //  --------------
        //  Reloaded event
        //  --------------

        public event EventHandler Reloaded;

        //  ------------------
        //  ReloadFailed event
        //  ------------------

        public event EventHandler<ExceptionEventArgs> ReloadFailed;

        #endregion events
    }

    #endregion ReloadableCollection class

    #region ExceptionEventArgs class

    //  ------------------------
    //  ExceptionEventArgs class
    //  ------------------------

    public class ExceptionEventArgs : EventArgs
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionEventArgs"/> class
        /// with the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>

        public ExceptionEventArgs(Exception exception) { Exception = exception; }

        #endregion construction

        #region properties

        //  ------------------
        //  Exception property
        //  ------------------

        public Exception Exception { get; private set; }

        #endregion properties
    }

    #endregion ExceptionEventArgs class
}

// eof "IReloadable.cs"

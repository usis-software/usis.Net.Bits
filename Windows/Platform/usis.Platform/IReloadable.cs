//
//  @(#) IReloadable.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace usis.Platform
{
    #region IReloadable interface

    //  ---------------------
    //  IReloadable interface
    //  ---------------------

    /// <summary>
    /// Describes a method that allows an implementing type to reload its content.
    /// </summary>

    public interface IReloadable
    {
        //  -------------
        //  Reload method
        //  -------------

        /// <summary>
        /// Loads the content of the implementing type and optionally re-throws exceptions.
        /// </summary>
        /// <param name="throwException">If set to <c>true</c>, an exception during loading should be re-thrown.</param>
        /// <param name="completionHandler">A completion handler to invoke, when loading is completed.</param>

        void Reload(bool throwException, Action completionHandler);
    }

    #endregion IReloadable interface

    #region ReloadableInterfaceExtensions class

    //  -----------------------------------
    //  ReloadableInterfaceExtensions class
    //  -----------------------------------

    /// <summary>
    /// Provides extension methods for the <see cref="IReloadable"/> interface.
    /// </summary>

    public static class ReloadableInterfaceExtensions
    {
        //  -------------
        //  Reload method
        //  -------------

        /// <summary>
        /// Loads the content of the implementing type.
        /// </summary>
        /// <param name="reloadable">An object that implements <see cref="IReloadable" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="reloadable" /> is a null reference (<c>Nothing</c> in Visual Basic).</exception>

        public static void Reload(this IReloadable reloadable)
        {
            if (reloadable == null) throw new ArgumentNullException(nameof(reloadable));
            reloadable.Reload(true, null);
        }

        /// <summary>
        /// Loads the content of the implementing type and optionally re-throws expceptions.
        /// </summary>
        /// <param name="reloadable">An object that implements <see cref="IReloadable" />.</param>
        /// <param name="throwException">If set to <c>true</c>, an exception during loading is re-thrown.</param>
        /// <exception cref="ArgumentNullException"><paramref name="reloadable" /> is a null reference (<c>Nothing</c> in Visual Basic).</exception>

        public static void Reload(this IReloadable reloadable, bool throwException)
        {
            if (reloadable == null) throw new ArgumentNullException(nameof(reloadable));
            reloadable.Reload(throwException, null);
        }

        /// <summary>
        /// Loads the content of the implementing type and the invokes the specified completion handler.
        /// </summary>
        /// <param name="reloadable">An object that implements <see cref="IReloadable"/>.</param>
        /// <param name="completionHandler">A completion handler to invoke, when the loading is completed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="reloadable" /> is a null reference (<c>Nothing</c> in Visual Basic).</exception>

        public static void Reload(this IReloadable reloadable, Action completionHandler)
        {
            if (reloadable == null) throw new ArgumentNullException(nameof(reloadable));
            reloadable.Reload(true, completionHandler);
        }
    }

    #endregion ReloadableInterfaceExtensions class

    #region ReloadableCollection class

    //  --------------------------
    //  ReloadableCollection class
    //  --------------------------

    /// <summary>
    /// Provides an <see cref="ObservableCollection{T}"/> that can reload its content.
    /// </summary>
    /// <typeparam name="T">The type of the collection items.</typeparam>
    /// <seealso cref="System.Collections.ObjectModel.ObservableCollection{T}" />
    /// <seealso cref="IReloadable" />

    public class ReloadableCollection<T> : ObservableCollection<T>, IReloadable
    {
        #region Reload method

        //  -------------
        //  Reload method
        //  -------------

        /// <summary>
        /// Loads the content of the collection and optionally rethrows expceptions.
        /// </summary>
        /// <param name="throwException">If set to <c>true</c> an exception during the load should by rethrown.</param>
        /// <param name="completionHandler">A completion handler to invoke, when loading is completed.</param>

        public void Reload(bool throwException, Action completionHandler)
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
            completionHandler?.Invoke();
        }

        #endregion Reload method

        #region virtual methods

        //  ---------------
        //  OnReload method
        //  ---------------

        /// <summary>
        /// Raises the <see cref="ReloadableCollection{T}.PerformReload" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>

        protected virtual void OnReload(EventArgs e) { PerformReload?.Invoke(this, e); }

        //  ------------------
        //  OnReloading method
        //  ------------------

        /// <summary>
        /// Raises the <see cref="ReloadableCollection{T}.Reloading" /> event.
        /// </summary>
        /// <param name="e">The <see cref="CancelEventArgs"/> instance containing the event data.</param>

        protected virtual void OnReloading(CancelEventArgs e) { Reloading?.Invoke(this, e); }

        //  -----------------
        //  OnReloaded method
        //  -----------------

        /// <summary>
        /// Raises the <see cref="ReloadableCollection{T}.Reloaded" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>

        protected virtual void OnReloaded(EventArgs e) { Reloaded?.Invoke(this, e); }

        //  ---------------------
        //  OnReloadFailed method
        //  ---------------------

        /// <summary>
        /// Raises the <see cref="ReloadableCollection{T}.ReloadFailed" /> event.
        /// </summary>
        /// <param name="e">The <see cref="ExceptionEventArgs"/> instance containing the event data.</param>

        protected virtual void OnReloadFailed(ExceptionEventArgs e) { ReloadFailed?.Invoke(this, e); }

        #endregion virtual methods

        #region events

        //  ---------------
        //  Reloading event
        //  ---------------

        /// <summary>
        /// Occurs when the collection is beginning to reload its content.
        /// </summary>

        public event EventHandler<CancelEventArgs> Reloading;

        //  -------------------
        //  PerformReload event
        //  -------------------

        /// <summary>
        /// Occurs when the collection needs to reload its content.
        /// </summary>

        public event EventHandler PerformReload;

        //  --------------
        //  Reloaded event
        //  --------------

        /// <summary>
        /// Occurs when the collection has reloaded its content.
        /// </summary>

        public event EventHandler Reloaded;

        //  ------------------
        //  ReloadFailed event
        //  ------------------

        /// <summary>
        /// Occurs when the reload of the collection's content failed.
        /// </summary>

        public event EventHandler<ExceptionEventArgs> ReloadFailed;

        #endregion events
    }

    #endregion ReloadableCollection class
}

// eof "IReloadable.cs"

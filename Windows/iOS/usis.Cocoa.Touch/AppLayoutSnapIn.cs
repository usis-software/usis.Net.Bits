//
//  @(#) AppLayoutSnapIn.cs
//
//  Project:    usis Mobile App Framework
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using UIKit;
using usis.Cocoa.Framework;
using usis.Cocoa.UIKit;
using usis.Framework;

#pragma warning disable 1591

namespace usis.Mobile
{
    //  ---------------------
    //  AppLayoutSnapIn class
    //  ---------------------

    public class AppLayoutSnapIn : SnapIn
    {
        #region overrides

        //  -------------------
        //  OnConnecting method
        //  -------------------

        /// <summary>
        /// Raises the <see cref="SnapIn.Connecting" /> event.
        /// </summary>
        /// <param name="e">A <see cref="CancelEventArgs" /> object that contains the event data.</param>

        protected override void OnConnecting(CancelEventArgs e)
        {
            Application.Extensions.Add(new AppLayoutExtension<AppLayoutFactory>());
            base.OnConnecting(e);
        }

        #endregion overrides
    }

    //  ----------------------
    //  AppLayoutFactory class
    //  ----------------------

    public sealed class AppLayoutFactory : IAppLayoutFactory
    {
        #region fields

        private Dictionary<string, Type> mapping = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

        #endregion fields

        #region construction

        //  ------------
        //  construction
        //  ------------

        public AppLayoutFactory()
        {
            mapping.Add("ListView", typeof(ListViewController<object>));
            mapping.Add("DetailView", typeof(DetailViewController));
        }

        #endregion construction

        #region private methods

        //  --------------
        //  GetType method
        //  --------------

        private Type GetType(ViewDescription viewDescription)
        {
            if (!mapping.TryGetValue(viewDescription.TypeName, out Type type))
            {
                type = Type.GetType(viewDescription.TypeName);
            }
            if (!type.IsSubclassOf(typeof(UIViewController))) return null;
            return type;
        }

        #endregion private methods

        #region IAppLayoutFactory implementation

        //  ---------------------------
        //  CreateViewController method
        //  ---------------------------

        object IAppLayoutFactory.CreateView(ViewDescription viewDescription, params object[] parameters)
        {
            if (viewDescription == null) throw new ArgumentNullException(nameof(viewDescription));
            var type = GetType(viewDescription);
            if (type == null) return null;
            var viewController = Activator.CreateInstance(type, parameters) as UIViewController;
            return viewDescription.Navigation ? viewController.WithNavigation() : viewController;
        }

        //  -------------------
        //  SetStartView method
        //  -------------------

        void IAppLayoutFactory.SetStartView(IApplication application, object view)
        {
            application.SetRootViewController(view as UIViewController);
        }

        #endregion IAppLayoutFactory implementation
    }

    #region SnapInExtensions class

    //  ----------------------
    //  SnapInExtensions class
    //  ----------------------

    /// <summary>
    /// Provides extension methods to the <see cref="SnapIn"/> class.
    /// </summary>

    public static class SnapInExtensions
    {
        //  ----------------------
        //  SetActiveLayout method
        //  ----------------------

        public static void SetActiveLayout(this SnapIn snapIn, AppLayout layout)
        {
            if (snapIn == null) throw new ArgumentNullException(nameof(snapIn));
            var extension = snapIn?.Application?.With<AppLayoutExtension>();
            if (extension != null) extension.Layout = layout;
        }

        //  ----------------------
        //  GetActiveLayout method
        //  ----------------------

        public static AppLayout GetActiveLayout(this SnapIn snapIn)
        {
            return snapIn?.Application?.With<AppLayoutExtension>()?.Layout;
        }
    }

    #endregion SnapInExtensions class
}

// eof "AppLayoutSnapIn.cs"

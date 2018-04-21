//
//  @(#) AppLayoutSnapIn.cs
//
//  Project:    usis Mobile App Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using System.ComponentModel;
using usis.Framework.Portable;
using usis.Cocoa.Framework;
using usis.Mobile;
using System.Linq;
using UIKit;

namespace usis.Mobile
{
    //  ---------------------
    //  AppLayoutSnapIn class
    //  ---------------------

    internal class AppLayoutSnapIn : SnapIn
    {
        //  -------------------
        //  OnConnecting method
        //  -------------------

        protected override void OnConnecting(CancelEventArgs e)
        {
            Application.Extensions.Add(new AppLayoutExtension());
            base.OnConnecting(e);
        }
    }

    //  ------------------------
    //  AppLayoutExtension class
    //  ------------------------

    internal class AppLayoutExtension : ApplicationExtension
    {
        #region fields

        private AppLayoutFactory factory;

        #endregion fields

        #region properties

        //  ---------------
        //  Layout property
        //  ---------------

        public AppLayout Layout { get; set; }

        //  ----------------
        //  Factory property
        //  ----------------

        public AppLayoutFactory Factory
        {
            get
            {
                if (factory == null) factory = new AppLayoutFactory();
                return factory;
            }
        }

        #endregion properties

        #region overrides

        //  --------------
        //  OnStart method
        //  --------------

        protected override void OnStart(IApplication owner)
        {
            if (Layout != null)
            {
                ViewDescription view = null;
                if (Layout.RootViewKey == null || !Layout.Views.TryGetValue(Layout.RootViewKey, out view))
                {
                    if (Layout.Views.Values.Count == 1) view = Layout.Views.Values.ElementAt(0);
                }
                Owner.SetRootViewController(Factory.CreateViewController(view));
            }
            base.OnStart(owner);
        }

        #endregion overrides
    }

    //  ----------------------
    //  AppLayoutFactory class
    //  ----------------------

    internal class AppLayoutFactory
    {
        public UIViewController CreateViewController(ViewDescription viewDescription)
        {
            return new Cocoa.UIKit.TableViewController();
        }
    }

    //  ---------------------
    //  SnapInExtension class
    //  ---------------------

    internal static class SnapInExtension
    {
        public static void SetActiveLayout(this SnapIn snapIn, AppLayout layout)
        {
            snapIn.Application.UseExtension<AppLayoutExtension>().Layout = layout;
        }
        public static AppLayout GetActiveLayout(this SnapIn snapIn)
        {
            return snapIn?.Application?.FindExtension<AppLayoutExtension>()?.Layout;
        }
    }
}

// eof "AppLayoutSnapIn.cs"

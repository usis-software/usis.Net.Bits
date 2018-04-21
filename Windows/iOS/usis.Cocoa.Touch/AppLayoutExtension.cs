//
//  @(#) AppLayoutExtension.cs
//
//  Project:    usis Cocoa
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using System.Linq;
using usis.Framework.Portable;

namespace usis.Mobile
{
    //  ------------------------
    //  AppLayoutExtension class
    //  ------------------------

    internal abstract class AppLayoutExtension : ApplicationExtension
    {
        #region fields

        private IAppLayoutFactory factory;

        #endregion fields

        #region properties

        //  ---------------
        //  Layout property
        //  ---------------

        public AppLayout Layout { get; set; }

        //  ----------------
        //  Factory property
        //  ----------------

        public IAppLayoutFactory Factory
        {
            get
            {
                if (factory == null) factory = CreateFactory();
                return factory;
            }
        }

        #endregion properties

        #region abstract methods

        //  --------------------
        //  CreateFactory method
        //  --------------------

        protected abstract IAppLayoutFactory CreateFactory();

        #endregion abstract methods

        #region overrides

        //  --------------
        //  OnStart method
        //  --------------

        protected override void OnStart(IApplication owner)
        {
            if (Layout != null)
            {
                ViewDescription view = null;
                if (Layout.RootView == null || !Layout.Views.TryGetValue(Layout.RootView, out view))
                {
                    if (Layout.Views.Values.Count == 1) view = Layout.Views.Values.ElementAt(0);
                }
                Factory.SetStartView(owner, CreateViewController(view));
            }
            base.OnStart(owner);
        }

        #endregion overrides

        #region private methods

        //  ---------------------------
        //  CreateViewController method
        //  ---------------------------

        private object CreateViewController(ViewDescription view)
        {
            var instance = Factory.CreateView(view);
            (instance as IAppLayoutAttributes)?.SetAttributes(view.Attributes);
            (instance as IApplicationInjectable)?.Inject(Owner);
            return instance;
        }

        #endregion private methods
    }

    #region IAppLayoutFactory interface

    //  ---------------------------
    //  IAppLayoutFactory interface
    //  ---------------------------

    internal interface IAppLayoutFactory
    {
        //  -----------------
        //  CreateView method
        //  -----------------

        object CreateView(ViewDescription viewDescription);

        //  -------------------
        //  SetStartView method
        //  -------------------

        void SetStartView(IApplication application, object view);
    }

    #endregion IAppLayoutFactory interface

    #region AppLayoutExtension<TFactory> class

    //  ----------------------------------
    //  AppLayoutExtension<TFactory> class
    //  ----------------------------------

    internal class AppLayoutExtension<TFactory> : AppLayoutExtension
        where TFactory : IAppLayoutFactory, new()
    {
        //  --------------------
        //  CreateFactory method
        //  --------------------

        protected override IAppLayoutFactory CreateFactory()
        {
            return new TFactory();
        }
    }

    #endregion AppLayoutExtension<TFactory> class
}

// eof "AppLayoutExtension.cs"

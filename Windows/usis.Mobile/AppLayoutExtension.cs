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
using usis.Platform.Portable;

namespace usis.Mobile
{
    //  ------------------------
    //  AppLayoutExtension class
    //  ------------------------

    public abstract class AppLayoutExtension : ApplicationExtension
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

        protected override void OnStart()
        {
            if (Layout != null)
            {
                ViewDescription view = null;
                if (Layout.RootView == null || !Layout.Views.TryGetValue(Layout.RootView, out view))
                {
                    if (Layout.Views.Values.Count == 1) view = Layout.Views.Values.ElementAt(0);
                }
                Factory.SetStartView(Owner, CreateView(view));
            }
            base.OnStart();
        }

        #endregion overrides

        #region public methods

        //  -----------------
        //  CreateView method
        //  -----------------

        public object CreateView(string viewKey, params object[] parameters)
        {
            if (viewKey == null) return null;
            ViewDescription view = null;
            if (Layout.Views.TryGetValue(viewKey, out view))
            {
                return CreateView(view, parameters);
            }
            return null;
        }

        #endregion public methods

        #region private methods

        //  ---------------------------
        //  CreateViewController method
        //  ---------------------------

        private object CreateView(ViewDescription view, params object[] parameters)
        {
            var instance = Factory.CreateView(view, parameters);
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

    public interface IAppLayoutFactory
    {
        //  -----------------
        //  CreateView method
        //  -----------------

        object CreateView(ViewDescription viewDescription, params object[] parameters);

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

    public class AppLayoutExtension<TFactory> : AppLayoutExtension
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

    #region IAppLayoutAttributes interface

    //  ------------------------------
    //  IAppLayoutAttributes interface
    //  ------------------------------

    public interface IAppLayoutAttributes
    {
        void SetAttributes(IValueStore attributes);
    }

    #endregion IAppLayoutAttributes interface
}

// eof "AppLayoutExtension.cs"

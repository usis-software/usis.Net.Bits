//
//  @(#) ViewCommand.cs
//
//  Project:    usis Cocoa
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using System;
using UIKit;
using usis.Mobile;
using usis.Platform;

#pragma warning disable 1591

namespace usis.Cocoa.UIKit
{
    //  -----------------
    //  ViewCommand class
    //  -----------------

    public abstract class ViewCommand : ICommand, IAppLayoutAttributes
    {
        #region properties

        //  -------------------
        //  Controller property
        //  -------------------

        public object Controller { get; set; }

        //  -------------
        //  View property
        //  -------------

        public string View { get; set; }

        //  -----------------------
        //  ViewController property
        //  -----------------------

        protected UIViewController ViewController => Controller as UIViewController;

        #endregion properties

        #region methods

        //  --------------
        //  Execute method
        //  --------------

        public abstract void Execute(params object[] parameters);

        //  --------------------
        //  SetAttributes method
        //  --------------------

        public virtual void SetAttributes(IValueStorage attributes) { }

        //  ---------------------------
        //  CreateViewController method
        //  ---------------------------

        protected UIViewController CreateViewController(string viewKey, params object[] parameters)
        {
            if (viewKey == null) return null;
            return ViewController?.GetAppLayoutExtension()?.CreateView(View, parameters) as UIViewController;
        }

        #endregion methods
    }

    #region ValueStoreExtensions class

    //  --------------------------
    //  ValueStoreExtensions class
    //  --------------------------

    public static class ValueStoreExtensions
    {
        //  -----------------
        //  GetCommand method
        //  -----------------

        public static ICommand GetCommand(this IValueStorage store, string name, UIViewController controller)
        {
            var hStore = store as IHierarchicalValueStorage;
            if (hStore == null) return null;
            var commandStore = hStore.OpenStorage(name);
            if (commandStore == null) return null;
            var typeName = commandStore.GetString("TypeName");
            if (string.IsNullOrWhiteSpace(typeName)) return null;
            var type = Type.GetType(typeName, false, true);
            if (type == null) return null;
            var command = Activator.CreateInstance(type) as ICommand;
            if (command != null)
            {
                command.Controller = controller;
                (command as IAppLayoutAttributes)?.SetAttributes(commandStore);
            }
            return command;
        }
    }

    #endregion ValueStoreExtensions class
}

// eof "ViewCommand.cs"

//
//  @(#) Application.cs
//
//  Project:    usis.Universal
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using usis.Framework;
using usis.Framework.Configuration;
using usis.Platform;

namespace usis.Universal
{
    //  -----------------
    //  Application class
    //  -----------------

    public class Application : Windows.UI.Xaml.Application, IApplication
    {
        #region fields

        private readonly SnapInHost<SnapInActivator> snapInHost = new SnapInHost<SnapInActivator>();
        private ExtensionCollection<IApplication> extensions;
        private readonly HierarchicalValueStore properties = new HierarchicalValueStore();
        private Guid applicationToken;

        #endregion fields

        #region properties

        //  -------------------------
        //  ConnectedSnapIns property
        //  -------------------------

        public IEnumerable<ISnapIn> ConnectedSnapIns { get { return snapInHost.ConnectedSnapIns; } }

        //  -------------------
        //  Extensions property
        //  -------------------

        public IExtensionCollection<IApplication> Extensions
        {
            get
            {
                if (extensions == null) extensions = new ExtensionCollection<IApplication>(this);
                return extensions;
            }
        }

        //  -------------------
        //  Properties property
        //  -------------------

        public IHierarchicalValueStore Properties { get { return properties; } }

        //  -------------------------
        //  ApplicationToken property
        //  -------------------------

        public Guid ApplicationToken
        {
            get
            {
                if (applicationToken.IsEmpty())
                {
                    var token = Windows.Storage.ApplicationData.Current.LocalSettings.Values[nameof(ApplicationToken)];
                    if (token != null && token is Guid)
                    {
                        applicationToken = (Guid)token;
                    }
                    else
                    {
                        applicationToken = Guid.NewGuid();
                        Windows.Storage.ApplicationData.Current.LocalSettings.Values[nameof(ApplicationToken)] = applicationToken;
                    }
                }
                return applicationToken;
            }
        }

        #endregion properties

        #region IApplication methods

        //  -----------------------------
        //  ConnectRequiredSnapIns method
        //  -----------------------------

        public void ConnectRequiredSnapIns(ISnapIn instance, params Type[] snapInTypes)
        {
            snapInHost.ConnectRequiredSnapIns(this, instance, snapInTypes);
        }

        //  ----------------------
        //  ReportException method
        //  ----------------------

        public void ReportException(Exception exception)
        {
            throw new NotImplementedException();
        }

        #endregion IApplication methods

        #region public methods

        //  ----------
        //  Run method
        //  ----------

        public static void Run<TApplication>() where TApplication : Application, new()
        {
            Run<TApplication>(null);
        }

        public static void Run<TApplication>(ApplicationConfiguration configuration) where TApplication : Application, new()
        {
            Start((p) =>
            {
                var application = new TApplication();
                if (configuration != null) application.snapInHost.Configure(configuration);
                application.snapInHost.Startup(application);
            });
        }

        #endregion public methods
    }
}

// eof "Application.cs"

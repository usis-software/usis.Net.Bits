//
//  @(#) AppLayout.cs
//
//  Project:    usis Mobile App Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;

namespace usis.Mobile
{
    //  ---------------
    //  AppLayout class
    //  ---------------

    public class AppLayout
    {
        #region fields

        private Dictionary<string, ViewDescription> views = new Dictionary<string, ViewDescription>(StringComparer.OrdinalIgnoreCase);

        #endregion fields

        #region properties

        //  --------------
        //  Views property
        //  --------------

        public IDictionary<string, ViewDescription> Views { get { return views; } }

        //  -----------------
        //  RootView property
        //  -----------------

        public string RootView { get; set; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        public AppLayout() { }

        public AppLayout(params ViewDescription[] views)
        {
            if (views == null) throw new ArgumentNullException(nameof(views));
            foreach (var view in views) { Views.Add(view.Key, view); }
        }

        #endregion construction
    }
}

// eof "AppLayout.cs"

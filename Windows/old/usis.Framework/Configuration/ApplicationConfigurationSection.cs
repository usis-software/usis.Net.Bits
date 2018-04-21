//
//  @(#) ApplicationConfigurationSection.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using System.Configuration;
using usis.Framework.Configuration;

namespace usis.Framework.Configuration
{
    //  -------------------------------------
    //  ApplicationConfigurationSection class
    //  -------------------------------------

    internal class ApplicationConfigurationSection : ConfigurationSection
    {
        #region SnapIns property

        //  ----------------
        //  SnapIns property
        //  ----------------

        private const string snapIns = "snapIns";
        private const string snapIn = "snapIn";

        [ConfigurationProperty(snapIns)]
        [ConfigurationCollection(typeof(SnapInConfigurationElement), AddItemName = snapIn)]
        public SnapInConfigurationElementCollection SnapIns
        {
            get { return this[snapIns] as SnapInConfigurationElementCollection; }
        }

        #endregion SnapIns property
    }
}

// eof "ApplicationConfigurationSection.cs"

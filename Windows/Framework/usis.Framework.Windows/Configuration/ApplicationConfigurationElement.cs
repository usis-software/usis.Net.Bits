//
//  @(#) ApplicationConfigurationElement.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

using System.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace usis.Framework.Configuration
{
    //  -------------------------------------
    //  ApplicationConfigurationElement class
    //  -------------------------------------

    internal class ApplicationConfigurationElement : ConfigurationElement
    {
        #region Name property

        //  -------------
        //  Name property
        //  -------------

        private const string name = nameof(name);

        [ConfigurationProperty(name)]
        internal string Name => this[name] as string;

        #endregion Name property

        #region SnapIns collection

        //  ----------------
        //  SnapIns property
        //  ----------------

        private const string snapIns = "snapIns";
        private const string snapIn = "snapIn";

        [ConfigurationProperty(snapIns)]
        [ConfigurationCollection(typeof(SnapInConfigurationElement), AddItemName = snapIn)]
        public SnapInConfigurationElementCollection SnapIns => this[snapIns] as SnapInConfigurationElementCollection;

        #endregion SnapIns collection

        #region ApplicationProperties collection

        //  ------------------------------
        //  ApplicationProperties property
        //  ------------------------------

        private const string properties = "properties";
        private const string property = "property";

        [ConfigurationProperty(properties)]
        [ConfigurationCollection(typeof(PropertyConfigurationElement), AddItemName = property)]
        public PropertyConfigurationElementCollection ApplicationProperties => this[properties] as PropertyConfigurationElementCollection;

        #endregion ApplicationProperties collection

        #region methods

        //  ---------------------------------
        //  ToApplicationConfiguration method
        //  ---------------------------------

        internal ApplicationConfiguration ToApplicationConfiguration()
        {
            var configuration = new ApplicationConfiguration()
            {
                Name = Name
            };
            System.Diagnostics.Debug.Assert(SnapIns.Count == 0, "TODO: Implement the reading of snap-ins from the configuration element.");
            return configuration;
        }

        #endregion methods
    }

    #region ApplicationConfigurationElementCollection class

    //  -----------------------------------------------
    //  ApplicationConfigurationElementCollection class
    //  -----------------------------------------------

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class ApplicationConfigurationElementCollection : ConfigurationElementCollection
    {
        #region overrrides

        //  -----------------------
        //  CreateNewElement method
        //  -----------------------

        protected override ConfigurationElement CreateNewElement()
        {
            return new ApplicationConfigurationElement();
        }

        //  --------------------
        //  GetElementKey method
        //  --------------------

        protected override object GetElementKey(ConfigurationElement element)
        {
            var application = element as ApplicationConfigurationElement;
            return application?.Name;
        }

        #endregion overrrides
    }

    #endregion ApplicationConfigurationElementCollection class
}

// eof "ApplicationConfigurationElement.cs"

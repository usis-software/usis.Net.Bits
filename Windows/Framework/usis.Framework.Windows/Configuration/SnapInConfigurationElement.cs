//
//  @(#) SnapInConfigurationElement.cs
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
    //  --------------------------------
    //  SnapInConfigurationElement class
    //  --------------------------------

    internal class SnapInConfigurationElement : ConfigurationElement
    {
        #region TypeName property

        //  -----------------
        //  TypeName property
        //  -----------------

        private const string type = nameof(type);

        [ConfigurationProperty(type, IsRequired = true)]
        internal string TypeName => this[type] as string;
        
        #endregion TypeName property

        #region AssemblyFile property

        //  ---------------------
        //  AssemblyFile property
        //  ---------------------

        private const string assemblyFile = nameof(assemblyFile);

        [ConfigurationProperty(assemblyFile)]
        internal string AssemblyFile => this[assemblyFile] as string;
        
        #endregion AssemblyFile property

        #region SnapInProperties collection

        //  -------------------------
        //  SnapInProperties property
        //  -------------------------

        private const string properties = nameof(properties);
        private const string property = nameof(property);

        [ConfigurationProperty(properties)]
        [ConfigurationCollection(typeof(PropertyConfigurationElement), AddItemName = property)]
        public PropertyConfigurationElementCollection SnapInProperties => this[properties] as PropertyConfigurationElementCollection;
        
        #endregion SnapInProperties collection
    }

    #region SnapInConfigurationElementCollection class

    //  ------------------------------------------
    //  SnapInConfigurationElementCollection class
    //  ------------------------------------------

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class SnapInConfigurationElementCollection : ConfigurationElementCollection
    {
        #region overrides

        //  -----------------------
        //  CreateNewElement method
        //  -----------------------

        protected override ConfigurationElement CreateNewElement()
        {
            var snapIn = new SnapInConfigurationElement();
            return snapIn;
        }

        //  --------------------
        //  GetElementKey method
        //  --------------------

        protected override object GetElementKey(ConfigurationElement element)
        {
            var snapIn = element as SnapInConfigurationElement;
            return snapIn?.TypeName;
        }

        #endregion overrides
    }

    #endregion SnapInConfigurationElementCollection class
}

// eof "SnapInConfigurationElement.cs"

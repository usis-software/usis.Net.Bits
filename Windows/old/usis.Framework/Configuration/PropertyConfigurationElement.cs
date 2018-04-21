//
//  @(#) PropertyConfigurationElement.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2016 usis GmbH. All rights reserved.

using System.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace usis.Framework.Configuration
{
    //  ----------------------------------
    //  PropertyConfigurationElement class
    //  ----------------------------------

    internal class PropertyConfigurationElement : ConfigurationElement
    {
        #region Name property

        //  -------------
        //  Name property
        //  -------------

        private const string name = nameof(name);

        [ConfigurationProperty(name, IsRequired = true, IsKey = true)]
        internal string Name { get { return this[name] as string; } }

        #endregion Name property

        #region Value

        //  -----------------
        //  RawValue property
        //  -----------------

        private const string valuePropertyName = "value";

        [ConfigurationProperty(valuePropertyName)]
        internal string RawValue { get { return this[valuePropertyName] as string; } }

        #endregion Value
    }

    #region PropertyConfigurationElementCollection class

    //  --------------------------------------------
    //  PropertyConfigurationElementCollection class
    //  --------------------------------------------

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class PropertyConfigurationElementCollection : ConfigurationElementCollection
    {
        #region overrides

        //  -----------------------
        //  CreateNewElement method
        //  -----------------------

        protected override ConfigurationElement CreateNewElement()
        {
            var property = new PropertyConfigurationElement();
            return property;
        }

        //  --------------------
        //  GetElementKey method
        //  --------------------

        protected override object GetElementKey(ConfigurationElement element)
        {
            var property = element as PropertyConfigurationElement;
            return property?.Name;
        }

        #endregion overrides
    }

    #endregion PropertyConfigurationElementCollection class
}

// eof "PropertyConfigurationElement.cs"

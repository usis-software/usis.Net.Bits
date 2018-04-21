//
//  @(#) ViewDescription.cs
//
//  Project:    usis Mobile App Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System;
using usis.Platform;

namespace usis.Mobile
{
    //  ---------------------
    //  ViewDescription class
    //  ---------------------

    public class ViewDescription
    {
        #region fields

        private HierarchicalValueStorage attributes = new HierarchicalValueStorage();

        #endregion fields

        #region properties

        //  ------------
        //  Key property
        //  ------------

        public string Key { get; private set; }

        //  -----------------
        //  TypeName property
        //  -----------------

        public string TypeName { get; set; }

        //  -------------------
        //  Attributes property
        //  -------------------

        public IHierarchicalValueStorage Attributes { get { return attributes; } }

        //  -------------------
        //  Navigation property
        //  -------------------

        public bool Navigation { get; set; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        public ViewDescription() { }

        public ViewDescription(string key, Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            TypeName = type.AssemblyQualifiedName;
            Key = key;
        }

        public ViewDescription(string key, string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName)) throw new ArgumentNullOrWhiteSpaceException(nameof(typeName));
            TypeName = typeName;
            Key = key;
        }

        #endregion construction

        #region methods

        //  -------------------
        //  AddAttribute method
        //  -------------------

        public ViewDescription AddAttribute(string name, object value)
        {
            Attributes.SetValue(name, value);
            return this;
        }

        #endregion methods
    }
}

// eof "ViewDescription.cs"

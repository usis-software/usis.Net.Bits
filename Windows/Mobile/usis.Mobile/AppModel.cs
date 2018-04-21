//
//  @(#) AppModel.cs
//
//  Project:    usis Mobile App Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using System.Collections;
using System.Reflection;
using usis.Framework;

namespace usis.Mobile
{
    //  --------------
    //  AppModel class
    //  --------------

    public class AppModel : ApplicationExtension
    {
        //  --------------------
        //  GetCollection method
        //  --------------------

        public IEnumerable GetCollection(string name)
        {
            var property = GetType().GetTypeInfo().GetDeclaredProperty(name);
            if (typeof(IEnumerable).GetTypeInfo().IsAssignableFrom(property.PropertyType.GetTypeInfo()))
            {
                return property.GetValue(this) as IEnumerable;
            }
            return null;
        }
    }

    #region IReloadable interface

    //  ---------------------
    //  IReloadable interface
    //  ---------------------

    //public interface IReloadable
    //{
    //    //  -------------
    //    //  Reload method
    //    //  -------------

    //    void Reload(Action completionHandler);
    //}

    #endregion IReloadable interface
}

// eof "AppModel.cs"

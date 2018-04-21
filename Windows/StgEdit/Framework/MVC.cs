//
//  @(#) MVC.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using usis.Platform;

namespace usis.Framework
{
    //interface IModel
    //{
    //}

    #region IView<TModel> interface

    //  -----------------------
    //  IView<TModel> interface
    //  -----------------------

    interface IView<TModel> : IInjectable<TModel>
    {
        //  --------------
        //  Model property
        //  --------------

        TModel Model { get; }
    }

    #endregion IView<TModel> interface

    #region IController<TModel, TView> interface

    //  ------------------------------------
    //  IController<TModel, TView> interface
    //  ------------------------------------

    interface IController<TModel, TView> : IInjectable<TModel> where TView : IView<TModel> // where TModel : IModel
    {
        //  --------------
        //  Model property
        //  --------------

        TModel Model { get; }

        //  -------------
        //  View property
        //  -------------

        TView View { get; }
    }

    #endregion IController<TModel, TView> interface
}

// eof "MVC.cs"

//
//  @(#) ApnsRouter.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2016 usis GmbH. All rights reserved.

using System.Diagnostics.CodeAnalysis;
using usis.Framework.ServiceModel.Web;

namespace usis.PushNotification.Web
{
    //  ----------------
    //  ApnsRouter class
    //  ----------------

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal sealed class ApnsRouter : PushNotification.ApnsRouter { }

    #region ApnsRouterSnapIn class

    //  ----------------------
    //  ApnsRouterSnapIn class
    //  ----------------------

    internal sealed class ApnsRouterSnapIn : WebServiceHostSnapIn<ApnsRouter> { }

    #endregion ApnsRouterSnapIn class
}

// eof "ApnsRouter.cs"

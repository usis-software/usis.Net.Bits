//
//  @(#) Wns.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2017 usis GmbH. All rights reserved.

using System.Diagnostics.CodeAnalysis;
using usis.Framework.ServiceModel.Web;

namespace usis.PushNotification.Web
{
    //  ---------
    //  Wns class
    //  ---------

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal sealed class Wns : PushNotification.Wns { }

    #region WnsSnapIn class

    //  ---------------
    //  WnsSnapIn class
    //  ---------------

    internal sealed class WnsSnapIn : WebServiceHostSnapIn<Wns>
    {
        public WnsSnapIn() : base(true, false) { }
    }

    #endregion WnsSnapIn class
}

// eof "Wns.cs"

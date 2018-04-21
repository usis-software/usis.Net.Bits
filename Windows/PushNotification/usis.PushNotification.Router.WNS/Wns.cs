//
//  @(#) Wns.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2017 usis GmbH. All rights reserved.

using System;
using System.Diagnostics.CodeAnalysis;
using usis.Framework;
using usis.Framework.ServiceModel;

namespace usis.PushNotification
{
    //  ---------
    //  Wns class
    //  ---------

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class Wns : ServiceBase, IWns
    {
        #region RegisterReceiver method

        //  -----------------------
        //  RegisterReceiver method
        //  -----------------------

        OperationResult<Guid> IWns.RegisterReceiver(string packageSid, string deviceIdentifier, Uri channelUri)
        {
            return OperationResult.Invoke<Guid>(
                (result) => Model.RegisterReceiver(
                    result,
                    new WnsReceiverKey(new WnsChannelKey(packageSid), deviceIdentifier) { ChannelUri = channelUri },
                    null, null, null, null));
        }

        #endregion RegisterReceiver method
    }

    #region WnsSnapIn class

    //  ---------------
    //  WnsSnapIn class
    //  ---------------

    internal class WnsSnapIn : ServiceHostSnapIn<WcfServiceHostFactory<Wns>>
    {
        public WnsSnapIn() : base(true, false) { }
    }

    #endregion WnsSnapIn class
}

// eof "Wns.cs"

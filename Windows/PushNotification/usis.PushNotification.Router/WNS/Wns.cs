//
//  @(#) Wns.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2016 usis GmbH. All rights reserved.

using System;
using System.Diagnostics.CodeAnalysis;
using usis.Framework.Portable;
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
            return OperationResult.Invoke((result) => Model.RegisterReceiver(result,
                new WnsReceiverKey(new WnsChannelKey(packageSid), deviceIdentifier)
                {
                    ChannelUri = channelUri
                },
                null, null, null, null));
        }

        #endregion RegisterReceiver method
    }

    #region WnsSnapIn class

    //  ---------------
    //  WnsSnapIn class
    //  ---------------

    internal class WnsSnapIn : ServiceHostBaseSnapIn<ServiceHostConfigurator<Wns>> { }

    #endregion WnsSnapIn class
}

// eof "Wns.cs"

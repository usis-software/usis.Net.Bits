//
//  @(#) Apns.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2016 usis GmbH. All rights reserved.

using System;
using usis.Framework.Portable;
using usis.Framework.ServiceModel.Web;

namespace usis.PushNotification.Web
{
    //  ----------
    //  Apns class
    //  ----------

    internal sealed class Apns : ServiceBase, IApns
    {
        #region RegisterReceiver method

        //  -----------------------
        //  RegisterReceiver method
        //  -----------------------

        OperationResult<Guid> IApns.RegisterReceiver(
            string bundleId,
            Environment environment,
            string deviceToken,
            string hexDeviceToken,
            string name,
            string account,
            string groups,
            string info)
        {
            return OperationResult.Invoke((result) => Model.RegisterReceiver(result,
                new ApnsReceiverKey(new ApnsChannelKey(bundleId, environment),
                deviceToken, hexDeviceToken), name, account, groups, info));
        }

        #endregion RegisterReceiver method
    }

    #region ApnsSnapIn class

    //  ----------------
    //  ApnsSnapIn class
    //  ----------------

    internal sealed class ApnsSnapIn : WebServiceHostSnapIn<Apns> { }

    #endregion ApnsSnapIn class
}

// eof "Apns.cs"

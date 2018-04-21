//
//  @(#) NetworkOperationPool.cs
//
//  Project:    usis Cocoa
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using UIKit;
using usis.Cocoa.Framework;
using usis.Framework;
using usis.Framework.Net;
using usis.Platform;

#pragma warning disable 1591

namespace usis.Cocoa.Net
{
    //  --------------------------
    //  NetworkOperationPool class
    //  --------------------------

    public class NetworkOperationPool : usis.Framework.Net.NetworkOperationPool
    {
        //  -------------------------------
        //  OnNetworkActivityStarted method
        //  -------------------------------

        protected override void OnNetworkActivityStarted()
        {
            UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
        }

        //  -------------------------------
        //  OnNetworkActivityStopped method
        //  -------------------------------

        protected override void OnNetworkActivityStopped()
        {
            UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
        }
    }

    #region ApplicationInterfaceExtension class

    //  -----------------------------------
    //  ApplicationInterfaceExtension class
    //  -----------------------------------

    public static class ApplicationInterfaceExtension
    {
        //  ------------------------------
        //  PerformNetworkOperation method
        //  ------------------------------

        public static void PerformNetworkOperation(this IExtensibleObject<IApplication> application, INetworkOperation operation)
        {
            application.With<NetworkOperationPool>(true).Perform(operation);
        }
    }

    #endregion ApplicationInterfaceExtension class

    #region UIApplicationExtension class

    //  ----------------------------
    //  UIApplicationExtension class
    //  ----------------------------

    public static class UIApplicationExtension
    {
        //  ------------------------------
        //  PerformNetworkOperation method
        //  ------------------------------

        public static void PerformNetworkOperation(this UIApplication application, INetworkOperation operation)
        {
            application.UseExtension<NetworkOperationPool>().Perform(operation);
        }
    }

    #endregion UIApplicationExtension class
}

// eof "NetworkOperationPool.cs"

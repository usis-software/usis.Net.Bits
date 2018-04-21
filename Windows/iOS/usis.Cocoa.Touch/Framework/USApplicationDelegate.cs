//
//  @(#) USApplicationDelegate.cs
//
//  Project:    usis Cocoa
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using Foundation;
using System.Diagnostics.CodeAnalysis;
using UIKit;

namespace usis.Cocoa.Framework
{
    //  ---------------------------
    //  USApplicationDelegate class
    //  ---------------------------

    /// <summary>
    /// Provides a base class for an application delegate
    /// that connects snap-ins on launch and disconnects snap-ins on termination.
    /// </summary>
    /// <seealso cref="Cocoa.USApplicationDelegate" />

    [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
    public class USApplicationDelegate : Cocoa.USApplicationDelegate
    {
        //  --------------------------
        //  WillFinishLaunching method
        //  --------------------------

        /// <summary>
        /// Indicates that launching has begun, but state restoration has not yet occurred.
        /// </summary>
        /// <param name="application">Reference to the UIApplication that invoked this delegate method.</param>
        /// <param name="launchOptions">
        /// An NSDictionary with the launch options, can be null.
        /// Possible key values are UIApplication's LaunchOption static properties.
        /// </param>
        /// <returns>
        /// False if the application is unable to handle the specified URL, true otherwise.
        /// </returns>

        public override bool WillFinishLaunching(UIApplication application, NSDictionary launchOptions)
        {
            Application.RunningApplication?.Start(application);
            return false;
        }

        //  --------------------
        //  WillTerminate method
        //  --------------------

        /// <summary>
        /// Called if the application is being terminated due to memory constraints or directly by the user.
        /// </summary>
        /// <param name="application">Reference to the UIApplication that invoked this delegate method.</param>

        public override void WillTerminate(UIApplication application)
        {
            Application.RunningApplication?.Stop();
        }
    }
}

// eof "USApplicationDelegate.cs"

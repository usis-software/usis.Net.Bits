//
//  @(#) USApplicationDelegate.cs
//
//  Project:    usis Cocoa
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using Foundation;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics.CodeAnalysis;
using UIKit;
using usis.Cocoa.Foundation;
using usis.Cocoa.UIKit;
using usis.Cocoa.Touch;

namespace usis.Cocoa
{
    //  ---------------------------
    //  USApplicationDelegate class
    //  ---------------------------

    /// <summary>
    /// Provides a base class for an application delegate.
    /// </summary>
    /// <seealso cref="UIApplicationDelegate" />

    [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
    public class USApplicationDelegate : UIApplicationDelegate
    {
        #region fields

        private UIWindow window;

        #endregion fields

        #region properties

        //  ---------------
        //  Window property
        //  ---------------

        /// <summary>
        /// The window used to display the app on the device's main screen.
        /// </summary>

        public override UIWindow Window
        {
            get
            {
                if (window == null)
                {
                    // create a new window instance based on the screen size
                    window = new UIWindow(UIScreen.MainScreen.Bounds);
                }
                return window;
            }
            set => window = value;
        }

        #endregion properties

        #region overrides

        //  ------------------------
        //  FinishedLaunching method
        //  ------------------------

        /// <summary>
        /// Method invoked after the application has launched to configure the main window and view controller.
        /// </summary>
        /// <param name="application">Reference to the UIApplication that invoked this delegate method.</param>
        /// <param name="launchOptions">
        /// An NSDictionary with the launch options, can be null.
        /// Possible key values are UIApplication's LaunchOption static properties.
        /// </param>
        /// <returns>
        /// <b>false</b> if the app cannot handle the URL resource or continue a user activity,
        /// otherwise return <b>true</b>.
        /// The return value is ignored if the app is launched as a result of a remote notification.
        /// </returns>

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            if (Window.RootViewController == null)
            {
                using (var viewController = new FullScreenLabelViewController())
                {
                    viewController.Label.Text = Strings.NoRootView;
                    viewController.Label.TextColor = UIColor.White;
                    viewController.View.BackgroundColor = UIColor.DarkGray;
                    Window.RootViewController = viewController;
                }
            }

            // make the window visible
            if (!Window.IsKeyWindow) Window.MakeKeyWindow();
            if (Window.Hidden) Window.Hidden = false;

            return false;
        }

        //  ---------------------------------------------
        //  FailedToRegisterForRemoteNotifications method
        //  ---------------------------------------------

        /// <summary>
        /// Indicates that a call to <see cref="M:UIKit.UIApplication.RegisterForRemoteNotifications" /> failed.
        /// </summary>
        /// <param name="application">Reference to the UIApplication that invoked this delegate method.</param>
        /// <param name="error">An NSError object that encapsulates information why registration did not succeed.
        /// The app can choose to display this information to the user.</param>
        /// <exception cref="ArgumentNullException"><i>error</i> is a null reference.</exception>

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            if (error == null) throw new ArgumentNullException(nameof(error));

            // remote notification not supported in simulator
            if (NSError.CocoaErrorDomain.ToString().Equals(error.Domain, StringComparison.Ordinal) && error.Code == 3010)
            {
                System.Diagnostics.Debug.WriteLine(error.LocalizedDescription);
                return;
            }

            Window.RootViewController.ShowAlert(error);
        }

        //  ---------------------------------
        //  ReceivedRemoteNotification method
        //  ---------------------------------

        /// <summary>
        /// Indicates that the application received a remote notification.
        /// </summary>
        /// <param name="application">Reference to the UIApplication that invoked this delegate method.</param>
        /// <param name="userInfo">A dictionary whose "aps" key contains information related to the notification.</param>

        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            ReceivedRemoteNotification(application, userInfo.ToJObject());
        }

        #endregion overrides

        #region methods

        //  ---------------------------------
        //  ReceivedRemoteNotification method
        //  ---------------------------------

        /// <summary>
        /// Indicates that the application received a remote notification.
        /// </summary>
        /// <param name="application">Reference to the UIApplication that received a remote notification.</param>
        /// <param name="userInfo">A JSON dictionary that contains information related to the notification.</param>

        public virtual void ReceivedRemoteNotification(UIApplication application, JObject userInfo) { }

        #endregion methods
    }
}

// eof "USApplicationDelegate.cs"

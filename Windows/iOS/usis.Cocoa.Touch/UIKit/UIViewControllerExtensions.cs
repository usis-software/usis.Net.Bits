//
//  @(#) UIViewControllerExtensions.cs
//
//  Project:    usis Cocoa
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using Foundation;
using System;
using System.Linq;
using UIKit;
using usis.Cocoa.Foundation;
using usis.Framework;
using usis.Mobile;
using usis.Platform;
using usis.Cocoa.Touch;

#pragma warning disable 1591

namespace usis.Cocoa.UIKit
{
    //  --------------------------------
    //  UIViewControllerExtensions class
    //  --------------------------------

    public static class UIViewControllerExtensions
    {
        #region WithNavigation method

        //  ---------------------
        //  WithNavigation method
        //  ---------------------

        public static UINavigationController WithNavigation(this UIViewController viewController)
        {
            return new NavigationController(viewController);
        }

        #endregion WithNavigation method

        #region PresentModalNavigationController method

        //  ---------------------------------------
        //  PresentModalNavigationController method
        //  ---------------------------------------

        public static void PresentModalNavigationController(
            this UIViewController viewController,
            UIViewController modalViewController,
            bool animated)
        {
            if (viewController == null) throw new ArgumentNullException(nameof(viewController));
            using (var navigationController = new NavigationController(modalViewController))
            {
                navigationController.Inject((viewController as IContextInjectable<IApplication>)?.Context);
                viewController.PresentModalViewController(navigationController, animated);
            }
        }

        #endregion PresentModalNavigationController method

        #region ShowAlert method

        //  ----------------
        //  ShowAlert method
        //  ----------------

        public static void ShowAlert(this UIViewController viewController, NSError error)
        {
            viewController.ShowAlert(error.CreateException());
        }

        public static void ShowAlert(this UIViewController viewController, Exception exception)
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception));

            var title = Strings.Error;
            var message = exception.Message;

            viewController.ShowAlert(title, message);
        }

        public static void ShowAlert(this UIViewController viewController, string title, string message)
        {
            if (viewController == null) throw new ArgumentNullException(nameof(viewController));

            var closeButtonText = Strings.Close;

            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                using (var alertController = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert))
                {
                    using (var alertAction = UIAlertAction.Create(closeButtonText, UIAlertActionStyle.Default, null))
                    {
                        alertController.AddAction(alertAction);
                        viewController.PresentViewController(alertController, true, null);
                    }
                }
            }
            else
            {
#pragma warning disable CS0618 // Type or member is obsolete
                using (var alertView = new UIAlertView(title, message, null, closeButtonText))
#pragma warning restore CS0618 // Type or member is obsolete
                {
                    alertView.Show();
                }
            }
        }

        #endregion ShowAlert method

        #region GetApplication method

        //  ---------------------
        //  GetApplication method
        //  ---------------------

        public static IApplication GetApplication(this UIViewController viewController)
        {
            return (viewController as IContextInjectable<IApplication>)?.Context;
        }

        #endregion GetApplication method

        #region GetAppLayoutExtension method

        //  ----------------------------
        //  GetAppLayoutExtension method
        //  ----------------------------

        public static AppLayoutExtension GetAppLayoutExtension(this UIViewController viewController)
        {
            return viewController.GetApplication()?.With<AppLayoutExtension>();
        }

        #endregion GetAppLayoutExtension method
    }

    #region NavigationController class

    //  --------------------------
    //  NavigationController class
    //  --------------------------

    internal class NavigationController : UINavigationController, IInjectable<IApplication>, IAppLayoutAttributes
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        public NavigationController(UIViewController viewController) : base(viewController) { }

        #endregion construction

        #region properties

        //  --------------------
        //  Application property
        //  --------------------

        public IApplication Application { get; private set; }

        #endregion properties

        #region methods

        //  -------------
        //  Inject method
        //  -------------

        public void Inject(IApplication dependency)
        {
            if (Application != dependency)
            {
                Application = dependency;
                (ViewControllers.FirstOrDefault() as IInjectable<IApplication>)?.Inject(Application);
            }
        }

        //  --------------------
        //  SetAttributes method
        //  --------------------

        public void SetAttributes(IValueStorage attributes)
        {
            (ViewControllers.FirstOrDefault() as IAppLayoutAttributes)?.SetAttributes(attributes);
        }

        #endregion methods

        #region PushViewController method

        //  -------------------------
        //  PushViewController method
        //  -------------------------

        public override void PushViewController(UIViewController viewController, bool animated)
        {
            (viewController as IInjectable<IApplication>)?.Inject(Application);
            base.PushViewController(viewController, animated);
        }

        #endregion PushViewController method
    }

    #endregion NavigationController class
}

// eof "UIViewControllerExtensions.cs"

//
//  @(#) Application.cs
//
//  Project:    usis Cocoa
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using System;
using System.Diagnostics.CodeAnalysis;
using UIKit;
using usis.Cocoa.UIKit;
using usis.Framework;
using usis.Framework.Configuration;
using usis.Platform;

namespace usis.Cocoa.Framework
{
    #region Application class

    //  -----------------
    //  Application class
    //  -----------------

    /// <summary>
    /// Represents an usis iOS application.
    /// </summary>
    /// <seealso cref="usis.Framework.Application{TActivator}" />

    public abstract class Application : usis.Framework.Application<SnapInActivator>
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class.
        /// </summary>

        internal protected Application() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class
        /// with the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration for the application.</param>

        internal protected Application(IApplicationConfiguration configuration) : base(null, configuration) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class
        /// with the specified snap-ins.
        /// </summary>
        /// <param name="snapInTypes">The snap-in types.</param>

        internal protected Application(params Type[] snapInTypes) : base(snapInTypes) { }

        #endregion construction

        #region properties

        //  ---------------------------
        //  RunningApplication property
        //  ---------------------------

        /// <summary>
        /// Gets the running application.
        /// </summary>
        /// <value>
        /// The running application.
        /// </value>

        public static Application RunningApplication { get; internal set; }

        //  ----------------------
        //  UIApplication property
        //  ----------------------

        /// <summary>
        /// Gets the MonoTouch application.
        /// </summary>
        /// <value>
        /// The MonoTouch application.
        /// </value>

        public UIApplication UIApplication { get; private set; }

        #endregion properties

        #region internal methods

        //  ------------
        //  Start method
        //  ------------

        internal void Start(UIApplication application)
        {
            UIApplication = application;
            Startup();
        }

        //  -----------
        //  Stop method
        //  -----------

        internal void Stop() { Shutdown(); }

        #endregion internal methods

        #region overrides

        //  ----------------------
        //  ReportException method
        //  ----------------------

        /// <summary>
        /// Allows the application to receive notifications about exceptions that occurred.
        /// </summary>
        /// <param name="exception">The exception that occurred.</param>

        public override void ReportException(Exception exception)
        {
            if (UIApplication.SharedApplication.Delegate is UIApplicationDelegate appDelegate)
            {
                appDelegate.InvokeOnMainThread(delegate
                {
                    var viewController = appDelegate?.Window?.RootViewController;
                    if (viewController == null) throw exception;
                    else viewController.ShowAlert(exception);
                });
            }
            else throw exception;
        }

        #endregion overrides

        #region public methods

        //  ----------
        //  Run method
        //  ----------

        /// <summary>
        /// Starts an application with the specified arguments and snap-ins.
        /// </summary>
        /// <typeparam name="TApplicationDelegate">The type of the application delegate.</typeparam>
        /// <param name="args">The arguments.</param>
        /// <param name="snapInTypes">The types of the snap-ins to connect.</param>

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void Run<TApplicationDelegate>(string[] args, params Type[] snapInTypes) where TApplicationDelegate : IUIApplicationDelegate
        {
            new Application<TApplicationDelegate>(snapInTypes).Run(args);
        }

        /// <summary>
        /// Starts an application with the specified arguments and snap-ins.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="snapInTypes">The types of the snap-ins to connect.</param>

        public static void Run(string[] args, params Type[] snapInTypes)
        {
            new Application<USApplicationDelegate>(snapInTypes).Run(args);
        }

        #endregion public methods
    }

    #endregion Application class

    #region Application<TApplicationDelegate> class

    //  ---------------------------------------
    //  Application<TApplicationDelegate> class
    //  ---------------------------------------

    /// <summary>
    /// Represents an usis iOS application
    /// that launches the main application loop with the specified delegate class.
    /// </summary>
    /// <typeparam name="TUIApplicationDelegate">The type of the application delegate.</typeparam>
    /// <seealso cref="Application{UIApplication, TUIApplicationDelegate}" />

    internal class Application<TUIApplicationDelegate> : Application<UIApplication, TUIApplicationDelegate>
        where TUIApplicationDelegate : IUIApplicationDelegate
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="Application{TUIApplicationDelegate}"/> class.
        /// </summary>

        internal Application() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Application{TUIApplicationDelegate}"/> class.
        /// with the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration for the application.</param>

        internal Application(IApplicationConfiguration configuration) : base(configuration) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Application{TUIApplicationDelegate}"/> class
        /// with the specified snap-ins.
        /// </summary>
        /// <param name="snapInTypes">The snap-in types.</param>

        internal Application(params Type[] snapInTypes) : base(snapInTypes) { }

        #endregion construction
    }

    #endregion Application<TApplicationDelegate> class

    #region Application<TApplication, TApplicationDelegate> class

    //  -----------------------------------------------------
    //  Application<TApplication, TApplicationDelegate> class
    //  -----------------------------------------------------

    /// <summary>
    /// Represents an usis iOS application
    /// that launches the main application loop with the specified
    /// principle and delegate class.
    /// </summary>
    /// <typeparam name="TUIApplication">The type of the principle class.</typeparam>
    /// <typeparam name="TUIApplicationDelegate">The type of the application delegate.</typeparam>
    /// <seealso cref="Application" />

    internal class Application<TUIApplication, TUIApplicationDelegate> : Application
        where TUIApplication : UIApplication
        where TUIApplicationDelegate : IUIApplicationDelegate
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="Application{TUIApplication, TUIApplicationDelegate}"/> class.
        /// </summary>

        internal Application() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Application{TUIApplication, TUIApplicationDelegate}"/> class
        /// with the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration for the application.</param>

        internal Application(IApplicationConfiguration configuration) : base(configuration) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Application{TUIApplication, TUIApplicationDelegate}"/> class
        /// with the specified snap-ins.
        /// </summary>
        /// <param name="snapInTypes">The snap-in types.</param>

        internal Application(params Type[] snapInTypes) : base(snapInTypes) { }

        #endregion construction

        #region methods

        //  ----------
        //  Run method
        //  ----------

        /// <summary>
        /// Starts the application with the specified command line arguments.
        /// </summary>
        /// <param name="args">The command line arguments.</param>

        internal void Run(string[] args)
        {
            RunningApplication = this;
            UIApplication.Main(args, typeof(TUIApplication), typeof(TUIApplicationDelegate));
        }

        #endregion methods
    }

    #endregion Application<TApplication, TApplicationDelegate> class

    #region ApplicationInterfaceExtension class

    //  -----------------------------------
    //  ApplicationInterfaceExtension class
    //  -----------------------------------

    internal static class ApplicationInterfaceExtension
    {
        //  -----------------------
        //  GetUIApplication method
        //  -----------------------

        internal static UIApplication GetUIApplication(this IApplication application)
        {
            var app = application as Application;
            return app?.UIApplication;
        }

        //  ----------------------------
        //  SetRootViewController method
        //  ----------------------------

        internal static void SetRootViewController(this IApplication application, UIViewController viewController)
        {
            var window = application.GetUIApplication()?.GetWindow();
            if (window != null)
            {
                (viewController as IInjectable<IApplication>)?.Inject(application);
                window.RootViewController = viewController;
            }
        }
    }

    #endregion ApplicationInterfaceExtension class
}

// eof "Application.cs"

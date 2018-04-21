//
//  @(#) ApnsFeedback.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2017 usis GmbH. All rights reserved.

using PushSharp.Apple;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using usis.Framework;
using usis.Framework.Windows;
using usis.Platform;

namespace usis.PushNotification
{
    //  ------------------
    //  ApnsFeedback class
    //  ------------------

    internal class ApnsFeedback : ApplicationExtension, IDisposable
    {
        #region fields

        private int checkFeedbackInterval = 60 * 5;
        private System.Timers.Timer checkFeedbackTimer;

        #endregion fields

        #region overrides

        //  ---------------
        //  OnAttach method
        //  ---------------

        protected override void OnAttach()
        {
            // get interval from registry
            var settings = Owner.Extensions.Find<RegistrySettings>();
            if (settings != null) checkFeedbackInterval = settings.LocalMachine.Get("CheckFeedbackInterval", checkFeedbackInterval);
        }

        //  --------------
        //  OnStart method
        //  --------------

        protected override void OnStart()
        {
            // start "feedback service" timer
            checkFeedbackTimer = new System.Timers.Timer(Convert.ToDouble(new TimeSpan(0, 0, checkFeedbackInterval).TotalMilliseconds));
            checkFeedbackTimer.Elapsed += CheckFeedback;
            checkFeedbackTimer.AutoReset = false;
            checkFeedbackTimer.Start();
        }

        //  ---------------
        //  OnDetach method
        //  ---------------

        protected override void OnDetach()
        {
            // stop timer
            checkFeedbackTimer?.Stop();
        }

        #endregion overrides

        #region IDisposable implementation

        //  --------------
        //  Dispose method
        //  --------------

        public void Dispose()
        {
            if (checkFeedbackTimer != null)
            {
                checkFeedbackTimer.Dispose();
                checkFeedbackTimer = null;
            }
        }
        
        #endregion IDisposable implementation

        #region CheckFeedback method

        //  --------------------
        //  CheckFeedback method
        //  --------------------

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void CheckFeedback(object sender, System.Timers.ElapsedEventArgs e)
        {
            var model = Owner.Extensions.Find<Model>();
            if (model != null && model.IsDatabaseInitialized)
            {
                Trace.WriteLine("Checking feedback service...");
                try
                {
                    foreach (var channel in ApnsModel.ListChannels(model))
                    {
                        var configuration = ApnsModel.CreateConfiguration(channel);
                        if (configuration != null)
                        {
                            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "- {0}", channel));
                            var feedbackService = new FeedbackService(configuration);
                            feedbackService.FeedbackReceived += (string deviceToken, DateTime timestamp) =>
                            {
                                ApnsModel.MarkReceiverAsExpired(model, ApnsDeviceToken.FromHexString(deviceToken), timestamp);
                                Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, " - Device '{0}' expired at {1}", deviceToken, timestamp));
                            };
                            feedbackService.Check();
                        }
                    }
                    Trace.WriteLine("...feedback check completed.");
                }
                catch (Exception exception) { Owner.ReportException(exception); }
            }
            checkFeedbackTimer?.Start();
        }

        #endregion CheckFeedback method
    }
}

// eof "ApnsFeedback.cs"

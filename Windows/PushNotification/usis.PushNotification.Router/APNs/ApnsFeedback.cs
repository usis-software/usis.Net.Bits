//
//  @(#) ApnsFeedback.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2016 usis GmbH. All rights reserved.

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using PushSharp.Apple;
using usis.Framework.Portable;
using usis.Framework.Windows;
using usis.Platform.Portable;

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
            checkFeedbackTimer = new System.Timers.Timer();
            checkFeedbackTimer.Interval = Convert.ToDouble(new TimeSpan(0, 0, checkFeedbackInterval).TotalMilliseconds);
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
            if (model != null)
            {
                Debug.WriteLine("Checking feedback service...");
                try
                {
                    foreach (var channel in ApnsModel.ListChannels(model))
                    {
                        var configuration = ApnsModel.CreateConfiguration(channel);
                        if (configuration != null)
                        {
                            Debug.Print("- {0}", channel);
                            var feedbackService = new FeedbackService(configuration);
                            feedbackService.FeedbackReceived += (string deviceToken, DateTime timestamp) =>
                            {
                                ApnsModel.MarkReceiverAsExpired(model, ApnsDeviceToken.FromHexString(deviceToken), timestamp);
                                Debug.Print(" - Device '{0}' expired at {1}", deviceToken, timestamp);
                            };
                            feedbackService.Check();
                        }
                    }
                }
                catch (Exception exception)
                {
                    Owner.ReportException(exception);
                    Debug.WriteLine(exception.ToString());
                }
                Debug.WriteLine("...feedback check completed.");
            }
            checkFeedbackTimer?.Start();
        }

        #endregion CheckFeedback method
    }
}

// eof "ApnsFeedback.cs"

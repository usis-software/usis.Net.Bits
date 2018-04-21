//
//  @(#) ProgressMonitor.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;

namespace usis.Platform
{
    //  ---------------------
    //  ProgressMonitor class
    //  ---------------------

    /// <summary>
    /// Allows to monitor the progress of a long-term task.
    /// </summary>

    [Obsolete("Use the Progress class instead.")]
    public class ProgressMonitor
    {
        #region properties

        //  -----------------
        //  Progress property
        //  -----------------

        /// <summary>
        /// Gets the information about the progress.
        /// </summary>
        /// <value>
        /// The information about the progress.
        /// </value>

        public ProgressInfo Progress { get; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressMonitor"/> class.
        /// </summary>

        public ProgressMonitor() { Progress = new ProgressInfo(); }

        #endregion construction

        #region methods

        //  --------------
        //  SetStep method
        //  --------------

        /// <summary>
        /// Sets the current step, the total number of steps and a step description.
        /// </summary>
        /// <param name="current">The current step.</param>
        /// <param name="total">The total number of steps.</param>
        /// <param name="description">The description of the current process.</param>

        public void SetStep(int current, int total, string description)
        {
            if (Progress.CurrentStep != current ||
                Progress.TotalSteps != total ||
                (description != null && !description.Equals(Progress.StepDescription, StringComparison.CurrentCulture)))
            {
                Progress.CurrentStep = current;
                Progress.TotalSteps = total;
                if (description != null) Progress.StepDescription = description;

                Progress.MinValue = 0;
                Progress.MaxValue = long.MaxValue;
                Progress.Value = 0;

                OnProgressChanged();
            }
        }

        //  ---------------------
        //  UpdateProgress method
        //  ---------------------

        /// <summary>
        /// Updates the progress information.
        /// </summary>
        /// <param name="value">The value that indicates the progress of the current step.</param>

        public void UpdateProgress(long value) { UpdateProgress(value, null); }

        /// <summary>
        /// Updates the progress information.
        /// </summary>
        /// <param name="value">The value that indicates the progress of the current step.</param>
        /// <param name="message">The message that describes the ongoing action.</param>

        public void UpdateProgress(long value, string message)
        {
            UpdateProgress(Progress.MinValue, Progress.MaxValue, value, message);
        }

        /// <summary>
        /// Updates the progress information.
        /// </summary>
        /// <param name="minValue">The minimum progress value.</param>
        /// <param name="maxValue">The maximum progress value.</param>
        /// <param name="value">The value that indicates the progress of the current step.</param>

        public void UpdateProgress(long minValue, long maxValue, long value)
        {
            UpdateProgress(minValue, maxValue, value, null);
        }

        /// <summary>
        /// Updates the progress.
        /// </summary>
        /// <param name="minValue">The minimum progress value.</param>
        /// <param name="maxValue">The maximum progress value.</param>
        /// <param name="value">The value that indicates the progress of the current step.</param>
        /// <param name="message">The message that describes the ongoing action.</param>

        public void UpdateProgress(long minValue, long maxValue, long value, string message)
        {
            if (Progress.MinValue != minValue ||
                Progress.MaxValue != maxValue ||
                Progress.Value != value ||
                (message != null && !message.Equals(Progress.Message, StringComparison.CurrentCulture)))
            {
                Progress.MinValue = minValue;
                Progress.MaxValue = maxValue;
                Progress.Value = value;
                if (message != null) Progress.Message = message;

                OnProgressChanged();
            }
        }

        //  ------------------------
        //  OnProgressChanged method
        //  ------------------------

        private void OnProgressChanged()
        {
            ProgressChanged.Invoke(this, new ProgressChangedEventArgs(Progress));
        }

        #endregion methods

        #region events

        //  ---------------------
        //  ProgressChanged event
        //  ---------------------

        /// <summary>
        /// Occurs when progress has changed.
        /// </summary>

        public event EventHandler<ProgressChangedEventArgs> ProgressChanged;

        #endregion events
    }

    #region ProgressChangedEventArgs class

    //  ------------------------------
    //  ProgressChangedEventArgs class
    //  ------------------------------

    /// <summary>
    /// Provides information about <see cref="ProgressMonitor.ProgressChanged"/> event.
    /// </summary>

    public class ProgressChangedEventArgs : CancelEventArgs
    {
        #region properties

        //  -----------------
        //  Progress property
        //  -----------------

        /// <summary>
        /// Gets the progress of a long-term task.
        /// </summary>
        /// <value>
        /// The progress of a long-term task.
        /// </value>

        public ProgressInfo Progress { get; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressChangedEventArgs"/> class.
        /// </summary>
        /// <param name="progress">The progress of a long-term task.</param>

        public ProgressChangedEventArgs(ProgressInfo progress) { Progress = progress; }

        #endregion construction
    }

    #endregion ProgressChangedEventArgs class

    #region ProgressInfo class

    //  ------------------
    //  ProgressInfo class
    //  ------------------

    /// <summary>
    /// Provides informations about the progress of a long-term task.
    /// </summary>

    [DataContract]
    public class ProgressInfo
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressInfo"/> class.
        /// </summary>

        public ProgressInfo() { MaxValue = long.MaxValue; StepDescription = string.Empty; Message = string.Empty; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressInfo" /> class.
        /// </summary>
        /// <param name="progressInfo">The progress information.</param>
        /// <exception cref="ArgumentNullException"><paramref name="progressInfo" /> is a <c>null</c> reference.</exception>

        public ProgressInfo(ProgressInfo progressInfo)
        {
            if (progressInfo == null) throw new ArgumentNullException(nameof(progressInfo));

            TotalSteps = progressInfo.TotalSteps;
            CurrentStep = progressInfo.CurrentStep;
            StepDescription = progressInfo.StepDescription;
            MinValue = progressInfo.MinValue;
            MaxValue = progressInfo.MaxValue;
            Value = progressInfo.Value;
            Message = progressInfo.Message;
        }

        #endregion construction

        #region properties

        //  --------------------
        //  CurrentStep property
        //  --------------------

        /// <summary>
        /// Gets the current step.
        /// </summary>
        /// <value>
        /// The current step.
        /// </value>

        [DataMember]
        public int CurrentStep { get; internal set; }

        //  -------------------
        //  TotalSteps property
        //  -------------------

        /// <summary>
        /// Gets the total number of steps.
        /// </summary>
        /// <value>
        /// The total number of steps.
        /// </value>

        [DataMember]
        public int TotalSteps { get; set; }

        //  ------------------------
        //  StepDescription property
        //  ------------------------

        /// <summary>
        /// Gets the description for the current step.
        /// </summary>
        /// <value>
        /// The description for the current step.
        /// </value>

        [DataMember]
        public string StepDescription { get; set; }

        //  -----------------
        //  MinValue property
        //  -----------------

        /// <summary>
        /// Gets the minimum value.
        /// </summary>
        /// <value>
        /// The minimum value.
        /// </value>

        [DataMember]
        public long MinValue { get; set; }

        //  -----------------
        //  MaxValue property
        //  -----------------

        /// <summary>
        /// Gets the maximum value.
        /// </summary>
        /// <value>
        /// The maximum value.
        /// </value>

        [DataMember]
        public long MaxValue { get; set; }

        //  --------------
        //  Value property
        //  --------------

        /// <summary>
        /// Gets the value that indicates the progress of the current step.
        /// </summary>
        /// <value>
        /// The value that indicates the progress of the current step.
        /// </value>

        [DataMember]
        public long Value { get; set; }

        //  ----------------
        //  Message property
        //  ----------------

        /// <summary>
        /// Gets a message that describes the ongoing action.
        /// </summary>
        /// <value>
        /// A message that describes the ongoing action.
        /// </value>

        [DataMember]
        public string Message { get; set; }

        //  -------------------------
        //  PercentCompleted property
        //  -------------------------

        internal float PercentCompleted
        {
            get { return 100.0f * Value / (MaxValue - MinValue + 1); }
        }

        #endregion properties

        #region overrides

        //  ---------------
        //  ToString method
        //  ---------------

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}: {1}/{2} ({3:F0}%) - {4}", StepDescription, CurrentStep, TotalSteps, PercentCompleted, Message);
        }

        #endregion overrides
    }

    #endregion ProgressInfo class
}

// eof "ProgressMonitor.cs"

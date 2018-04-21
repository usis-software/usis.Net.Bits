//
//  @(#) Progress.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System;

namespace usis.Platform
{
    //  --------------
    //  Progress class
    //  --------------

    /// <summary>
    /// Provides informations about the progress of a long-term task.
    /// </summary>

    public class Progress : ProgressInfo
    {
        #region events

        //  ---------------------
        //  ProgressChanged event
        //  ---------------------

        /// <summary>
        /// Occurs when progress has changed.
        /// </summary>

        public event EventHandler<ProgressChangedEventArgs> ProgressChanged;

        #endregion events

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
            if (CurrentStep != current || TotalSteps != total || (description != null && !description.Equals(StepDescription, StringComparison.CurrentCulture)))
            {
                CurrentStep = current;
                TotalSteps = total;
                if (description != null) StepDescription = description;

                MinValue = 0;
                MaxValue = long.MaxValue;
                Value = 0;
                Message = string.Empty;

                OnProgressChanged();
            }
        }

        #region UpdateProgress method overloads

        //  ---------------------
        //  UpdateProgress method
        //  ---------------------

        /// <summary>
        /// Updates the progress information.
        /// </summary>
        /// <param name="value">The value that indicates the progress of the current step.</param>

        [Obsolete("Use the Update instead.")]
        public void UpdateProgress(long value) { UpdateProgress(value, null); }

        /// <summary>
        /// Updates the progress information.
        /// </summary>
        /// <param name="value">The value that indicates the progress of the current step.</param>
        /// <param name="message">The message that describes the ongoing action.</param>

        [Obsolete("Use the Update instead.")]
        public void UpdateProgress(long value, string message)
        {
            UpdateProgress(MinValue, MaxValue, value, message);
        }

        /// <summary>
        /// Updates the progress information.
        /// </summary>
        /// <param name="minValue">The minimum progress value.</param>
        /// <param name="maxValue">The maximum progress value.</param>
        /// <param name="value">The value that indicates the progress of the current step.</param>

        [Obsolete("Use the Update instead.")]
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

        [Obsolete("Use the Update instead.")]
        public void UpdateProgress(long minValue, long maxValue, long value, string message)
        {
            if (MinValue != minValue || MaxValue != maxValue || Value != value || (message != null && !message.Equals(Message, StringComparison.CurrentCulture)))
            {
                MinValue = minValue;
                MaxValue = maxValue;
                Value = value;
                if (message != null) Message = message;

                OnProgressChanged();
            }
        }

        #endregion UpdateProgress method overloads

        #region Update method overloads

        //  -------------
        //  Update method
        //  -------------

        /// <summary>
        /// Updates the progress with the specified value and message.
        /// </summary>
        /// <param name="value">The value.</param>

        public void Update(long value)
        {
            Update(MinValue, MaxValue, value, null);
        }

        /// <summary>
        /// Updates the progress with the specified value and message.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="message">The message.</param>

        public void Update(long value, string message)
        {
            Update(MinValue, MaxValue, value, message);
        }

        /// <summary>
        /// Updates the progress.
        /// </summary>
        /// <param name="minValue">The minimum progress value.</param>
        /// <param name="maxValue">The maximum progress value.</param>
        /// <param name="value">The value that indicates the progress of the current step.</param>
        /// <param name="message">The message that describes the ongoing action.</param>

        public void Update(long minValue, long maxValue, long value, string message)
        {
            if (MinValue != minValue || MaxValue != maxValue || Value != value || (message != null && !message.Equals(Message, StringComparison.CurrentCulture)))
            {
                MinValue = minValue;
                MaxValue = maxValue;
                Value = value;
                if (message != null) Message = message;

                OnProgressChanged();
            }
        }

        #endregion Update method overloads

        //  ------------------------
        //  OnProgressChanged method
        //  ------------------------

        private void OnProgressChanged()
        {
            ProgressChanged?.Invoke(this, new ProgressChangedEventArgs(this));
        }

        #endregion methods
    }

    #region IProgressUpdate interface

    //  -------------------------
    //  IProgressUpdate interface
    //  -------------------------

    /// <summary>
    /// Defines member to update the progress informatiion for an operation.
    /// </summary>

    public interface IProgressUpdate
    {
        //  -----------------
        //  MinValue property
        //  -----------------

        /// <summary>
        /// Gets the minimum value.
        /// </summary>
        /// <value>
        /// The minimum value.
        /// </value>

        long MinValue { get; }

        //  -----------------
        //  MaxValue property
        //  -----------------

        /// <summary>
        /// Gets the maximum value.
        /// </summary>
        /// <value>
        /// The maximum value.
        /// </value>

        long MaxValue { get; }

        //  --------------
        //  SetStep method
        //  --------------

        /// <summary>
        /// Sets the current step, the total number of steps and a step description.
        /// </summary>
        /// <param name="current">The current step.</param>
        /// <param name="total">The total number of steps.</param>
        /// <param name="description">The description of the current process.</param>

        [Obsolete("Use the SetProgressStep method instead.")]
        void SetStep(int current, int total, string description);

        //  ----------------------
        //  SetProgressStep method
        //  ----------------------

        /// <summary>
        /// Sets the current step, the total number of steps and a step description.
        /// </summary>
        /// <param name="current">The current step.</param>
        /// <param name="total">The total number of steps.</param>
        /// <param name="description">The description of the current process.</param>

        void SetProgressStep(int current, int total, string description);

        //  -------------
        //  Update method
        //  -------------

        /// <summary>
        /// Updates the progress.
        /// </summary>
        /// <param name="minValue">The minimum progress value.</param>
        /// <param name="maxValue">The maximum progress value.</param>
        /// <param name="value">The value that indicates the progress of the current step.</param>
        /// <param name="message">The message that describes the ongoing action.</param>

        [Obsolete("Use the UpdateProgress method instead.")]
        void Update(long minValue, long maxValue, long value, string message);

        //  ---------------------
        //  UpdateProgress method
        //  ---------------------

        /// <summary>
        /// Updates the progress.
        /// </summary>
        /// <param name="minValue">The minimum progress value.</param>
        /// <param name="maxValue">The maximum progress value.</param>
        /// <param name="value">The value that indicates the progress of the current step.</param>
        /// <param name="message">The message that describes the ongoing action.</param>

        void UpdateProgress(long minValue, long maxValue, long value, string message);
    }

    #endregion IProgressUpdate interface

    #region ProgressUpdateInterfaceExtensions class

    //  ---------------------------------------
    //  ProgressUpdateInterfaceExtensions class
    //  ---------------------------------------

    /// <summary>
    /// Provides extension methods to the <see cref="IProgressUpdate"/> interface.
    /// </summary>

    public static class ProgressUpdateInterfaceExtensions
    {
        #region Update method overloads

        //  -------------
        //  Update method
        //  -------------

        /// <summary>
        /// Updates the progress information.
        /// </summary>
        /// <param name="progress">The progress object to update.</param>
        /// <param name="value">The value that indicates the progress of the current step.</param>

        [Obsolete("Use the UpdateProgress method instead.")]
        public static void Update(this IProgressUpdate progress, long value) { progress.Update(value, null); }

        /// <summary>
        /// Updates the progress information.
        /// </summary>
        /// <param name="progress">The progress object to update.</param>
        /// <param name="value">The value that indicates the progress of the current step.</param>
        /// <param name="message">The message that describes the ongoing action.</param>

        [Obsolete("Use the UpdateProgress method instead.")]
        public static void Update(this IProgressUpdate progress, long value, string message)
        {
            if (progress == null) throw new ArgumentNullException(nameof(progress));

            progress.Update(progress.MinValue, progress.MaxValue, value, message);
        }

        /// <summary>
        /// Updates the progress information.
        /// </summary>
        /// <param name="progress">The progress object to update.</param>
        /// <param name="minValue">The minimum progress value.</param>
        /// <param name="maxValue">The maximum progress value.</param>
        /// <param name="value">The value that indicates the progress of the current step.</param>

        [Obsolete("Use the UpdateProgress method instead.")]
        public static void Update(this IProgressUpdate progress, long minValue, long maxValue, long value)
        {
            if (progress == null) throw new ArgumentNullException(nameof(progress));

            progress.Update(minValue, maxValue, value, null);
        }

        #endregion Update method overloads

        #region UpdateProgress method overloads

        //  ---------------------
        //  UpdateProgress method
        //  ---------------------

        /// <summary>
        /// Updates the progress information.
        /// </summary>
        /// <param name="progress">The progress object to update.</param>
        /// <param name="value">The value that indicates the progress of the current step.</param>

        public static void UpdateProgress(this IProgressUpdate progress, long value) { progress.UpdateProgress(value, null); }

        /// <summary>
        /// Updates the progress information.
        /// </summary>
        /// <param name="progress">The progress object to update.</param>
        /// <param name="value">The value that indicates the progress of the current step.</param>
        /// <param name="message">The message that describes the ongoing action.</param>

        public static void UpdateProgress(this IProgressUpdate progress, long value, string message)
        {
            if (progress == null) throw new ArgumentNullException(nameof(progress));

            progress.UpdateProgress(progress.MinValue, progress.MaxValue, value, message);
        }

        /// <summary>
        /// Updates the progress information.
        /// </summary>
        /// <param name="progress">The progress object to update.</param>
        /// <param name="minValue">The minimum progress value.</param>
        /// <param name="maxValue">The maximum progress value.</param>
        /// <param name="value">The value that indicates the progress of the current step.</param>

        public static void UpdateProgress(this IProgressUpdate progress, long minValue, long maxValue, long value)
        {
            if (progress == null) throw new ArgumentNullException(nameof(progress));

            progress.UpdateProgress(minValue, maxValue, value, null);
        }

        #endregion UpdateProgress method overloads
    }

    #endregion ProgressUpdateInterfaceExtensions class
}

// eof "Progress.cs"

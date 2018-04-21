//
//  @(#) ServiceSnapIn.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2017 usis GmbH. All rights reserved.

using System;
using System.ComponentModel;

namespace usis.Framework
{
    //  -------------------
    //  ServiceSnapIn class
    //  -------------------

    /// <summary>
    /// Provides a base class that implements the <see cref="IServiceSnapIn" />
    /// interface.
    /// </summary>
    /// <seealso cref="SnapIn" />
    /// <seealso cref="IServiceSnapIn" />

    public class ServiceSnapIn : SnapIn, IServiceSnapIn
    {
        #region properties

        //  --------------------------
        //  CanPauseAndResume property
        //  --------------------------

        /// <summary>
        /// Gets or sets a value indicating whether the snap-in can pause and resume.
        /// </summary>
        /// <value>
        /// <c>true</c> if the snap-in can pause and resume; otherwise, <c>false</c>.
        /// </value>

        public bool CanPauseAndResume
        {
            get; set;
        }

        #endregion properties

        #region events

        //  -------------
        //  Pausing event
        //  -------------

        /// <summary>
        /// Occurs when the snap-in is about to be paused.
        /// </summary>

        [Description("Occurs when the snap-in is about to be paused.")]
        public event CancelEventHandler Pausing;

        //  ------------
        //  Paused event
        //  ------------

        /// <summary>
        /// Occurs when the application received a Pause command.
        /// </summary>

        [Description("Occurs when the application received a Pause command.")]
        public event EventHandler Paused;

        //  --------------
        //  Resuming event
        //  --------------

        /// <summary>
        /// Occurs when the snap-in is about to be resumed.
        /// </summary>

        [Description("Occurs when the snap-in is about to be resumed.")]
        public event CancelEventHandler Resuming;

        //  -------------
        //  Resumed event
        //  -------------

        /// <summary>
        /// Occurs when the application received a Resume command.
        /// </summary>

        [Description("Occurs when the application received a Resume command.")]
        public event EventHandler Resumed;

        #endregion events

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceSnapIn"/> class.
        /// </summary>

        public ServiceSnapIn()
        {
            CanPauseAndResume = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceSnapIn" /> class.
        /// </summary>
        /// <param name="canPauseAndResume">if set to <c>true</c> the snap-in can pause and resume.</param>

        protected ServiceSnapIn(bool canPauseAndResume)
        {
            CanPauseAndResume = canPauseAndResume;
        }

        #endregion construction

        #region virtual methods

        //  ----------------
        //  OnPausing method
        //  ----------------

        /// <summary>
        /// Raises the <see cref="Pausing" /> event.
        /// </summary>
        /// <param name="e">The <see cref="CancelEventArgs" /> instance containing the event data.</param>
        /// <exception cref="ArgumentNullException"><i>e</i> is a null reference (<b>Nothing</b> in Visual Basic).</exception>

        protected virtual void OnPausing(CancelEventArgs e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            if (!e.Cancel) Pausing?.Invoke(this, e);
        }

        //  ---------------
        //  OnPaused method
        //  ---------------

        /// <summary>
        /// Raises the <see cref="Paused" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>

        protected virtual void OnPaused(EventArgs e)
        {
            Paused?.Invoke(this, e);
        }

        //  -----------------
        //  OnResuming method
        //  -----------------

        /// <summary>
        /// Raises the <see cref="Resuming" /> event.
        /// </summary>
        /// <param name="e">The <see cref="CancelEventArgs" /> instance containing the event data.</param>
        /// <exception cref="ArgumentNullException"><i>e</i> is a null reference (<b>Nothing</b> in Visual Basic).</exception>

        protected virtual void OnResuming(CancelEventArgs e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            if (!e.Cancel) Resuming?.Invoke(this, e);
        }

        //  ----------------
        //  OnResumed method
        //  ----------------

        /// <summary>
        /// Raises the <see cref="Resumed" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>

        protected virtual void OnResumed(EventArgs e)
        {
            Resumed?.Invoke(this, e);
        }

        #endregion virtual methods

        #region IServiceSnapIn implementation

        //  ------------
        //  Pause method
        //  ------------

        /// <summary>
        /// Called to pause all operations performed by the snap-in.
        /// </summary>

        public void Pause()
        {
            if (!CanPauseAndResume) return;

            CancelEventArgs e = new CancelEventArgs();
            OnPausing(e);
            if (!e.Cancel) OnPaused(EventArgs.Empty);
        }

        //  -------------
        //  Resume method
        //  -------------

        /// <summary>
        /// Called to resume all operations performed by the snap-in.
        /// </summary>

        public void Resume()
        {
            if (!CanPauseAndResume) return;

            CancelEventArgs e = new CancelEventArgs();
            OnResuming(e);
            if (!e.Cancel) OnResumed(EventArgs.Empty);
        }

        #endregion IServiceSnapIn implementation
    }
}

// eof "ServiceSnapIn.cs"

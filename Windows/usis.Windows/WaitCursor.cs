//
//  @(#) WaitCursor.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 12
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014,2015 audius GmbH, usis GmbH. All rights reserved.

using System;
using System.Threading;
using System.Windows.Input;
using System.Windows.Threading;

namespace usis.Windows
{
    //  ----------------
    //  WaitCursor class
    //  ----------------

    public sealed class WaitCursor : IDisposable
    {
        static private Cursor previousCursor;
        static private int waitCounter;

        private DispatcherObject DispatcherObject
        {
            get;
            set;
        }

        public WaitCursor()
            : this(null)
        {
        }

        public WaitCursor(DispatcherObject dispatcherObject)
        {
            this.DispatcherObject = dispatcherObject;

            if (WaitCursor.waitCounter < 0)
                WaitCursor.waitCounter = 0; // something went wrong. Someone didn't call Dispose!

            if (WaitCursor.waitCounter == 0)
            {
                WaitCursor.previousCursor = Mouse.OverrideCursor;

                Mouse.OverrideCursor = Cursors.Wait;
            }
            WaitCursor.waitCounter++;
        }

        #region IDisposable Members

        public void Dispose()
        {
            WaitCursor.waitCounter--;

            if (WaitCursor.waitCounter == 0)
            {
                if (this.DispatcherObject == null)
                {
                    Mouse.OverrideCursor = previousCursor;
                }
                else
                {
                    this.DispatcherObject.Dispatcher.BeginInvoke(DispatcherPriority.Render, (ThreadStart)delegate
                    {
                        Mouse.OverrideCursor = null;
                    });
                }
            }
        }

        #endregion

    } // WaitCursor class

} // namespace usis.Windows

// eof "WaitCursor.cs"

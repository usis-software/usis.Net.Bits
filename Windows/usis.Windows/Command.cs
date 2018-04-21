//
//  @(#) Command.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 12
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014,2015 usis GmbH. All rights reserved.

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Input;

namespace usis.Windows
{
    //  -------------
    //  Command class
    //  -------------

    public class Command : ICommand
    {
        #region fields

        private bool enabled = true;

        #endregion fields

        #region properties

        //  ----------------
        //  Enabled property
        //  ----------------

        public bool Enabled
        {
            get
            {
                return this.enabled;
            }
            set
            {
                if (this.enabled != value)
                {
                    this.enabled = value;
                    if (this.CanExecuteChanged != null)
                    {
                        this.CanExecuteChanged(this, EventArgs.Empty);
                    }
                }
            }

        } // Enabled property

        //  -----------------------
        //  ShowWaitCursor property
        //  -----------------------

        public bool ShowWaitCursor
        {
            get;
            set;

        } // ShowWaitCursor property

        #endregion properties

        #region events

        //  --------------
        //  Executed event
        //  --------------

        public event EventHandler Executed;

        #endregion events

        #region construction

        //  -----------
        //  constructor
        //  -----------

        public Command()
        {
        } // constructor

        public Command(bool enabled)
        {
            this.enabled = enabled;

        } // constructor

        #endregion construction

        #region ICommand members

        //  -----------------------
        //  CanExecuteChanged event
        //  -----------------------

        public event EventHandler CanExecuteChanged;

        //  -----------------
        //  CanExecute method
        //  -----------------

        public bool CanExecute(object parameter)
        {
            return this.enabled;

        } // CanExecute method

        //  --------------
        //  Execute method
        //  --------------

        public void Execute(object parameter)
        {
            if (this.Executed != null)
            {
                if (this.ShowWaitCursor)
                {
                    using (var waitCursor = new WaitCursor())
                    {
                        this.OnExecute();
                    }
                }
                else this.OnExecute();
            }

        } // Execute method

        #endregion ICommand members

        #region private methods

        //  ----------------
        //  OnExecute method
        //  ----------------

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void OnExecute()
        {
            if (!Debugger.IsAttached)
            {
                try
                {
                    this.Executed(this, EventArgs.Empty);
                }
                catch (Exception exception)
                {
                    Application.Current.ShowErrorDialog(exception);
                }
            }
            else this.Executed(this, EventArgs.Empty);

        } // OnExecute method

        #endregion private methods

    } // Command class

} // usis.Windows namespace

// eof "Command.cs"

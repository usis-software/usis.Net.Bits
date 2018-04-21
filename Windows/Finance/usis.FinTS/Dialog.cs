//
//  @(#) Dialog.cs
//
//  Project:    usis.FinTS
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System;
using usis.FinTS.Messages;

namespace usis.FinTS
{
    //  ------------
    //  Dialog class
    //  ------------

    /// <summary>
    /// Represents a sequence of request and response messages
    /// between a customer and a bank.
    /// </summary>

    public abstract class Dialog : IDisposable
    {
        #region fields

        private ITransport transport;

        private int messageNumber = 0;

        #endregion fields

        #region properties

        //  --------------
        //  State property
        //  --------------

        internal DialogState State { get; set; }

        //  -----------------
        //  DialogId property
        //  -----------------

        internal string DialogId { get; set; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="Dialog"/> class
        /// with the specified transport provider.
        /// </summary>
        /// <param name="transport">The transport.</param>
        /// <exception cref="ArgumentNullException"><paramref name="transport"/> is a <c>null</c> reference.</exception>
        /// <remarks>
        /// The dialog keeps a reference to the transport provider and disposes it when the dialog itself is being disposed.
        /// </remarks>

        protected Dialog(ITransport transport)
        {
            this.transport = transport ?? throw new ArgumentNullException(nameof(transport));

            State = DialogState.Created;
        }

        #endregion construction

        #region IDisposable implementation

        //  --------------
        //  Dispose method
        //  --------------

        private bool disposed = false;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    transport.Dispose();
                }
                disposed = true;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable implementation

        #region methods

        //  --------------
        //  Receive method
        //  --------------

        /// <summary>
        /// Receives a FinTS message.
        /// </summary>
        /// <returns>The message received.</returns>

        protected Message Receive()
        {
            var message = transport.Receive();
            if (message == null) State = DialogState.Disconnected;
            return message;
        }

        //  ------------------
        //  SendMessage method
        //  ------------------

        /// <summary>
        /// Sends the specified message.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <exception cref="ArgumentNullException"><paramref name="message"/> is a <c>null</c> reference.</exception>

        protected void Send(Message message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            message.DialogId = DialogId;
            message.Number = ++messageNumber;
            transport.Send(message);
        }

        #endregion methods
    }

    #region CustomerDialog class

    //  --------------------
    //  CustomerDialog class
    //  --------------------

    /// <summary>
    /// Prepresents a dialog on the side of a customer system.
    /// </summary>
    /// <seealso cref="Dialog" />

    public sealed class CustomerDialog : Dialog
    {
        #region properties

        //  -------------------
        //  BankAccess property
        //  -------------------

        private BankAccess BankAccess { get; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        private CustomerDialog(BankAccess bankAccess, ITransport transport) : base(transport)
        {
            DialogId = "0";
            BankAccess = bankAccess ?? throw new ArgumentNullException(nameof(bankAccess));
        }

        #endregion construction

        #region methods

        //  -----------------
        //  Initialize method
        //  -----------------

        /// <summary>
        /// Initializes a new dialog with the specified transport provider,
        /// bank access and customer system informatons.
        /// </summary>
        /// <param name="transport">The transport provider.</param>
        /// <param name="bankAccess">The bank access.</param>
        /// <param name="customerSystem">The customer system.</param>
        /// <returns>A newly created <see cref="CustomerDialog"/> object.</returns>

        public static CustomerDialog Initialize(ITransport transport, BankAccess bankAccess, CustomerSystem customerSystem)
        {
            CustomerDialog dialog = null;
            CustomerDialog tmpDialog = null;
            try
            {
                tmpDialog = new CustomerDialog(bankAccess, transport);
                tmpDialog.Send(new DialogInitialization(tmpDialog.BankAccess, customerSystem));
                var message = tmpDialog.Receive();
                if (message is DialogCancellation)
                {
                    tmpDialog.State = DialogState.Canceled;
                }
                else tmpDialog.State = DialogState.Initialized;
                dialog = tmpDialog;
                tmpDialog = null;
            }
            finally
            {
                if (tmpDialog != null) tmpDialog.Dispose();
            }
            return dialog;
        }

        //  ----------------
        //  Terminate method
        //  ----------------

        /// <summary>
        /// Closes the dialog by sending a dialog termination message.
        /// </summary>

        public void Terminate()
        {
            if (State == DialogState.Initialized)
            {
                Send(new DialogTermination(BankAccess));
                State = DialogState.Terminated;
            }
        }

        #endregion methods
    }

    #endregion CustomerDialog class

    #region BankDialog class

    //  ----------------
    //  BankDialog class
    //  ----------------

    /// <summary>
    /// Prepresents a dialog on the side of a bank system.
    /// </summary>
    /// <seealso cref="Dialog" />

    public sealed class BankDialog : Dialog
    {
        #region properties

        //  --------------------
        //  HbciVersion property
        //  --------------------

        /// <summary>
        /// Gets the HBCI version that the bank system supports.
        /// </summary>
        /// <value>
        /// The HBCI version as an <see cref="int"/>: <b>300</b> corresponds to HBCI/FinTS 3.0.
        /// </value>

        public int HbciVersion { get; }

        //  -----------------------------
        //  KeepConnectionActive property
        //  -----------------------------

        /// <summary>
        /// Gets a value indicating whether to keep the underlying connection active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the underlying connection should kept active; otherwise, <c>false</c>.
        /// </value>

        public bool KeepConnectionActive => State == DialogState.Created || State == DialogState.Initialized;

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        private BankDialog(ITransport transport, int hbciVersion) : base(transport) { HbciVersion = hbciVersion; }

        #endregion construction

        #region methods

        //  -----------------
        //  Initialize method
        //  -----------------

        /// <summary>
        /// Initializes a new dialog with the specified transport provider.
        /// </summary>
        /// <param name="transport">The transport provider.</param>
        /// <returns>A newly created <see cref="BankDialog"/> object.</returns>

        public static BankDialog Initialize(ITransport transport)
        {
            return new BankDialog(transport, 300);
        }

        //  --------------
        //  Process method
        //  --------------

        /// <summary>
        /// Receives a message, processes it by the specified action
        /// and the sends it back to the customer system.
        /// </summary>
        /// <param name="action">The action to process incoming messages.</param>

        public void Process(Func<Message, Message> action)
        {
            var message = Receive();
            if (message != null) message = action?.Invoke(message);
            if (message != null) Send(message);
        }

        //  -------------
        //  Cancel method
        //  -------------

        /// <summary>
        /// Cancels a dialog by sending a canncellation message in response to the specified message.
        /// </summary>
        /// <param name="message">The message received from the customer system.</param>

        public void Cancel(Message message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            Message response = null;
            if (message is DialogInitialization)
            {
                response = new DialogCancellation(message.HbciVersion, 1);
                DialogId = Constants.CancellationDialogId;
            }
            else
            {
                response = new DialogCancellation(message.HbciVersion, message.Number);
            }
            if (response != null)
            {
                Send(response);
                State = DialogState.Canceled;
            }
        }

        #endregion methods
    }

    #endregion BankDialog class
}

// eof "Dialog.cs"

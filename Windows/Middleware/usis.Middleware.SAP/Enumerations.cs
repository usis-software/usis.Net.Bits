//
//  @(#) Enumerations.cs
//
//  Project:    usis Middleware
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015-2017 usis GmbH. All rights reserved.

namespace usis.Middleware.SAP
{
    #region IDocDirection enumeration

    //  -------------------------
    //  IDocDirection enumeration
    //  -------------------------

    /// <summary>
    /// Specifies the direction of an IDoc transmission.
    /// </summary>

    public enum IDocDirection
    {
        /// <summary>
        /// The direction of the IDoc transmission is not specified.
        /// </summary>

        Undefined,

        /// <summary>
        /// An outbound IDoc transmission.
        /// </summary>

        Outbound,

        /// <summary>
        /// An inboundIDoc transmission.
        /// </summary>

        Inbound
    }

    #endregion IDocDirection enumeration

    #region IDocOutputMode enumeration

    //  --------------------------
    //  IDocOutputMode enumeration
    //  --------------------------

    /// <summary>
    /// Specifies the output mode of an IDoc transmission.
    /// </summary>

    public enum IDocOutputMode
    {
        /// <summary>
        /// The output mode is not specified.
        /// </summary>

        Undefined,

        /// <summary>
        /// Transfer IDoc immediately and start external subsystem.
        /// </summary>

        TransferImmediatelyStartSubsystem,

        /// <summary>
        /// Transfer IDoc immediately.
        /// </summary>

        TransferImmediately,

        /// <summary>
        /// Collect IDocs, transfer and start external subsystem.
        /// </summary>

        CollectTransferStartSubsystem,

        /// <summary>
        /// Collect IDocs and transfer.
        /// </summary>

        CollectTransfer
    }

    #endregion IDocOutputMode enumeration
}

// eof "Enumerations.cs"

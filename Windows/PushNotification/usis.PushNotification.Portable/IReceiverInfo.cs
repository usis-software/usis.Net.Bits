//
//  @(#) IReceiverInfo.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2016 usis GmbH. All rights reserved.

namespace usis.PushNotification
{
    //  -----------------------
    //  IReceiverInfo interface
    //  -----------------------

    /// <summary>
    /// Describes common properties of push notification receivers (devices).
    /// </summary>

    public interface IReceiverInfo
    {
        //  -------------
        //  Name property
        //  -------------

        /// <summary>
        /// Gets or sets the name of the receiver.
        /// </summary>
        /// <value>
        /// The name of the receiver.
        /// </value>

        string Name { get; set; }

        //  ----------------
        //  Account property
        //  ----------------

        /// <summary>
        /// Gets or sets the user account.
        /// </summary>
        /// <value>
        /// The user account.
        /// </value>

        string Account { get; set; }

        //  ---------------
        //  Groups property
        //  ---------------

        /// <summary>
        /// Gets or sets the groups that the account belongs to.
        /// </summary>
        /// <value>
        /// The groups that the account belongs to.
        /// </value>

        string Groups { get; set; }

        //  -------------
        //  Info property
        //  -------------

        /// <summary>
        /// Gets or sets additional information for the receiver.
        /// </summary>
        /// <value>
        /// Additional information for the receiver.
        /// </value>

        string Info { get; set; }
    }
}

// eof "IReceiverInfo.cs"

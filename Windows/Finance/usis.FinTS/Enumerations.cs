//
//  @(#) Enumerations.cs
//
//  Project:    usis.FinTS
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

namespace usis.FinTS
{
    #region DialogState enumeration

    //  -----------------------
    //  DialogState enumeration
    //  -----------------------

    internal enum DialogState
    {
        /// <summary>
        /// The dialog is newly created.
        /// </summary>

        Created,

        Initialized,
        Terminated,
        Canceled,
        Error,
        Disconnected,
    }

    #endregion DialogState enumeration
}

// eof "Enumerations.cs"

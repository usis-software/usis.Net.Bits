//
//  @(#) StartView.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

using Microsoft.ManagementConsole;

namespace usis.PushNotification.Administration
{
    //  ---------------
    //  StartView class
    //  ---------------

    internal class StartView : FormView
    {
        //  --------------------
        //  Description property
        //  --------------------

        internal static ViewDescription Description => new FormViewDescription(typeof(StartViewControl))
        {
            DisplayName = Strings.StartViewDisplayName,
            ViewType = typeof(StartView)
        };
    }
}

// eof "StartView.cs"

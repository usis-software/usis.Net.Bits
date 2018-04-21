//
//  @(#) ActionFactory.cs
//
//  Project:    usis.ManagementConsole
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using Microsoft.ManagementConsole;

namespace usis.ManagementConsole
{
    //  -------------------
    //  ActionFactory class
    //  -------------------

    internal static class ActionFactory
    {
        //  -------------
        //  Create method
        //  -------------

        public static Action Create(System.Action<ActionEventArgs> action)
        {
            var actionItem = new Action();
            actionItem.Triggered += (sender, e) => { action.Invoke(e); };
            return actionItem;
        }
    }
}

// eof "ActionFactory.cs"

//
//  @(#) PushViewCommand.cs
//
//  Project:    usis Cocoa
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using usis.Platform;

#pragma warning disable 1591

namespace usis.Cocoa.UIKit
{
    //  ---------------------
    //  PushViewCommand class
    //  ---------------------

    public class PushViewCommand : ViewCommand
    {
        #region methods

        //  --------------
        //  Execute method
        //  --------------

        public override void Execute(params object[] parameters)
        {
            var viewController = CreateViewController(View, parameters);
            if (viewController == null) return;
            ViewController?.NavigationController?.PushViewController(viewController, true);
        }

        //  --------------------
        //  SetAttributes method
        //  --------------------

        public override void SetAttributes(IValueStorage attributes)
        {
            View = attributes.GetString(nameof(View));
        }

        #endregion methods
    }
}

// eof "PushViewCommand.cs"

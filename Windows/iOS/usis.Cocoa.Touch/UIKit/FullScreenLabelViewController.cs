//
//  @(#) FullScreenLabelViewController.cs
//
//  Project:    usis Cocoa
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using UIKit;

namespace usis.Cocoa.UIKit
{
    //  -----------------------------------
    //  FullScreenLabelViewController class
    //  -----------------------------------

    internal class FullScreenLabelViewController : UIViewController
    {
        #region properties

        //  --------------
        //  Label property
        //  --------------

        public UILabel Label => View as UILabel;

        #endregion properties

        #region overrides

        //  ---------------
        //  LoadView method
        //  ---------------

        public override void LoadView()
        {
            using (var label = new UILabel(CoreGraphics.CGRect.Empty))
            {
                label.TextAlignment = UITextAlignment.Center;
                View = label;
            }
        }

        #endregion overrides
    }
}

// eof "FullScreenLabelViewController.cs"

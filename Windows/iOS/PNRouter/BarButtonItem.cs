using System;
using UIKit;

namespace usis.Cocoa.PNRouter
{
    public static class BarButtonItem
    {
        public static UIBarButtonItem Cancel(EventHandler handler)
        {
            return new UIBarButtonItem(UIBarButtonSystemItem.Cancel, handler);
        }
        public static UIBarButtonItem BarButtonItemCancelModalView(this UIViewController viewController)
        {
            return viewController.BarButtonItemCancelModalView(true);
        }
        public static UIBarButtonItem BarButtonItemCancelModalView(this UIViewController viewController, bool animated)
        {
            return Cancel((sender, e) => { viewController.DismissModalViewController(animated); });
        }
    }
}

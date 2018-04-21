using UIKit;

namespace usis.iOS.PNRouter
{
    internal static class USApplication
    {
        public static UIWindow Window
        {
            get
            {
                var applicationDelegate = UIApplication.SharedApplication?.Delegate as UIApplicationDelegate;
                return applicationDelegate?.GetWindow();
            }
        }
    }
}

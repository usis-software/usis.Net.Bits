using System.ComponentModel;
using usis.Cocoa.UIKit;
using usis.Mobile;

namespace usis.Cocoa.FieldService
{
    internal class FieldServiceSnapIn : usis.Framework.SnapIn
    {
        protected override void OnConnecting(CancelEventArgs e)
        {
            ViewDescription rootView;
            var layout = new AppLayout(
                rootView = new ViewDescription("root", typeof(TableViewController))
                {
                    Navigation = true
                }
                .AddAttribute(ViewControllerAttributeName.Title, "Assignments"));
            layout.RootView = rootView.Key;
            this.SetActiveLayout(layout);

            base.OnConnecting(e);
        }
    }
}

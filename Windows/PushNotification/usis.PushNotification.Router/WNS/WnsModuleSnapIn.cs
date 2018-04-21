using System.ComponentModel;

namespace usis.PushNotification
{
    //  ---------------------
    //  WnsModuleSnapIn class
    //  ---------------------

    public sealed class WnsModuleSnapIn : Framework.Portable.SnapIn
    {
        protected override void OnConnecting(CancelEventArgs e)
        {
            Model.RegisterPushServiceModel(ChannelType.WindowsNotificationService, new WnsModel());

            Application.ConnectRequiredSnapIns(this,
                typeof(Web.WnsSnapIn),
                typeof(WnsSnapIn),
                typeof(WnsRouterSnapIn),
                typeof(WnsRouterMgmtSnapIn));

            base.OnConnecting(e);
        }
    }
}

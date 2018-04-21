using System.ComponentModel;
using usis.Framework;
using usis.Framework.Data;
using usis.Platform;

namespace usis.Server.ObjectBroker
{
    //  ------------
    //  SnapIn class
    //  ------------

    public class SnapIn : ServiceSnapIn
    {
        protected override void OnConnecting(CancelEventArgs e)
        {
            // configure registry key and settings
            Application.SetSettingsPath(@"Software\usis\ObjectBroker");
            Application.CreateRegistrySettings();

            var store = Application.GetRegistrySettings().LocalMachine.OpenStorage("DataSources");
            Application.CreateDataSourceRepository(store);

            Application.Extensions.Add(new Repository());
            Application.Extensions.Add(new Broker());

            base.OnConnecting(e);
        }
    }
}

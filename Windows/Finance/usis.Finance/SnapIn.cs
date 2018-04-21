using System.ComponentModel;
using usis.Framework;
using usis.Framework.Data;
using usis.Framework.Entity;
using usis.Platform.Data;

namespace usis.Finance
{
    internal sealed class SnapIn : ExtensionSnapIn<Model>
    {
        protected override void OnConnecting(CancelEventArgs e)
        {
            var dataSource = new DataSource("MySql.Data.MySqlClient", "server=iGanymede04.local;user=usis;password=master;persistsecurityinfo=true;database=usis.Finance");
            Application.Extensions.Add(new DataSourceApplicationExtension(dataSource));

            // configure Entity Framework
            DBConfiguration.SetDataSourceConfiguration(dataSource);

            base.OnConnecting(e);
        }
    }
}

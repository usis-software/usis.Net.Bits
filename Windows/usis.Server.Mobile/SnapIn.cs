using System;
using System.Collections.Generic;
using System.ComponentModel;
using usis.Framework;
using usis.Framework.ServiceModel.Web;

namespace usis.Server.Mobile
{
    public class SnapIn : Framework.SnapIn
    {
        protected override void OnConnecting(CancelEventArgs e)
        {
            Application.ConnectRequiredSnapIns(this,
                typeof(WebServiceHostSnapIn<SyncService, ISyncService>));

            base.OnConnecting(e);
        }
    }

    public interface IAppLayoutService
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        OperationResult<IEnumerable<AppLayoutInfo>> ListLayouts();

        //OperationResultList<AppLayoutInfo> ListLayouts();

        void LoadLayout();
    }

    public class AppLayoutInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}

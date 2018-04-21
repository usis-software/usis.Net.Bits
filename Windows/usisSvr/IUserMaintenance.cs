using System.ServiceModel;
using System.ServiceModel.Web;

namespace usis.Server.Services
{
    [ServiceContract]
    interface IUserMaintenance
    {
        [OperationContract]
		[WebGet]
        void RegisterByEmail();
    }
}

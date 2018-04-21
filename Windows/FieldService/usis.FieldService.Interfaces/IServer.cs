using System.ServiceModel;

namespace usis.FieldService
{
    [ServiceContract]
    public interface IServer
    {
        [OperationContract]
        string CreateSession();
    }
}

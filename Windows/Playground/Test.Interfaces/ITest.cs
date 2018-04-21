using System.ServiceModel;

namespace Test.Interfaces
{
    [ServiceContract]
    public interface ITest
    {
        [OperationContract]
        string Hello();
    }

}

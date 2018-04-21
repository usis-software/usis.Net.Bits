using System;
using System.ServiceModel;

namespace Playground.JobEngine
{
    [ServiceContract]
    public interface ITestJob
    {
        [OperationContract]
        Guid Start();
    }
}

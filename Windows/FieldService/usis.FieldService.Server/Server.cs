using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Security.Principal;
using System.ServiceModel;

namespace usis.FieldService.Server
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    internal class Server : Framework.ServiceModel.ServiceBase<Model>, IServer
    {
        public string CreateSession()
        {
            Debug.WriteLine("CreateSession called.");
            System.Threading.Thread.Sleep(50);
            var identity =  (System.Threading.Thread.CurrentPrincipal.Identity as WindowsIdentity);
            //return identity.User.Value;

            var sw = new Stopwatch();
            sw.Start();
            var userId = Model.FindUserIdBySid(identity.User.Value);
            sw.Stop();
            Debug.WriteLine("Model.FindUserIdBySid(string) completed in {0} ms", sw.ElapsedMilliseconds);
            sw.Reset();

            sw.Start();
            var clientId = Model.GetClientIdForUser(userId.Value);
            sw.Stop();
            Debug.WriteLine("Model.GetClientIdForUser(string) completed in {0} ms", sw.ElapsedMilliseconds);

            return clientId?.ToString();
        }
    }
}

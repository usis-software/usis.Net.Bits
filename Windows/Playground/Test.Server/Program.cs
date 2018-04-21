using Test.Interfaces;
using usis.ApplicationServer;
using usis.Framework.ServiceModel;
using usis.Platform.Windows;

namespace Test.Server
{
    internal static class Program
    {
        internal static void Main(string[] args)
        {
            ServicesHost.StartServicesOrConsole(args, Service.FromSnapIn<ServiceHostSnapIn<WcfServiceHostFactory<Test>>>());
        }
    }

    internal class Test : ITest
    {
        string ITest.Hello()
        {
            return "World!";
        }
    }

    //internal class MyServiceFactory<TService> : WcfServiceHostFactory<TService>
    //{
    //    public override ServiceHostBase CreateServiceHost()
    //    {
    //        ServiceHost host = null;
    //        ServiceHost tmp = null;
    //        try
    //        {
    //            //var serviceAddress = CreateServiceAddress(typeof(TService).Name);
    //            tmp = new ServiceHost(typeof(TService)/*, serviceAddress*/);
    //            //tmp.Description.Behaviors.Add(new ServiceMetadataBehavior
    //            //{
    //            //    HttpGetEnabled = true
    //            //});
    //            host = tmp;
    //            tmp = null;
    //        }
    //        finally
    //        {
    //            if (tmp != null) (tmp as IDisposable)?.Dispose();
    //        }
    //        return host;
    //    }
    //}
}

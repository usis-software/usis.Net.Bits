using System;
using System.Globalization;
using System.ServiceModel;
using usis.Framework.Configuration;
using usis.Framework.ServiceModel.Web;

namespace Playground
{
    internal static class AppDomainSample
    {
        internal static void Main()
        {
            Console.WriteLine(AppDomain.CurrentDomain.FriendlyName);


            var configuration = new ApplicationConfiguration(typeof(WebServiceHostSnapIn<WebService, IWebService>));


            var appDomain = AppDomain.CreateDomain("usis");
            var proxy = appDomain.CreateInstanceAndUnwrap(
                typeof(ApplicationProxy).Assembly.FullName,
                typeof(ApplicationProxy).FullName) as ApplicationProxy;
            proxy.CreateApplication(configuration);
            proxy.Start();
            ConsoleTool.PressAnyKey();
            proxy.Stop();

            //var application = new TestApplication(configuration);
            //application.Start();
            //ConsoleTool.PressAnyKey();
            //application.Stop();

        }
        private class ApplicationProxy : MarshalByRefObject
        {
            private TestApplication Application
            {
                get; set;
            }

            internal void CreateApplication(ApplicationConfiguration configuration)
            {
                Application = new TestApplication(configuration);
            }

            public void Start()
            {
                Console.WriteLine(AppDomain.CurrentDomain.FriendlyName);
                Application.Start();
            }

            public void Stop()
            {
                Application.Stop();
            }
        }
    }

    internal class TestApplication : usis.Framework.Application
    {
        internal TestApplication(ApplicationConfiguration configuration) : base(configuration) { }

        internal void Start()
        {
            Startup();
        }

        internal void Stop()
        {
            Shutdown();
        }
    }

    [ServiceContract]
    public interface IWebService
    {
        [OperationContract]
        string Hello(string name);
    }

    internal class WebService : IWebService
    {
        public string Hello(string name)
        {
            return string.Format(CultureInfo.CurrentUICulture, "Hello {0}!", string.IsNullOrWhiteSpace(name) ? "World" : name);
        }
    }
}

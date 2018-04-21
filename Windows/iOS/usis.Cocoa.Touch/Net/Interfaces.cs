using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace usis.Framework.Portable.Net
{
    //  ---------------------------
    //  INetworkOperation interface
    //  ---------------------------

    public interface INetworkOperation
    {
        //  --------------
        //  Perform method
        //  --------------

        void Perform(IApplication application, NetworkOperationPool pool);

        //  ------------------------
        //  ReportException property
        //  ------------------------

        bool ReportException { get; }
    }

    //  -----------------------
    //  IHttpResponse interface
    //  -----------------------

    public interface IHttpResponse
    {
        //  -------------------
        //  BodyToString method
        //  -------------------

        string BodyToString();
    }

    //  --------------------------
    //  IHttpNetworkTask interface
    //  --------------------------

    public interface IHttpNetworkTask : IDisposable
    {
        //  --------------
        //  Perform method
        //  --------------

        void Perform(HttpRequest httpRequest, Action<IHttpResponse, Exception> handler);
    }
}

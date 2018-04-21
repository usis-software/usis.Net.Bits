//
//  @(#) Signup.cs
//
//  Project:    Basis - User Management
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using System.Diagnostics.CodeAnalysis;
using System.ServiceModel;
using System.ServiceModel.Web;
using usis.Framework;
using usis.Framework.ServiceModel;
using usis.Framework.ServiceModel.Web;

namespace usis.Basis.UserManagement
{
    #region ISignup interface

    //  -----------------
    //  ISignup interface
    //  -----------------

    [ServiceContract]
    public interface ISignup
    {
        [OperationContract]
        [WebInvoke(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        OperationResult WithEmailAddress(string emailAddress, string password);
    }

    #endregion ISignup interface

    #region Signup class

    //  ------------
    //  Signup class
    //  ------------

    internal class Signup : ServiceBase<Model>, ISignup
    {
        public OperationResult WithEmailAddress(string emailAddress, string password)
        {
            return OperationResult.Invoke(() => Model.SignUpWithEmailAddress(emailAddress, password));
        }
    }

    #endregion Signup class

    #region SignupSnapIn class

    //  ------------------
    //  SignupSnapIn class
    //  ------------------

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class SignupSnapIn : WebServiceHostSnapIn<Signup, ISignup> { }

    #endregion SignupSnapIn class
}

// eof "Signup.cs"

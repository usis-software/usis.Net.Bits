//
//  @(#) OperationResult.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 14.0
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2015 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace usis.Framework
{
    #region OperationResultType enumeration

    //  -------------------------------
    //  OperationResultType enumeration
    //  -------------------------------

    public enum OperationResultType
    {
        Success,
        Information,
        Warning,
        Exception

    } // OperationResultType enumeration

    #endregion OperationResultType enumeration

    //  ---------------------
    //  OperationResult class
    //  ---------------------

    [DataContract]
    public class OperationResult
    {
        #region fields

        private List<OperationResultItem> items;

        #endregion fields

        #region properties

        //  -------------------
        //  ReturnCode property
        //  -------------------

        [DataMember]
        public int ReturnCode
        {
            get;
            private set;
        }

        [DataMember]
        public ICollection<OperationResultItem> ResultItems
        {
            get
            {
                if (this.items == null)
                {
                    this.items = new List<OperationResultItem>();
                }
                return this.items;
            }
        }

        #endregion properties

        #region construction

        public OperationResult() { }

        public OperationResult(bool succeeded)
        {
            this.Return(succeeded);
        }

        public OperationResult(Exception exception)
        {
            this.ReportException(exception);
        }

        #endregion construction

        #region methods

        public void Return(bool succeeded)
        {
            this.ReturnCode = succeeded ? 0 : -1;
        }

        public void Return(int returnCode)
        {
            this.ReturnCode = returnCode;
        }

        public void ReportException(Exception exception)
        {
            if (exception == null) throw new ArgumentNullException("exception");

            this.ReturnCode = exception.HResult;
            this.ResultItems.Add(new OperationResultItem(exception));
        }

        public void ReportSuccess(string message)
        {
            this.ReturnCode = 0;
            this.ResultItems.Add(new OperationResultItem(OperationResultType.Success, message));
        }

        public void ReportInformation(string message)
        {
            this.ResultItems.Add(new OperationResultItem(OperationResultType.Information, message));
        }

        public void ReportInformation(string message, int returnCode)
        {
            this.ReportInformation(message);
            this.ReturnCode = returnCode;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in this.ResultItems)
            {
                if (sb.Length > 0) sb.AppendLine();
                sb.AppendFormat("{0} - {1}", item.ResultType, item.Message.Trim());
            }
            if (sb.Length > 0) sb.AppendLine();
            sb.AppendFormat("Return Code: {0}", this.ReturnCode);
            return sb.ToString();
        }

        #endregion methods

    } // OperationResult class

    //  -------------------------
    //  OperationResultItem class
    //  -------------------------

    [DataContract]
    public class OperationResultItem
    {
        #region properties

        [DataMember]
        public String Message
        {
            get;
            private set;
        }

        [DataMember]
        public OperationResultType ResultType
        {
            get;
            private set;
        }

        #endregion properties

        #region construction

        internal OperationResultItem(Exception exception)
        {
            this.ResultType = OperationResultType.Exception;
            this.Message = exception.Message;
        }

        internal OperationResultItem(OperationResultType type, string message)
        {
            this.ResultType = type;
            this.Message = message;
        }

        #endregion construction

    } // OperationResultItem class

} // namespace usis.Framework

// eof "OperationResult.cs"

//
//  @(#) OperationResult.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2016 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;

namespace usis.Framework
{
    #region OperationResultType enumeration

    //  -------------------------------
    //  OperationResultType enumeration
    //  -------------------------------

    /// <summary>
    /// Specifies the type of an operation result item.
    /// </summary>

    public enum OperationResultType
    {
        /// <summary>
        /// The operation completed successfully.
        /// </summary>

        Success,

        /// <summary>
        /// The result item provides information about the execution of an operation.
        /// </summary>

        Information,

        /// <summary>
        /// The result item indicates a warning that is not immediately significant,
        /// but that may signify conditions that could cause future problems.
        /// </summary>

        Warning,

        /// <summary>
        /// The result item indicates that an exception occurred during the execution of an operation.
        /// </summary>

        Exception
    }

    #endregion OperationResultType enumeration

    //  ---------------------
    //  OperationResult class
    //  ---------------------

    /// <summary>
    /// Represents the result of an operation.
    /// Provides methods to set return values, exceptions and informational data
    /// and return them to the caller.
    /// </summary>

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

        /// <summary>
        /// Gets the return code.
        /// </summary>
        /// <value>
        /// The return code.
        /// </value>

        [DataMember]
        public int ReturnCode
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets an emuerator to iterate the items of the operation result.
        /// </summary>
        /// <value>
        /// An emuerator to iterate the items of the operation result.
        /// </value>

        [DataMember]
        public ICollection<OperationResultItem> ResultItems
        {
            get
            {
                if (items == null)
                {
                    items = new List<OperationResultItem>();
                }
                return items;
            }
        }

        #endregion properties

        #region construction

        //  ------------
        //  constructors
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResult"/> class.
        /// </summary>

        public OperationResult() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResult"/> class.
        /// </summary>
        /// <param name="succeeded">
        /// If set to <b>true</b> the operation succeeded, otherise <b>false</b>.
        /// </param>

        public OperationResult(bool succeeded)
        {
            Return(succeeded);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResult"/> class.
        /// </summary>
        /// <param name="exception">
        /// An exception that occurred during the execution of the operation.
        /// </param>

        public OperationResult(Exception exception)
        {
            ReportException(exception);
        }

        #endregion construction

        #region methods

        private void Return(bool succeeded)
        {
            ReturnCode = succeeded ? 0 : -1;
        }

        //public void Return(int returnCode)
        //{
        //    ReturnCode = returnCode;
        //}

        //  --------------------
        //  ReportSuccess method
        //  --------------------

        /// <summary>
        /// Adds an item with the <see cref="OperationResultType.Success"/> type to the operation result.
        /// </summary>
        /// <param name="message">
        /// The message for the result item.
        /// </param>
        /// <remarks>
        /// A call to this method sets the <see cref="ReturnCode"/> to 0.
        /// </remarks>

        public void ReportSuccess(string message)
        {
            ReturnCode = 0;
            ResultItems.Add(new OperationResultItem(OperationResultType.Success, message));
        }

        //  ------------------------
        //  ReportInformation method
        //  ------------------------

        private void ReportInformation(string message)
        {
            ResultItems.Add(new OperationResultItem(OperationResultType.Information, message));
        }

        /// <summary>
        /// Adds an item with the <see cref="OperationResultType.Information" /> type to the operation result.
        /// </summary>
        /// <param name="message">The message for the result item.</param>
        /// <param name="returnCode">The <see cref="ReturnCode" /> to set.</param>

        public void ReportInformation(string message, int returnCode)
        {
            ReportInformation(message);
            ReturnCode = returnCode;
        }

        //  ----------------------
        //  ReportException method
        //  ----------------------

        /// <summary>
        /// Adds an item with the <see cref="OperationResultType.Exception"/> type to the operation result.
        /// </summary>
        /// <param name="exception">
        /// The exception that occurred during the execution of the operation.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///  <i>exception</i> is a null reference (<b>Nothing</b> in Visual Basic).
        /// </exception>

        public void ReportException(Exception exception)
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception));

            ReturnCode = exception.HResult;
            ResultItems.Add(new OperationResultItem(exception));
        }

        //  --------------------
        //  ReportWarning method
        //  --------------------

        /// <summary>
        /// Adds an item with the <see cref="OperationResultType.Warning"/> type to the operation result.
        /// </summary>
        /// <param name="message">The message for the result item.</param>
        /// <param name="returnCode">The <see cref="ReturnCode" /> to set.</param>

        public void ReportWarning(string message, int? returnCode)
        {
            ResultItems.Add(new OperationResultItem(OperationResultType.Warning, message));
            if (returnCode.HasValue) ReturnCode = returnCode.Value;
        }

        //public void ReportWarning(string message)
        //{
        //    ReportWarning(message, null);
        //}

        //  ---------------
        //  ToString method
        //  ---------------

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in ResultItems)
            {
                if (sb.Length > 0) sb.AppendLine();
                sb.AppendFormat(CultureInfo.CurrentCulture, "{0} - {1}", item.ResultType, item.Message.Trim());
            }
            if (sb.Length > 0) sb.AppendLine();
            sb.AppendFormat(CultureInfo.CurrentCulture, "Return Code: {0}", ReturnCode);
            return sb.ToString();
        }

        #endregion methods
    }

    #region OperationResultItem class

    //  -------------------------
    //  OperationResultItem class
    //  -------------------------

    /// <summary>
    /// Represents a single item that is a part of an operation result.
    /// </summary>

    [DataContract]
    public class OperationResultItem
    {
        #region properties

        //  ----------------
        //  Message property
        //  ----------------

        /// <summary>
        /// Gets the message associated with this item.
        /// </summary>
        /// <value>
        /// The message associated with this item.
        /// </value>

        [DataMember]
        public string Message
        {
            get;
            private set;
        }

        //  -------------------
        //  ResultType property
        //  -------------------

        /// <summary>
        /// Gets the type of the result.
        /// </summary>
        /// <value>
        /// The type of the result.
        /// </value>

        [DataMember]
        public OperationResultType ResultType
        {
            get;
            private set;
        }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal OperationResultItem(Exception exception)
        {
            ResultType = OperationResultType.Exception;
            Message = exception.Message;
        }

        internal OperationResultItem(OperationResultType type, string message)
        {
            ResultType = type;
            Message = message;
        }

        #endregion construction
    }

    #endregion OperationResultItem class
}

// eof "OperationResult.cs"

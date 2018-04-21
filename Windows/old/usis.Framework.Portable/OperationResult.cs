//
//  @(#) OperationResult.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;

namespace usis.Framework.Portable
{
    #region OperationResultType enumeration

    //  -------------------------------
    //  OperationResultType enumeration
    //  -------------------------------

    /// <summary>
    /// Specifies the type of an operation result item.
    /// </summary>

    [Obsolete("Use the type from usis.Framework namespace instead.")]
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

    #region OperationResult class

    //  ---------------------
    //  OperationResult class
    //  ---------------------

    /// <summary>
    /// Represents the result of an operation.
    /// Provides methods to set return values, exceptions and informational data
    /// and return them to the caller.
    /// </summary>

    [DataContract]
    [Obsolete("Use the type from usis.Framework namespace instead.")]
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
        public int ReturnCode { get; private set; }

        //  --------------------
        //  ResultItems property
        //  --------------------

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

        //  ------------------
        //  Succeeded property
        //  ------------------

        /// <summary>
        /// Gets a value indicating whether the operation succeeded.
        /// </summary>
        /// <value>
        ///   <c>true</c> if succeeded; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// The operation succeeded when <see cref="ReturnCode"/> is not negative.
        /// </remarks>

        public bool Succeeded { get { return ReturnCode >= 0; } }

        //  ---------------
        //  Failed property
        //  ---------------

        /// <summary>
        /// Gets a value indicating whether the operation failed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if failed; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// The operation failed when <see cref="ReturnCode"/> is negative.
        /// </remarks>

        public bool Failed { get { return !Succeeded; } }

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

        public OperationResult(bool succeeded) { ReportSuccess(succeeded); }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResult"/> class
        /// with the specified exception.
        /// </summary>
        /// <param name="exception">
        /// An exception that occurred during the execution of the operation.
        /// </param>

        public OperationResult(Exception exception) { ReportException(exception); }

        #endregion construction

        #region methods

        #region Report... methods

        //  --------------------
        //  ReportSuccess method
        //  --------------------

        /// <summary>
        /// Sets the <see cref="ReturnCode"/> according to the provided flag.
        /// </summary>
        /// <param name="succeeded">if set to <c>true</c> <see cref="ReturnCode"/> is set to 0, otherwise to -1.</param>

        public void ReportSuccess(bool succeeded) { ReturnCode = succeeded ? 0 : -1; }

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

        /// <summary>
        /// Adds an item with the <see cref="OperationResultType.Information" /> type to the operation result.
        /// </summary>
        /// <param name="message">The message for the result item.</param>

        public void ReportInformation(string message)
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

        /// <summary>
        /// Adds an item with the <see cref="OperationResultType.Warning"/> type to the operation result.
        /// </summary>
        /// <param name="message">The message for the result item.</param>

        public void ReportWarning(string message) { ReportWarning(message, null); }

        //  -------------------
        //  ReportResult method
        //  -------------------

        /// <summary>
        /// Append the result items and sets the <see cref="ReturnCode"/> from the specified operation result.
        /// </summary>
        /// <param name="result">A result of another operation.</param>
        /// <exception cref="ArgumentNullException">
        /// <c>result</c> is a <b>null</b> reference (<b>Nothing</b> in Visual Basic).
        /// </exception>

        public void ReportResult(OperationResult result)
        {
            if (result == null) throw new ArgumentNullException(nameof(result));
            foreach (var item in result.ResultItems)
            {
                ResultItems.Add(item);
            }
            ReturnCode = result.ReturnCode;
        }

        #endregion Report... methods

        #region overrides

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
                sb.AppendFormat(CultureInfo.CurrentCulture, "{0} - {1}", item.ResultType, item.Message?.Trim());
            }
            if (sb.Length > 0) sb.AppendLine();
            sb.AppendFormat(CultureInfo.CurrentCulture, "Return Code: {0}", ReturnCode);
            return sb.ToString();
        }

        #endregion overrides

        #region Invoke methods

        //  --------------
        //  Invoke methods
        //  --------------

        /// <summary>
        /// Invokes the specified action.
        /// </summary>
        /// <param name="action">The action to invoke.</param>
        /// <returns>
        /// An <b>OperationResult</b> that describes an exception,
        /// if one occured during exection of the action.
        /// </returns>
        /// <exception cref="ArgumentNullException"><b>action</b> is a null reference.</exception>

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static OperationResult Invoke(Action action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            var result = new OperationResult();
            try { action.Invoke(); }
            catch (Exception exception) { result.ReportException(exception); }
            return result;
        }

        /// <summary>
        /// Invokes the specified action
        /// by passing the later returned <see cref="OperationResult"/>
        /// as an argument.
        /// </summary>
        /// <param name="action">The action to invoke.</param>
        /// <returns>
        /// An <b>OperationResult</b> that describes an exception,
        /// if one occured during exection of the action.
        /// </returns>
        /// <exception cref="ArgumentNullException"><b>action</b> is a null reference.</exception>

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static OperationResult Invoke(Action<OperationResult> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            var result = new OperationResult();
            try { action.Invoke(result); }
            catch (Exception exception) { result.ReportException(exception); }
            return result;
        }

        /// <summary>
        /// Invokes the specified function.
        /// </summary>
        /// <typeparam name="TReturnValue">The type of the return value.</typeparam>
        /// <param name="function">The function to invoke.</param>
        /// <returns>
        /// An <b>OperationResult</b> with the result of the function's invocation and the returned value.
        /// </returns>

        public static OperationResult<TReturnValue> Invoke<TReturnValue>(Func<TReturnValue> function)
        {
            return new OperationResult<TReturnValue>().Return(function);
        }

        /// <summary>
        /// Invokes the specified function
        /// by passing the later returned <see cref="OperationResult{TReturnValue}"/>
        /// as an argument.
        /// </summary>
        /// <typeparam name="TReturnValue">The type of the return value.</typeparam>
        /// <param name="function">The function to invoke.</param>
        /// <returns>
        /// An <b>OperationResult</b> with the result of the function's invocation and the returned value.
        /// </returns>

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static OperationResult<TReturnValue> Invoke<TReturnValue>(Func<OperationResult<TReturnValue>, TReturnValue> function)
        {
            if (function == null) throw new ArgumentNullException(nameof(function));
            var result = new OperationResult<TReturnValue>();
            try { result.ReturnValue = function.Invoke(result); }
            catch (Exception exception) { result.ReportException(exception); }
            return result;
        }

        #endregion Invoke methods

        #region exception methods

        //  ----------------------
        //  CreateException method
        //  ----------------------

        /// <summary>
        /// Creates an exception if the operation failed.
        /// </summary>
        /// <returns>
        /// An exception with the information from the result items as message.
        /// </returns>

        public Exception CreateException()
        {
            if (Succeeded) return null;
            return new OperationResultException(this);
        }

        //  ------------------
        //  ThrowOnFail method
        //  ------------------

        /// <summary>
        /// Throws an exception when the operation failed.
        /// </summary>

        public void ThrowOnFail() { if (Failed) throw CreateException(); }

        #endregion exception methods

        #endregion methods
    }

    #endregion OperationResult class

    #region OperationResult<TReturnValue> class

    //  -----------------------------------
    //  OperationResult<TReturnValue> class
    //  -----------------------------------

    /// <summary>
    /// Represents the result of an operation and its return value.
    /// Provides methods to set return values, exceptions and informational data
    /// and return them to the caller.
    /// </summary>
    /// <typeparam name="TReturnValue">The type of the return value.</typeparam>
    /// <seealso cref="OperationResult" />

    [DataContract]
    [Obsolete("Use the type from usis.Framework namespace instead.")]
    public class OperationResult<TReturnValue> : OperationResult
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResult{TReturnValue}"/> class.
        /// </summary>

        public OperationResult() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResult{TReturnValue}"/> class
        /// with the specified flag to set the return code.
        /// </summary>
        /// <param name="succeeded">If set to <b>true</b> the operation succeeded, otherise <b>false</b>.</param>

        public OperationResult(bool succeeded) : base(succeeded) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResult{TReturnValue}"/> class
        /// with the specified exception.
        /// </summary>
        /// <param name="exception">An exception that occurred during the execution of the operation.</param>

        public OperationResult(Exception exception) : base(exception) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResult{TReturnValue}"/> class
        /// with the specified return value and message text.
        /// </summary>
        /// <param name="returnValue">The return value.</param>
        /// <param name="message">The message for the result item.</param>

        public OperationResult(TReturnValue returnValue, string message) : base(true)
        {
            ReportInformation(message);
            ReturnValue = returnValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResult{TReturnValue}"/> class
        /// with the specified return value.
        /// </summary>
        /// <param name="returnValue">The return value.</param>

        public OperationResult(TReturnValue returnValue) : base(true) { ReturnValue = returnValue; }

        #endregion construction

        #region properties

        //  --------------------
        //  ReturnValue property
        //  --------------------

        /// <summary>
        /// Gets or sets the return value.
        /// </summary>
        /// <value>
        /// The return value.
        /// </value>

        [DataMember]
        public TReturnValue ReturnValue { get; set; }

        #endregion properties

        #region methods

        //  --------------------
        //  ReportSuccess method
        //  --------------------

        /// <summary>
        /// Adds an item with the <see cref="OperationResultType.Success" /> type to the operation result.
        /// and sets the specified return value.
        /// </summary>
        /// <param name="returnValue">The return value.</param>
        /// <param name="message">The message for the result item.</param>
        /// <remarks>
        /// A call to this method sets the <see cref="OperationResult.ReturnCode" /> to 0.
        /// </remarks>

        public void ReportSuccess(TReturnValue returnValue, string message)
        {
            ReturnValue = returnValue;
            ReportSuccess(message);
        }

        //  -------------------
        //  ReportResult method
        //  -------------------

        /// <summary>
        /// Append the result items and sets the <see cref="OperationResult.ReturnCode" /> and the <see cref="ReturnValue" />
        /// from the specified operation result.
        /// </summary>
        /// <param name="result">A result of another operation.</param>
        /// <returns>
        /// An <b>OperationResult</b> with the result of the operation and the returned value.
        /// </returns>
        /// <exception cref="ArgumentNullException"><c>result</c> is a <b>nulll</b> reference (<b>Nothing</b> in Visual Basic).</exception>

        public OperationResult<TReturnValue> ReportResult(OperationResult<TReturnValue> result)
        {
            base.ReportResult(result);
            ReturnValue = result.ReturnValue;
            return result;
        }

        //  --------------
        //  Return methods
        //  --------------

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        internal OperationResult<TReturnValue> Return(Func<TReturnValue> function)
        {
            if (function == null) throw new ArgumentNullException(nameof(function));
            try
            {
                ReturnValue = function();
            }
            catch (Exception exception) { ReportException(exception); }
            return this;
        }

        //  -------------------------
        //  ThrowOrReturnValue method
        //  -------------------------

        /// <summary>
        /// Throws an exception when the operation failed or gets the return value.
        /// </summary>
        /// <returns>The return value.</returns>

        public TReturnValue ThrowOrReturnValue() { ThrowOnFail(); return ReturnValue; }

        #endregion methods

        #region overrides

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
            StringBuilder sb = new StringBuilder(base.ToString());
            if (sb.Length > 0) sb.AppendLine();
            sb.AppendFormat(CultureInfo.CurrentCulture, "Return Value: {0}", ReturnValue);
            return sb.ToString();
        }

        #endregion overrides
    }

    #endregion OperationResult<TReturnValue> class

    #region OperationResultList class

    //  -------------------------
    //  OperationResultList class
    //  -------------------------

    /// <summary>
    /// Provides methods to return a <see cref="OperationResult{TReturnValue}"/>
    /// by invoking functions that return enumerations.
    /// </summary>

    [Obsolete("Use the type from usis.Framework namespace instead.")]
    public static class OperationResultList
    {
        /// <summary>
        /// Invokes the specified function.
        /// </summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="function">The function to invoke.</param>
        /// <returns>
        /// An operation result that hold the enumeration return by <b>function</b> as return value.
        /// </returns>

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public static OperationResultList<TItem> Invoke<TItem>(Func<IEnumerable<TItem>> function)
        {
            if (function == null) throw new ArgumentNullException(nameof(function));
            return new OperationResultList<TItem>().Return(function);
        }
    }

    #endregion OperationResultList class

    #region OperationResultList<TItem> class

    //  --------------------------------
    //  OperationResultList<TItem> class
    //  --------------------------------

    /// <summary>
    /// Represents the result of an operation with an enumeration as return value.
    /// </summary>
    /// <typeparam name="TItem">The type of the enumeration items.</typeparam>

    [DataContract]
    [Obsolete("Use the type from usis.Framework namespace instead.")]
    public class OperationResultList<TItem> : OperationResult<IEnumerable<TItem>>
    {
        //  -------------
        //  Return method
        //  -------------

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        internal new OperationResultList<TItem> Return(Func<IEnumerable<TItem>> function)
        {
            if (function == null) throw new ArgumentNullException(nameof(function));
            try
            {
                ReturnValue = function();
            }
            catch (Exception exception) { ReportException(exception); }
            return this;
        }

        //  --------------
        //  Iterate method
        //  --------------

        /// <summary>
        /// Iterates thru the return values of the operation result.
        /// </summary>
        /// <returns>
        /// An enumerator thru the return values of the operation result.
        /// </returns>

        public IEnumerable<TItem> Iterate()
        {
            return Iterate(true);
        }

        /// <summary>
        /// Iterates thru the return values of the operation result.
        /// </summary>
        /// <param name="throwException">if set to <c>true</c> an operation result exception is rethrown.</param>
        /// <returns>
        /// An enumerator thru the return values of the operation result.
        /// </returns>

        public IEnumerable<TItem> Iterate(bool throwException)
        {
            if (Failed && throwException) throw CreateException();
            if (ReturnValue == null) yield break;
            else foreach (var item in ReturnValue) yield return item;
        }
    }

    #endregion OperationResultList<TItem> class

    #region OperationResultItem class

    //  -------------------------
    //  OperationResultItem class
    //  -------------------------

    /// <summary>
    /// Represents a single item that is a part of an operation result.
    /// </summary>

    [DataContract]
    [Obsolete("Use the type from usis.Framework namespace instead.")]
    public class OperationResultItem
    {
        #region properties

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
        public OperationResultType ResultType { get; private set; }

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
        public string Message { get; private set; }

        //  ----------------
        //  Details property
        //  ----------------

        /// <summary>
        /// Provides additional informations about the result item.
        /// </summary>
        /// <value>
        /// Additional informations about the result item.
        /// </value>

        [DataMember]
        public object Details { get; private set; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal OperationResultItem(Exception exception)
        {
            ResultType = OperationResultType.Exception;
            Message = exception.Message;
            Details = exception.ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResultItem"/> class
        /// with the specified type and message.
        /// </summary>
        /// <param name="type">The item's <b>OperationResultType</b>.</param>
        /// <param name="message">The item's message.</param>

        public OperationResultItem(OperationResultType type, string message)
        {
            ResultType = type;
            Message = message;
        }

        #endregion construction
    }

    #endregion OperationResultItem class

    #region OperationResultException class

    //  ------------------------------
    //  OperationResultException class
    //  ------------------------------

    /// <summary>
    /// Provides an exception that can be thrown when an operation failed.
    /// </summary>
    /// <seealso cref="Exception" />

    [Obsolete("Use the type from usis.Framework namespace instead.")]
    public class OperationResultException : Exception
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResultException"/> class.
        /// </summary>

        public OperationResultException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResultException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>

        public OperationResultException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResultException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>

        public OperationResultException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResultException"/> class
        /// with the specified operation result.
        /// </summary>
        /// <param name="result">The operation result.</param>

        public OperationResultException(OperationResult result) : base(CreateMessage(result)) { Result = result; }

        #endregion construction

        #region private methods

        //  --------------------
        //  CreateMessage method
        //  --------------------

        private static string CreateMessage(OperationResult result)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in result.ResultItems)
            {
                sb.AppendLine(item.Message);
            }
            return sb.ToString();
        }

        #endregion private methods

        #region properties

        //  ---------------
        //  Result property
        //  ---------------

        /// <summary>
        /// Gets the result of the operation.
        /// </summary>
        /// <value>
        /// The result of the operation.
        /// </value>

        public OperationResult Result { get; private set; }

        #endregion properties
    }

    #endregion OperationResultException class
}

// eof "OperationResult.cs"

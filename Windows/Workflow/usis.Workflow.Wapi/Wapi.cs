using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace usis.Workflow.Wapi
{
    #region types

    using WMTInt16 = System.Int16;
    using WMTInt32 = System.Int32;
    using WMTUInt32 = System.UInt32;
    using WMTPText = System.String;
    using WMTBoolean = System.Boolean;
    using WMTPPrivate = System.Object;

    using WMTPInt32 = Nullable<int>;

    using WMTPConnectInfo = Nullable<WMTConnectInfo>;
    using WMTPSessionHandle = Nullable<WMTSessionHandle>;
    using WMTPFilter = Nullable<WMTFilter>;
    using WMTPQueryHandle = Nullable<WMTQueryHandle>;
    using WMTPProcDef = Nullable<WMTProcDef>;

    #endregion types

    #region structures

    public struct WMTErrRetType : IEquatable<WMTErrRetType>
    {
        public WMTInt16 main_code;
        public WMTInt16 sub_code;

        public override bool Equals(object obj)
        {
            if (!(obj is WMTErrRetType)) return false;
            return Equals((WMTErrRetType)obj);
        }

        public bool Equals(WMTErrRetType other)
        {
            if (main_code != other.main_code) return false;
            return sub_code == other.sub_code;
        }

        public override int GetHashCode()
        {
            return main_code ^ sub_code;
        }

        public static bool operator ==(WMTErrRetType error1, WMTErrRetType error2)
        {
            return error1.Equals(error2);
        }

        public static bool operator !=(WMTErrRetType error1, WMTErrRetType error2)
        {
            return !error1.Equals(error2);
        }
    }

    [SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")]
    public struct WMTConnectInfo
    {
        public WMTPText user_identification;
        public WMTPText password;
        public WMTPText engine_name;
        public WMTPText scope;
    }

    [SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")]
    public struct WMTSessionHandle
    {
        [CLSCompliant(false)]
        public WMTUInt32 session_id;
        public WMTPPrivate pprivate;
    }

    [SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")]
    public struct WMTFilter
    {
        public WMTInt32 filter_type;
        public WMTInt32 filter_length;
        public WMTPText attribute_name;
        [CLSCompliant(false)]
        public WMTUInt32 comparison;
        public WMTPText filter_string;
    }

    public struct WMTQueryHandle
    {
    }

    [SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")]
    public struct WMTProcDefID
    {
        public WMTPText proc_def_id;
    }

    [SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")]
    public struct WMTProcDefState
    {
        public WMTPText proc_def_state;
    }

    [SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")]
    public struct WMTProcDef
    {
        public WMTPText process_name;
        public WMTProcDefID proc_def_id;
        public WMTProcDefState state;
    }

    #endregion structures

    public static class WM
    {
        #region constants

        public const WMTInt16 SUCCESS = 0;
        public const WMTInt16 CONNECT_FAILED = 0x0010;
        public const WMTInt16 INVALID_PROCESS_DEFINITION = 0x0020;
        public const WMTInt16 INVALID_ACTIVITY_NAME = 0x0030;
        public const WMTInt16 INVALID_PROCESS_INSTANCE = 0x0040;
        public const WMTInt16 INVALID_ACTIVITY_INSTANCE = 0x0050;
        public const WMTInt16 INVALID_WORKITEM = 0x0060;
        public const WMTInt16 INVALID_ATTRIBUTE = 0x0070;
        public const WMTInt16 ATTRIBUTE_ASSIGNMENT_FAILED = 0x0080;
        public const WMTInt16 INVALID_STATE = 0x0090;
        public const WMTInt16 TRANSITION_NOT_ALLOWED = 0x00A0;
        public const WMTInt16 INVALID_SESSION_HANDLE = 0x00B0;
        public const WMTInt16 INVALID_QUERY_HANDLE = 0x00C0;
        public const WMTInt16 INVALID_SOURCE_USER = 0x00D0;
        public const WMTInt16 INVALID_TARGET_USER = 0x00E0;
        public const WMTInt16 INVALID_FILTER = 0x00F0;
        public const WMTInt16 LOCKED = 0x00F1;
        public const WMTInt16 NOT_LOCKED = 0x00F2;
        public const WMTInt16 NO_MORE_DATA = 0x00F3;
        public const WMTInt16 INSUFFICIENT_BUFFER_SIZE = 0x00F4;
        public const WMTInt16 EXECUTE_FAILED = 0x00F5;

        #endregion constants

        #region Connection Functions

        //  --------------
        //  Connect method
        //  --------------

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static WMTErrRetType Connect(WMTPConnectInfo pconnect_info, out WMTPSessionHandle psession_handle)
        {
            if (!pconnect_info.HasValue) throw new ArgumentNullException(nameof(pconnect_info));
            Session session = null;
            try
            {
                session = new Session(
                    pconnect_info.Value.user_identification,
                    pconnect_info.Value.password,
                    pconnect_info.Value.engine_name,
                    pconnect_info.Value.scope);
                psession_handle = new WMTSessionHandle() { pprivate = session, session_id = (WMTUInt32)session.GetHashCode() };
                return new WMTErrRetType { main_code = SUCCESS };
            }
            catch (Exception exception)
            {
                Trace.WriteLine(exception);
                psession_handle = null;
                return new WMTErrRetType { main_code = CONNECT_FAILED };
            }
            finally
            {
                if (session != null) session.Dispose();
            }
        }

        //  -----------------
        //  Disconnect method
        //  -----------------

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static WMTErrRetType Disconnect(WMTPSessionHandle psession_handle)
        {
            if (!psession_handle.HasValue) throw new ArgumentNullException(nameof(psession_handle));
            try
            {
                (psession_handle.Value.pprivate as Session).Dispose();
                psession_handle = null;
                return new WMTErrRetType { main_code = SUCCESS };
            }
            catch (Exception exception)
            {
                Trace.WriteLine(exception);
                return new WMTErrRetType { main_code = INVALID_SESSION_HANDLE };
            }
        }

        #endregion Connection Functions

        //  ---------------------------------
        //  OpenProcessDefinitionsList method
        //  ---------------------------------

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "count_flag")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "pproc_def_filter")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "psession_handle")]
        public static WMTErrRetType OpenProcessDefinitionsList(
            WMTPSessionHandle psession_handle,
            WMTPFilter pproc_def_filter,
            WMTBoolean count_flag,
            out WMTPQueryHandle pquery_handle,
            out WMTPInt32 pcount)
        {
            pquery_handle = null;
            pcount = null;
            return new WMTErrRetType { main_code = INVALID_FILTER };
        }

        //  -----------------------------
        //  FetchProcessDefinition method
        //  -----------------------------

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "pquery_handle")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "psession_handle")]
        public static WMTErrRetType FetchProcessDefinition(
            WMTPSessionHandle psession_handle,
            WMTPQueryHandle pquery_handle,
            out WMTPProcDef pproc_def_buf_ptr)
        {
            pproc_def_buf_ptr = null;
            return new WMTErrRetType { main_code = INVALID_QUERY_HANDLE };
        }

        //  ----------------------------------
        //  CloseProcessDefinitionsList method
        //  ----------------------------------

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "psession_handle")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "pquery_handle")]
        public static WMTErrRetType CloseProcessDefinitionsList(
            WMTPSessionHandle psession_handle,
            WMTPQueryHandle pquery_handle)
        {
            return new WMTErrRetType { main_code = INVALID_QUERY_HANDLE };
        }
    }
}

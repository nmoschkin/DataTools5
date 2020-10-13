// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: Async
//         Native Async.
// 
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the Microsoft Public License   
// ************************************************* ''


using System;
using System.Runtime.InteropServices;

namespace DataTools.Hardware.Native
{
    internal static class Async
    {
        public const int STATUS_WAIT_0 = 0x0;
        public const int STATUS_ABANDONED_WAIT_0 = 0x80;
        public const int STATUS_USER_APC = 0xC0;
        public const int STATUS_TIMEOUT = 0x102;
        public const int STATUS_PENDING = 0x103;
        public const int DBG_EXCEPTION_HANDLED = 0x10001;
        public const int DBG_CONTINUE = 0x10002;
        public const int STATUS_SEGMENT_NOTIFICATION = 0x40000005;
        public const int STATUS_FATAL_APP_EXIT = 0x40000015;
        public const int DBG_TERMINATE_THREAD = 0x40010003;
        public const int DBG_TERMINATE_PROCESS = 0x40010004;
        public const int DBG_CONTROL_C = 0x40010005;
        public const int DBG_PRINTEXCEPTION_C = 0x40010006;
        public const int DBG_RIPEXCEPTION = 0x40010007;
        public const int DBG_CONTROL_BREAK = 0x40010008;
        public const int DBG_COMMAND_EXCEPTION = 0x40010009;

        public const uint STATUS_GUARD_PAGE_VIOLATION = 0x80000001;
        public const uint STATUS_DATATYPE_MISALIGNMENT = 0x80000002;
        public const uint STATUS_BREAKPOINT = 0x80000003;
        public const uint STATUS_SINGLE_STEP = 0x80000004;
        public const uint STATUS_LONGJUMP = 0x80000026;
        public const uint STATUS_UNWIND_CONSOLIDATE = 0x80000029;
        public const uint DBG_EXCEPTION_NOT_HANDLED = 0x80010001;
        public const uint STATUS_ACCESS_VIOLATION = 0xC0000005;
        public const uint STATUS_IN_PAGE_ERROR = 0xC0000006;
        public const uint STATUS_INVALID_HANDLE = 0xC0000008;
        public const uint STATUS_INVALID_PARAMETER = 0xC000000D;
        public const uint STATUS_NO_MEMORY = 0xC0000017;
        public const uint STATUS_ILLEGAL_INSTRUCTION = 0xC000001D;
        public const uint STATUS_NONCONTINUABLE_EXCEPTION = 0xC0000025;
        public const uint STATUS_INVALID_DISPOSITION = 0xC0000026;
        public const uint STATUS_ARRAY_BOUNDS_EXCEEDED = 0xC000008C;
        public const uint STATUS_FLOAT_DENORMAL_OPERAND = 0xC000008D;
        public const uint STATUS_FLOAT_DIVIDE_BY_ZERO = 0xC000008E;
        public const uint STATUS_FLOAT_INEXACT_RESULT = 0xC000008F;
        public const uint STATUS_FLOAT_INVALID_OPERATION = 0xC0000090;
        public const uint STATUS_FLOAT_OVERFLOW = 0xC0000091;
        public const uint STATUS_FLOAT_STACK_CHECK = 0xC0000092;
        public const uint STATUS_FLOAT_UNDERFLOW = 0xC0000093;
        public const uint STATUS_INTEGER_DIVIDE_BY_ZERO = 0xC0000094;
        public const uint STATUS_INTEGER_OVERFLOW = 0xC0000095;
        public const uint STATUS_PRIVILEGED_INSTRUCTION = 0xC0000096;
        public const uint STATUS_STACK_OVERFLOW = 0xC00000FD;
        public const uint STATUS_DLL_NOT_FOUND = 0xC0000135;
        public const uint STATUS_ORDINAL_NOT_FOUND = 0xC0000138;
        public const uint STATUS_ENTRYPOINT_NOT_FOUND = 0xC0000139;
        public const uint STATUS_CONTROL_C_EXIT = 0xC000013A;
        public const uint STATUS_DLL_INIT_FAILED = 0xC0000142;
        public const uint STATUS_FLOAT_MULTIPLE_FAULTS = 0xC00002B4;
        public const uint STATUS_FLOAT_MULTIPLE_TRAPS = 0xC00002B5;
        public const uint STATUS_REG_NAT_CONSUMPTION = 0xC00002C9;
        public const uint STATUS_HEAP_CORRUPTION = 0xC0000374;
        public const uint STATUS_STACK_BUFFER_OVERRUN = 0xC0000409;
        public const uint STATUS_INVALID_CRUNTIME_PARAMETER = 0xC0000417;
        public const uint STATUS_ASSERTION_FAILURE = 0xC0000420;
        public const uint STATUS_SXS_EARLY_DEACTIVATION = 0xC015000F;
        public const uint STATUS_SXS_INVALID_DEACTIVATION = 0xC0150010;

        //
        // Used to represent information related to a thread impersonation
        //

        public const int DISABLE_MAX_PRIVILEGE = 0x1;
        public const int SANDBOX_INERT = 0x2;
        public const int LUA_TOKEN = 0x4;
        public const int WRITE_RESTRICTED = 0x8;
        public const long OWNER_SECURITY_INFORMATION = 0x1L;
        public const long GROUP_SECURITY_INFORMATION = 0x2L;
        public const long DACL_SECURITY_INFORMATION = 0x4L;
        public const long SACL_SECURITY_INFORMATION = 0x8L;
        public const long LABEL_SECURITY_INFORMATION = 0x10L;
        public const long ATTRIBUTE_SECURITY_INFORMATION = 0x20L;
        public const long SCOPE_SECURITY_INFORMATION = 0x40L;
        public const long PROCESS_TRUST_LABEL_SECURITY_INFORMATION = 0x80L;
        public const long BACKUP_SECURITY_INFORMATION = 0x10000L;
        public const long PROTECTED_DACL_SECURITY_INFORMATION = 0x80000000L;
        public const long PROTECTED_SACL_SECURITY_INFORMATION = 0x40000000L;
        public const long UNPROTECTED_DACL_SECURITY_INFORMATION = 0x20000000L;
        public const long UNPROTECTED_SACL_SECURITY_INFORMATION = 0x10000000L;
        public const int PROCESS_TERMINATE = 0x1;
        public const int PROCESS_CREATE_THREAD = 0x2;
        public const int PROCESS_SET_SESSIONID = 0x4;
        public const int PROCESS_VM_OPERATION = 0x8;
        public const int PROCESS_VM_READ = 0x10;
        public const int PROCESS_VM_WRITE = 0x20;
        public const int PROCESS_DUP_HANDLE = 0x40;
        public const int PROCESS_CREATE_PROCESS = 0x80;
        public const int PROCESS_SET_QUOTA = 0x100;
        public const int PROCESS_SET_INFORMATION = 0x200;
        public const int PROCESS_QUERY_INFORMATION = 0x400;
        public const int PROCESS_SUSPEND_RESUME = 0x800;
        public const int PROCESS_QUERY_LIMITED_INFORMATION = 0x1000;
        public const int PROCESS_SET_LIMITED_INFORMATION = 0x2000;
        // (NTDDI_VERSION >= NTDDI_VISTA) Then
        public const int PROCESS_ALL_ACCESS = FileApi.STANDARD_RIGHTS_REQUIRED | FileApi.SYNCHRONIZE | 0xFFFF;
        //
        // defined(_WIN64)

        public static int MAXIMUM_PROC_PER_GROUP
        {
            get
            {
                if (IntPtr.Size == 8)
                    return 64;
                else
                    return 32;
            }
        }

        //

        public readonly static int MAXIMUM_PROCESSORS = MAXIMUM_PROC_PER_GROUP;
        public const int THREAD_TERMINATE = 0x1;
        public const int THREAD_SUSPEND_RESUME = 0x2;
        public const int THREAD_GET_CONTEXT = 0x8;
        public const int THREAD_SET_CONTEXT = 0x10;
        public const int THREAD_QUERY_INFORMATION = 0x40;
        public const int THREAD_SET_INFORMATION = 0x20;
        public const int THREAD_SET_THREAD_TOKEN = 0x80;
        public const int THREAD_IMPERSONATE = 0x100;
        public const int THREAD_DIRECT_IMPERSONATION = 0x200;
        // begin_wdm
        public const int THREAD_SET_LIMITED_INFORMATION = 0x400;  // winnt
        public const int THREAD_QUERY_LIMITED_INFORMATION = 0x800;  // winnt
        public const int THREAD_RESUME = 0x1000;  // winnt
        // (NTDDI_VERSION >= NTDDI_VISTA) Then
        public const int THREAD_ALL_ACCESS = FileApi.STANDARD_RIGHTS_REQUIRED | FileApi.SYNCHRONIZE | 0xFFFF;
        //
        //
        public const int JOB_OBJECT_ASSIGN_PROCESS = 0x1;
        public const int JOB_OBJECT_SET_ATTRIBUTES = 0x2;
        public const int JOB_OBJECT_QUERY = 0x4;
        public const int JOB_OBJECT_TERMINATE = 0x8;
        public const int JOB_OBJECT_SET_SECURITY_ATTRIBUTES = 0x10;
        public const int JOB_OBJECT_ALL_ACCESS = FileApi.STANDARD_RIGHTS_REQUIRED | FileApi.SYNCHRONIZE | 0x1F;

        [DllImport("kernel32")]
        public static extern IntPtr OpenThread(int wdDesiredAccess, bool binheritHandle, int dwThreadId);
    }
}
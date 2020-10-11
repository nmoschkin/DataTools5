'' ************************************************* ''
'' DataTools Visual Basic Utility Library - Interop
''
'' Module: Async
''         Native Async.
'' 
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''


Imports System.Runtime.InteropServices
Imports DataTools.Interop.Native
Imports DataTools.Interop
Imports System.ComponentModel


Namespace Native

    Friend Module Async

        Public Const STATUS_WAIT_0 = &H0
        Public Const STATUS_ABANDONED_WAIT_0 = &H80
        Public Const STATUS_USER_APC = &HC0
        Public Const STATUS_TIMEOUT = &H102
        Public Const STATUS_PENDING = &H103
        Public Const DBG_EXCEPTION_HANDLED = &H10001
        Public Const DBG_CONTINUE = &H10002
        Public Const STATUS_SEGMENT_NOTIFICATION = &H40000005
        Public Const STATUS_FATAL_APP_EXIT = &H40000015
        Public Const DBG_TERMINATE_THREAD = &H40010003
        Public Const DBG_TERMINATE_PROCESS = &H40010004
        Public Const DBG_CONTROL_C = &H40010005
        Public Const DBG_PRINTEXCEPTION_C = &H40010006
        Public Const DBG_RIPEXCEPTION = &H40010007
        Public Const DBG_CONTROL_BREAK = &H40010008
        Public Const DBG_COMMAND_EXCEPTION = &H40010009
        Public Const STATUS_GUARD_PAGE_VIOLATION = &H80000001
        Public Const STATUS_DATATYPE_MISALIGNMENT = &H80000002
        Public Const STATUS_BREAKPOINT = &H80000003
        Public Const STATUS_SINGLE_STEP = &H80000004
        Public Const STATUS_LONGJUMP = &H80000026
        Public Const STATUS_UNWIND_CONSOLIDATE = &H80000029
        Public Const DBG_EXCEPTION_NOT_HANDLED = &H80010001
        Public Const STATUS_ACCESS_VIOLATION = &HC0000005
        Public Const STATUS_IN_PAGE_ERROR = &HC0000006
        Public Const STATUS_INVALID_HANDLE = &HC0000008
        Public Const STATUS_INVALID_PARAMETER = &HC000000D
        Public Const STATUS_NO_MEMORY = &HC0000017
        Public Const STATUS_ILLEGAL_INSTRUCTION = &HC000001D
        Public Const STATUS_NONCONTINUABLE_EXCEPTION = &HC0000025
        Public Const STATUS_INVALID_DISPOSITION = &HC0000026
        Public Const STATUS_ARRAY_BOUNDS_EXCEEDED = &HC000008C
        Public Const STATUS_FLOAT_DENORMAL_OPERAND = &HC000008D
        Public Const STATUS_FLOAT_DIVIDE_BY_ZERO = &HC000008E
        Public Const STATUS_FLOAT_INEXACT_RESULT = &HC000008F
        Public Const STATUS_FLOAT_INVALID_OPERATION = &HC0000090
        Public Const STATUS_FLOAT_OVERFLOW = &HC0000091
        Public Const STATUS_FLOAT_STACK_CHECK = &HC0000092
        Public Const STATUS_FLOAT_UNDERFLOW = &HC0000093
        Public Const STATUS_INTEGER_DIVIDE_BY_ZERO = &HC0000094
        Public Const STATUS_INTEGER_OVERFLOW = &HC0000095
        Public Const STATUS_PRIVILEGED_INSTRUCTION = &HC0000096
        Public Const STATUS_STACK_OVERFLOW = &HC00000FD
        Public Const STATUS_DLL_NOT_FOUND = &HC0000135
        Public Const STATUS_ORDINAL_NOT_FOUND = &HC0000138
        Public Const STATUS_ENTRYPOINT_NOT_FOUND = &HC0000139
        Public Const STATUS_CONTROL_C_EXIT = &HC000013A
        Public Const STATUS_DLL_INIT_FAILED = &HC0000142
        Public Const STATUS_FLOAT_MULTIPLE_FAULTS = &HC00002B4
        Public Const STATUS_FLOAT_MULTIPLE_TRAPS = &HC00002B5
        Public Const STATUS_REG_NAT_CONSUMPTION = &HC00002C9
        Public Const STATUS_HEAP_CORRUPTION = &HC0000374
        Public Const STATUS_STACK_BUFFER_OVERRUN = &HC0000409
        Public Const STATUS_INVALID_CRUNTIME_PARAMETER = &HC0000417
        Public Const STATUS_ASSERTION_FAILURE = &HC0000420
        Public Const STATUS_SXS_EARLY_DEACTIVATION = &HC015000F
        Public Const STATUS_SXS_INVALID_DEACTIVATION = &HC0150010

        ''
        '' Used to represent information related to a thread impersonation
        ''

        Public Const DISABLE_MAX_PRIVILEGE = &H1
        Public Const SANDBOX_INERT = &H2
        Public Const LUA_TOKEN = &H4
        Public Const WRITE_RESTRICTED = &H8

        Public Const OWNER_SECURITY_INFORMATION = (&H1L)
        Public Const GROUP_SECURITY_INFORMATION = (&H2L)
        Public Const DACL_SECURITY_INFORMATION = (&H4L)
        Public Const SACL_SECURITY_INFORMATION = (&H8L)
        Public Const LABEL_SECURITY_INFORMATION = (&H10L)
        Public Const ATTRIBUTE_SECURITY_INFORMATION = (&H20L)
        Public Const SCOPE_SECURITY_INFORMATION = (&H40L)
        Public Const PROCESS_TRUST_LABEL_SECURITY_INFORMATION = (&H80L)
        Public Const BACKUP_SECURITY_INFORMATION = (&H10000L)

        Public Const PROTECTED_DACL_SECURITY_INFORMATION = (&H80000000L)
        Public Const PROTECTED_SACL_SECURITY_INFORMATION = (&H40000000L)
        Public Const UNPROTECTED_DACL_SECURITY_INFORMATION = (&H20000000L)
        Public Const UNPROTECTED_SACL_SECURITY_INFORMATION = (&H10000000L)


        Public Const PROCESS_TERMINATE = (&H1)
        Public Const PROCESS_CREATE_THREAD = (&H2)
        Public Const PROCESS_SET_SESSIONID = (&H4)
        Public Const PROCESS_VM_OPERATION = (&H8)
        Public Const PROCESS_VM_READ = (&H10)
        Public Const PROCESS_VM_WRITE = (&H20)
        Public Const PROCESS_DUP_HANDLE = (&H40)
        Public Const PROCESS_CREATE_PROCESS = (&H80)
        Public Const PROCESS_SET_QUOTA = (&H100)
        Public Const PROCESS_SET_INFORMATION = (&H200)
        Public Const PROCESS_QUERY_INFORMATION = (&H400)
        Public Const PROCESS_SUSPEND_RESUME = (&H800)
        Public Const PROCESS_QUERY_LIMITED_INFORMATION = (&H1000)
        Public Const PROCESS_SET_LIMITED_INFORMATION = (&H2000)
        '' (NTDDI_VERSION >= NTDDI_VISTA) Then
        Public Const PROCESS_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED Or SYNCHRONIZE Or
                                           &HFFFF)
        ''
        '' defined(_WIN64)

        Public ReadOnly Property MAXIMUM_PROC_PER_GROUP As Integer
            Get
                If IntPtr.Size = 8 Then Return 64 Else Return 32
            End Get
        End Property

        ''

        Public ReadOnly MAXIMUM_PROCESSORS As Integer = MAXIMUM_PROC_PER_GROUP

        Public Const THREAD_TERMINATE = (&H1)
        Public Const THREAD_SUSPEND_RESUME = (&H2)
        Public Const THREAD_GET_CONTEXT = (&H8)
        Public Const THREAD_SET_CONTEXT = (&H10)
        Public Const THREAD_QUERY_INFORMATION = (&H40)
        Public Const THREAD_SET_INFORMATION = (&H20)
        Public Const THREAD_SET_THREAD_TOKEN = (&H80)
        Public Const THREAD_IMPERSONATE = (&H100)
        Public Const THREAD_DIRECT_IMPERSONATION = (&H200)
        '' begin_wdm
        Public Const THREAD_SET_LIMITED_INFORMATION = (&H400)  '' winnt
        Public Const THREAD_QUERY_LIMITED_INFORMATION = (&H800)  '' winnt
        Public Const THREAD_RESUME = (&H1000)  '' winnt
        '' (NTDDI_VERSION >= NTDDI_VISTA) Then
        Public Const THREAD_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED Or SYNCHRONIZE Or
                                           &HFFFF)
        ''
        ''
        Public Const JOB_OBJECT_ASSIGN_PROCESS = (&H1)
        Public Const JOB_OBJECT_SET_ATTRIBUTES = (&H2)
        Public Const JOB_OBJECT_QUERY = (&H4)
        Public Const JOB_OBJECT_TERMINATE = (&H8)
        Public Const JOB_OBJECT_SET_SECURITY_ATTRIBUTES = (&H10)
        Public Const JOB_OBJECT_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED Or SYNCHRONIZE Or
                                                &H1F)


        Public Declare Function OpenThread Lib "kernel32" (
                                                          wdDesiredAccess As Integer,
                                                          binheritHandle As Boolean,
                                                          dwThreadId As Integer) As IntPtr


    End Module

End Namespace

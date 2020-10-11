'' ************************************************* ''
'' DataTools Visual Basic Utility Library 
''
'' Module: Native
''         Global Memory Methods.
'' 
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''

Imports System.Runtime.InteropServices
Imports System.Numerics
Imports System.ComponentModel
Imports System.Reflection.Emit
Imports System.Runtime.CompilerServices

Namespace Memory.Internal

#Region "Enumerations and Constants"

    <Flags>
    Public Enum MemoryTypes

        ''' <summary>
        '''  Indicates that the memory pages within the region are mapped into the view of an image section.
        ''' </summary>
        <Description(" Indicates that the memory pages within the region are mapped into the view of an image section.")>
        MEM_IMAGE = &H1000000

        ''' <summary>
        '''  Indicates that the memory pages within the region are mapped into the view of a section.
        ''' </summary>
        <Description(" Indicates that the memory pages within the region are mapped into the view of a section.")>
        MEM_MAPPED = &H40000

        ''' <summary>
        '''  Indicates that the memory pages within the region are private (that is, not shared by other processes
        ''' </summary>
        <Description(" Indicates that the memory pages within the region are private (that is, not shared by other processes")>
        MEM_PRIVATE = &H20000

    End Enum

    <Flags>
    Public Enum MemoryStates

        ''' <summary>
        '''  Indicates committed pages for which physical storage has been allocated, either in memory or in the paging file on disk.
        ''' </summary>
        <Description(" Indicates committed pages for which physical storage has been allocated, either in memory or in the paging file on disk.")>
        MEM_COMMIT = &H1000

        ''' <summary>
        '''  Indicates free pages not accessible to the calling process and available to be allocated. For free pages, the information in the AllocationBase, AllocationProtect, Protect, and Type members is undefined.
        ''' </summary>
        <Description(" Indicates free pages not accessible to the calling process and available to be allocated. For free pages, the information in the AllocationBase, AllocationProtect, Protect, and Type members is undefined.")>
        MEM_FREE = &H10000

        ''' <summary>
        '''  Indicates reserved pages where a range of the process's virtual address space is reserved without any physical storage 
        ''' </summary>
        <Description(" Indicates reserved pages where a range of the process's virtual address space is reserved without any physical storage ")>
        MEM_RESERVE = &H2000

    End Enum

    <Flags>
    Public Enum MemoryProtectionFlags

        ''' <summary>
        ''' Enables execute access to the committed region of pages. An attempt to read from or write to the committed region results in an access violation.
        ''' </summary>
        <Description("Enables execute access to the committed region of pages. An attempt to read from or write to the committed region results in an access violation.")>
        PAGE_EXECUTE = &H10

        ''' <summary>
        ''' Enables execute or read-only access to the committed region of pages. An attempt to write to the committed region results in an access violation.
        ''' </summary>
        <Description("Enables execute or read-only access to the committed region of pages. An attempt to write to the committed region results in an access violation.")>
        PAGE_EXECUTE_READ = &H20

        ''' <summary>
        ''' Enables execute, read-only, or read/write access to the committed region of pages.
        ''' </summary>
        <Description("Enables execute, read-only, or read/write access to the committed region of pages.")>
        PAGE_EXECUTE_READWRITE = &H40

        ''' <summary>
        ''' Enables execute, read-only, or copy-on-write access to a mapped view of a file mapping object. An attempt to write to a committed copy-on-write page results in a private copy of the page being made for the process. The private page is marked as PAGE_EXECUTE_READWRITE, and the change is written to the new page.
        ''' </summary>
        <Description("Enables execute, read-only, or copy-on-write access to a mapped view of a file mapping object. An attempt to write to a committed copy-on-write page results in a private copy of the page being made for the process. The private page is marked as PAGE_EXECUTE_READWRITE, and the change is written to the new page.")>
        PAGE_EXECUTE_WRITECOPY = &H80

        ''' <summary>
        ''' Disables all access to the committed region of pages. An attempt to read from, write to, or execute the committed region results in an access violation.
        ''' </summary>
        <Description("Disables all access to the committed region of pages. An attempt to read from, write to, or execute the committed region results in an access violation.")>
        PAGE_NOACCESS = &H1

        ''' <summary>
        ''' Enables read-only access to the committed region of pages. An attempt to write to the committed region results in an access violation. If Data Execution Prevention is enabled, an attempt to execute code in the committed region results in an access violation.
        ''' </summary>
        <Description("Enables read-only access to the committed region of pages. An attempt to write to the committed region results in an access violation. If Data Execution Prevention is enabled, an attempt to execute code in the committed region results in an access violation.")>
        PAGE_READONLY = &H2

        ''' <summary>
        ''' Enables read-only or read/write access to the committed region of pages. If Data Execution Prevention is enabled, attempting to execute code in the committed region results in an access violation.
        ''' </summary>
        <Description("Enables read-only or read/write access to the committed region of pages. If Data Execution Prevention is enabled, attempting to execute code in the committed region results in an access violation.")>
        PAGE_READWRITE = &H4

        ''' <summary>
        ''' Enables read-only or copy-on-write access to a mapped view of a file mapping object. An attempt to write to a committed copy-on-write page results in a private copy of the page being made for the process. The private page is marked as PAGE_READWRITE, and the change is written to the new page. If Data Execution Prevention is enabled, attempting to execute code in the committed region results in an access violation.
        ''' </summary>
        <Description("Enables read-only or copy-on-write access to a mapped view of a file mapping object. An attempt to write to a committed copy-on-write page results in a private copy of the page being made for the process. The private page is marked as PAGE_READWRITE, and the change is written to the new page. If Data Execution Prevention is enabled, attempting to execute code in the committed region results in an access violation.")>
        PAGE_WRITECOPY = &H8

        ''' <summary>
        ''' Pages in the region become guard pages. Any attempt to access a guard page causes the system to raise a STATUS_GUARD_PAGE_VIOLATION exception and turn off the guard page status. Guard pages thus act as a one-time access alarm. For more information, see Creating Guard Pages.
        ''' </summary>
        <Description("Pages in the region become guard pages. Any attempt to access a guard page causes the system to raise a STATUS_GUARD_PAGE_VIOLATION exception and turn off the guard page status. Guard pages thus act as a one-time access alarm. For more information, see Creating Guard Pages.")>
        PAGE_GUARD = &H100

        ''' <summary>
        ''' Sets all pages to be non-cachable. Applications should not use this attribute except when explicitly required for a device. Using the interlocked functions with memory that is mapped with SEC_NOCACHE can result in an EXCEPTION_ILLEGAL_INSTRUCTION exception.
        ''' </summary>
        <Description("Sets all pages to be non-cachable. Applications should not use this attribute except when explicitly required for a device. Using the interlocked functions with memory that is mapped with SEC_NOCACHE can result in an EXCEPTION_ILLEGAL_INSTRUCTION exception.")>
        PAGE_NOCACHE = &H200

        ''' <summary>
        ''' Sets all pages to be write-combined.
        ''' </summary>
        <Description("Sets all pages to be write-combined.")>
        PAGE_WRITECOMBINE = &H400

    End Enum

    <Flags>
    Public Enum VMemFreeFlags

        ''' <summary>
        ''' Decommits the specified region of committed pages. After the operation, the pages are in the reserved state.
        ''' The function does not fail if you attempt to decommit an uncommitted page. This means that you can decommit a range of pages without first determining the current commitment state.
        ''' Do not use this value with MEM_RELEASE.
        ''' </summary>
        ''' <remarks></remarks>
        <Description("Decommits the specified region of committed pages. After the operation, the pages are in the reserved state.")>
        MEM_DECOMMIT = &H4000

        ''' <summary>
        ''' Releases the specified region of pages. After this operation, the pages are in the free state.
        ''' </summary>
        ''' <remarks></remarks>
        <Description("Releases the specified region of pages. After this operation, the pages are in the free state.")>
        MEM_RELEASE = &H8000

    End Enum

    <Flags>
    Public Enum VMemAllocFlags

        ''' <summary>
        ''' Allocates memory charges (from the overall size of memory and the paging files on disk) for the specified reserved memory pages. The function also guarantees that when the caller later initially accesses the memory, the contents will be zero. Actual physical pages are not allocated unless/until the virtual addresses are actually accessed.
        ''' </summary>
        <Description("Allocates memory charges (from the overall size of memory and the paging files on disk) for the specified reserved memory pages. The function also guarantees that when the caller later initially accesses the memory, the contents will be zero. Actual physical pages are not allocated unless/until the virtual addresses are actually accessed.")>
        MEM_COMMIT = &H1000

        ''' <summary>
        ''' Reserves a range of the process's virtual address space without allocating any actual physical storage in memory or in the paging file on disk.
        ''' </summary>
        <Description("Reserves a range of the process's virtual address space without allocating any actual physical storage in memory or in the paging file on disk.")>
        MEM_RESERVE = &H2000

        ''' <summary>
        ''' Indicates that data in the memory range specified by lpAddress and dwSize is no longer of interest. The pages should not be read from or written to the paging file. However, the memory block will be used again later, so it should not be decommitted. This value cannot be used with any other value.
        ''' </summary>
        <Description("Indicates that data in the memory range specified by lpAddress and dwSize is no longer of interest. The pages should not be read from or written to the paging file. However, the memory block will be used again later, so it should not be decommitted. This value cannot be used with any other value.")>
        MEM_RESET = &H80000

        ''' <summary>
        ''' MEM_RESET_UNDO should only be called on an address range to which MEM_RESET was successfully applied earlier. It indicates that the data in the specified memory range specified by lpAddress and dwSize is of interest to the caller and attempts to reverse the effects of MEM_RESET. If the function succeeds, that means all data in the specified address range is intact. If the function fails, at least some of the data in the address range has been replaced with zeroes.
        ''' </summary>
        <Description("MEM_RESET_UNDO should only be called on an address range to which MEM_RESET was successfully applied earlier. It indicates that the data in the specified memory range specified by lpAddress and dwSize is of interest to the caller and attempts to reverse the effects of MEM_RESET. If the function succeeds, that means all data in the specified address range is intact. If the function fails, at least some of the data in the address range has been replaced with zeroes.")>
        MEM_RESET_UNDO = &H1000000

        ''' <summary>
        ''' Allocates memory using large page support. The size and alignment must be a multiple of the large-page minimum. To obtain this value, use the GetLargePageMinimum function.
        ''' </summary>
        <Description("Allocates memory using large page support. The size and alignment must be a multiple of the large-page minimum. To obtain this value, use the GetLargePageMinimum function.")>
        MEM_LARGE_PAGES = &H20000000

        ''' <summary>
        ''' Reserves an address range that can be used to map Address Windowing Extensions (AWE) pages.
        ''' This value must be used with MEM_RESERVE and no other values.
        ''' </summary>
        <Description("Reserves an address range that can be used to map Address Windowing Extensions (AWE) pages. This value must be used with MEM_RESERVE and no other values.")>
        MEM_PHYSICAL = &H400000

        ''' <summary>
        ''' Allocates memory at the highest possible address. This can be slower than regular allocations, especially when there are many allocations.
        ''' </summary>
        <Description("Allocates memory at the highest possible address. This can be slower than regular allocations, especially when there are many allocations.")>
        MEM_TOP_DOWN = &H100000

        ''' <summary>
        ''' Causes the system to track pages that are written to in the allocated region. If you specify this value, you must also specify MEM_RESERVE.
        ''' </summary>
        <Description("Causes the system to track pages that are written to in the allocated region. If you specify this value, you must also specify MEM_RESERVE.")>
        MEM_WRITE_WATCH = &H200000

    End Enum

#End Region

#Region "Structures"

    <StructLayout(LayoutKind.Sequential)>
    Public Structure MEMORY_BASIC_INFORMATION

        Public BaseAddress As IntPtr
        Public AllocationBase As IntPtr
        Public AllocationProtect As MemoryProtectionFlags
        Public RegionSize As IntPtr
        Public State As MemoryStates
        Public Protect As MemoryProtectionFlags
        Public Type As MemoryTypes

    End Structure

#End Region

#Region "Native Declarations"

    Public Module Native

#Region "Memory Functions"

#Region "NetApi Memory Functions"

        Friend Declare Function NetApiBufferAllocate Lib "netapi32.dll" (ByteCount As Integer, ByRef Buffer As IntPtr) As Integer
        Friend Declare Function NetApiBufferFree Lib "netapi32.dll" (Buffer As IntPtr) As Integer

#End Region

#Region "Virtual Memory Functions"

        Friend Declare Function VirtualAlloc Lib "kernel32" (lpAddress As IntPtr,
                                                              dwSize As IntPtr,
                                                              flAllocationType As VMemAllocFlags,
                                                              flProtect As MemoryProtectionFlags) As IntPtr

        Friend Declare Function VirtualProtect Lib "kernel32" (lpAddress As IntPtr,
                                                               dwSize As IntPtr,
                                                               flNewProtect As MemoryProtectionFlags,
                                                               ByRef flOldProtect As MemoryProtectionFlags) As Boolean

        Friend Declare Function VirtualFree Lib "kernel32" (lpAddress As IntPtr,
                                                             Optional dwSize As IntPtr = Nothing,
                                                             Optional dwFreeType As VMemFreeFlags = VMemFreeFlags.MEM_RELEASE) As Boolean

        Friend Declare Function VirtualQuery Lib "kernel32" (lpAddress As IntPtr,
                                                              ByRef lpBuffer As MEMORY_BASIC_INFORMATION,
                                                              dwLength As IntPtr) As IntPtr

        Friend Declare Function VirtualLock Lib "kernel32" (lpAddress As IntPtr,
                                                             dwSize As IntPtr) As Boolean

        Friend Declare Function VirtualUnlock Lib "kernel32" (lpAddress As IntPtr,
                                                         dwSize As IntPtr) As Boolean

        Friend Declare Function SetProcessWorkingSetSize Lib "kernel32" (hProcess As IntPtr,
                                                                          dwMinimumWorkingSetSize As IntPtr,
                                                                          dwMaximumWorkingSetSize As IntPtr) As Boolean

        Friend Declare Function GetLargePageMinimum Lib "kernel32" () As IntPtr

#End Region

#Region "Heap Functions"

        <DllImport("kernel32", EntryPoint:="HeapCreate", CharSet:=CharSet.Unicode, PreserveSig:=True, SetLastError:=True)>
        Friend Function HeapCreate(dwOptions As Integer,
                                initSize As IntPtr,
                                maxSize As IntPtr) As IntPtr

        End Function

        <DllImport("kernel32", EntryPoint:="HeapDestroy", CharSet:=CharSet.Unicode, PreserveSig:=True, SetLastError:=True)>
        Friend Function HeapDestroy(hHeap As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function


        <DllImport("kernel32", EntryPoint:="GetProcessHeap", CharSet:=CharSet.Unicode, PreserveSig:=True, SetLastError:=True)>
        Friend Function GetProcessHeap() As IntPtr
        End Function

        <DllImport("kernel32", EntryPoint:="HeapQueryInformation", CharSet:=CharSet.Unicode, PreserveSig:=True, SetLastError:=True)>
        Friend Function HeapQueryInformation(HeapHandle As IntPtr,
                                            HeapInformationClass As Integer,
                                            ByRef HeapInformation As ULong,
                                            HeapInformationLength As IntPtr,
                                            ReturnLength As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        '' As per the MSDN manual, we're using ONLY Heap functions, here.

        <DllImport("kernel32", EntryPoint:="HeapAlloc", CharSet:=CharSet.Unicode, PreserveSig:=True, SetLastError:=True)>
        Friend Function HeapAlloc(hHeap As IntPtr, dwOptions As UInteger, dwBytes As IntPtr) As IntPtr
        End Function


        <DllImport("kernel32", EntryPoint:="HeapReAlloc", CharSet:=CharSet.Unicode, PreserveSig:=True, SetLastError:=True)>
        Friend Function HeapReAlloc(hHeap As IntPtr,
                                    dwOptions As Integer,
                                    lpMem As IntPtr,
                                    dwBytes As IntPtr) As IntPtr

        End Function


        <DllImport("kernel32", EntryPoint:="HeapFree", CharSet:=CharSet.Unicode, PreserveSig:=True, SetLastError:=True)>
        Friend Function HeapFree(hHeap As IntPtr, dwOptions As UInteger, lpMem As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function


        <DllImport("kernel32", EntryPoint:="HeapSize", CharSet:=CharSet.Unicode, PreserveSig:=True, SetLastError:=True)>
        Friend Function HeapSize(hHeap As IntPtr, dwOptions As UInteger, lpMem As IntPtr) As IntPtr
        End Function

        <DllImport("kernel32", EntryPoint:="HeapLock", CharSet:=CharSet.Unicode, PreserveSig:=True, SetLastError:=True)>
        Friend Function HeapLock(hHeap As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function


        <DllImport("kernel32", EntryPoint:="HeapUnlock", CharSet:=CharSet.Unicode, PreserveSig:=True, SetLastError:=True)>
        Friend Function HeaUnlock(hHeap As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function


        <DllImport("kernel32", EntryPoint:="HeapValidate", CharSet:=CharSet.Unicode, PreserveSig:=True, SetLastError:=True)>
        Friend Function HeapValidate(hHeap As IntPtr, dwOptions As UInteger, lpMem As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        '' used for specific operating system functions.
        Declare Function LocalFree Lib "kernel32.dll" (hMem As IntPtr) As IntPtr

        '' used for specific operating system functions.
        Declare Function GlobalFree Lib "kernel32.dll" (hMem As IntPtr) As IntPtr

#End Region

#Region "CopyMemory"

#Region "IntPtr Size"

#Region "Object to Pointer"

        'Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (<MarshalAs(UnmanagedType.AsAny)>
        '                                                             pDst As Object,
        '                                                             pSrc As IntPtr,
        '                                                             byteLen As IntPtr)

        'Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
        '                                                             <MarshalAs(UnmanagedType.AsAny)>
        '                                                             pSrc As Object,
        '                                                             byteLen As IntPtr)

#End Region

#Region "Arrays"

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         pSrc As Char(),
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As Char(),
                                                                         pSrc As IntPtr,
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         pSrc As Byte(),
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As Byte(),
                                                                         pSrc As IntPtr,
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         pSrc As SByte(),
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As SByte(),
                                                                         pSrc As IntPtr,
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         pSrc As ULong(),
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As ULong(),
                                                                         pSrc As IntPtr,
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         pSrc As UShort(),
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UShort(),
                                                                         pSrc As IntPtr,
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         pSrc As UInteger(),
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UInteger(),
                                                                         pSrc As IntPtr,
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         pSrc As Long(),
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As Long(),
                                                                         pSrc As IntPtr,
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         pSrc As Short(),
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As Short(),
                                                                         pSrc As IntPtr,
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         pSrc As Integer(),
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As Integer(),
                                                                         pSrc As IntPtr,
                                                                         byteLen As IntPtr)

#End Region

#Region "Scalar Declares"

        '' IntPtr
        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         ByRef pSrc As Byte,
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As Byte,
                                                                         pSrc As IntPtr,
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         ByRef pSrc As SByte,
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As SByte,
                                                                         pSrc As IntPtr,
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         ByRef pSrc As ULong,
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As ULong,
                                                                         pSrc As IntPtr,
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         ByRef pSrc As UShort,
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As UShort,
                                                                         pSrc As IntPtr,
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         ByRef pSrc As UInteger,
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As UInteger,
                                                                         pSrc As IntPtr,
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         ByRef pSrc As Long,
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As Long,
                                                                         pSrc As IntPtr,
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         ByRef pSrc As Short,
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As Short,
                                                                         pSrc As IntPtr,
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         ByRef pSrc As Char,
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As Char,
                                                                         pSrc As IntPtr,
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         ByRef pSrc As Integer,
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As Integer,
                                                                         pSrc As IntPtr,
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         <MarshalAs(UnmanagedType.LPWStr)> pSrc As String,
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (<MarshalAs(UnmanagedType.LPWStr)> pDst As String,
                                                                         pSrc As IntPtr,
                                                                         byteLen As IntPtr)

#End Region

#Region "Unique Types"

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         ByRef pSrc As Date,
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As Date,
                                                                         pSrc As IntPtr,
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         pSrc As Date(),
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As Date(),
                                                                         pSrc As IntPtr,
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         ByRef pSrc As System.Guid,
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As System.Guid,
                                                                         pSrc As IntPtr,
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         pSrc As System.Guid(),
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As System.Guid(),
                                                                         pSrc As IntPtr,
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         pSrc As BigInteger,
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As BigInteger,
                                                                         pSrc As IntPtr,
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         pSrc As System.Drawing.Color,
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As System.Drawing.Color,
                                                                         pSrc As IntPtr,
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         pSrc As System.Drawing.Color(),
                                                                         byteLen As IntPtr)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As System.Drawing.Color(),
                                                                         pSrc As IntPtr,
                                                                         byteLen As IntPtr)

#End Region

#End Region

#Region "Long Size"

#Region "Pointer to Pointer"

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                     pSrc As IntPtr,
                                                                     ByteLen As IntPtr)
#End Region

#Region "Object to Pointer"

        'Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (<MarshalAs(UnmanagedType.AsAny)>
        '                                                             pDst As Object,
        '                                                             pSrc As IntPtr,
        '                                                             ByteLen As Long)

        'Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
        '                                                             <MarshalAs(UnmanagedType.AsAny)>
        '                                                             pSrc As Object,
        '                                                             ByteLen As Long)

#End Region

#Region "Arrays"

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         pSrc As Char(),
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As Char(),
                                                                         pSrc As IntPtr,
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         pSrc As Byte(),
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As Byte(),
                                                                         pSrc As IntPtr,
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         pSrc As SByte(),
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As SByte(),
                                                                         pSrc As IntPtr,
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         pSrc As ULong(),
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As ULong(),
                                                                         pSrc As IntPtr,
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         pSrc As UShort(),
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UShort(),
                                                                         pSrc As IntPtr,
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         pSrc As UInteger(),
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UInteger(),
                                                                         pSrc As IntPtr,
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         pSrc As Long(),
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As Long(),
                                                                         pSrc As IntPtr,
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         pSrc As Short(),
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As Short(),
                                                                         pSrc As IntPtr,
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         pSrc As Integer(),
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As Integer(),
                                                                         pSrc As IntPtr,
                                                                         ByteLen As Long)

#End Region

#Region "Scalar Declares"

        '' IntPtr
        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         ByRef pSrc As Byte,
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As Byte,
                                                                         pSrc As IntPtr,
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         ByRef pSrc As SByte,
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As SByte,
                                                                         pSrc As IntPtr,
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         ByRef pSrc As ULong,
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As ULong,
                                                                         pSrc As IntPtr,
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         ByRef pSrc As UShort,
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As UShort,
                                                                         pSrc As IntPtr,
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         ByRef pSrc As UInteger,
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As UInteger,
                                                                         pSrc As IntPtr,
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         ByRef pSrc As Long,
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As Long,
                                                                         pSrc As IntPtr,
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         ByRef pSrc As Short,
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As Short,
                                                                         pSrc As IntPtr,
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         ByRef pSrc As Char,
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As Char,
                                                                         pSrc As IntPtr,
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         ByRef pSrc As Integer,
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As Integer,
                                                                         pSrc As IntPtr,
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         <MarshalAs(UnmanagedType.LPWStr)> pSrc As String,
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (<MarshalAs(UnmanagedType.LPWStr)> pDst As String,
                                                                         pSrc As IntPtr,
                                                                         ByteLen As Long)

#End Region

#Region "Unique Types"

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         ByRef pSrc As Date,
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As Date,
                                                                         pSrc As IntPtr,
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         pSrc As Date(),
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As Date(),
                                                                         pSrc As IntPtr,
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         ByRef pSrc As System.Guid,
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As System.Guid,
                                                                         pSrc As IntPtr,
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         pSrc As System.Guid(),
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As System.Guid(),
                                                                         pSrc As IntPtr,
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         pSrc As BigInteger,
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As BigInteger,
                                                                         pSrc As IntPtr,
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         pSrc As System.Drawing.Color,
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As System.Drawing.Color,
                                                                         pSrc As IntPtr,
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
                                                                         pSrc As System.Drawing.Color(),
                                                                         ByteLen As Long)

        Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As System.Drawing.Color(),
                                                                         pSrc As IntPtr,
                                                                         ByteLen As Long)

#End Region

#End Region

#End Region

#End Region

#Region "Dynamic IL Code Memory Copy Functions"

        Private dyngenlist As System.Type() =
            {
                GetType(Byte),
                GetType(SByte),
                GetType(Char),
                GetType(Short),
                GetType(UShort),
                GetType(Integer),
                GetType(UInteger),
                GetType(Long),
                GetType(ULong),
                GetType(Single),
                GetType(Double),
                GetType(Decimal),
                GetType(System.Guid),
                GetType(Date)
                }

        Private dyngensizes As Integer() = {1, 1, 2, 2, 2, 4, 4, 8, 8, 4, 8, 16, 16, 8}

        Friend MethodList As New List(Of MulticastDelegate)

        ' main MemCpy IL function

        ''' <summary>
        ''' IL Code: Copies memory from one memory pointer to another using IL cpblk.
        ''' </summary>
        ''' <param name="dest">The destination pointer.</param>
        ''' <param name="src">The source pointer.</param>
        ''' <param name="byteLen">The number of bytes to copy.</param>
        ''' <remarks></remarks>
        Public Delegate Sub MemCpyFunc(dest As IntPtr, src As IntPtr, byteLen As UInteger)

        ''' <summary>
        ''' IL Code: Scans for a null-terminated string starting at the specified pointer.
        ''' </summary>
        ''' <param name="p">Pointer in memory at which to begin scanning.</param>
        ''' <returns>The zero-based character index of the null character.</returns>
        ''' <remarks></remarks>
        Public Delegate Function StrZeroF(p As IntPtr) As String

        ''' <summary>
        ''' IL Code: Scans for a null byte starting at the specified pointer.
        ''' </summary>
        ''' <param name="p">Pointer in memory at which to begin scanning.</param>
        ''' <returns>The zero-based byte index of the null byte.</returns>
        ''' <remarks></remarks>
        Public Delegate Function ByteZeroF(p As IntPtr) As Integer

        ''' <summary>
        ''' IL Code: Returns a null-terminated Unicode string starting at the specified pointer.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly StrZero As StrZeroF

        ''' <summary>
        ''' IL Code: Copies memory from one memory pointer to another using IL cpblk.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly MemCpyDyn As MemCpyFunc

        Public Delegate Sub ILCopySinglet(Of T)(ByRef dest As T, ByRef src As T)

        Public Delegate Sub ILCopyTo(Of T)(ByRef dest As T, src As IntPtr)
        Public Delegate Sub ILCopyFrom(Of T)(dest As IntPtr, ByRef src As T)

        Public Delegate Sub ILCopyToArr(Of T)(dest() As T, src As IntPtr, len As UInteger)
        Public Delegate Sub ILCopyFromArr(Of T)(dest As IntPtr, src() As T, len As UInteger)

        '' Dynamically-created pure Intermediate Language delegates for faster memory handling
        '' Delegate creation is handled in Interop.vb.  The delegates are
        '' stored in a list of MulticastDelegate.  The pure-IL execution
        '' has shown a dramatic increase in performance, and is as good as 
        '' it can possibly get in VB.
#Region "IL Delegates"

        ' singles

        Public ReadOnly charAtget As ILCopyTo(Of Char)
        Public ReadOnly charAtset As ILCopyFrom(Of Char)

        Public ReadOnly byteAtget As ILCopyTo(Of Byte)
        Public ReadOnly byteAtset As ILCopyFrom(Of Byte)

        Public ReadOnly sbyteAtget As ILCopyTo(Of SByte)
        Public ReadOnly sbyteAtset As ILCopyFrom(Of SByte)

        Public ReadOnly shortAtget As ILCopyTo(Of Short)
        Public ReadOnly shortAtset As ILCopyFrom(Of Short)

        Public ReadOnly ushortAtget As ILCopyTo(Of UShort)
        Public ReadOnly ushortAtset As ILCopyFrom(Of UShort)

        Public ReadOnly intAtget As ILCopyTo(Of Integer)
        Public ReadOnly intAtset As ILCopyFrom(Of Integer)

        Public ReadOnly uintAtget As ILCopyTo(Of UInteger)
        Public ReadOnly uintAtset As ILCopyFrom(Of UInteger)

        Public ReadOnly longAtget As ILCopyTo(Of Long)
        Public ReadOnly longAtset As ILCopyFrom(Of Long)

        Public ReadOnly ulongAtget As ILCopyTo(Of ULong)
        Public ReadOnly ulongAtset As ILCopyFrom(Of ULong)

        Public ReadOnly singleAtget As ILCopyTo(Of Single)
        Public ReadOnly singleAtset As ILCopyFrom(Of Single)

        Public ReadOnly doubleAtget As ILCopyTo(Of Double)
        Public ReadOnly doubleAtset As ILCopyFrom(Of Double)

        Public ReadOnly guidAtget As ILCopyTo(Of Guid)
        Public ReadOnly guidAtset As ILCopyFrom(Of Guid)

        Public ReadOnly decimalAtget As ILCopyTo(Of Decimal)
        Public ReadOnly decimalAtset As ILCopyFrom(Of Decimal)

        Public ReadOnly dateAtget As ILCopyTo(Of Date)
        Public ReadOnly dateAtset As ILCopyFrom(Of Date)

        ' arrays

        Public ReadOnly charArrAtget As ILCopyToArr(Of Char)
        Public ReadOnly charArrAtset As ILCopyFromArr(Of Char)

        Public ReadOnly byteArrAtget As ILCopyToArr(Of Byte)
        Public ReadOnly byteArrAtset As ILCopyFromArr(Of Byte)

        Public ReadOnly sbyteArrAtget As ILCopyToArr(Of SByte)
        Public ReadOnly sbyteArrAtset As ILCopyFromArr(Of SByte)

        Public ReadOnly shortArrAtget As ILCopyToArr(Of Short)
        Public ReadOnly shortArrAtset As ILCopyFromArr(Of Short)

        Public ReadOnly ushortArrAtget As ILCopyToArr(Of UShort)
        Public ReadOnly ushortArrAtset As ILCopyFromArr(Of UShort)

        Public ReadOnly intArrAtget As ILCopyToArr(Of Integer)
        Public ReadOnly intArrAtset As ILCopyFromArr(Of Integer)

        Public ReadOnly uintArrAtget As ILCopyToArr(Of UInteger)
        Public ReadOnly uintArrAtset As ILCopyFromArr(Of UInteger)

        Public ReadOnly longArrAtget As ILCopyToArr(Of Long)
        Public ReadOnly longArrAtset As ILCopyFromArr(Of Long)

        Public ReadOnly ulongArrAtget As ILCopyToArr(Of ULong)
        Public ReadOnly ulongArrAtset As ILCopyFromArr(Of ULong)

        Public ReadOnly singleArrAtget As ILCopyToArr(Of Single)
        Public ReadOnly singleArrAtset As ILCopyFromArr(Of Single)

        Public ReadOnly doubleArrAtget As ILCopyToArr(Of Double)
        Public ReadOnly doubleArrAtset As ILCopyFromArr(Of Double)

        Public ReadOnly guidArrAtget As ILCopyToArr(Of Guid)
        Public ReadOnly guidArrAtset As ILCopyFromArr(Of Guid)

        Public ReadOnly decimalArrAtget As ILCopyToArr(Of Decimal)
        Public ReadOnly decimalArrAtset As ILCopyFromArr(Of Decimal)

        Public ReadOnly dateArrAtget As ILCopyToArr(Of Date)
        Public ReadOnly dateArrAtset As ILCopyFromArr(Of Date)

#End Region '' IL Delegates

        Sub New()

            ' create the canonical pointer-to-pointer copy method.
            Dim dynMtd As New DynamicMethod _
                    (
                        "MemCpy",
                        GetType(Void),
                        {GetType(IntPtr), GetType(IntPtr), GetType(UInteger)}, GetType(Native)
                    )

            Dim ilGen As ILGenerator = dynMtd.GetILGenerator()

            ilGen.Emit(OpCodes.Ldarg_0)
            ilGen.Emit(OpCodes.Ldarg_1)
            ilGen.Emit(OpCodes.Ldarg_2)

            ilGen.Emit(OpCodes.Volatile)
            If (IntPtr.Size = 8) Then ilGen.Emit(OpCodes.Unaligned, 1)
            ilGen.Emit(OpCodes.Cpblk)
            ilGen.Emit(OpCodes.Ret)

            MemCpyDyn = CType(dynMtd.CreateDelegate(GetType(MemCpyFunc)), MemCpyFunc)

            '' String length function

            Dim lb As Label,
                lb2 As Label

            '' Return a null-terminated Unicode string from a pointer.

            dynMtd = New DynamicMethod _
            (
                "StrZero",
                GetType(String),
                {GetType(IntPtr)}, GetType(Native)
            )

            ilGen = dynMtd.GetILGenerator()

            If IntPtr.Size = 8 Then
                ilGen.DeclareLocal(GetType(Long))
            Else
                ilGen.DeclareLocal(GetType(Integer))
            End If

            ilGen.DeclareLocal(GetType(UShort), True)
            ilGen.DeclareLocal(GetType(Integer))
            ilGen.DeclareLocal(GetType(String), True)

            lb = ilGen.DefineLabel
            lb2 = ilGen.DefineLabel
            Dim lb3 As Label = ilGen.DefineLabel

            ilGen.Emit(OpCodes.Ldstr, "")
            ilGen.Emit(OpCodes.Stloc_3)

            ilGen.Emit(OpCodes.Ldarg_0)

            If IntPtr.Size = 8 Then
                ilGen.Emit(OpCodes.Conv_I8)
            Else
                ilGen.Emit(OpCodes.Conv_I4)
            End If

            ilGen.Emit(OpCodes.Stloc_0)
            ilGen.Emit(OpCodes.Ldloc_0)

            ilGen.Emit(OpCodes.Ldc_I4_0)
            ilGen.Emit(OpCodes.Stloc_1)

            ilGen.Emit(OpCodes.Ldc_I4_0)
            ilGen.Emit(OpCodes.Stloc_1)

            ilGen.MarkLabel(lb)

            ilGen.Emit(OpCodes.Ldloca, 1)
            ilGen.Emit(OpCodes.Ldloc_0)
            ilGen.Emit(OpCodes.Ldc_I4_2)

            ilGen.Emit(OpCodes.Cpblk)

            ilGen.Emit(OpCodes.Ldc_I4_0)
            ilGen.Emit(OpCodes.Ldloc_1)

            ilGen.Emit(OpCodes.Beq, lb2)

            ilGen.Emit(OpCodes.Ldloc_0)

            If IntPtr.Size = 8 Then
                ilGen.Emit(OpCodes.Ldc_I8, CLng(2))
            Else
                ilGen.Emit(OpCodes.Ldc_I4_2)
            End If

            ilGen.Emit(OpCodes.Add)
            ilGen.Emit(OpCodes.Stloc_0)

            ilGen.Emit(OpCodes.Br, lb)

            ilGen.MarkLabel(lb2)

            ilGen.Emit(OpCodes.Ldloc_0)
            ilGen.Emit(OpCodes.Ldarg_0)
            ilGen.Emit(OpCodes.Sub)
            ilGen.Emit(OpCodes.Stloc_2)
            ilGen.Emit(OpCodes.Ldloc_2)
            ilGen.Emit(OpCodes.Ldc_I4_0)
            ilGen.Emit(OpCodes.Beq, lb3)

            ilGen.Emit(OpCodes.Ldc_I4_0)
            ilGen.Emit(OpCodes.Ldloc_2)
            ilGen.Emit(OpCodes.Ldc_I4_1)
            ilGen.Emit(OpCodes.Shr)

            ilGen.Emit(OpCodes.Newobj, GetType(String).GetConstructor({GetType(Char), GetType(Integer)}))
            ilGen.Emit(OpCodes.Stloc_3)
            ilGen.Emit(OpCodes.Ldloc_3)
            ilGen.Emit(OpCodes.Ldc_I4, 0)
            ilGen.Emit(OpCodes.Ldelema, GetType(Char))

            ' I have no idea why this works, but for 64-bit systems, subtract 4 from the pointer
            If IntPtr.Size = 8 Then
                ilGen.Emit(OpCodes.Ldc_I4_4)
                ilGen.Emit(OpCodes.Sub)
            End If

            ilGen.Emit(OpCodes.Ldarg_0)
            ilGen.Emit(OpCodes.Ldloc_2)

            ilGen.Emit(OpCodes.Volatile)
            If (IntPtr.Size = 8) Then ilGen.Emit(OpCodes.Unaligned, 1)

            ilGen.Emit(OpCodes.Cpblk)

            ilGen.MarkLabel(lb3)

            ilGen.Emit(OpCodes.Ldloc_3)
            ilGen.Emit(OpCodes.Ret)

            StrZero = CType(dynMtd.CreateDelegate(GetType(StrZeroF)), StrZeroF)


            ' initialize our dynamic methods.
            genDynList()

            '' Initialize the delegates

            charAtget = CType(FindDelegateTo(Of Char)(), ILCopyTo(Of Char))
            charAtset = CType(FindDelegateFrom(Of Char)(), ILCopyFrom(Of Char))

            byteAtget = CType(FindDelegateTo(Of Byte)(), ILCopyTo(Of Byte))
            byteAtset = CType(FindDelegateFrom(Of Byte)(), ILCopyFrom(Of Byte))

            sbyteAtget = CType(FindDelegateTo(Of SByte)(), ILCopyTo(Of SByte))
            sbyteAtset = CType(FindDelegateFrom(Of SByte)(), ILCopyFrom(Of SByte))

            shortAtget = CType(FindDelegateTo(Of Short)(), ILCopyTo(Of Short))
            shortAtset = CType(FindDelegateFrom(Of Short)(), ILCopyFrom(Of Short))

            ushortAtget = CType(FindDelegateTo(Of UShort)(), ILCopyTo(Of UShort))
            ushortAtset = CType(FindDelegateFrom(Of UShort)(), ILCopyFrom(Of UShort))

            intAtget = CType(FindDelegateTo(Of Integer)(), ILCopyTo(Of Integer))
            intAtset = CType(FindDelegateFrom(Of Integer)(), ILCopyFrom(Of Integer))

            uintAtget = CType(FindDelegateTo(Of UInteger)(), ILCopyTo(Of UInteger))
            uintAtset = CType(FindDelegateFrom(Of UInteger)(), ILCopyFrom(Of UInteger))

            longAtget = CType(FindDelegateTo(Of Long)(), ILCopyTo(Of Long))
            longAtset = CType(FindDelegateFrom(Of Long)(), ILCopyFrom(Of Long))

            ulongAtget = CType(FindDelegateTo(Of ULong)(), ILCopyTo(Of ULong))
            ulongAtset = CType(FindDelegateFrom(Of ULong)(), ILCopyFrom(Of ULong))

            singleAtget = CType(FindDelegateTo(Of Single)(), ILCopyTo(Of Single))
            singleAtset = CType(FindDelegateFrom(Of Single)(), ILCopyFrom(Of Single))

            doubleAtget = CType(FindDelegateTo(Of Double)(), ILCopyTo(Of Double))
            doubleAtset = CType(FindDelegateFrom(Of Double)(), ILCopyFrom(Of Double))

            guidAtget = CType(FindDelegateTo(Of Guid)(), ILCopyTo(Of Guid))
            guidAtset = CType(FindDelegateFrom(Of Guid)(), ILCopyFrom(Of Guid))

            decimalAtget = CType(FindDelegateTo(Of Decimal)(), ILCopyTo(Of Decimal))
            decimalAtset = CType(FindDelegateFrom(Of Decimal)(), ILCopyFrom(Of Decimal))

            dateAtget = CType(FindDelegateTo(Of Date)(), ILCopyTo(Of Date))
            dateAtset = CType(FindDelegateFrom(Of Date)(), ILCopyFrom(Of Date))

            ' arrays

            charArrAtget = CType(FindDelegateTo(Of Char())(), ILCopyToArr(Of Char))
            charArrAtset = CType(FindDelegateFrom(Of Char())(), ILCopyFromArr(Of Char))

            byteArrAtget = CType(FindDelegateTo(Of Byte())(), ILCopyToArr(Of Byte))
            byteArrAtset = CType(FindDelegateFrom(Of Byte())(), ILCopyFromArr(Of Byte))

            sbyteArrAtget = CType(FindDelegateTo(Of SByte())(), ILCopyToArr(Of SByte))
            sbyteArrAtset = CType(FindDelegateFrom(Of SByte())(), ILCopyFromArr(Of SByte))

            shortArrAtget = CType(FindDelegateTo(Of Short())(), ILCopyToArr(Of Short))
            shortArrAtset = CType(FindDelegateFrom(Of Short())(), ILCopyFromArr(Of Short))

            ushortArrAtget = CType(FindDelegateTo(Of UShort())(), ILCopyToArr(Of UShort))
            ushortArrAtset = CType(FindDelegateFrom(Of UShort())(), ILCopyFromArr(Of UShort))

            intArrAtget = CType(FindDelegateTo(Of Integer())(), ILCopyToArr(Of Integer))
            intArrAtset = CType(FindDelegateFrom(Of Integer())(), ILCopyFromArr(Of Integer))

            uintArrAtget = CType(FindDelegateTo(Of UInteger())(), ILCopyToArr(Of UInteger))
            uintArrAtset = CType(FindDelegateFrom(Of UInteger())(), ILCopyFromArr(Of UInteger))

            longArrAtget = CType(FindDelegateTo(Of Long())(), ILCopyToArr(Of Long))
            longArrAtset = CType(FindDelegateFrom(Of Long())(), ILCopyFromArr(Of Long))

            ulongArrAtget = CType(FindDelegateTo(Of ULong())(), ILCopyToArr(Of ULong))
            ulongArrAtset = CType(FindDelegateFrom(Of ULong())(), ILCopyFromArr(Of ULong))

            singleArrAtget = CType(FindDelegateTo(Of Single())(), ILCopyToArr(Of Single))
            singleArrAtset = CType(FindDelegateFrom(Of Single())(), ILCopyFromArr(Of Single))

            doubleArrAtget = CType(FindDelegateTo(Of Double())(), ILCopyToArr(Of Double))
            doubleArrAtset = CType(FindDelegateFrom(Of Double())(), ILCopyFromArr(Of Double))

            guidArrAtget = CType(FindDelegateTo(Of Guid())(), ILCopyToArr(Of Guid))
            guidArrAtset = CType(FindDelegateFrom(Of Guid())(), ILCopyFromArr(Of Guid))

            decimalArrAtget = CType(FindDelegateTo(Of Decimal())(), ILCopyToArr(Of Decimal))
            decimalArrAtset = CType(FindDelegateFrom(Of Decimal())(), ILCopyFromArr(Of Decimal))

            dateArrAtget = CType(FindDelegateTo(Of Date())(), ILCopyToArr(Of Date))
            dateArrAtset = CType(FindDelegateFrom(Of Date())(), ILCopyFromArr(Of Date))



        End Sub

        Private Sub genDynList()

            Dim i As Integer,
                c As Integer

            Dim l As New List(Of System.Type)
            Dim il As New List(Of Integer)

            Dim j As Integer

            c = dyngenlist.Length - 1

            For i = 0 To c
                l.Add(Type.GetType(dyngenlist(i).FullName + "&"))
                il.Add(dyngensizes(i))
            Next

            For i = 0 To c
                l.Add(Type.GetType(dyngenlist(i).FullName + "[]"))
                il.Add(dyngensizes(i))
            Next

            Dim dynMtd As DynamicMethod
            Dim ilGen As ILGenerator
            Dim toptr As String,
                fromptr As String

            Dim ir As Boolean = False

            j = 0

            For Each tn As System.Type In l

                If tn.IsArray Then

                    toptr = "mem" & tn.Name.Substring(0, tn.Name.Length - 2) & "arr"
                    fromptr = tn.Name.Substring(0, tn.Name.Length - 2) & "mem" & "arr"

                    dynMtd = New DynamicMethod _
                                (
                                    toptr,
                                    GetType(Void),
                                    {tn, GetType(IntPtr), GetType(UInteger)}, GetType(Native)
                                )

                    ilGen = dynMtd.GetILGenerator()

                    ilGen.Emit(OpCodes.Ldarg_0)
                    ilGen.Emit(OpCodes.Ldc_I4_0)
                    ilGen.Emit(OpCodes.Ldelema, tn.GetElementType)

                    ilGen.Emit(OpCodes.Ldarg_1)

                    ilGen.Emit(OpCodes.Ldarg_2)
                    ilGen.Emit(OpCodes.Volatile)
                    If (IntPtr.Size = 8) Then ilGen.Emit(OpCodes.Unaligned, 1)
                    ilGen.Emit(OpCodes.Cpblk)

                    ilGen.Emit(OpCodes.Ret)

                    MethodList.Add(dynMtd.CreateDelegate(GetType(ILCopyToArr(Of )).MakeGenericType(Type.GetType(tn.FullName.Substring(0, tn.FullName.Length - 2)))))

                    dynMtd = New DynamicMethod _
                                (
                                    fromptr,
                                    GetType(Void),
                                    {GetType(IntPtr), tn, GetType(UInteger)}, GetType(Native)
                                )

                    ilGen = dynMtd.GetILGenerator()

                    ilGen.Emit(OpCodes.Ldarg_0)

                    ilGen.Emit(OpCodes.Ldarg_1)
                    ilGen.Emit(OpCodes.Ldc_I4_0)
                    ilGen.Emit(OpCodes.Ldelema, tn.GetElementType)

                    ilGen.Emit(OpCodes.Ldarg_2)
                    ilGen.Emit(OpCodes.Volatile)
                    If (IntPtr.Size = 8) Then ilGen.Emit(OpCodes.Unaligned, 1)
                    ilGen.Emit(OpCodes.Cpblk)

                    ilGen.Emit(OpCodes.Ret)

                    MethodList.Add(dynMtd.CreateDelegate(GetType(ILCopyFromArr(Of )).MakeGenericType(Type.GetType(tn.FullName.Substring(0, tn.FullName.Length - 2)))))


                Else

                    toptr = "mem" & tn.Name.Substring(0, tn.Name.Length - 1)
                    fromptr = tn.Name.Substring(0, tn.Name.Length - 1) & "mem"

                    dynMtd = New DynamicMethod _
                                (
                                    toptr,
                                    GetType(Void),
                                    {tn, GetType(IntPtr)}, GetType(Native)
                                )

                    ilGen = dynMtd.GetILGenerator()

                    ilGen.Emit(OpCodes.Ldarg_0)
                    ilGen.Emit(OpCodes.Ldarg_1)

                    EmitOpCodeForByteLength(ilGen, il(j))

                    ilGen.Emit(OpCodes.Volatile)
                    If (IntPtr.Size = 8) Then ilGen.Emit(OpCodes.Unaligned, 1)
                    ilGen.Emit(OpCodes.Cpblk)

                    ilGen.Emit(OpCodes.Ret)

                    MethodList.Add(dynMtd.CreateDelegate(GetType(ILCopyTo(Of )).MakeGenericType(Type.GetType(tn.FullName.Substring(0, tn.FullName.Length - 1)))))

                    dynMtd = New DynamicMethod _
                                (
                                    fromptr,
                                    GetType(Void),
                                    {GetType(IntPtr), tn}, GetType(Native)
                                )

                    ilGen = dynMtd.GetILGenerator()

                    ilGen.Emit(OpCodes.Ldarg_0)
                    ilGen.Emit(OpCodes.Ldarg_1)

                    EmitOpCodeForByteLength(ilGen, il(j))

                    ilGen.Emit(OpCodes.Volatile)

                    If (IntPtr.Size = 8) Then ilGen.Emit(OpCodes.Unaligned, 1)

                    ilGen.Emit(OpCodes.Cpblk)

                    ilGen.Emit(OpCodes.Ret)

                    MethodList.Add(dynMtd.CreateDelegate(GetType(ILCopyFrom(Of )).MakeGenericType(Type.GetType(tn.FullName.Substring(0, tn.FullName.Length - 1)))))

                End If

                j += 1
            Next

        End Sub

        Private Sub EmitOpCodeForByteLength(il As ILGenerator, l As Integer)
            Select Case l

                Case 1
                    il.Emit(OpCodes.Ldc_I4_1)

                Case 2
                    il.Emit(OpCodes.Ldc_I4_2)

                Case 3
                    il.Emit(OpCodes.Ldc_I4_3)

                Case 4
                    il.Emit(OpCodes.Ldc_I4_4)

                Case 5
                    il.Emit(OpCodes.Ldc_I4_5)

                Case 6
                    il.Emit(OpCodes.Ldc_I4_6)

                Case 7
                    il.Emit(OpCodes.Ldc_I4_7)

                Case 8
                    il.Emit(OpCodes.Ldc_I4_8)

                Case Else
                    il.Emit(OpCodes.Ldc_I4, l)

            End Select
        End Sub

        Public Sub CopyToIL(Of T)(ByRef dest As T, src As IntPtr)
            CType(FindDelegateTo(Of T)(), ILCopyTo(Of T)).Invoke(dest, src)
        End Sub

        Public Sub CopyFromIL(Of T)(dest As IntPtr, ByRef src As T)
            CType(FindDelegateFrom(Of T)(), ILCopyFrom(Of T)).Invoke(dest, src)
        End Sub

        Public Sub CopyToILArray(Of T)(dest As T(), src As IntPtr, l As UInteger)
            CType(FindDelegateTo(Of T())(), ILCopyToArr(Of T)).Invoke(dest, src, l)
        End Sub

        Public Sub CopyFromILArray(Of T)(dest As IntPtr, src As T(), l As UInteger)
            CType(FindDelegateFrom(Of T())(), ILCopyFromArr(Of T)).Invoke(dest, src, l)
        End Sub

        Public Function FindDelegateTo(Of T)() As MulticastDelegate

            Dim tn As System.Type = GetType(T)

            Dim s As String
            If tn.IsArray Then
                s = "mem" & tn.Name.Substring(0, tn.Name.Length - 2) & "arr"
            Else
                s = "mem" & tn.Name
            End If

            For Each mt In MethodList
                If mt.Method.Name = s Then
                    Return mt
                End If
            Next

            Return Nothing
        End Function

        Public Function FindDelegateFrom(Of T)() As MulticastDelegate

            Dim tn As System.Type = GetType(T)

            Dim s As String
            If tn.IsArray Then
                s = tn.Name.Substring(0, tn.Name.Length - 2) & "mem" & "arr"
            Else
                s = tn.Name & "mem"
            End If

            For Each mt In MethodList
                If mt.Method.Name = s Then
                    Return mt
                End If
            Next

            Return Nothing
        End Function

        ''' <summary>
        ''' Determine whether an object is blittable and can be copied to a memory location.
        ''' </summary>
        ''' <param name="obj"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CanCopyObject(obj As Object) As Boolean
            Dim gs As GCHandle = Nothing
            CanCopyObject = internalCanCopyObject(obj, gs)
            If CanCopyObject Then gs.Free()
        End Function

        ''' <summary>
        ''' Internal can-copy function.  Saves time by returning the allocated GCHandle.
        ''' </summary>
        ''' <param name="obj"></param>
        ''' <param name="g"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function internalCanCopyObject(obj As Object, ByRef g As GCHandle) As Boolean
            Dim gs As GCHandle = Nothing

            Try
                gs = GCHandle.Alloc(obj, GCHandleType.Pinned)
                Dim ptr As IntPtr = gs.AddrOfPinnedObject
                g = gs
                Return True
            Catch ex As Exception
                If gs.IsAllocated Then
                    gs.Free()
                End If

                g = New GCHandle
                Return False
            End Try

        End Function

        ''' <summary>
        ''' Copy the contents of a memory location into a blittable object.
        ''' </summary>
        ''' <param name="dest"></param>
        ''' <param name="src"></param>
        ''' <param name="byteLen"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CopyObject(dest As Object, src As IntPtr, byteLen As UInteger) As Boolean
            Dim gs As New GCHandle

            If Not internalCanCopyObject(dest, gs) Then Return False
            Dim ptr As IntPtr = gs.AddrOfPinnedObject

            MemCpy(ptr, src, byteLen)
            gs.Free()

            Return True
        End Function

        ''' <summary>
        ''' Copy the contents of a blittable object into a memory location.
        ''' </summary>
        ''' <param name="dest"></param>
        ''' <param name="src"></param>
        ''' <param name="byteLen"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CopyObject(dest As IntPtr, src As Object, byteLen As UInteger) As Boolean
            Dim gs As New GCHandle

            If Not internalCanCopyObject(src, gs) Then Return False
            Dim ptr As IntPtr = gs.AddrOfPinnedObject

            MemCpy(dest, ptr, byteLen)
            gs.Free()

            Return True
        End Function

        ''' <summary>
        ''' Copy the contents of a memory location into a blittable object.  No checking is done.
        ''' </summary>
        ''' <param name="dest"></param>
        ''' <param name="src"></param>
        ''' <param name="byteLen"></param>
        ''' <remarks></remarks>
        Public Sub QuickCopyObject(Of T)(ByRef dest As T, src As IntPtr, byteLen As UInteger)
            Dim gs As GCHandle = GCHandle.Alloc(dest, GCHandleType.Pinned)
            Dim ptr As IntPtr = gs.AddrOfPinnedObject

            MemCpy(ptr, src, byteLen)
            gs.Free()
        End Sub

        ''' <summary>
        ''' Copy the contents of a blittable object into a memory location.  No checking is done.
        ''' </summary>
        ''' <param name="dest"></param>
        ''' <param name="src"></param>
        ''' <param name="byteLen"></param>
        ''' <remarks></remarks>
        Public Sub QuickCopyObject(Of T)(dest As IntPtr, src As T, byteLen As UInteger)
            Dim gs As GCHandle = GCHandle.Alloc(src, GCHandleType.Pinned)
            Dim ptr As IntPtr = gs.AddrOfPinnedObject

            MemCpy(dest, ptr, byteLen)
            gs.Free()
        End Sub

#End Region

#Region "Zero Memory"

        Sub RtlZeroMemory(dest As IntPtr, count As IntPtr)
            If count.ToInt64 And &HFFFFFFFF00000000L Then
                n_memset(dest, 0, count)
            Else
                MemSet(dest, 0, count)
            End If
        End Sub

#End Region

#Region "Native Memcpy"

        ''' <summary>
        ''' Native Visual C Runtime memset.
        ''' </summary>
        <DllImport("msvcrt.dll", CallingConvention:=CallingConvention.Cdecl, EntryPoint:="memset", PreserveSig:=True)>
        Sub n_memset(dest As IntPtr, c As Integer, count As IntPtr)
        End Sub

        ''' <summary>
        ''' Native Visual C Runtime memcpy.
        ''' </summary>
        <DllImport("msvcrt.dll", EntryPoint:="memcpy", CallingConvention:=CallingConvention.Cdecl, SetLastError:=False)>
        Public Function n_memcpy(dest As IntPtr, src As IntPtr, count As UIntPtr) As IntPtr
        End Function

#End Region

#Region "Direct IL"

        ''' <summary>
        ''' IL Code: Scans for a null character starting at the specified pointer.
        ''' </summary>
        ''' <param name="p">Pointer in memory at which to begin scanning.</param>
        ''' <returns>The zero-based character index of the null character (the string length)</returns>
        ''' <remarks></remarks>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function CharZero(p As IntPtr) As Integer
            Return 0
        End Function


        ''' <summary>
        ''' IL Code: Scans for a null byte at the specified pointer.
        ''' </summary>
        ''' <param name="p">Pointer in memory at which to begin scanning.</param>
        ''' <returns>The zero-based index of the null character (the string length)</returns>
        ''' <remarks></remarks>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ByteZero(p As IntPtr) As Integer
            Return 0
        End Function


        '' MemSet using CIL code.


        ''' <summary>
        ''' CIL MemSet.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemSet(dest As IntPtr, [char] As Byte, length As UInteger)
        End Sub



        '' All the MemCpy's!!!!


        ''' <summary>
        ''' IL Code: Copies memory from one memory pointer to another using IL cpblk.
        ''' </summary>
        ''' <remarks></remarks>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As IntPtr, src As IntPtr, length As UInteger)
        End Sub

        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As IntPtr, src As Byte(), length As UInteger)
        End Sub

        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As Byte(), src As IntPtr, length As UInteger)
        End Sub

        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As IntPtr, src As SByte(), length As UInteger)
        End Sub

        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As SByte(), src As IntPtr, length As UInteger)
        End Sub

        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As IntPtr, src As Char(), length As UInteger)
        End Sub

        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As Char(), src As IntPtr, length As UInteger)
        End Sub

        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As IntPtr, src As Short(), length As UInteger)
        End Sub

        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As Short(), src As IntPtr, length As UInteger)
        End Sub

        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As IntPtr, src As UShort(), length As UInteger)
        End Sub

        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As UShort(), src As IntPtr, length As UInteger)
        End Sub

        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As IntPtr, src As Integer(), length As UInteger)
        End Sub

        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As Integer(), src As IntPtr, length As UInteger)
        End Sub

        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As IntPtr, src As UInteger(), length As UInteger)
        End Sub

        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As UInteger(), src As IntPtr, length As UInteger)
        End Sub

        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As IntPtr, src As Long(), length As UInteger)
        End Sub

        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As Long(), src As IntPtr, length As UInteger)
        End Sub


        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As IntPtr, src As ULong(), length As UInteger)
        End Sub


        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As ULong(), src As IntPtr, length As UInteger)
        End Sub


        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As IntPtr, src As Single(), length As UInteger)
        End Sub


        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As Single(), src As IntPtr, length As UInteger)
        End Sub


        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As IntPtr, src As Double(), length As UInteger)
        End Sub


        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As Double(), src As IntPtr, length As UInteger)
        End Sub


        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As IntPtr, src As System.Drawing.Color(), length As UInteger)
        End Sub


        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As System.Drawing.Color(), src As IntPtr, length As UInteger)
        End Sub


        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As IntPtr, src As Date(), length As UInteger)
        End Sub


        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As Date(), src As IntPtr, length As UInteger)
        End Sub


        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As IntPtr, src As Decimal(), length As UInteger)
        End Sub


        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As Decimal(), src As IntPtr, length As UInteger)
        End Sub


        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As IntPtr, src As Guid(), length As UInteger)
        End Sub


        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As Guid(), src As IntPtr, length As UInteger)
        End Sub



        '' Additionals

        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As Byte(), src As Byte(), length As UInteger)
        End Sub


        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As Byte(), src As SByte(), length As UInteger)
        End Sub

        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As SByte(), src As Byte(), length As UInteger)
        End Sub


        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As Byte(), src As Char(), length As UInteger)
        End Sub

        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As Char(), src As Byte(), length As UInteger)
        End Sub


        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As Byte(), src As Short(), length As UInteger)
        End Sub

        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As Short(), src As Byte(), length As UInteger)
        End Sub


        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As Byte(), src As UShort(), length As UInteger)
        End Sub


        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As UShort(), src As Byte(), length As UInteger)
        End Sub


        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As Byte(), src As Integer(), length As UInteger)
        End Sub


        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As Integer(), src As Byte(), length As UInteger)
        End Sub


        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As Byte(), src As UInteger(), length As UInteger)
        End Sub


        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As UInteger(), src As Byte(), length As UInteger)
        End Sub


        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As Byte(), src As Long(), length As UInteger)
        End Sub


        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As Long(), src As Byte(), length As UInteger)
        End Sub

        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As Byte(), src As ULong(), length As UInteger)
        End Sub

        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As ULong(), src As Byte(), length As UInteger)
        End Sub

        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As Byte(), src As Single(), length As UInteger)
        End Sub

        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As Single(), src As Byte(), length As UInteger)
        End Sub

        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As Byte(), src As Double(), length As UInteger)
        End Sub

        ''' <summary>
        ''' CIL MemCpy using the cpblk instruction.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub MemCpy(dest As Double(), src As Byte(), length As UInteger)
        End Sub


        '' End Additionals


        '' Blittibles!
#Region "Blittles"

        ''' <summary>
        ''' Grabs a Char from Bytes().
        ''' </summary>
        ''' <param name="src"></param>
        ''' <param name="index"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToChar(src As Byte(), index As UInteger) As Char
            Return Nothing
        End Function

        ''' <summary>
        ''' Grabs Bytes() from a Char.
        ''' </summary>
        ''' <param name="src"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToBytes(src As Char) As Byte()
            Return Nothing
        End Function


        ''' <summary>
        ''' Grabs a Short from Bytes().
        ''' </summary>
        ''' <param name="src"></param>
        ''' <param name="index"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToShort(src As Byte(), index As UInteger) As Short
            Return Nothing
        End Function

        ''' <summary>
        ''' Grabs Bytes() from a Short.
        ''' </summary>
        ''' <param name="src"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToBytes(src As Short) As Byte()
            Return Nothing
        End Function

        ''' <summary>
        ''' Grabs a UShort from Bytes().
        ''' </summary>
        ''' <param name="src"></param>
        ''' <param name="index"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToUShort(src As Byte(), index As UInteger) As UShort
            Return Nothing
        End Function

        ''' <summary>
        ''' Grabs Bytes() from a UShort.
        ''' </summary>
        ''' <param name="src"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToBytes(src As UShort) As Byte()
            Return Nothing
        End Function

        ''' <summary>
        ''' Grabs a Integer from Bytes().
        ''' </summary>
        ''' <param name="src"></param>
        ''' <param name="index"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToInteger(src As Byte(), index As UInteger) As Integer
            Return Nothing
        End Function

        ''' <summary>
        ''' Grabs Bytes() from a Integer.
        ''' </summary>
        ''' <param name="src"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToBytes(src As Integer) As Byte()
            Return Nothing
        End Function

        ''' <summary>
        ''' Grabs a UInteger from Bytes().
        ''' </summary>
        ''' <param name="src"></param>
        ''' <param name="index"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToUInteger(src As Byte(), index As UInteger) As UInteger
            Return Nothing
        End Function

        ''' <summary>
        ''' Grabs Bytes() from a UInteger.
        ''' </summary>
        ''' <param name="src"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToBytes(src As UInteger) As Byte()
            Return Nothing
        End Function


        ''' <summary>
        ''' Grabs a Long from Bytes().
        ''' </summary>
        ''' <param name="src"></param>
        ''' <param name="index"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToLong(src As Byte(), index As UInteger) As Long
            Return Nothing
        End Function

        ''' <summary>
        ''' Grabs Bytes() from a Long.
        ''' </summary>
        ''' <param name="src"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToBytes(src As Long) As Byte()
            Return Nothing
        End Function


        ''' <summary>
        ''' Grabs a ULong from Bytes().
        ''' </summary>
        ''' <param name="src"></param>
        ''' <param name="index"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToULong(src As Byte(), index As UInteger) As ULong
            Return Nothing
        End Function

        ''' <summary>
        ''' Grabs Bytes() from a ULong.
        ''' </summary>
        ''' <param name="src"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToBytes(src As ULong) As Byte()
            Return Nothing
        End Function


        ''' <summary>
        ''' Grabs a Single from Bytes().
        ''' </summary>
        ''' <param name="src"></param>
        ''' <param name="index"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToSingle(src As Byte(), index As UInteger) As Single
            Return Nothing
        End Function

        ''' <summary>
        ''' Grabs Bytes() from a Single.
        ''' </summary>
        ''' <param name="src"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToBytes(src As Single) As Byte()
            Return Nothing
        End Function


        ''' <summary>
        ''' Grabs a Double from Bytes().
        ''' </summary>
        ''' <param name="src"></param>
        ''' <param name="index"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToDouble(src As Byte(), index As UInteger) As Double
            Return Nothing
        End Function

        ''' <summary>
        ''' Grabs Bytes() from a Double.
        ''' </summary>
        ''' <param name="src"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToBytes(src As Double) As Byte()
            Return Nothing
        End Function


        ''' <summary>
        ''' Grabs a Date from Bytes().
        ''' </summary>
        ''' <param name="src"></param>
        ''' <param name="index"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToDate(src As Byte(), index As UInteger) As Date
            Return Nothing
        End Function

        ''' <summary>
        ''' Grabs Bytes() from a Date.
        ''' </summary>
        ''' <param name="src"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToBytes(src As Date) As Byte()
            Return Nothing
        End Function


        ''' <summary>
        ''' Grabs a Decimal from Bytes().
        ''' </summary>
        ''' <param name="src"></param>
        ''' <param name="index"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToDecimal(src As Byte(), index As UInteger) As Decimal
            Return Nothing
        End Function

        ''' <summary>
        ''' Grabs Bytes() from a Decimal.
        ''' </summary>
        ''' <param name="src"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToBytes(src As Decimal) As Byte()
            Return Nothing
        End Function

        ''' <summary>
        ''' Grabs a Guid from Bytes().
        ''' </summary>
        ''' <param name="src"></param>
        ''' <param name="index"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToGuid(src As Byte(), index As UInteger) As Guid
            Return Nothing
        End Function

        ''' <summary>
        ''' Grabs Bytes() from a Guid.
        ''' </summary>
        ''' <param name="src"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToBytes(src As Guid) As Byte()
            Return Nothing
        End Function



#End Region




        '' Array blittibles!
#Region "Array Blittibles"

        ''' <summary>
        ''' Grabs a Char() array from Bytes().
        ''' </summary>
        ''' <param name="src"></param>
        ''' <param name="index"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToChars(src As Byte(), index As UInteger) As Char()
            Return Nothing
        End Function

        ''' <summary>
        ''' Grabs Bytes() from Char() array.
        ''' </summary>
        ''' <param name="src"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToBytes(src As Char()) As Byte()
            Return Nothing
        End Function


        ''' <summary>
        ''' Grabs a Short() array from Bytes().
        ''' </summary>
        ''' <param name="src"></param>
        ''' <param name="index"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToShorts(src As Byte(), index As UInteger) As Short()
            Return Nothing
        End Function

        ''' <summary>
        ''' Grabs Bytes() from Short() array.
        ''' </summary>
        ''' <param name="src"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToBytes(src As Short()) As Byte()
            Return Nothing
        End Function



        ''' <summary>
        ''' Grabs a UShort() array from Bytes().
        ''' </summary>
        ''' <param name="src"></param>
        ''' <param name="index"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToUShorts(src As Byte(), index As UInteger) As UShort()
            Return Nothing
        End Function

        ''' <summary>
        ''' Grabs Bytes() from UShort() array.
        ''' </summary>
        ''' <param name="src"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToBytes(src As UShort()) As Byte()
            Return Nothing
        End Function



        ''' <summary>
        ''' Grabs a Integer() array from Bytes().
        ''' </summary>
        ''' <param name="src"></param>
        ''' <param name="index"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToIntegers(src As Byte(), index As UInteger) As Integer()
            Return Nothing
        End Function

        ''' <summary>
        ''' Grabs Bytes() from Integer() array.
        ''' </summary>
        ''' <param name="src"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToBytes(src As Integer()) As Byte()
            Return Nothing
        End Function



        ''' <summary>
        ''' Grabs a UInteger() array from Bytes().
        ''' </summary>
        ''' <param name="src"></param>
        ''' <param name="index"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToUIntegers(src As Byte(), index As UInteger) As UInteger()
            Return Nothing
        End Function

        ''' <summary>
        ''' Grabs Bytes() from UInteger() array.
        ''' </summary>
        ''' <param name="src"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToBytes(src As UInteger()) As Byte()
            Return Nothing
        End Function



        ''' <summary>
        ''' Grabs a Long() array from Bytes().
        ''' </summary>
        ''' <param name="src"></param>
        ''' <param name="index"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToLongs(src As Byte(), index As UInteger) As Long()
            Return Nothing
        End Function

        ''' <summary>
        ''' Grabs Bytes() from Long() array.
        ''' </summary>
        ''' <param name="src"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToBytes(src As Long()) As Byte()
            Return Nothing
        End Function



        ''' <summary>
        ''' Grabs a ULong() array from Bytes().
        ''' </summary>
        ''' <param name="src"></param>
        ''' <param name="index"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToULongs(src As Byte(), index As UInteger) As ULong()
            Return Nothing
        End Function

        ''' <summary>
        ''' Grabs Bytes() from ULong() array.
        ''' </summary>
        ''' <param name="src"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToBytes(src As ULong()) As Byte()
            Return Nothing
        End Function




        ''' <summary>
        ''' Grabs a Single() array from Bytes().
        ''' </summary>
        ''' <param name="src"></param>
        ''' <param name="index"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToSingles(src As Byte(), index As UInteger) As Single()
            Return Nothing
        End Function

        ''' <summary>
        ''' Grabs Bytes() from Single() array.
        ''' </summary>
        ''' <param name="src"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToBytes(src As Single()) As Byte()
            Return Nothing
        End Function


        ''' <summary>
        ''' Grabs a Double() array from Bytes().
        ''' </summary>
        ''' <param name="src"></param>
        ''' <param name="index"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToDoubles(src As Byte(), index As UInteger) As Double()
            Return Nothing
        End Function

        ''' <summary>
        ''' Grabs Bytes() from Double() array.
        ''' </summary>
        ''' <param name="src"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToBytes(src As Double()) As Byte()
            Return Nothing
        End Function



        ''' <summary>
        ''' Grabs a Date() array from Bytes().
        ''' </summary>
        ''' <param name="src"></param>
        ''' <param name="index"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToDates(src As Byte(), index As UInteger) As Date()
            Return Nothing
        End Function

        ''' <summary>
        ''' Grabs Bytes() from Date() array.
        ''' </summary>
        ''' <param name="src"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToBytes(src As Date()) As Byte()
            Return Nothing
        End Function


        ''' <summary>
        ''' Grabs a Decimal() array from Bytes().
        ''' </summary>
        ''' <param name="src"></param>
        ''' <param name="index"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToDecimals(src As Byte(), index As UInteger) As Decimal()
            Return Nothing
        End Function

        ''' <summary>
        ''' Grabs Bytes() from Decimal() array.
        ''' </summary>
        ''' <param name="src"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToBytes(src As Decimal()) As Byte()
            Return Nothing
        End Function


        ''' <summary>
        ''' Grabs a Guid() array from Bytes().
        ''' </summary>
        ''' <param name="src"></param>
        ''' <param name="index"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToGuids(src As Byte(), index As UInteger) As Guid()
            Return Nothing
        End Function

        ''' <summary>
        ''' Grabs Bytes() from Guid() array.
        ''' </summary>
        ''' <param name="src"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToBytes(src As Guid()) As Byte()
            Return Nothing
        End Function


#End Region








        '' bit-wise verbatim tools

        ''' <summary>
        ''' Bit-wise, verbatim translation from <see cref="Long"/> to <see cref="IntPtr"/>.
        ''' </summary>
        ''' <param name="value"><see cref="Long"/> value to convert.</param>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function CIntPtr(value As Long) As IntPtr
        End Function

        ''' <summary>
        ''' Bit-wise, verbatim translation from <see cref="Integer"/> to <see cref="IntPtr"/>.
        ''' </summary>
        ''' <param name="value"><see cref="Integer"/> value to convert.</param>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function CIntPtr(value As Integer) As IntPtr
        End Function

        ''' <summary>
        ''' Bit-wise, verbatim translation from <see cref="Intptr"/> to <see cref="UIntPtr"/>.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToUnsigned(v As IntPtr) As UIntPtr
            Return 0
        End Function

        ''' <summary>
        ''' Bit-wise, verbatim translation from <see cref="UIntptr"/> to <see cref="IntPtr"/>.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToSigned(v As UIntPtr) As IntPtr
            Return 0
        End Function

        ''' <summary>
        ''' Bit-wise, verbatim transation from signed to unsigned.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToUnsigned(v As SByte) As Byte
            Return 0
        End Function

        ''' <summary>
        ''' Bit-wise, verbatim transation from unsigned to signed.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToSigned(v As Byte) As SByte
            Return 0
        End Function

        ''' <summary>
        ''' Bit-wise, verbatim transation from signed to unsigned.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToUnsigned(v As Short) As UShort
            Return 0
        End Function

        ''' <summary>
        ''' Bit-wise, verbatim transation from unsigned to signed.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToSigned(v As UShort) As Short
            Return 0
        End Function

        ''' <summary>
        ''' Bit-wise, verbatim transation from signed to unsigned.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToUnsigned(v As Integer) As UInteger
            Return 0
        End Function

        ''' <summary>
        ''' Bit-wise, verbatim transation from unsigned to signed.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToSigned(v As UInteger) As Integer
            Return 0
        End Function

        ''' <summary>
        ''' Bit-wise, verbatim transation from signed to unsigned.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToUnsigned(v As Long) As ULong
            Return 0
        End Function

        ''' <summary>
        ''' Bit-wise, verbatim transation from unsigned to signed.
        ''' </summary>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function ToSigned(v As ULong) As Long
            Return 0
        End Function


#End Region

        ''' <summary>
        ''' Returns true for all unsigned primitives (and enumerations) and UIntPtr. Returns false otherwise.
        ''' </summary>
        ''' <param name="t">The type to test for the specified condition.</param>
        ''' <returns>True or False</returns>
        ''' <remarks></remarks>
        Public Function Unsigned(t As System.Type) As Boolean

            Select Case t
                Case GetType(UShort), GetType(Byte), GetType(ULong), GetType(UInteger), GetType(UIntPtr)
                    Unsigned = True

                Case Else

                    If t.IsEnum = True Then
                        Unsigned = Unsigned(t.GetEnumUnderlyingType)
                    Else
                        Unsigned = False
                    End If

            End Select
        End Function

    End Module

#End Region '' Native, signed class (most common).

#Region "UNative Declarations"

    '    'Public Module UNative

    '    '#Region "Supporting Types for Declares"

    '    '    Public Enum ArchitectureType As Short

    '    '        ''' <summary>
    '    '        ''' 32-bit system.
    '    '        ''' </summary>
    '    '        ''' <remarks></remarks>
    '    '        x86 = 0

    '    '        ''' <summary>
    '    '        ''' Iatium-based system.
    '    '        ''' </summary>
    '    '        ''' <remarks></remarks>
    '    '        IA64 = 6

    '    '        ''' <summary>
    '    '        ''' 64-bit system.
    '    '        ''' </summary>
    '    '        ''' <remarks></remarks>
    '    '        x64 = 9

    '    '    End Enum

    '    '    <StructLayout(LayoutKind.Sequential, Pack:=1)> _
    '    '    Structure SYSTEM_INFO
    '    '        Public wProcessorArchitecture As ArchitectureType
    '    '        Public wReserved As Short
    '    '        Public dwPageSize As Integer
    '    '        Public lpMinimumApplicationAddress As UIntPtr
    '    '        Public lpMaximumApplicationAddress As UIntPtr
    '    '        Public dwActiveProcessorMask As Integer
    '    '        Public dwNumberOfProcessors As Integer
    '    '        Public dwProcessorType As Integer
    '    '        Public dwAllocationGranularity As Integer
    '    '        Public wProcessorLevel As Short
    '    '        Public wProcessorRevision As Short
    '    '    End Structure

    '    '#End Region

    '    '#Region "System Information"

    '    '    Declare Function GetSystemInfo Lib "kernel32" (ByRef lpSysInfo As SYSTEM_INFO) As UIntPtr

    '    '#End Region

    '    '#Region "Memory Functions"

    '    '#Region "NetApi Memory Functions"

    '    '    Public Declare Function NetApiBufferAllocate Lib "netapi32.dll" (ByteCount As Integer, ByRef Buffer As UIntPtr) As Integer
    '    '    Public Declare Function NetApiBufferFree Lib "netapi32.dll" (Buffer As UIntPtr) As Integer

    '    '#End Region

    '    '#Region "Heap Functions"

    '    '    Declare Function HeapCreate Lib "kernel32" (dwOptions As Integer, _
    '    '                                                initSize As UIntPtr, _
    '    '                                                maxSize As UIntPtr) As UIntPtr

    '    '    Declare Function HeapDestroy Lib "kernel32" (hHeap As UIntPtr) As Boolean

    '    '    Declare Function GetProcessHeap Lib "kernel32" () As UIntPtr

    '    '    Declare Function HeapQueryInformation Lib "kernel32" (HeapHandle As UIntPtr, _
    '    '                                                          HeapInformationClass As Integer, _
    '    '                                                          ByRef HeapInformation As UInteger, _
    '    '                                                          HeapInformationLength As UIntPtr, _
    '    '                                                          ReturnLength As UIntPtr) As Boolean

    '    '#End Region

    '    '#Region "Memory Blocks"

    '    '    '' As per the MSDN manual, we're using ONLY Heap functions, here.

    '    '    Declare Function HeapAlloc Lib "kernel32" (hHeap As UIntPtr, _
    '    '                                               dwOptions As Integer, _
    '    '                                               dwBytes As UIntPtr) As UIntPtr

    '    '    Declare Function HeapReAlloc Lib "kernel32" (hHeap As UIntPtr, _
    '    '                                                 dwOptions As Integer, _
    '    '                                                 lpMem As UIntPtr, _
    '    '                                                 dwBytes As UIntPtr) As UIntPtr

    '    '    Declare Function HeapFree Lib "kernel32" (hHeap As UIntPtr, _
    '    '                                               dwOptions As Integer, _
    '    '                                               lpMem As UIntPtr) As UIntPtr

    '    '    Declare Function HeapSize Lib "kernel32" (hHeap As UIntPtr, _
    '    '                                              dwOptions As Integer, _
    '    '                                              lpMem As UIntPtr) As UIntPtr

    '    '    Declare Function HeapValidate Lib "kernel32" (hHeap As UIntPtr, _
    '    '                                              dwOptions As Integer, _
    '    '                                              lpMem As UIntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

    '    '    '' used for specific operating system functions.
    '    '    Declare Function LocalFree Lib "kernel32.dll" (hMem As UIntPtr) As UIntPtr

    '    '    '' used for specific operating system functions.
    '    '    Declare Function GlobalFree Lib "kernel32.dll" (hMem As UIntPtr) As UIntPtr

    '    '#End Region

    '    '#Region "CopyMemory"

    '    '#Region "Pointer to Pointer"

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    '    '                                                                 pSrc As UIntPtr, _
    '    '                                                                 byteLen As UIntPtr)
    '    '#End Region

    '    '#Region "Object to Pointer"

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (<MarshalAs(UnmanagedType.AsAny)> _
    '    '                                                                 pDst As Object, _
    '    '                                                                 pSrc As UIntPtr, _
    '    '                                                                 byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    '    '                                                                 <MarshalAs(UnmanagedType.AsAny)> _
    '    '                                                                 pSrc As Object, _
    '    '                                                                 byteLen As UIntPtr)

    '    '#End Region

    '    '#Region "Arrays"

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    '    '                                                                     pSrc As Char(), _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As Char(), _
    '    '                                                                     pSrc As UIntPtr, _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    '    '                                                                     pSrc As Byte(), _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As Byte(), _
    '    '                                                                     pSrc As UIntPtr, _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    '    '                                                                     pSrc As SByte(), _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As SByte(), _
    '    '                                                                     pSrc As UIntPtr, _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    '    '                                                                     pSrc As ULong(), _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As ULong(), _
    '    '                                                                     pSrc As UIntPtr, _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    '    '                                                                     pSrc As UShort(), _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UShort(), _
    '    '                                                                     pSrc As UIntPtr, _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    '    '                                                                     pSrc As UInteger(), _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UInteger(), _
    '    '                                                                     pSrc As UIntPtr, _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    '    '                                                                     pSrc As Long(), _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As Long(), _
    '    '                                                                     pSrc As UIntPtr, _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    '    '                                                                     pSrc As Short(), _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As Short(), _
    '    '                                                                     pSrc As UIntPtr, _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    '    '                                                                     pSrc As Integer(), _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As Integer(), _
    '    '                                                                     pSrc As UIntPtr, _
    '    '                                                                     byteLen As UIntPtr)

    '    '#End Region

    '    '#Region "Scalar Declares"

    '    '    '' UIntPtr
    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    '    '                                                                     ByRef pSrc As Byte, _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As Byte, _
    '    '                                                                     pSrc As UIntPtr, _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    '    '                                                                     ByRef pSrc As SByte, _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As SByte, _
    '    '                                                                     pSrc As UIntPtr, _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    '    '                                                                     ByRef pSrc As ULong, _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As ULong, _
    '    '                                                                     pSrc As UIntPtr, _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    '    '                                                                     ByRef pSrc As UShort, _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As UShort, _
    '    '                                                                     pSrc As UIntPtr, _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    '    '                                                                     ByRef pSrc As UInteger, _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As UInteger, _
    '    '                                                                     pSrc As UIntPtr, _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    '    '                                                                     ByRef pSrc As Long, _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As Long, _
    '    '                                                                     pSrc As UIntPtr, _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    '    '                                                                     ByRef pSrc As Short, _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As Short, _
    '    '                                                                     pSrc As UIntPtr, _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    '    '                                                                     ByRef pSrc As Char, _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As Char, _
    '    '                                                                     pSrc As UIntPtr, _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    '    '                                                                     ByRef pSrc As Integer, _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As Integer, _
    '    '                                                                     pSrc As UIntPtr, _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    '    '                                                                     <MarshalAs(UnmanagedType.LPWStr)> pSrc As String, _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (<MarshalAs(UnmanagedType.LPWStr)> pDst As String, _
    '    '                                                                     pSrc As UIntPtr, _
    '    '                                                                     byteLen As UIntPtr)

    '    '#End Region

    '    '#Region "Unique Types"

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    '    '                                                                     ByRef pSrc As Date, _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As Date, _
    '    '                                                                     pSrc As UIntPtr, _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    '    '                                                                     pSrc As Date(), _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As Date(), _
    '    '                                                                     pSrc As UIntPtr, _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    '    '                                                                     ByRef pSrc As System.Guid, _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As System.Guid, _
    '    '                                                                     pSrc As UIntPtr, _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    '    '                                                                     pSrc As System.Guid(), _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As System.Guid(), _
    '    '                                                                     pSrc As UIntPtr, _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    '    '                                                                     pSrc As BigInteger, _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As BigInteger, _
    '    '                                                                     pSrc As UIntPtr, _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    '    '                                                                     pSrc As System.Drawing.Color, _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As System.Drawing.Color, _
    '    '                                                                     pSrc As UIntPtr, _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    '    '                                                                     pSrc As System.Drawing.Color(), _
    '    '                                                                     byteLen As UIntPtr)

    '    '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As System.Drawing.Color(), _
    '    '                                                                     pSrc As UIntPtr, _
    '    '                                                                     byteLen As UIntPtr)

    '    '#End Region

    '    '#End Region

    '    '#Region "Zero Memory"

    '    '    Declare Sub RtlZeroMemory Lib "kernel32" Alias "RtlZeroMemory" (pDst As UIntPtr, _
    '    '                                                                             byteLen As UIntPtr)

    '    '#End Region

    '    '#End Region

    '    '    ' Friend myHeapPtr As UIntPtr
    '    '    ' Friend myHeap As New Mem

    '    '    Public Function Unsigned(t As System.Type) As Boolean
    '    '        Select Case t
    '    '            Case GetType(UShort), GetType(Byte), GetType(ULong), GetType(UInteger)
    '    '                Unsigned = True
    '    '            Case Else
    '    '                Unsigned = False
    '    '        End Select
    '    '    End Function

    '    'End Module

#End Region '' UNative, unsigned class -- deprecated.  Preserved for posterity.

End Namespace

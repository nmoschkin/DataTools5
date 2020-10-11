// ' ************************************************* ''
// ' DataTools Visual Basic Utility Library 
// '
// ' Module: Native
// '         Global Memory Methods.
// ' 
// ' Copyright (C) 2011-2020 Nathan Moschkin
// ' All Rights Reserved
// '
// ' Licensed Under the Microsoft Public License   
// ' ************************************************* ''

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic.CompilerServices;

namespace DataTools.Memory.Internal
{

    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    [Flags]
    public enum MemoryTypes
    {

        /// <summary>
        /// Indicates that the memory pages within the region are mapped into the view of an image section.
        /// </summary>
        [Description(" Indicates that the memory pages within the region are mapped into the view of an image section.")]
        MEM_IMAGE = 0x1000000,

        /// <summary>
        /// Indicates that the memory pages within the region are mapped into the view of a section.
        /// </summary>
        [Description(" Indicates that the memory pages within the region are mapped into the view of a section.")]
        MEM_MAPPED = 0x40000,

        /// <summary>
        /// Indicates that the memory pages within the region are private (that is, not shared by other processes
        /// </summary>
        [Description(" Indicates that the memory pages within the region are private (that is, not shared by other processes")]
        MEM_PRIVATE = 0x20000
    }

    [Flags]
    public enum MemoryStates
    {

        /// <summary>
        /// Indicates committed pages for which physical storage has been allocated, either in memory or in the paging file on disk.
        /// </summary>
        [Description(" Indicates committed pages for which physical storage has been allocated, either in memory or in the paging file on disk.")]
        MEM_COMMIT = 0x1000,

        /// <summary>
        /// Indicates free pages not accessible to the calling process and available to be allocated. For free pages, the information in the AllocationBase, AllocationProtect, Protect, and Type members is undefined.
        /// </summary>
        [Description(" Indicates free pages not accessible to the calling process and available to be allocated. For free pages, the information in the AllocationBase, AllocationProtect, Protect, and Type members is undefined.")]
        MEM_FREE = 0x10000,

        /// <summary>
        /// Indicates reserved pages where a range of the process's virtual address space is reserved without any physical storage
        /// </summary>
        [Description(" Indicates reserved pages where a range of the process's virtual address space is reserved without any physical storage ")]
        MEM_RESERVE = 0x2000
    }

    [Flags]
    public enum MemoryProtectionFlags
    {

        /// <summary>
        /// Enables execute access to the committed region of pages. An attempt to read from or write to the committed region results in an access violation.
        /// </summary>
        [Description("Enables execute access to the committed region of pages. An attempt to read from or write to the committed region results in an access violation.")]
        PAGE_EXECUTE = 0x10,

        /// <summary>
        /// Enables execute or read-only access to the committed region of pages. An attempt to write to the committed region results in an access violation.
        /// </summary>
        [Description("Enables execute or read-only access to the committed region of pages. An attempt to write to the committed region results in an access violation.")]
        PAGE_EXECUTE_READ = 0x20,

        /// <summary>
        /// Enables execute, read-only, or read/write access to the committed region of pages.
        /// </summary>
        [Description("Enables execute, read-only, or read/write access to the committed region of pages.")]
        PAGE_EXECUTE_READWRITE = 0x40,

        /// <summary>
        /// Enables execute, read-only, or copy-on-write access to a mapped view of a file mapping object. An attempt to write to a committed copy-on-write page results in a private copy of the page being made for the process. The private page is marked as PAGE_EXECUTE_READWRITE, and the change is written to the new page.
        /// </summary>
        [Description("Enables execute, read-only, or copy-on-write access to a mapped view of a file mapping object. An attempt to write to a committed copy-on-write page results in a private copy of the page being made for the process. The private page is marked as PAGE_EXECUTE_READWRITE, and the change is written to the new page.")]
        PAGE_EXECUTE_WRITECOPY = 0x80,

        /// <summary>
        /// Disables all access to the committed region of pages. An attempt to read from, write to, or execute the committed region results in an access violation.
        /// </summary>
        [Description("Disables all access to the committed region of pages. An attempt to read from, write to, or execute the committed region results in an access violation.")]
        PAGE_NOACCESS = 0x1,

        /// <summary>
        /// Enables read-only access to the committed region of pages. An attempt to write to the committed region results in an access violation. If Data Execution Prevention is enabled, an attempt to execute code in the committed region results in an access violation.
        /// </summary>
        [Description("Enables read-only access to the committed region of pages. An attempt to write to the committed region results in an access violation. If Data Execution Prevention is enabled, an attempt to execute code in the committed region results in an access violation.")]
        PAGE_READONLY = 0x2,

        /// <summary>
        /// Enables read-only or read/write access to the committed region of pages. If Data Execution Prevention is enabled, attempting to execute code in the committed region results in an access violation.
        /// </summary>
        [Description("Enables read-only or read/write access to the committed region of pages. If Data Execution Prevention is enabled, attempting to execute code in the committed region results in an access violation.")]
        PAGE_READWRITE = 0x4,

        /// <summary>
        /// Enables read-only or copy-on-write access to a mapped view of a file mapping object. An attempt to write to a committed copy-on-write page results in a private copy of the page being made for the process. The private page is marked as PAGE_READWRITE, and the change is written to the new page. If Data Execution Prevention is enabled, attempting to execute code in the committed region results in an access violation.
        /// </summary>
        [Description("Enables read-only or copy-on-write access to a mapped view of a file mapping object. An attempt to write to a committed copy-on-write page results in a private copy of the page being made for the process. The private page is marked as PAGE_READWRITE, and the change is written to the new page. If Data Execution Prevention is enabled, attempting to execute code in the committed region results in an access violation.")]
        PAGE_WRITECOPY = 0x8,

        /// <summary>
        /// Pages in the region become guard pages. Any attempt to access a guard page causes the system to raise a STATUS_GUARD_PAGE_VIOLATION exception and turn off the guard page status. Guard pages thus act as a one-time access alarm. For more information, see Creating Guard Pages.
        /// </summary>
        [Description("Pages in the region become guard pages. Any attempt to access a guard page causes the system to raise a STATUS_GUARD_PAGE_VIOLATION exception and turn off the guard page status. Guard pages thus act as a one-time access alarm. For more information, see Creating Guard Pages.")]
        PAGE_GUARD = 0x100,

        /// <summary>
        /// Sets all pages to be non-cachable. Applications should not use this attribute except when explicitly required for a device. Using the interlocked functions with memory that is mapped with SEC_NOCACHE can result in an EXCEPTION_ILLEGAL_INSTRUCTION exception.
        /// </summary>
        [Description("Sets all pages to be non-cachable. Applications should not use this attribute except when explicitly required for a device. Using the interlocked functions with memory that is mapped with SEC_NOCACHE can result in an EXCEPTION_ILLEGAL_INSTRUCTION exception.")]
        PAGE_NOCACHE = 0x200,

        /// <summary>
        /// Sets all pages to be write-combined.
        /// </summary>
        [Description("Sets all pages to be write-combined.")]
        PAGE_WRITECOMBINE = 0x400
    }

    [Flags]
    public enum VMemFreeFlags
    {

        /// <summary>
        /// Decommits the specified region of committed pages. After the operation, the pages are in the reserved state.
        /// The function does not fail if you attempt to decommit an uncommitted page. This means that you can decommit a range of pages without first determining the current commitment state.
        /// Do not use this value with MEM_RELEASE.
        /// </summary>
        /// <remarks></remarks>
        [Description("Decommits the specified region of committed pages. After the operation, the pages are in the reserved state.")]
        MEM_DECOMMIT = 0x4000,

        /// <summary>
        /// Releases the specified region of pages. After this operation, the pages are in the free state.
        /// </summary>
        /// <remarks></remarks>
        [Description("Releases the specified region of pages. After this operation, the pages are in the free state.")]
        MEM_RELEASE = 0x8000
    }

    [Flags]
    public enum VMemAllocFlags
    {

        /// <summary>
        /// Allocates memory charges (from the overall size of memory and the paging files on disk) for the specified reserved memory pages. The function also guarantees that when the caller later initially accesses the memory, the contents will be zero. Actual physical pages are not allocated unless/until the virtual addresses are actually accessed.
        /// </summary>
        [Description("Allocates memory charges (from the overall size of memory and the paging files on disk) for the specified reserved memory pages. The function also guarantees that when the caller later initially accesses the memory, the contents will be zero. Actual physical pages are not allocated unless/until the virtual addresses are actually accessed.")]
        MEM_COMMIT = 0x1000,

        /// <summary>
        /// Reserves a range of the process's virtual address space without allocating any actual physical storage in memory or in the paging file on disk.
        /// </summary>
        [Description("Reserves a range of the process's virtual address space without allocating any actual physical storage in memory or in the paging file on disk.")]
        MEM_RESERVE = 0x2000,

        /// <summary>
        /// Indicates that data in the memory range specified by lpAddress and dwSize is no longer of interest. The pages should not be read from or written to the paging file. However, the memory block will be used again later, so it should not be decommitted. This value cannot be used with any other value.
        /// </summary>
        [Description("Indicates that data in the memory range specified by lpAddress and dwSize is no longer of interest. The pages should not be read from or written to the paging file. However, the memory block will be used again later, so it should not be decommitted. This value cannot be used with any other value.")]
        MEM_RESET = 0x80000,

        /// <summary>
        /// MEM_RESET_UNDO should only be called on an address range to which MEM_RESET was successfully applied earlier. It indicates that the data in the specified memory range specified by lpAddress and dwSize is of interest to the caller and attempts to reverse the effects of MEM_RESET. If the function succeeds, that means all data in the specified address range is intact. If the function fails, at least some of the data in the address range has been replaced with zeroes.
        /// </summary>
        [Description("MEM_RESET_UNDO should only be called on an address range to which MEM_RESET was successfully applied earlier. It indicates that the data in the specified memory range specified by lpAddress and dwSize is of interest to the caller and attempts to reverse the effects of MEM_RESET. If the function succeeds, that means all data in the specified address range is intact. If the function fails, at least some of the data in the address range has been replaced with zeroes.")]
        MEM_RESET_UNDO = 0x1000000,

        /// <summary>
        /// Allocates memory using large page support. The size and alignment must be a multiple of the large-page minimum. To obtain this value, use the GetLargePageMinimum function.
        /// </summary>
        [Description("Allocates memory using large page support. The size and alignment must be a multiple of the large-page minimum. To obtain this value, use the GetLargePageMinimum function.")]
        MEM_LARGE_PAGES = 0x20000000,

        /// <summary>
        /// Reserves an address range that can be used to map Address Windowing Extensions (AWE) pages.
        /// This value must be used with MEM_RESERVE and no other values.
        /// </summary>
        [Description("Reserves an address range that can be used to map Address Windowing Extensions (AWE) pages. This value must be used with MEM_RESERVE and no other values.")]
        MEM_PHYSICAL = 0x400000,

        /// <summary>
        /// Allocates memory at the highest possible address. This can be slower than regular allocations, especially when there are many allocations.
        /// </summary>
        [Description("Allocates memory at the highest possible address. This can be slower than regular allocations, especially when there are many allocations.")]
        MEM_TOP_DOWN = 0x100000,

        /// <summary>
        /// Causes the system to track pages that are written to in the allocated region. If you specify this value, you must also specify MEM_RESERVE.
        /// </summary>
        [Description("Causes the system to track pages that are written to in the allocated region. If you specify this value, you must also specify MEM_RESERVE.")]
        MEM_WRITE_WATCH = 0x200000
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    [StructLayout(LayoutKind.Sequential)]
    public struct MEMORY_BASIC_INFORMATION
    {
        public IntPtr BaseAddress;
        public IntPtr AllocationBase;
        public MemoryProtectionFlags AllocationProtect;
        public IntPtr RegionSize;
        public MemoryStates State;
        public MemoryProtectionFlags Protect;
        public MemoryTypes Type;
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    public static class Native
    {

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        [DllImport("netapi32.dll")]
        internal static extern int NetApiBufferAllocate(int ByteCount, ref IntPtr Buffer);
        [DllImport("netapi32.dll")]
        internal static extern int NetApiBufferFree(IntPtr Buffer);

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        [DllImport("kernel32")]
        internal static extern IntPtr VirtualAlloc(IntPtr lpAddress, IntPtr dwSize, VMemAllocFlags flAllocationType, MemoryProtectionFlags flProtect);
        [DllImport("kernel32")]
        internal static extern bool VirtualProtect(IntPtr lpAddress, IntPtr dwSize, MemoryProtectionFlags flNewProtect, ref MemoryProtectionFlags flOldProtect);
        [DllImport("kernel32")]
        internal static extern bool VirtualFree(IntPtr lpAddress, IntPtr dwSize = default, VMemFreeFlags dwFreeType = VMemFreeFlags.MEM_RELEASE);
        [DllImport("kernel32")]
        internal static extern IntPtr VirtualQuery(IntPtr lpAddress, ref MEMORY_BASIC_INFORMATION lpBuffer, IntPtr dwLength);
        [DllImport("kernel32")]
        internal static extern bool VirtualLock(IntPtr lpAddress, IntPtr dwSize);
        [DllImport("kernel32")]
        internal static extern bool VirtualUnlock(IntPtr lpAddress, IntPtr dwSize);
        [DllImport("kernel32")]
        internal static extern bool SetProcessWorkingSetSize(IntPtr hProcess, IntPtr dwMinimumWorkingSetSize, IntPtr dwMaximumWorkingSetSize);
        [DllImport("kernel32")]
        internal static extern IntPtr GetLargePageMinimum();

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        [DllImport("kernel32", EntryPoint = "HeapCreate", CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true)]
        internal static extern IntPtr HeapCreate(int dwOptions, IntPtr initSize, IntPtr maxSize);
        [DllImport("kernel32", EntryPoint = "HeapDestroy", CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true)]
        internal static extern bool HeapDestroy(IntPtr hHeap);
        [DllImport("kernel32", EntryPoint = "GetProcessHeap", CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true)]
        internal static extern IntPtr GetProcessHeap();
        [DllImport("kernel32", EntryPoint = "HeapQueryInformation", CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true)]
        internal static extern bool HeapQueryInformation(IntPtr HeapHandle, int HeapInformationClass, ref ulong HeapInformation, IntPtr HeapInformationLength, IntPtr ReturnLength);

        // ' As per the MSDN manual, we're using ONLY Heap functions, here.

        [DllImport("kernel32", EntryPoint = "HeapAlloc", CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true)]
        internal static extern IntPtr HeapAlloc(IntPtr hHeap, uint dwOptions, IntPtr dwBytes);
        [DllImport("kernel32", EntryPoint = "HeapReAlloc", CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true)]
        internal static extern IntPtr HeapReAlloc(IntPtr hHeap, int dwOptions, IntPtr lpMem, IntPtr dwBytes);
        [DllImport("kernel32", EntryPoint = "HeapFree", CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true)]
        internal static extern bool HeapFree(IntPtr hHeap, uint dwOptions, IntPtr lpMem);
        [DllImport("kernel32", EntryPoint = "HeapSize", CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true)]
        internal static extern IntPtr HeapSize(IntPtr hHeap, uint dwOptions, IntPtr lpMem);
        [DllImport("kernel32", EntryPoint = "HeapLock", CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true)]
        internal static extern bool HeapLock(IntPtr hHeap);
        [DllImport("kernel32", EntryPoint = "HeapUnlock", CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true)]
        internal static extern bool HeaUnlock(IntPtr hHeap);
        [DllImport("kernel32", EntryPoint = "HeapValidate", CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true)]
        internal static extern bool HeapValidate(IntPtr hHeap, uint dwOptions, IntPtr lpMem);

        // ' used for specific operating system functions.
        [DllImport("kernel32.dll")]
        static extern IntPtr LocalFree(IntPtr hMem);

        // ' used for specific operating system functions.
        [DllImport("kernel32.dll")]
        static extern IntPtr GlobalFree(IntPtr hMem);

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        // Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (<MarshalAs(UnmanagedType.AsAny)>
        // pDst As Object,
        // pSrc As IntPtr,
        // byteLen As IntPtr)

        // Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
        // <MarshalAs(UnmanagedType.AsAny)>
        // pSrc As Object,
        // byteLen As IntPtr)

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, char[] pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(char[] pDst, IntPtr pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, byte[] pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(byte[] pDst, IntPtr pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, sbyte[] pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(sbyte[] pDst, IntPtr pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, ulong[] pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(ulong[] pDst, IntPtr pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, ushort[] pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(ushort[] pDst, IntPtr pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, uint[] pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(uint[] pDst, IntPtr pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, long[] pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(long[] pDst, IntPtr pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, short[] pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(short[] pDst, IntPtr pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, int[] pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(int[] pDst, IntPtr pSrc, IntPtr byteLen);

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        // ' IntPtr
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, ref byte pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(ref byte pDst, IntPtr pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, ref sbyte pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(ref sbyte pDst, IntPtr pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, ref ulong pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(ref ulong pDst, IntPtr pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, ref ushort pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(ref ushort pDst, IntPtr pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, ref uint pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(ref uint pDst, IntPtr pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, ref long pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(ref long pDst, IntPtr pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, ref short pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(ref short pDst, IntPtr pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, ref char pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(ref char pDst, IntPtr pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, ref int pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(ref int pDst, IntPtr pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, [MarshalAs(UnmanagedType.LPWStr)] string pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory([MarshalAs(UnmanagedType.LPWStr)] string pDst, IntPtr pSrc, IntPtr byteLen);

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, ref DateTime pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(ref DateTime pDst, IntPtr pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, DateTime[] pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(DateTime[] pDst, IntPtr pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, ref Guid pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(ref Guid pDst, IntPtr pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, Guid[] pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(Guid[] pDst, IntPtr pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, BigInteger pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(BigInteger pDst, IntPtr pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, System.Drawing.Color pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(ref System.Drawing.Color pDst, IntPtr pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, System.Drawing.Color[] pSrc, IntPtr byteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(System.Drawing.Color[] pDst, IntPtr pSrc, IntPtr byteLen);

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, IntPtr pSrc, IntPtr ByteLen);
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        // Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (<MarshalAs(UnmanagedType.AsAny)>
        // pDst As Object,
        // pSrc As IntPtr,
        // ByteLen As Long)

        // Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As IntPtr,
        // <MarshalAs(UnmanagedType.AsAny)>
        // pSrc As Object,
        // ByteLen As Long)

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, char[] pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(char[] pDst, IntPtr pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, byte[] pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(byte[] pDst, IntPtr pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, sbyte[] pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(sbyte[] pDst, IntPtr pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, ulong[] pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(ulong[] pDst, IntPtr pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, ushort[] pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(ushort[] pDst, IntPtr pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, uint[] pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(uint[] pDst, IntPtr pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, long[] pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(long[] pDst, IntPtr pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, short[] pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(short[] pDst, IntPtr pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, int[] pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(int[] pDst, IntPtr pSrc, long ByteLen);

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        // ' IntPtr
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, ref byte pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(ref byte pDst, IntPtr pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, ref sbyte pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(ref sbyte pDst, IntPtr pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, ref ulong pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(ref ulong pDst, IntPtr pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, ref ushort pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(ref ushort pDst, IntPtr pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, ref uint pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(ref uint pDst, IntPtr pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, ref long pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(ref long pDst, IntPtr pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, ref short pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(ref short pDst, IntPtr pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, ref char pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(ref char pDst, IntPtr pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, ref int pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(ref int pDst, IntPtr pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, [MarshalAs(UnmanagedType.LPWStr)] string pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory([MarshalAs(UnmanagedType.LPWStr)] string pDst, IntPtr pSrc, long ByteLen);

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, ref DateTime pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(ref DateTime pDst, IntPtr pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, DateTime[] pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(DateTime[] pDst, IntPtr pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, ref Guid pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(ref Guid pDst, IntPtr pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, Guid[] pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(Guid[] pDst, IntPtr pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, BigInteger pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(BigInteger pDst, IntPtr pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, System.Drawing.Color pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(ref System.Drawing.Color pDst, IntPtr pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr pDst, System.Drawing.Color[] pSrc, long ByteLen);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(System.Drawing.Color[] pDst, IntPtr pSrc, long ByteLen);

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        private static Type[] dyngenlist = new[] { typeof(byte), typeof(sbyte), typeof(char), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal), typeof(Guid), typeof(DateTime) };
        private static int[] dyngensizes = new[] { 1, 1, 2, 2, 2, 4, 4, 8, 8, 4, 8, 16, 16, 8 };
        internal static List<MulticastDelegate> MethodList = new List<MulticastDelegate>();

        // main MemCpy IL function

        /// <summary>
        /// IL Code: Copies memory from one memory pointer to another using IL cpblk.
        /// </summary>
        /// <param name="dest">The destination pointer.</param>
        /// <param name="src">The source pointer.</param>
        /// <param name="byteLen">The number of bytes to copy.</param>
        /// <remarks></remarks>
        public delegate void MemCpyFunc(IntPtr dest, IntPtr src, uint byteLen);

        /// <summary>
        /// IL Code: Scans for a null-terminated string starting at the specified pointer.
        /// </summary>
        /// <param name="p">Pointer in memory at which to begin scanning.</param>
        /// <returns>The zero-based character index of the null character.</returns>
        /// <remarks></remarks>
        public delegate string StrZeroF(IntPtr p);

        /// <summary>
        /// IL Code: Scans for a null byte starting at the specified pointer.
        /// </summary>
        /// <param name="p">Pointer in memory at which to begin scanning.</param>
        /// <returns>The zero-based byte index of the null byte.</returns>
        /// <remarks></remarks>
        public delegate int ByteZeroF(IntPtr p);

        /// <summary>
        /// IL Code: Returns a null-terminated Unicode string starting at the specified pointer.
        /// </summary>
        /// <remarks></remarks>
        public readonly static StrZeroF StrZero;

        /// <summary>
        /// IL Code: Copies memory from one memory pointer to another using IL cpblk.
        /// </summary>
        /// <remarks></remarks>
        public readonly static MemCpyFunc MemCpyDyn;

        public delegate void ILCopySinglet<T>(ref T dest, ref T src);

        public delegate void ILCopyTo<T>(ref T dest, IntPtr src);

        public delegate void ILCopyFrom<T>(IntPtr dest, ref T src);

        public delegate void ILCopyToArr<T>(T[] dest, IntPtr src, uint len);

        public delegate void ILCopyFromArr<T>(IntPtr dest, T[] src, uint len);

        // ' Dynamically-created pure Intermediate Language delegates for faster memory handling
        // ' Delegate creation is handled in Interop.vb.  The delegates are
        // ' stored in a list of MulticastDelegate.  The pure-IL execution
        // ' has shown a dramatic increase in performance, and is as good as 
        // ' it can possibly get in VB.
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        // singles

        public readonly static ILCopyTo<char> charAtget;
        public readonly static ILCopyFrom<char> charAtset;
        public readonly static ILCopyTo<byte> byteAtget;
        public readonly static ILCopyFrom<byte> byteAtset;
        public readonly static ILCopyTo<sbyte> sbyteAtget;
        public readonly static ILCopyFrom<sbyte> sbyteAtset;
        public readonly static ILCopyTo<short> shortAtget;
        public readonly static ILCopyFrom<short> shortAtset;
        public readonly static ILCopyTo<ushort> ushortAtget;
        public readonly static ILCopyFrom<ushort> ushortAtset;
        public readonly static ILCopyTo<int> intAtget;
        public readonly static ILCopyFrom<int> intAtset;
        public readonly static ILCopyTo<uint> uintAtget;
        public readonly static ILCopyFrom<uint> uintAtset;
        public readonly static ILCopyTo<long> longAtget;
        public readonly static ILCopyFrom<long> longAtset;
        public readonly static ILCopyTo<ulong> ulongAtget;
        public readonly static ILCopyFrom<ulong> ulongAtset;
        public readonly static ILCopyTo<float> singleAtget;
        public readonly static ILCopyFrom<float> singleAtset;
        public readonly static ILCopyTo<double> doubleAtget;
        public readonly static ILCopyFrom<double> doubleAtset;
        public readonly static ILCopyTo<Guid> guidAtget;
        public readonly static ILCopyFrom<Guid> guidAtset;
        public readonly static ILCopyTo<decimal> decimalAtget;
        public readonly static ILCopyFrom<decimal> decimalAtset;
        public readonly static ILCopyTo<DateTime> dateAtget;
        public readonly static ILCopyFrom<DateTime> dateAtset;

        // arrays

        public readonly static ILCopyToArr<char> charArrAtget;
        public readonly static ILCopyFromArr<char> charArrAtset;
        public readonly static ILCopyToArr<byte> byteArrAtget;
        public readonly static ILCopyFromArr<byte> byteArrAtset;
        public readonly static ILCopyToArr<sbyte> sbyteArrAtget;
        public readonly static ILCopyFromArr<sbyte> sbyteArrAtset;
        public readonly static ILCopyToArr<short> shortArrAtget;
        public readonly static ILCopyFromArr<short> shortArrAtset;
        public readonly static ILCopyToArr<ushort> ushortArrAtget;
        public readonly static ILCopyFromArr<ushort> ushortArrAtset;
        public readonly static ILCopyToArr<int> intArrAtget;
        public readonly static ILCopyFromArr<int> intArrAtset;
        public readonly static ILCopyToArr<uint> uintArrAtget;
        public readonly static ILCopyFromArr<uint> uintArrAtset;
        public readonly static ILCopyToArr<long> longArrAtget;
        public readonly static ILCopyFromArr<long> longArrAtset;
        public readonly static ILCopyToArr<ulong> ulongArrAtget;
        public readonly static ILCopyFromArr<ulong> ulongArrAtset;
        public readonly static ILCopyToArr<float> singleArrAtget;
        public readonly static ILCopyFromArr<float> singleArrAtset;
        public readonly static ILCopyToArr<double> doubleArrAtget;
        public readonly static ILCopyFromArr<double> doubleArrAtset;
        public readonly static ILCopyToArr<Guid> guidArrAtget;
        public readonly static ILCopyFromArr<Guid> guidArrAtset;
        public readonly static ILCopyToArr<decimal> decimalArrAtget;
        public readonly static ILCopyFromArr<decimal> decimalArrAtset;
        public readonly static ILCopyToArr<DateTime> dateArrAtget;
        public readonly static ILCopyFromArr<DateTime> dateArrAtset;

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        static Native()
        {

            // create the canonical pointer-to-pointer copy method.
            var dynMtd = new DynamicMethod("MemCpy", typeof(void), new[] { typeof(IntPtr), typeof(IntPtr), typeof(uint) }, typeof(Native));
            var ilGen = dynMtd.GetILGenerator();
            ilGen.Emit(OpCodes.Ldarg_0);
            ilGen.Emit(OpCodes.Ldarg_1);
            ilGen.Emit(OpCodes.Ldarg_2);
            ilGen.Emit(OpCodes.Volatile);
            if (IntPtr.Size == 8)
                ilGen.Emit(OpCodes.Unaligned, 1);
            ilGen.Emit(OpCodes.Cpblk);
            ilGen.Emit(OpCodes.Ret);
            MemCpyDyn = (MemCpyFunc)dynMtd.CreateDelegate(typeof(MemCpyFunc));

            // ' String length function

            Label lb;
            Label lb2;

            // ' Return a null-terminated Unicode string from a pointer.

            dynMtd = new DynamicMethod("StrZero", typeof(string), new[] { typeof(IntPtr) }, typeof(Native));
            ilGen = dynMtd.GetILGenerator();
            if (IntPtr.Size == 8)
            {
                ilGen.DeclareLocal(typeof(long));
            }
            else
            {
                ilGen.DeclareLocal(typeof(int));
            }

            ilGen.DeclareLocal(typeof(ushort), true);
            ilGen.DeclareLocal(typeof(int));
            ilGen.DeclareLocal(typeof(string), true);
            lb = ilGen.DefineLabel();
            lb2 = ilGen.DefineLabel();
            var lb3 = ilGen.DefineLabel();
            ilGen.Emit(OpCodes.Ldstr, "");
            ilGen.Emit(OpCodes.Stloc_3);
            ilGen.Emit(OpCodes.Ldarg_0);
            if (IntPtr.Size == 8)
            {
                ilGen.Emit(OpCodes.Conv_I8);
            }
            else
            {
                ilGen.Emit(OpCodes.Conv_I4);
            }

            ilGen.Emit(OpCodes.Stloc_0);
            ilGen.Emit(OpCodes.Ldloc_0);
            ilGen.Emit(OpCodes.Ldc_I4_0);
            ilGen.Emit(OpCodes.Stloc_1);
            ilGen.Emit(OpCodes.Ldc_I4_0);
            ilGen.Emit(OpCodes.Stloc_1);
            ilGen.MarkLabel(lb);
            ilGen.Emit(OpCodes.Ldloca, 1);
            ilGen.Emit(OpCodes.Ldloc_0);
            ilGen.Emit(OpCodes.Ldc_I4_2);
            ilGen.Emit(OpCodes.Cpblk);
            ilGen.Emit(OpCodes.Ldc_I4_0);
            ilGen.Emit(OpCodes.Ldloc_1);
            ilGen.Emit(OpCodes.Beq, lb2);
            ilGen.Emit(OpCodes.Ldloc_0);
            if (IntPtr.Size == 8)
            {
                ilGen.Emit(OpCodes.Ldc_I8, 2);
            }
            else
            {
                ilGen.Emit(OpCodes.Ldc_I4_2);
            }

            ilGen.Emit(OpCodes.Add);
            ilGen.Emit(OpCodes.Stloc_0);
            ilGen.Emit(OpCodes.Br, lb);
            ilGen.MarkLabel(lb2);
            ilGen.Emit(OpCodes.Ldloc_0);
            ilGen.Emit(OpCodes.Ldarg_0);
            ilGen.Emit(OpCodes.Sub);
            ilGen.Emit(OpCodes.Stloc_2);
            ilGen.Emit(OpCodes.Ldloc_2);
            ilGen.Emit(OpCodes.Ldc_I4_0);
            ilGen.Emit(OpCodes.Beq, lb3);
            ilGen.Emit(OpCodes.Ldc_I4_0);
            ilGen.Emit(OpCodes.Ldloc_2);
            ilGen.Emit(OpCodes.Ldc_I4_1);
            ilGen.Emit(OpCodes.Shr);
            ilGen.Emit(OpCodes.Newobj, typeof(string).GetConstructor(new[] { typeof(char), typeof(int) }));
            ilGen.Emit(OpCodes.Stloc_3);
            ilGen.Emit(OpCodes.Ldloc_3);
            ilGen.Emit(OpCodes.Ldc_I4, 0);
            ilGen.Emit(OpCodes.Ldelema, typeof(char));

            // I have no idea why this works, but for 64-bit systems, subtract 4 from the pointer
            if (IntPtr.Size == 8)
            {
                ilGen.Emit(OpCodes.Ldc_I4_4);
                ilGen.Emit(OpCodes.Sub);
            }

            ilGen.Emit(OpCodes.Ldarg_0);
            ilGen.Emit(OpCodes.Ldloc_2);
            ilGen.Emit(OpCodes.Volatile);
            if (IntPtr.Size == 8)
                ilGen.Emit(OpCodes.Unaligned, 1);
            ilGen.Emit(OpCodes.Cpblk);
            ilGen.MarkLabel(lb3);
            ilGen.Emit(OpCodes.Ldloc_3);
            ilGen.Emit(OpCodes.Ret);
            StrZero = (StrZeroF)dynMtd.CreateDelegate(typeof(StrZeroF));


            // initialize our dynamic methods.
            genDynList();

            // ' Initialize the delegates

            charAtget = (ILCopyTo<char>)FindDelegateTo<char>();
            charAtset = (ILCopyFrom<char>)FindDelegateFrom<char>();
            byteAtget = (ILCopyTo<byte>)FindDelegateTo<byte>();
            byteAtset = (ILCopyFrom<byte>)FindDelegateFrom<byte>();
            sbyteAtget = (ILCopyTo<sbyte>)FindDelegateTo<sbyte>();
            sbyteAtset = (ILCopyFrom<sbyte>)FindDelegateFrom<sbyte>();
            shortAtget = (ILCopyTo<short>)FindDelegateTo<short>();
            shortAtset = (ILCopyFrom<short>)FindDelegateFrom<short>();
            ushortAtget = (ILCopyTo<ushort>)FindDelegateTo<ushort>();
            ushortAtset = (ILCopyFrom<ushort>)FindDelegateFrom<ushort>();
            intAtget = (ILCopyTo<int>)FindDelegateTo<int>();
            intAtset = (ILCopyFrom<int>)FindDelegateFrom<int>();
            uintAtget = (ILCopyTo<uint>)FindDelegateTo<uint>();
            uintAtset = (ILCopyFrom<uint>)FindDelegateFrom<uint>();
            longAtget = (ILCopyTo<long>)FindDelegateTo<long>();
            longAtset = (ILCopyFrom<long>)FindDelegateFrom<long>();
            ulongAtget = (ILCopyTo<ulong>)FindDelegateTo<ulong>();
            ulongAtset = (ILCopyFrom<ulong>)FindDelegateFrom<ulong>();
            singleAtget = (ILCopyTo<float>)FindDelegateTo<float>();
            singleAtset = (ILCopyFrom<float>)FindDelegateFrom<float>();
            doubleAtget = (ILCopyTo<double>)FindDelegateTo<double>();
            doubleAtset = (ILCopyFrom<double>)FindDelegateFrom<double>();
            guidAtget = (ILCopyTo<Guid>)FindDelegateTo<Guid>();
            guidAtset = (ILCopyFrom<Guid>)FindDelegateFrom<Guid>();
            decimalAtget = (ILCopyTo<decimal>)FindDelegateTo<decimal>();
            decimalAtset = (ILCopyFrom<decimal>)FindDelegateFrom<decimal>();
            dateAtget = (ILCopyTo<DateTime>)FindDelegateTo<DateTime>();
            dateAtset = (ILCopyFrom<DateTime>)FindDelegateFrom<DateTime>();

            // arrays

            charArrAtget = (ILCopyToArr<char>)FindDelegateTo<char[]>();
            charArrAtset = (ILCopyFromArr<char>)FindDelegateFrom<char[]>();
            byteArrAtget = (ILCopyToArr<byte>)FindDelegateTo<byte[]>();
            byteArrAtset = (ILCopyFromArr<byte>)FindDelegateFrom<byte[]>();
            sbyteArrAtget = (ILCopyToArr<sbyte>)FindDelegateTo<sbyte[]>();
            sbyteArrAtset = (ILCopyFromArr<sbyte>)FindDelegateFrom<sbyte[]>();
            shortArrAtget = (ILCopyToArr<short>)FindDelegateTo<short[]>();
            shortArrAtset = (ILCopyFromArr<short>)FindDelegateFrom<short[]>();
            ushortArrAtget = (ILCopyToArr<ushort>)FindDelegateTo<ushort[]>();
            ushortArrAtset = (ILCopyFromArr<ushort>)FindDelegateFrom<ushort[]>();
            intArrAtget = (ILCopyToArr<int>)FindDelegateTo<int[]>();
            intArrAtset = (ILCopyFromArr<int>)FindDelegateFrom<int[]>();
            uintArrAtget = (ILCopyToArr<uint>)FindDelegateTo<uint[]>();
            uintArrAtset = (ILCopyFromArr<uint>)FindDelegateFrom<uint[]>();
            longArrAtget = (ILCopyToArr<long>)FindDelegateTo<long[]>();
            longArrAtset = (ILCopyFromArr<long>)FindDelegateFrom<long[]>();
            ulongArrAtget = (ILCopyToArr<ulong>)FindDelegateTo<ulong[]>();
            ulongArrAtset = (ILCopyFromArr<ulong>)FindDelegateFrom<ulong[]>();
            singleArrAtget = (ILCopyToArr<float>)FindDelegateTo<float[]>();
            singleArrAtset = (ILCopyFromArr<float>)FindDelegateFrom<float[]>();
            doubleArrAtget = (ILCopyToArr<double>)FindDelegateTo<double[]>();
            doubleArrAtset = (ILCopyFromArr<double>)FindDelegateFrom<double[]>();
            guidArrAtget = (ILCopyToArr<Guid>)FindDelegateTo<Guid[]>();
            guidArrAtset = (ILCopyFromArr<Guid>)FindDelegateFrom<Guid[]>();
            decimalArrAtget = (ILCopyToArr<decimal>)FindDelegateTo<decimal[]>();
            decimalArrAtset = (ILCopyFromArr<decimal>)FindDelegateFrom<decimal[]>();
            dateArrAtget = (ILCopyToArr<DateTime>)FindDelegateTo<DateTime[]>();
            dateArrAtset = (ILCopyFromArr<DateTime>)FindDelegateFrom<DateTime[]>();
        }

        private static void genDynList()
        {
            int i;
            int c;
            var l = new List<Type>();
            var il = new List<int>();
            int j;
            c = dyngenlist.Length - 1;
            var loopTo = c;
            for (i = 0; i <= loopTo; i++)
            {
                l.Add(Type.GetType(dyngenlist[i].FullName + "&"));
                il.Add(dyngensizes[i]);
            }

            var loopTo1 = c;
            for (i = 0; i <= loopTo1; i++)
            {
                l.Add(Type.GetType(dyngenlist[i].FullName + "[]"));
                il.Add(dyngensizes[i]);
            }

            DynamicMethod dynMtd;
            ILGenerator ilGen;
            string toptr;
            string fromptr;
            bool ir = false;
            j = 0;
            foreach (Type tn in l)
            {
                if (tn.IsArray)
                {
                    toptr = "mem" + tn.Name.Substring(0, tn.Name.Length - 2) + "arr";
                    fromptr = tn.Name.Substring(0, tn.Name.Length - 2) + "mem" + "arr";
                    dynMtd = new DynamicMethod(toptr, typeof(void), new[] { tn, typeof(IntPtr), typeof(uint) }, typeof(Native));
                    ilGen = dynMtd.GetILGenerator();
                    ilGen.Emit(OpCodes.Ldarg_0);
                    ilGen.Emit(OpCodes.Ldc_I4_0);
                    ilGen.Emit(OpCodes.Ldelema, tn.GetElementType());
                    ilGen.Emit(OpCodes.Ldarg_1);
                    ilGen.Emit(OpCodes.Ldarg_2);
                    ilGen.Emit(OpCodes.Volatile);
                    if (IntPtr.Size == 8)
                        ilGen.Emit(OpCodes.Unaligned, 1);
                    ilGen.Emit(OpCodes.Cpblk);
                    ilGen.Emit(OpCodes.Ret);
                    MethodList.Add((MulticastDelegate)dynMtd.CreateDelegate(typeof(ILCopyToArr<>).MakeGenericType(Type.GetType(tn.FullName.Substring(0, tn.FullName.Length - 2)))));
                    dynMtd = new DynamicMethod(fromptr, typeof(void), new[] { typeof(IntPtr), tn, typeof(uint) }, typeof(Native));
                    ilGen = dynMtd.GetILGenerator();
                    ilGen.Emit(OpCodes.Ldarg_0);
                    ilGen.Emit(OpCodes.Ldarg_1);
                    ilGen.Emit(OpCodes.Ldc_I4_0);
                    ilGen.Emit(OpCodes.Ldelema, tn.GetElementType());
                    ilGen.Emit(OpCodes.Ldarg_2);
                    ilGen.Emit(OpCodes.Volatile);
                    if (IntPtr.Size == 8)
                        ilGen.Emit(OpCodes.Unaligned, 1);
                    ilGen.Emit(OpCodes.Cpblk);
                    ilGen.Emit(OpCodes.Ret);
                    MethodList.Add((MulticastDelegate)dynMtd.CreateDelegate(typeof(ILCopyFromArr<>).MakeGenericType(Type.GetType(tn.FullName.Substring(0, tn.FullName.Length - 2)))));
                }
                else
                {
                    toptr = "mem" + tn.Name.Substring(0, tn.Name.Length - 1);
                    fromptr = tn.Name.Substring(0, tn.Name.Length - 1) + "mem";
                    dynMtd = new DynamicMethod(toptr, typeof(void), new[] { tn, typeof(IntPtr) }, typeof(Native));
                    ilGen = dynMtd.GetILGenerator();
                    ilGen.Emit(OpCodes.Ldarg_0);
                    ilGen.Emit(OpCodes.Ldarg_1);
                    EmitOpCodeForByteLength(ilGen, il[j]);
                    ilGen.Emit(OpCodes.Volatile);
                    if (IntPtr.Size == 8)
                        ilGen.Emit(OpCodes.Unaligned, 1);
                    ilGen.Emit(OpCodes.Cpblk);
                    ilGen.Emit(OpCodes.Ret);
                    MethodList.Add((MulticastDelegate)dynMtd.CreateDelegate(typeof(ILCopyTo<>).MakeGenericType(Type.GetType(tn.FullName.Substring(0, tn.FullName.Length - 1)))));
                    dynMtd = new DynamicMethod(fromptr, typeof(void), new[] { typeof(IntPtr), tn }, typeof(Native));
                    ilGen = dynMtd.GetILGenerator();
                    ilGen.Emit(OpCodes.Ldarg_0);
                    ilGen.Emit(OpCodes.Ldarg_1);
                    EmitOpCodeForByteLength(ilGen, il[j]);
                    ilGen.Emit(OpCodes.Volatile);
                    if (IntPtr.Size == 8)
                        ilGen.Emit(OpCodes.Unaligned, 1);
                    ilGen.Emit(OpCodes.Cpblk);
                    ilGen.Emit(OpCodes.Ret);
                    MethodList.Add((MulticastDelegate)dynMtd.CreateDelegate(typeof(ILCopyFrom<>).MakeGenericType(Type.GetType(tn.FullName.Substring(0, tn.FullName.Length - 1)))));
                }

                j += 1;
            }
        }

        private static void EmitOpCodeForByteLength(ILGenerator il, int l)
        {
            switch (l)
            {
                case 1:
                    {
                        il.Emit(OpCodes.Ldc_I4_1);
                        break;
                    }

                case 2:
                    {
                        il.Emit(OpCodes.Ldc_I4_2);
                        break;
                    }

                case 3:
                    {
                        il.Emit(OpCodes.Ldc_I4_3);
                        break;
                    }

                case 4:
                    {
                        il.Emit(OpCodes.Ldc_I4_4);
                        break;
                    }

                case 5:
                    {
                        il.Emit(OpCodes.Ldc_I4_5);
                        break;
                    }

                case 6:
                    {
                        il.Emit(OpCodes.Ldc_I4_6);
                        break;
                    }

                case 7:
                    {
                        il.Emit(OpCodes.Ldc_I4_7);
                        break;
                    }

                case 8:
                    {
                        il.Emit(OpCodes.Ldc_I4_8);
                        break;
                    }

                default:
                    {
                        il.Emit(OpCodes.Ldc_I4, l);
                        break;
                    }
            }
        }

        public static void CopyToIL<T>(ref T dest, IntPtr src)
        {
            ((ILCopyTo<T>)FindDelegateTo<T>()).Invoke(ref dest, src);
        }

        public static void CopyFromIL<T>(IntPtr dest, ref T src)
        {
            ((ILCopyFrom<T>)FindDelegateFrom<T>()).Invoke(dest, ref src);
        }

        public static void CopyToILArray<T>(T[] dest, IntPtr src, uint l)
        {
            ((ILCopyToArr<T>)FindDelegateTo<T[]>()).Invoke(dest, src, l);
        }

        public static void CopyFromILArray<T>(IntPtr dest, T[] src, uint l)
        {
            ((ILCopyFromArr<T>)FindDelegateFrom<T[]>()).Invoke(dest, src, l);
        }

        public static MulticastDelegate FindDelegateTo<T>()
        {
            var tn = typeof(T);
            string s;
            if (tn.IsArray)
            {
                s = "mem" + tn.Name.Substring(0, tn.Name.Length - 2) + "arr";
            }
            else
            {
                s = "mem" + tn.Name;
            }

            foreach (var mt in MethodList)
            {
                if ((mt.Method.Name ?? "") == (s ?? ""))
                {
                    return mt;
                }
            }

            return null;
        }

        public static MulticastDelegate FindDelegateFrom<T>()
        {
            var tn = typeof(T);
            string s;
            if (tn.IsArray)
            {
                s = tn.Name.Substring(0, tn.Name.Length - 2) + "mem" + "arr";
            }
            else
            {
                s = tn.Name + "mem";
            }

            foreach (var mt in MethodList)
            {
                if ((mt.Method.Name ?? "") == (s ?? ""))
                {
                    return mt;
                }
            }

            return null;
        }

        /// <summary>
        /// Determine whether an object is blittable and can be copied to a memory location.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool CanCopyObject(object obj)
        {
            bool CanCopyObjectRet = default;
            GCHandle gs = default;
            CanCopyObjectRet = internalCanCopyObject(obj, ref gs);
            if (CanCopyObjectRet)
                gs.Free();
            return CanCopyObjectRet;
        }

        /// <summary>
        /// Internal can-copy function.  Saves time by returning the allocated GCHandle.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="g"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static bool internalCanCopyObject(object obj, ref GCHandle g)
        {
            GCHandle gs = default;
            try
            {
                gs = GCHandle.Alloc(obj, GCHandleType.Pinned);
                var ptr = gs.AddrOfPinnedObject();
                g = gs;
                return true;
            }
            catch (Exception ex)
            {
                if (gs.IsAllocated)
                {
                    gs.Free();
                }

                g = new GCHandle();
                return false;
            }
        }

        /// <summary>
        /// Copy the contents of a memory location into a blittable object.
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="src"></param>
        /// <param name="byteLen"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool CopyObject(object dest, IntPtr src, uint byteLen)
        {
            var gs = new GCHandle();
            if (!internalCanCopyObject(dest, ref gs))
                return false;
            var ptr = gs.AddrOfPinnedObject();
            MemCpy(ptr, src, byteLen);
            gs.Free();
            return true;
        }

        /// <summary>
        /// Copy the contents of a blittable object into a memory location.
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="src"></param>
        /// <param name="byteLen"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool CopyObject(IntPtr dest, object src, uint byteLen)
        {
            var gs = new GCHandle();
            if (!internalCanCopyObject(src, ref gs))
                return false;
            var ptr = gs.AddrOfPinnedObject();
            MemCpy(dest, ptr, byteLen);
            gs.Free();
            return true;
        }

        /// <summary>
        /// Copy the contents of a memory location into a blittable object.  No checking is done.
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="src"></param>
        /// <param name="byteLen"></param>
        /// <remarks></remarks>
        public static void QuickCopyObject<T>(ref T dest, IntPtr src, uint byteLen)
        {
            var gs = GCHandle.Alloc(dest, GCHandleType.Pinned);
            var ptr = gs.AddrOfPinnedObject();
            MemCpy(ptr, src, byteLen);
            gs.Free();
        }

        /// <summary>
        /// Copy the contents of a blittable object into a memory location.  No checking is done.
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="src"></param>
        /// <param name="byteLen"></param>
        /// <remarks></remarks>
        public static void QuickCopyObject<T>(IntPtr dest, T src, uint byteLen)
        {
            var gs = GCHandle.Alloc(src, GCHandleType.Pinned);
            var ptr = gs.AddrOfPinnedObject();
            MemCpy(dest, ptr, byteLen);
            gs.Free();
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public static void RtlZeroMemory(IntPtr dest, IntPtr count)
        {
            if (Conversions.ToBoolean(count.ToInt64() & 0xFFFFFFFF00000000L))
            {
                n_memset(dest, 0, count);
            }
            else
            {
                MemSet(dest, 0, (uint)count);
            }
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Native Visual C Runtime memset.
        /// </summary>
        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "memset", PreserveSig = true)]
        public static extern void n_memset(IntPtr dest, int c, IntPtr count);

        /// <summary>
        /// Native Visual C Runtime memcpy.
        /// </summary>
        [DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        public static extern IntPtr n_memcpy(IntPtr dest, IntPtr src, UIntPtr count);

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// IL Code: Scans for a null character starting at the specified pointer.
        /// </summary>
        /// <param name="p">Pointer in memory at which to begin scanning.</param>
        /// <returns>The zero-based character index of the null character (the string length)</returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static int CharZero(IntPtr p)
        {
            return 0;
        }


        /// <summary>
        /// IL Code: Scans for a null byte at the specified pointer.
        /// </summary>
        /// <param name="p">Pointer in memory at which to begin scanning.</param>
        /// <returns>The zero-based index of the null character (the string length)</returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static int ByteZero(IntPtr p)
        {
            return 0;
        }


        // ' MemSet using CIL code.


        /// <summary>
        /// CIL MemSet.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemSet(IntPtr dest, byte @char, uint length)
        {
        }



        // ' All the MemCpy's!!!!


        /// <summary>
        /// IL Code: Copies memory from one memory pointer to another using IL cpblk.
        /// </summary>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(IntPtr dest, IntPtr src, uint length)
        {
        }

        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(IntPtr dest, byte[] src, uint length)
        {
        }

        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(byte[] dest, IntPtr src, uint length)
        {
        }

        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(IntPtr dest, sbyte[] src, uint length)
        {
        }

        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(sbyte[] dest, IntPtr src, uint length)
        {
        }

        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(IntPtr dest, char[] src, uint length)
        {
        }

        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(char[] dest, IntPtr src, uint length)
        {
        }

        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(IntPtr dest, short[] src, uint length)
        {
        }

        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(short[] dest, IntPtr src, uint length)
        {
        }

        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(IntPtr dest, ushort[] src, uint length)
        {
        }

        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(ushort[] dest, IntPtr src, uint length)
        {
        }

        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(IntPtr dest, int[] src, uint length)
        {
        }

        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(int[] dest, IntPtr src, uint length)
        {
        }

        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(IntPtr dest, uint[] src, uint length)
        {
        }

        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(uint[] dest, IntPtr src, uint length)
        {
        }

        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(IntPtr dest, long[] src, uint length)
        {
        }

        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(long[] dest, IntPtr src, uint length)
        {
        }


        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(IntPtr dest, ulong[] src, uint length)
        {
        }


        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(ulong[] dest, IntPtr src, uint length)
        {
        }


        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(IntPtr dest, float[] src, uint length)
        {
        }


        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(float[] dest, IntPtr src, uint length)
        {
        }


        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(IntPtr dest, double[] src, uint length)
        {
        }


        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(double[] dest, IntPtr src, uint length)
        {
        }


        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(IntPtr dest, System.Drawing.Color[] src, uint length)
        {
        }


        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(System.Drawing.Color[] dest, IntPtr src, uint length)
        {
        }


        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(IntPtr dest, DateTime[] src, uint length)
        {
        }


        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(DateTime[] dest, IntPtr src, uint length)
        {
        }


        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(IntPtr dest, decimal[] src, uint length)
        {
        }


        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(decimal[] dest, IntPtr src, uint length)
        {
        }


        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(IntPtr dest, Guid[] src, uint length)
        {
        }


        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(Guid[] dest, IntPtr src, uint length)
        {
        }



        // ' Additionals

        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(byte[] dest, byte[] src, uint length)
        {
        }


        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(byte[] dest, sbyte[] src, uint length)
        {
        }

        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(sbyte[] dest, byte[] src, uint length)
        {
        }


        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(byte[] dest, char[] src, uint length)
        {
        }

        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(char[] dest, byte[] src, uint length)
        {
        }


        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(byte[] dest, short[] src, uint length)
        {
        }

        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(short[] dest, byte[] src, uint length)
        {
        }


        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(byte[] dest, ushort[] src, uint length)
        {
        }


        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(ushort[] dest, byte[] src, uint length)
        {
        }


        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(byte[] dest, int[] src, uint length)
        {
        }


        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(int[] dest, byte[] src, uint length)
        {
        }


        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(byte[] dest, uint[] src, uint length)
        {
        }


        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(uint[] dest, byte[] src, uint length)
        {
        }


        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(byte[] dest, long[] src, uint length)
        {
        }


        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(long[] dest, byte[] src, uint length)
        {
        }

        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(byte[] dest, ulong[] src, uint length)
        {
        }

        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(ulong[] dest, byte[] src, uint length)
        {
        }

        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(byte[] dest, float[] src, uint length)
        {
        }

        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(float[] dest, byte[] src, uint length)
        {
        }

        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(byte[] dest, double[] src, uint length)
        {
        }

        /// <summary>
        /// CIL MemCpy using the cpblk instruction.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(double[] dest, byte[] src, uint length)
        {
        }


        // ' End Additionals


        // ' Blittibles!
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Grabs a Char from Bytes().
        /// </summary>
        /// <param name="src"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static char ToChar(byte[] src, uint index)
        {
            return default;
        }

        /// <summary>
        /// Grabs Bytes() from a Char.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static byte[] ToBytes(char src)
        {
            return null;
        }


        /// <summary>
        /// Grabs a Short from Bytes().
        /// </summary>
        /// <param name="src"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static short ToShort(byte[] src, uint index)
        {
            return default;
        }

        /// <summary>
        /// Grabs Bytes() from a Short.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static byte[] ToBytes(short src)
        {
            return null;
        }

        /// <summary>
        /// Grabs a UShort from Bytes().
        /// </summary>
        /// <param name="src"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static ushort ToUShort(byte[] src, uint index)
        {
            return default;
        }

        /// <summary>
        /// Grabs Bytes() from a UShort.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static byte[] ToBytes(ushort src)
        {
            return null;
        }

        /// <summary>
        /// Grabs a Integer from Bytes().
        /// </summary>
        /// <param name="src"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static int ToInteger(byte[] src, uint index)
        {
            return default;
        }

        /// <summary>
        /// Grabs Bytes() from a Integer.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static byte[] ToBytes(int src)
        {
            return null;
        }

        /// <summary>
        /// Grabs a UInteger from Bytes().
        /// </summary>
        /// <param name="src"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static uint ToUInteger(byte[] src, uint index)
        {
            return default;
        }

        /// <summary>
        /// Grabs Bytes() from a UInteger.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static byte[] ToBytes(uint src)
        {
            return null;
        }


        /// <summary>
        /// Grabs a Long from Bytes().
        /// </summary>
        /// <param name="src"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static long ToLong(byte[] src, uint index)
        {
            return default;
        }

        /// <summary>
        /// Grabs Bytes() from a Long.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static byte[] ToBytes(long src)
        {
            return null;
        }


        /// <summary>
        /// Grabs a ULong from Bytes().
        /// </summary>
        /// <param name="src"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static ulong ToULong(byte[] src, uint index)
        {
            return default;
        }

        /// <summary>
        /// Grabs Bytes() from a ULong.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static byte[] ToBytes(ulong src)
        {
            return null;
        }


        /// <summary>
        /// Grabs a Single from Bytes().
        /// </summary>
        /// <param name="src"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static float ToSingle(byte[] src, uint index)
        {
            return default;
        }

        /// <summary>
        /// Grabs Bytes() from a Single.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static byte[] ToBytes(float src)
        {
            return null;
        }


        /// <summary>
        /// Grabs a Double from Bytes().
        /// </summary>
        /// <param name="src"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static double ToDouble(byte[] src, uint index)
        {
            return default;
        }

        /// <summary>
        /// Grabs Bytes() from a Double.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static byte[] ToBytes(double src)
        {
            return null;
        }


        /// <summary>
        /// Grabs a Date from Bytes().
        /// </summary>
        /// <param name="src"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static DateTime ToDate(byte[] src, uint index)
        {
            return default;
        }

        /// <summary>
        /// Grabs Bytes() from a Date.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static byte[] ToBytes(DateTime src)
        {
            return null;
        }


        /// <summary>
        /// Grabs a Decimal from Bytes().
        /// </summary>
        /// <param name="src"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static decimal ToDecimal(byte[] src, uint index)
        {
            return default;
        }

        /// <summary>
        /// Grabs Bytes() from a Decimal.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static byte[] ToBytes(decimal src)
        {
            return null;
        }

        /// <summary>
        /// Grabs a Guid from Bytes().
        /// </summary>
        /// <param name="src"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static Guid ToGuid(byte[] src, uint index)
        {
            return default;
        }

        /// <summary>
        /// Grabs Bytes() from a Guid.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static byte[] ToBytes(Guid src)
        {
            return null;
        }



        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */



        // ' Array blittibles!
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Grabs a Char() array from Bytes().
        /// </summary>
        /// <param name="src"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static char[] ToChars(byte[] src, uint index)
        {
            return null;
        }

        /// <summary>
        /// Grabs Bytes() from Char() array.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static byte[] ToBytes(char[] src)
        {
            return null;
        }


        /// <summary>
        /// Grabs a Short() array from Bytes().
        /// </summary>
        /// <param name="src"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static short[] ToShorts(byte[] src, uint index)
        {
            return null;
        }

        /// <summary>
        /// Grabs Bytes() from Short() array.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static byte[] ToBytes(short[] src)
        {
            return null;
        }



        /// <summary>
        /// Grabs a UShort() array from Bytes().
        /// </summary>
        /// <param name="src"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static ushort[] ToUShorts(byte[] src, uint index)
        {
            return null;
        }

        /// <summary>
        /// Grabs Bytes() from UShort() array.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static byte[] ToBytes(ushort[] src)
        {
            return null;
        }



        /// <summary>
        /// Grabs a Integer() array from Bytes().
        /// </summary>
        /// <param name="src"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static int[] ToIntegers(byte[] src, uint index)
        {
            return null;
        }

        /// <summary>
        /// Grabs Bytes() from Integer() array.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static byte[] ToBytes(int[] src)
        {
            return null;
        }



        /// <summary>
        /// Grabs a UInteger() array from Bytes().
        /// </summary>
        /// <param name="src"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static uint[] ToUIntegers(byte[] src, uint index)
        {
            return null;
        }

        /// <summary>
        /// Grabs Bytes() from UInteger() array.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static byte[] ToBytes(uint[] src)
        {
            return null;
        }



        /// <summary>
        /// Grabs a Long() array from Bytes().
        /// </summary>
        /// <param name="src"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static long[] ToLongs(byte[] src, uint index)
        {
            return null;
        }

        /// <summary>
        /// Grabs Bytes() from Long() array.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static byte[] ToBytes(long[] src)
        {
            return null;
        }



        /// <summary>
        /// Grabs a ULong() array from Bytes().
        /// </summary>
        /// <param name="src"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static ulong[] ToULongs(byte[] src, uint index)
        {
            return null;
        }

        /// <summary>
        /// Grabs Bytes() from ULong() array.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static byte[] ToBytes(ulong[] src)
        {
            return null;
        }




        /// <summary>
        /// Grabs a Single() array from Bytes().
        /// </summary>
        /// <param name="src"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static float[] ToSingles(byte[] src, uint index)
        {
            return null;
        }

        /// <summary>
        /// Grabs Bytes() from Single() array.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static byte[] ToBytes(float[] src)
        {
            return null;
        }


        /// <summary>
        /// Grabs a Double() array from Bytes().
        /// </summary>
        /// <param name="src"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static double[] ToDoubles(byte[] src, uint index)
        {
            return null;
        }

        /// <summary>
        /// Grabs Bytes() from Double() array.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static byte[] ToBytes(double[] src)
        {
            return null;
        }



        /// <summary>
        /// Grabs a Date() array from Bytes().
        /// </summary>
        /// <param name="src"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static DateTime[] ToDates(byte[] src, uint index)
        {
            return null;
        }

        /// <summary>
        /// Grabs Bytes() from Date() array.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static byte[] ToBytes(DateTime[] src)
        {
            return null;
        }


        /// <summary>
        /// Grabs a Decimal() array from Bytes().
        /// </summary>
        /// <param name="src"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static decimal[] ToDecimals(byte[] src, uint index)
        {
            return null;
        }

        /// <summary>
        /// Grabs Bytes() from Decimal() array.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static byte[] ToBytes(decimal[] src)
        {
            return null;
        }


        /// <summary>
        /// Grabs a Guid() array from Bytes().
        /// </summary>
        /// <param name="src"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static Guid[] ToGuids(byte[] src, uint index)
        {
            return null;
        }

        /// <summary>
        /// Grabs Bytes() from Guid() array.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static byte[] ToBytes(Guid[] src)
        {
            return null;
        }


        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */







        // ' bit-wise verbatim tools

        /// <summary>
        /// Bit-wise, verbatim translation from <see cref="Long"/> to <see cref="IntPtr"/>.
        /// </summary>
        /// <param name="value"><see cref="Long"/> value to convert.</param>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static IntPtr CIntPtr(long value)
        {
            return default;
        }

        /// <summary>
        /// Bit-wise, verbatim translation from <see cref="Integer"/> to <see cref="IntPtr"/>.
        /// </summary>
        /// <param name="value"><see cref="Integer"/> value to convert.</param>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static IntPtr CIntPtr(int value)
        {
            return default;
        }

        /// <summary>
        /// Bit-wise, verbatim translation from <see cref="Intptr"/> to <see cref="UIntPtr"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static UIntPtr ToUnsigned(IntPtr v)
        {
            return (UIntPtr)0;
        }

        /// <summary>
        /// Bit-wise, verbatim translation from <see cref="UIntptr"/> to <see cref="IntPtr"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static IntPtr ToSigned(UIntPtr v)
        {
            return (IntPtr)0;
        }

        /// <summary>
        /// Bit-wise, verbatim transation from signed to unsigned.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static byte ToUnsigned(sbyte v)
        {
            return 0;
        }

        /// <summary>
        /// Bit-wise, verbatim transation from unsigned to signed.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static sbyte ToSigned(byte v)
        {
            return 0;
        }

        /// <summary>
        /// Bit-wise, verbatim transation from signed to unsigned.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static ushort ToUnsigned(short v)
        {
            return 0;
        }

        /// <summary>
        /// Bit-wise, verbatim transation from unsigned to signed.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static short ToSigned(ushort v)
        {
            return 0;
        }

        /// <summary>
        /// Bit-wise, verbatim transation from signed to unsigned.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static uint ToUnsigned(int v)
        {
            return 0U;
        }

        /// <summary>
        /// Bit-wise, verbatim transation from unsigned to signed.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static int ToSigned(uint v)
        {
            return 0;
        }

        /// <summary>
        /// Bit-wise, verbatim transation from signed to unsigned.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static ulong ToUnsigned(long v)
        {
            return 0UL;
        }

        /// <summary>
        /// Bit-wise, verbatim transation from unsigned to signed.
        /// </summary>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static long ToSigned(ulong v)
        {
            return 0L;
        }


        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /// <summary>
        /// Returns true for all unsigned primitives (and enumerations) and UIntPtr. Returns false otherwise.
        /// </summary>
        /// <param name="t">The type to test for the specified condition.</param>
        /// <returns>True or False</returns>
        /// <remarks></remarks>
        public static bool Unsigned(Type t)
        {
            bool UnsignedRet = default;
            switch (t)
            {
                case var @case when @case == typeof(ushort):
                case var case1 when case1 == typeof(byte):
                case var case2 when case2 == typeof(ulong):
                case var case3 when case3 == typeof(uint):
                case var case4 when case4 == typeof(UIntPtr):
                    {
                        UnsignedRet = true;
                        break;
                    }

                default:
                    {
                        if (t.IsEnum == true)
                        {
                            UnsignedRet = Unsigned(t.GetEnumUnderlyingType());
                        }
                        else
                        {
                            UnsignedRet = false;
                        }

                        break;
                    }
            }

            return UnsignedRet;
        }
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    // 'Public Module UNative

    // '#Region "Supporting Types for Declares"

    // '    Public Enum ArchitectureType As Short

    // '        ''' <summary>
    // '        ''' 32-bit system.
    // '        ''' </summary>
    // '        ''' <remarks></remarks>
    // '        x86 = 0

    // '        ''' <summary>
    // '        ''' Iatium-based system.
    // '        ''' </summary>
    // '        ''' <remarks></remarks>
    // '        IA64 = 6

    // '        ''' <summary>
    // '        ''' 64-bit system.
    // '        ''' </summary>
    // '        ''' <remarks></remarks>
    // '        x64 = 9

    // '    End Enum

    // '    <StructLayout(LayoutKind.Sequential, Pack:=1)> _
    // '    Structure SYSTEM_INFO
    // '        Public wProcessorArchitecture As ArchitectureType
    // '        Public wReserved As Short
    // '        Public dwPageSize As Integer
    // '        Public lpMinimumApplicationAddress As UIntPtr
    // '        Public lpMaximumApplicationAddress As UIntPtr
    // '        Public dwActiveProcessorMask As Integer
    // '        Public dwNumberOfProcessors As Integer
    // '        Public dwProcessorType As Integer
    // '        Public dwAllocationGranularity As Integer
    // '        Public wProcessorLevel As Short
    // '        Public wProcessorRevision As Short
    // '    End Structure

    // '#End Region

    // '#Region "System Information"

    // '    Declare Function GetSystemInfo Lib "kernel32" (ByRef lpSysInfo As SYSTEM_INFO) As UIntPtr

    // '#End Region

    // '#Region "Memory Functions"

    // '#Region "NetApi Memory Functions"

    // '    Public Declare Function NetApiBufferAllocate Lib "netapi32.dll" (ByteCount As Integer, ByRef Buffer As UIntPtr) As Integer
    // '    Public Declare Function NetApiBufferFree Lib "netapi32.dll" (Buffer As UIntPtr) As Integer

    // '#End Region

    // '#Region "Heap Functions"

    // '    Declare Function HeapCreate Lib "kernel32" (dwOptions As Integer, _
    // '                                                initSize As UIntPtr, _
    // '                                                maxSize As UIntPtr) As UIntPtr

    // '    Declare Function HeapDestroy Lib "kernel32" (hHeap As UIntPtr) As Boolean

    // '    Declare Function GetProcessHeap Lib "kernel32" () As UIntPtr

    // '    Declare Function HeapQueryInformation Lib "kernel32" (HeapHandle As UIntPtr, _
    // '                                                          HeapInformationClass As Integer, _
    // '                                                          ByRef HeapInformation As UInteger, _
    // '                                                          HeapInformationLength As UIntPtr, _
    // '                                                          ReturnLength As UIntPtr) As Boolean

    // '#End Region

    // '#Region "Memory Blocks"

    // '    '' As per the MSDN manual, we're using ONLY Heap functions, here.

    // '    Declare Function HeapAlloc Lib "kernel32" (hHeap As UIntPtr, _
    // '                                               dwOptions As Integer, _
    // '                                               dwBytes As UIntPtr) As UIntPtr

    // '    Declare Function HeapReAlloc Lib "kernel32" (hHeap As UIntPtr, _
    // '                                                 dwOptions As Integer, _
    // '                                                 lpMem As UIntPtr, _
    // '                                                 dwBytes As UIntPtr) As UIntPtr

    // '    Declare Function HeapFree Lib "kernel32" (hHeap As UIntPtr, _
    // '                                               dwOptions As Integer, _
    // '                                               lpMem As UIntPtr) As UIntPtr

    // '    Declare Function HeapSize Lib "kernel32" (hHeap As UIntPtr, _
    // '                                              dwOptions As Integer, _
    // '                                              lpMem As UIntPtr) As UIntPtr

    // '    Declare Function HeapValidate Lib "kernel32" (hHeap As UIntPtr, _
    // '                                              dwOptions As Integer, _
    // '                                              lpMem As UIntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

    // '    '' used for specific operating system functions.
    // '    Declare Function LocalFree Lib "kernel32.dll" (hMem As UIntPtr) As UIntPtr

    // '    '' used for specific operating system functions.
    // '    Declare Function GlobalFree Lib "kernel32.dll" (hMem As UIntPtr) As UIntPtr

    // '#End Region

    // '#Region "CopyMemory"

    // '#Region "Pointer to Pointer"

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    // '                                                                 pSrc As UIntPtr, _
    // '                                                                 byteLen As UIntPtr)
    // '#End Region

    // '#Region "Object to Pointer"

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (<MarshalAs(UnmanagedType.AsAny)> _
    // '                                                                 pDst As Object, _
    // '                                                                 pSrc As UIntPtr, _
    // '                                                                 byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    // '                                                                 <MarshalAs(UnmanagedType.AsAny)> _
    // '                                                                 pSrc As Object, _
    // '                                                                 byteLen As UIntPtr)

    // '#End Region

    // '#Region "Arrays"

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    // '                                                                     pSrc As Char(), _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As Char(), _
    // '                                                                     pSrc As UIntPtr, _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    // '                                                                     pSrc As Byte(), _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As Byte(), _
    // '                                                                     pSrc As UIntPtr, _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    // '                                                                     pSrc As SByte(), _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As SByte(), _
    // '                                                                     pSrc As UIntPtr, _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    // '                                                                     pSrc As ULong(), _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As ULong(), _
    // '                                                                     pSrc As UIntPtr, _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    // '                                                                     pSrc As UShort(), _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UShort(), _
    // '                                                                     pSrc As UIntPtr, _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    // '                                                                     pSrc As UInteger(), _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UInteger(), _
    // '                                                                     pSrc As UIntPtr, _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    // '                                                                     pSrc As Long(), _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As Long(), _
    // '                                                                     pSrc As UIntPtr, _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    // '                                                                     pSrc As Short(), _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As Short(), _
    // '                                                                     pSrc As UIntPtr, _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    // '                                                                     pSrc As Integer(), _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As Integer(), _
    // '                                                                     pSrc As UIntPtr, _
    // '                                                                     byteLen As UIntPtr)

    // '#End Region

    // '#Region "Scalar Declares"

    // '    '' UIntPtr
    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    // '                                                                     ByRef pSrc As Byte, _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As Byte, _
    // '                                                                     pSrc As UIntPtr, _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    // '                                                                     ByRef pSrc As SByte, _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As SByte, _
    // '                                                                     pSrc As UIntPtr, _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    // '                                                                     ByRef pSrc As ULong, _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As ULong, _
    // '                                                                     pSrc As UIntPtr, _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    // '                                                                     ByRef pSrc As UShort, _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As UShort, _
    // '                                                                     pSrc As UIntPtr, _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    // '                                                                     ByRef pSrc As UInteger, _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As UInteger, _
    // '                                                                     pSrc As UIntPtr, _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    // '                                                                     ByRef pSrc As Long, _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As Long, _
    // '                                                                     pSrc As UIntPtr, _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    // '                                                                     ByRef pSrc As Short, _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As Short, _
    // '                                                                     pSrc As UIntPtr, _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    // '                                                                     ByRef pSrc As Char, _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As Char, _
    // '                                                                     pSrc As UIntPtr, _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    // '                                                                     ByRef pSrc As Integer, _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As Integer, _
    // '                                                                     pSrc As UIntPtr, _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    // '                                                                     <MarshalAs(UnmanagedType.LPWStr)> pSrc As String, _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (<MarshalAs(UnmanagedType.LPWStr)> pDst As String, _
    // '                                                                     pSrc As UIntPtr, _
    // '                                                                     byteLen As UIntPtr)

    // '#End Region

    // '#Region "Unique Types"

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    // '                                                                     ByRef pSrc As Date, _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As Date, _
    // '                                                                     pSrc As UIntPtr, _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    // '                                                                     pSrc As Date(), _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As Date(), _
    // '                                                                     pSrc As UIntPtr, _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    // '                                                                     ByRef pSrc As System.Guid, _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As System.Guid, _
    // '                                                                     pSrc As UIntPtr, _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    // '                                                                     pSrc As System.Guid(), _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As System.Guid(), _
    // '                                                                     pSrc As UIntPtr, _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    // '                                                                     pSrc As BigInteger, _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As BigInteger, _
    // '                                                                     pSrc As UIntPtr, _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    // '                                                                     pSrc As System.Drawing.Color, _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDst As System.Drawing.Color, _
    // '                                                                     pSrc As UIntPtr, _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As UIntPtr, _
    // '                                                                     pSrc As System.Drawing.Color(), _
    // '                                                                     byteLen As UIntPtr)

    // '    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As System.Drawing.Color(), _
    // '                                                                     pSrc As UIntPtr, _
    // '                                                                     byteLen As UIntPtr)

    // '#End Region

    // '#End Region

    // '#Region "Zero Memory"

    // '    Declare Sub RtlZeroMemory Lib "kernel32" Alias "RtlZeroMemory" (pDst As UIntPtr, _
    // '                                                                             byteLen As UIntPtr)

    // '#End Region

    // '#End Region

    // '    ' Friend myHeapPtr As UIntPtr
    // '    ' Friend myHeap As New Mem

    // '    Public Function Unsigned(t As System.Type) As Boolean
    // '        Select Case t
    // '            Case GetType(UShort), GetType(Byte), GetType(ULong), GetType(UInteger)
    // '                Unsigned = True
    // '            Case Else
    // '                Unsigned = False
    // '        End Select
    // '    End Function

    // 'End Module

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
}
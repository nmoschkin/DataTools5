// ' ************************************************* ''
// ' DataTools Visual Basic Utility Library 
// '
// ' Module: Heap
// '         Wrapper for system memory heaps.
// ' 
// ' Copyright (C) 2011-2020 Nathan Moschkin
// ' All Rights Reserved
// '
// ' Licensed Under the Microsoft Public License   
// ' ************************************************* ''

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using DataTools.Memory.Internal;

namespace DataTools.Memory
{

    // ' *********************************
    // ' *********************************
    // ' I N T E R F A C E S
    // ' *********************************
    // ' *********************************

    public interface IMemoryHeap<T> where T : IDisposable
    {

        /// <summary>
        /// Returns the underlying pointer of this heap.
        /// </summary>
        /// <returns></returns>
        IntPtr DangerousGetHandle();

        /// <summary>
        /// Gets a value indicating whether the heap has been destroyed.
        /// </summary>
        /// <returns></returns>
        bool IsDestroyed { get; }

        /// <summary>
        /// Returns true if this heap has a maximum size.
        /// </summary>
        /// <returns></returns>
        bool HasMaxSize { get; }

        /// <summary>
        /// Returns the maximum size of the heap.
        /// </summary>
        /// <returns></returns>
        IntPtr MaxSize { get; }

        /// <summary>
        /// Returns the initial size of the heap.
        /// </summary>
        /// <returns></returns>
        IntPtr InitialSize { get; }

        /// <summary>
        /// Returns an array of objects created from this heap
        /// </summary>
        /// <returns></returns>
        T[] GetItems();

        /// <summary>
        /// Create a new memory object from this heap.
        /// </summary>
        /// <returns></returns>
        T CreateItem();

        /// <summary>
        /// Destroys the allocation associated with the object, and calls its Dispose method.
        /// </summary>
        /// <param name="item"></param>
        void DestroyItem(T item);

        /// <summary>
        /// Destroys the buffer and all of its contents.
        /// </summary>
        /// <returns></returns>
        bool DestroyHeap();
    }

    // ' *********************************
    // ' *********************************
    // ' C L A S S E S  S T A R T  H E R E
    // ' *********************************
    // ' *********************************

    // ' *********************************
    // ' *********************************
    // ' BlobHeap
    // ' *********************************
    // ' *********************************

    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /// <summary>
    /// Represents a private heap in memory for
    /// a collection of blobs.
    /// </summary>
    public class BlobHeap : SafeHandle, IMemoryHeap<Blob>
    {
        protected IntPtr _max;
        protected IntPtr _init;
        private bool _isDestroyed;
        protected List<Blob> _List = new List<Blob>();
        private static BlobHeap _def = null;

        /// <summary>
        /// Gets or sets the default heap for creating all new instances of Blob in this process.
        /// </summary>
        /// <returns></returns>
        public static BlobHeap DefaultHeap
        {
            get
            {
                return _def;
            }

            set
            {
                _def = value;
                Blob.defaultHeap = _def.DangerousGetHandle();
            }
        }

        /// <summary>
        /// Returns the process heap wrapped in a BlobHeap object.
        /// </summary>
        /// <returns></returns>
        public static BlobHeap ProcessHeap { get; private set; }

        /// <summary>
        /// Returns true if this heap has a maximum size.
        /// </summary>
        /// <returns></returns>
        public virtual bool HasMaxSize
        {
            get
            {
                return _max != (IntPtr)0;
            }
        }

        /// <summary>
        /// Returns the maximum size of the heap.
        /// </summary>
        /// <returns></returns>
        public virtual IntPtr MaxSize
        {
            get
            {
                return _max;
            }
        }

        /// <summary>
        /// Returns the initial size of the heap.
        /// </summary>
        /// <returns></returns>
        public virtual IntPtr InitialSize
        {
            get
            {
                return _init;
            }
        }

        /// <summary>
        /// Returns an array of objects created from this heap
        /// </summary>
        /// <returns></returns>
        public virtual Blob[] GetItems()
        {
            return _List.ToArray();
        }

        public virtual Blob[] GetMembers() => GetItems();

        /// <summary>
        /// Create a new memory object from this heap.
        /// </summary>
        /// <returns></returns>
        public Blob CreateItem()
        {
            return new Blob() { activeHeap = this, hHeap = handle };
        }

        public Blob CreateObject() => CreateItem();

        /// <summary>
        /// Destroys the object, and calls its IDisposable.Dispose method.
        /// </summary>
        /// <param name="bl"></param>
        public void DestroyItem(Blob bl)
        {
            bl.Dispose();
            if (_List.Contains(bl))
                _List.Remove(bl);
        }

        public void DestroyObject(Blob bl) => DestroyItem(bl);

        internal void AddSelf(Blob bl)
        {
            if (!_List.Contains(bl))
                _List.Add(bl);
        }

        internal void RemoveSelf(Blob bl)
        {
            _List.Remove(bl);
        }

        public new IntPtr DangerousGetHandle()
        {
            return handle;
        }

        /// <summary>
        /// Returns a value indicating that the heap is invalid.
        /// </summary>
        /// <returns></returns>
        public override bool IsDestroyed
        {
            get
            {
                bool IsDestroyedRet = default;
                IsDestroyedRet = handle == (IntPtr)0;
                return IsDestroyedRet;
            }
        }

        /// <summary>
        /// Release the handle and destroy the heap.
        /// </summary>
        /// <returns></returns>
        protected override bool DestroyHeap()
        {
            if (Native.HeapValidate(handle, 0U, (IntPtr)0))
            {
                // For Each bl In _List
                // If Threading.Monitor.TryEnter(bl) Then
                // bl.Dispose()
                // Threading.Monitor.Exit(bl)
                // End If
                // Next

                // ' All blobs on this heap will now have invalid pointers, please use with caution.
                _List.Clear();
                Native.HeapDestroy(handle);
                handle = (IntPtr)0;
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override bool ReleaseHandle() => DestroyHeap();

        /// <summary>
        /// Create a new heap with an initial size and a maximum size.
        /// </summary>
        /// <param name="initSize">Initial size, in bytes, of the heap.</param>
        /// <param name="maxSize">Maximum size, in bytes, of the heap.</param>
        public BlobHeap(IntPtr initSize, IntPtr maxSize) : base((IntPtr)0, true)
        {
            long i = (long)initSize;
            int ps = SystemInformation.SysInfo.SystemInfo.dwPageSize;
            if (i < ps)
            {
                i = ps;
            }
            else
            {
                i = i + (ps - i % ps);
            }

            initSize = (IntPtr)i;
            handle = Native.HeapCreate(4, initSize, maxSize);
            _init = initSize;
            _max = maxSize;
        }

        /// <summary>
        /// Create a new heap with an initial size and an unlimited maximum size.
        /// </summary>
        /// <param name="initSize">Initial size, in bytes, of the heap.</param>
        public BlobHeap(IntPtr initSize) : this(initSize, (IntPtr)0)
        {
        }

        /// <summary>
        /// Create a new heap with an initial size of the system page size.
        /// </summary>
        public BlobHeap() : this((IntPtr)SystemInformation.SysInfo.SystemInfo.dwPageSize, (IntPtr)0)
        {
        }

        /// <summary>
        /// Wrap the process heap in a BlobHeap class.
        /// </summary>
        /// <param name="a"></param>
        private BlobHeap(bool a) : base((IntPtr)0, false)
        {
            handle = Native.GetProcessHeap();
        }

        static BlobHeap()
        {
            // ' make the process heap object.
            ProcessHeap = new BlobHeap(true);

            // ' set the default heap to the process heap.
            DefaultHeap = ProcessHeap;
        }

        public override string ToString()
        {
            return handle.ToString();
        }

        public static implicit operator IntPtr(BlobHeap operand)
        {
            return operand.handle;
        }
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
}
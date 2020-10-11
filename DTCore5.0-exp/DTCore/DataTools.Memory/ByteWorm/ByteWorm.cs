// ' ************************************************* ''
// ' DataTools Visual Basic Utility Library 
// '
// ' Module: ByteWorm
// '         Divides a stream of bytes into 
// '         a virtual collection Of pieces.
// ' 
// ' Copyright (C) 2011-2020 Nathan Moschkin
// ' All Rights Reserved
// '
// ' Licensed Under the Microsoft Public License   
// ' ************************************************* ''

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic.CompilerServices;

namespace DataTools.Memory
{

    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    public enum ByteWormStatus : int
    {
        OutOfBounds = -1,
        None = 0,
        NoMore = 1,
        More = 2,
        Less = 3,
        NoLess = 4,
        JustOne = 5
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct WormRecord
    {
        public Guid Guid;
        public int Length;
        [MarshalAs(UnmanagedType.LPWStr)]
        public byte[] Data;


        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public static explicit operator WormRecord(Blob operand)
        {
            var w = new WormRecord();
            w.Guid = operand.get_GuidAt(0L);
            w.Length = operand.get_IntegerAtAbsolute(16L);
            w.Data = new byte[w.Length];
            w.Data = operand.GrabBytes((IntPtr)20, w.Length);
            return w;
        }

        public static explicit operator Blob(WormRecord operand)
        {
            var bl = new Blob();
            bl = bl + operand.Guid;
            bl += operand.Length;
            bl += operand.Data;
            return bl;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */

    }


    /// <summary>
    /// Collects a stream of bytes into a virtual collection of pieces.
    /// A deliberately light-weight class designed to be used as a player (forward and reverse.)
    /// </summary>
    /// <remarks></remarks>
    public sealed class ByteWorm : IEnumerable<byte[]>, IDisposable
    {

        // ' The stream
        private Blob mHeap;

        // ' The size of the chunks
        private int[] mSizes;

        // ' The start position of the chunks
        private int[] mPos;

        // ' the current index
        private int mIdx = -1;

        // ' size in virtual elements
        private int mSize = 0;

        // ' current status
        private ByteWormStatus mStatus = ByteWormStatus.None;

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        // ' the on/off switch, if the switch is off, no commands perform any functions
        // ' this is used internally to prevent others from having access while a 
        // ' complicated task is being carried out.  Useful for multithreading.
        private bool mOff = false;
        private bool mUserOff = false;

        // ' Internally toggle "the big off."  These override any external setting of the Off parameter.
        // ' The value will be restored to the user's state when the task is completed.
        private void OffToggle(object toggle = null)
        {

            // System.Threading.Monitor.Enter(Off)
            if (Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(toggle, null, false)))
                mOff = !mOff;
            else
                mOff = Conversions.ToBoolean(toggle);
            // System.Threading.Monitor.Exit(Off)


        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public byte[] GetWorm()
        {
            return mHeap;
        }

        public void SetWorm(ref byte[] bytes, [Optional, DefaultParameterValue(null)] ref int[] sizes)
        {
            if (bytes is null)
                return;
            if (Off)
                return;
            else
                OffToggle();
            mHeap.SetBytes((IntPtr)0, bytes);
            mHeap.Length = bytes.Length;
            OffToggle();
            if (sizes is object)
            {
                Partition(sizes);
            }
            else
            {
                this.Partition(new[] { mHeap.Length }, ByteWormStatus.JustOne);
            }
        }

        public void Partition(int[] sizes)
        {
            Partition(sizes, ByteWormStatus.NoMore);
        }

        internal void Partition(int[] sizes, ByteWormStatus status)
        {
            if (sizes is null)
                return;
            if (Off)
                return;
            else
                OffToggle();
            int i = 0;
            int c = sizes.Length - 1;
            int d = 0;
            int e = 0;
            mPos = new int[c + 1];
            mSizes = new int[c + 1];
            mSize = 0;
            mIdx = -1;
            var loopTo = c;
            for (i = 0; i <= loopTo; i++)
            {
                mPos[i] = e;
                mSizes[i] = sizes[i];
                e += sizes[i];
            }

            mSize = c + 1;
            mIdx = c;
            mStatus = status;
            OffToggle();
        }

        public void Clear()
        {
            if (Off)
                return;
            else
                OffToggle();
            mHeap.Clear();
            mPos = null;
            mSizes = null;
            mSize = 0;
            mIdx = -1;
            mStatus = ByteWormStatus.None;
            if (Off)
                OffToggle();
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public byte[] this[int index]
        {
            get
            {
                if (Off)
                    return null;
                else
                    OffToggle();
                if (index < 0 || index >= mSize)
                {
                    mStatus = ByteWormStatus.OutOfBounds;
                    OffToggle();
                    return null;
                }

                byte[] b;
                b = mHeap.GrabBytes((IntPtr)mPos[index], mSizes[index]);
                OffToggle();
                return b;
            }
        }

        public byte[] Piece
        {
            get
            {
                return this[mIdx];
            }
        }

        public ByteWormStatus Status
        {
            get
            {
                return mStatus;
            }
        }

        public int Count
        {
            get
            {
                return mSize;
            }
        }

        public int BufferLength
        {
            get
            {
                return (int)mHeap.Length;
            }
        }

        public int Index
        {
            get
            {
                return mIdx;
            }

            set
            {
                if (Off)
                    return;
                ByteWormStatus argstatus = 0;
                ToIndex(value, status: ref argstatus);
            }
        }

        public bool Off
        {
            get
            {
                return mUserOff | mOff;
            }

            set
            {
                mUserOff = value;
            }
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public int Add(byte[] value)
        {
            if (Off)
                return -1;
            // ' We don't want empty objects. This is a worm!  It needs pieces! LOL.
            if (value is null)
                return -1;
            int l = value.Length;
            OffToggle();
            Array.Resize(ref mPos, mSize + 1);
            Array.Resize(ref mSizes, mSize + 1);
            mPos[mSize] = (int)mHeap.Length;
            mSizes[mSize] = l;
            mIdx = mSize;
            mHeap += value;
            mSize += 1;
            mStatus = ByteWormStatus.NoMore;
            OffToggle();
            return mIdx;
        }

        public int Truncate(int idx = -1)
        {
            if (Off)
                return -1;
            int x;
            if (idx == -1)
            {
                idx = mIdx;
            }

            if (idx > mSize - 1)
            {
                mStatus = ByteWormStatus.OutOfBounds;
                return -1;
            }
            else if (idx == mSize - 1)
            {
                return idx;
            }

            OffToggle();
            Array.Resize(ref mSizes, idx + 1);
            Array.Resize(ref mPos, idx + 1);
            x = mSizes[idx] + mPos[idx];
            mHeap.Length = x;
            mSize = idx + 1;
            mIdx = idx;
            if (mSize == 1)
            {
                mStatus = ByteWormStatus.JustOne;
            }
            else
            {
                mStatus = ByteWormStatus.NoMore;
            }

            OffToggle();
            return idx;
        }

        public int Shift()
        {
            if (Off)
                return -1;
            if (mSize == 0)
                return -1;
            if (mSize == 1)
            {
                Clear();
                return -1;
            }

            OffToggle();
            int c;
            int e = mSizes[0];
            c = (int)(mHeap.Length - e);
            Array.Copy((byte[])mHeap, e, (byte[])mHeap, 0, c);
            mHeap.Length = c;
            mSize -= 1;
            c = mSize - 1;
            Array.Copy(mSizes, 1, mSizes, 0, mSize);
            Array.Copy(mPos, 1, mPos, 0, mSize);
            Array.Resize(ref mSizes, c + 1);
            Array.Resize(ref mPos, c + 1);
            mIdx = mSize - 1;
            OffToggle();
            return mIdx;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public byte[] ToIndex(int index, [Optional, DefaultParameterValue(default(ByteWormStatus))] ref ByteWormStatus status)
        {
            if (Off)
                return null;
            if (index == mIdx)
            {
                if (status != default(int))
                    status = mStatus;
                return this[mIdx];
            }

            if (index >= mSize - 1)
            {
                index = mSize;
                mStatus = ByteWormStatus.OutOfBounds;
                if (status != default(int))
                    status = mStatus;
                return null;
            }
            else if (index < 0)
            {
                index = -1;
                mStatus = ByteWormStatus.OutOfBounds;
                if (status != default(int))
                    status = mStatus;
                return null;
            }

            if (mSize == 1)
            {
                mStatus = ByteWormStatus.JustOne;
            }
            else if (mIdx < index)
            {
                mStatus = ByteWormStatus.Less;
            }
            else
            {
                mStatus = ByteWormStatus.More;
            }

            mIdx = index;
            if (status != default(int))
                status = mStatus;
            return this[index];
        }

        public byte[] Forward([Optional, DefaultParameterValue(default(ByteWormStatus))] ref ByteWormStatus status)
        {
            byte[] ForwardRet = default;
            if (Off)
                return null;
            mIdx += 1;
            if (mIdx >= mSize)
            {
                mIdx = mSize - 1;
                mStatus = ByteWormStatus.NoMore;
                if (status != default(int))
                    status = mStatus;
                return null;
            }

            ForwardRet = this[mIdx];
            if (mSize == 1)
            {
                mStatus = ByteWormStatus.JustOne;
            }
            else
            {
                mStatus = ByteWormStatus.More;
                if (status != default(int))
                    status = mStatus;
            }

            return ForwardRet;
        }

        public byte[] Backward([Optional, DefaultParameterValue(default(ByteWormStatus))] ref ByteWormStatus status)
        {
            byte[] BackwardRet = default;
            if (Off)
                return null;
            mIdx -= 1;
            if (mIdx < 0)
            {
                mIdx = 0;
                mStatus = ByteWormStatus.NoLess;
                if (status != default(int))
                    status = mStatus;
                return null;
            }

            BackwardRet = this[mIdx];
            if (mSize == 1)
            {
                mStatus = ByteWormStatus.JustOne;
            }
            else
            {
                mStatus = ByteWormStatus.Less;
                if (status != default(int))
                    status = mStatus;
            }

            return BackwardRet;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public IEnumerator GetEnumerator()
        {
            if (Off)
                return null;
            var argworm = this;
            return new ByteWormEnumerator(ref argworm);
        }

        public IEnumerator<byte[]> GetEnumerator()
        {
            if (Off)
                return null;
            var argworm = this;
            return new ByteWormEnumerator(ref argworm);
        }

        public IEnumerator<byte[]> GetEnumerator1() => GetEnumerator();

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        private bool disposedValue; // To detect redundant calls

        // IDisposable
        protected void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (mHeap is object)
                    mHeap.Free();
            }

            disposedValue = true;
        }

        ~ByteWorm()
        {
            // Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(false);
        }

        // This code added by Visual Basic to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        public ByteWorm()
        {
            mHeap = new Blob();
        }
    }

    public class ByteWormEnumerator : IEnumerator<byte[]>
    {
        private int mIdx = -1;
        private ByteWorm mWorm;

        public ByteWorm Worm
        {
            get
            {
                return mWorm;
            }

            set
            {
                mWorm = value;
            }
        }

        public ByteWormEnumerator(ref ByteWorm worm)
        {
            mWorm = worm;
        }

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public byte[] Current
        {
            get
            {
                return mWorm[mIdx];
            }
        }

        public object Current
        {
            get
            {
                return mWorm[mIdx];
            }
        }

        public bool MoveNext()
        {
            if (mIdx >= mWorm.Count - 1)
                return false;
            mIdx += 1;
            return true;
        }

        public void Reset()
        {
            mIdx = -1;
        }

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        private bool disposedValue; // To detect redundant calls

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    mWorm = null;
                }
            }

            disposedValue = true;
        }

        // This code added by Visual Basic to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */

}
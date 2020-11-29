using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: InternalsVisibleTo("CTNativeLib")]
[assembly: InternalsVisibleTo("CTShell")]

namespace DataTools.Memory.NativeLib
{

    public static class Native
    {

        internal static unsafe void ZeroMemory(void* handle, long len)
        {
            unsafe
            {
 
                byte* bp1 = (byte*)handle;
                byte* bep = (byte*)handle + len;

                if (len >= IntPtr.Size)
                {
                    if (IntPtr.Size == 8)
                    {
                        long* lp1 = (long*)bp1;
                        long* lep = (long*)bep;

                        do
                        {
                            *lp1++ = 0L;
                        } while (lp1 < lep);

                        if (lp1 == lep) return;

                        lp1--;
                        bp1 = (byte*)lp1;
                    }
                    else
                    {
                        int* ip1 = (int*)bp1;
                        int* lep = (int*)bep;

                        do
                        {
                            *ip1++ = 0;
                        } while (ip1 < lep);

                        if (ip1 == lep) return;

                        ip1--;
                        bp1 = (byte*)ip1;
                    }
                }
                do
                {
                    *bp1++ = 0;
                } while (bp1 < bep);
            }
        }

        public static void MemCpy(IntPtr src, IntPtr dest, long len)
        {
            unsafe
            {
                Buffer.MemoryCopy((void*)src, (void*)dest, len, len);
            }
        }



//        internal static unsafe void MemCpy(void* src, void* dest, long len)
//        {
//            unsafe
//            {
//                //if (len >= 2048)
//                //{
//                Buffer.MemoryCopy(src, dest, len, len);
//                return;
//                //}

//                byte* bp1 = (byte*)src;
//                byte* bp2 = (byte*)dest;
//                byte* bep = (byte*)src + len;

//#if X64
//                if (len >= 16)
//                {
//                    do
//                    {
//                        *(long*)bp2 = *(long*)bp1;
//                        if (bep - bp1 < 8) break;

//                        bp1 += 8;
//                        bp2 += 8;

//                    } while (bp1 < bep);

//                    if (bp1 == bep) return;

//                    len = bep - bp1;
//                }

//#else
//                if (len >= 16)
//                {
//                    do
//                    {
//                        *(int*)bp2 = *(int*)bp1;
//                        if (bep - bp1 < 4) break;

//                        bp1 += 4;
//                        bp2 += 4;

//                    } while (bp1 < bep);

//                    if (bp1 == bep) return;
//                }
               
//#endif
//                do
//                {
//                    *bp2++ = *bp1++;
//                } while (bp1 < bep);
//            }
//        }

    }

    //[StructLayout(LayoutKind.Sequential, Size = 16, Pack = 1)]
    //internal struct Block16
    //{
    //    public ulong val1;
    //    public ulong val2;
    //}

    //[StructLayout(LayoutKind.Sequential, Size = 64, Pack = 1)]
    //internal struct Block64
    //{
    //    public ulong val1;
    //    public ulong val2;
    //    public ulong val3;
    //    public ulong val4;
    //    public ulong val5;
    //    public ulong val6;
    //    public ulong val7;
    //    public ulong val8;
    //}

}
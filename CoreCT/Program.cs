using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using CoreCT.Memory;

[assembly: InternalsVisibleTo("DTInterop")]
namespace CoreCT
{
    class Program
    {
        static void Main(string[] args)
        {
            MemPtr b = new MemPtr(10240);

            b.ByteAt(0) = 3;

            byte bt = b.ByteAt(0);

            Console.WriteLine($"{bt}");

            b.ByteAt(5) = 243;

            bt = b.ByteAt(5);

            Console.WriteLine($"{bt}");

            b.GuidAtAbsolute(123) = Guid.NewGuid();

            Guid gt = b.GuidAtAbsolute(123);

            Console.WriteLine($"{gt:d}");


            b.DateTimeAt(44) = DateTime.Now;

            var dt = b.DateTimeAt(44);

            Console.WriteLine($"{dt:F}");


            b.SetUTF8String(657, "Çélà une teste de les charactarismes Unicodie.");

            var st = b.GetUTF8String(657);

            Console.WriteLine($"{st}");

            b.Free();

            b.Alloc(95483);

            Console.Write($"MemPtr Zeroing {b.Size:#,##0} Bytes.\r\n");

            b.ZeroMemory();

            b = (MemPtr)"Testing testing 1, 2, 3, 4, 5. Four score and seven years ago our forefathers fathered 4 fathers who fathered 6 fathers times 10 to the eleventy.";

            Console.WriteLine(b);

            char[] chars = b.ToArray<char>();
            st = new string(chars).Trim('\x0');

            Console.Write($"Round Trip Text: {st}\r\n");

            b.Free();

            b.Alloc(100000000 * 8, true);

            Console.Write($"MemPtr Allocated Size: {b.Size:#,##0}\r\n");

            long i;
            long c = 100000000;
           
            for (i = 0; i < c; i++)
            {
                b.LongAt(i) = i + 1;

                if ((i + 1) % 25000000 == 0)
                {
                    Console.Write($"{b.LongAt(i):#,##0}      \r");
                    if (Console.KeyAvailable) break;
                }
            }

            Console.ReadLine();

            b.Free(true);

            Console.Read();

        }
    }
}

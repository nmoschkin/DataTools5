using System;


namespace DataTools.Text
{
    internal struct BASE64STRUCT
    {
        public int Length;
        public byte[] Data;
    }

    internal class Base64
    {
        const string BASE64TABLE = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
        const int BASE64PAD = 61;
        const int BASE64PADRETURN = 254;
        
        private byte[] B64CodeOut = new byte[64];
        private byte[] B64CodeReturn = new byte[256];
        public bool B64TableCreated { get; private set; }
                
        public Base64()
        {
            int i;
            int d;
            for (i = 0; i <= 255; i++)
                B64CodeReturn[i] = 0x7F;
            for (i = 0; i <= 63; i++)
            {
                d = BASE64TABLE.Substring(i, 1)[0];
                B64CodeOut[i] = (byte)(d & 255);
                B64CodeReturn[d] = (byte)(i & 0x3F);
            }

            B64CodeReturn[BASE64PAD] = BASE64PADRETURN;
            B64TableCreated = true;
        }

        public int Decode64(byte[] DataIn, int Length, out byte[] DataOut)
        {
            int Decode64Ret = default;

            int l;
            int j;
            int v;

            var Quartet = new byte[4];

            l = (int)(Length / 4d * 3d);
            
            DataOut = new byte[l + 1];
            
            v = 0;
            
            for (l = 0; l < Length; l += 0)
            {
                j = 0;
                while (j < 4)
                {
                    if (B64CodeReturn[DataIn[l]] != 0x7F)
                    {
                        Quartet[j] = B64CodeReturn[DataIn[l]];
                        j = j + 1;
                    }

                    l = l + 1;
                    if (l >= Length)
                    {
                        while (j < 4)
                        {
                            Quartet[j] = BASE64PADRETURN;
                            j = j + 1;
                        }

                        break;
                    }
                }

                if (Quartet[0] == BASE64PADRETURN | Quartet[1] == BASE64PADRETURN)
                {
                    l = (int)-1L;
                    break;
                }

                DataOut[v] = (byte)((Quartet[0] << 2) | (Quartet[1] >> 4));
                v = v + 1;
                if (Quartet[2] == BASE64PADRETURN)
                {
                    l = (int)-1L;
                    break;
                }

                DataOut[v] = (byte)((Quartet[1] << 4) | (Quartet[2] >> 2));
                v = v + 1;
                if (Quartet[3] == BASE64PADRETURN)
                {
                    l = (int)-1L;
                    break;
                }

                DataOut[v] = (byte)((Quartet[2] << 6) | Quartet[3]);
                v = v + 1;
            }

            Array.Resize(ref DataOut, v - 1 + 1);

            Decode64Ret = v;
            return Decode64Ret;
        }

        public bool Encode64(byte[] DataIn, int Length, out BASE64STRUCT b64Out)
        {
            int l;
            int x;
            int v;
            var lC = default(int);
            int lProcess;
            int lReturn;
            int lActual;

            lActual = Length;
            lProcess = Length;

            b64Out = new BASE64STRUCT();

            if (0 != (lProcess % 3))
            {
                lProcess = lProcess + (3 - lProcess % 3);
            }

            lReturn = (int)(lProcess / 3d * 4d);

            Array.Resize(ref DataIn, lProcess - 1 + 1);

            if (Math.Round(lReturn / 76d) != lReturn / 76d)
            {
                v = (int)(Math.Round(lReturn / 76d) + 1d);
            }
            else
            {
                v = (int)(lReturn / 76d);
            }

            v = v * 2;
            l = lReturn - 1 + v;

            b64Out.Data = new byte[l + 1];
            b64Out.Length = l + 1;

            l = lProcess;
            v = 0;

            for (x = 0; x < l; x += 3)
            {
                b64Out.Data[v] = B64CodeOut[(DataIn[x] >> 2)];
                b64Out.Data[v + 1] = B64CodeOut[(DataIn[x] << 4) & 0x30 | (DataIn[x + 1] >> 4)];
                b64Out.Data[v + 2] = B64CodeOut[(DataIn[x + 1] << 2) & 0x3C | (DataIn[x + 2] >> 6)];
                b64Out.Data[v + 3] = B64CodeOut[DataIn[x + 2] & 0x3F];
                v = v + 4;
                lC = lC + 4;
                if (lC >= 76)
                {
                    lC = 0;
                    if (l - x > 3)
                    {
                        b64Out.Data[v] = 13;
                        b64Out.Data[v + 1] = 10;
                        v = v + 2;
                    }
                }

            }

            switch (lProcess - lActual)
            {
                case 1:
                    {
                        b64Out.Data[v - 1] = BASE64PAD;
                        break;
                    }

                case 2:
                    {
                        b64Out.Data[v - 1] = BASE64PAD;
                        b64Out.Data[v - 2] = BASE64PAD;
                        break;
                    }
            }

            if (lC != 0)
            {
                b64Out.Data[v] = 13;
                b64Out.Data[v + 1] = 10;
                Array.Resize(ref b64Out.Data, v + 1 + 1);
            }
            else
            {
                Array.Resize(ref b64Out.Data, v);
            }

            return 0 != (b64Out.Length);
        }
    }
}
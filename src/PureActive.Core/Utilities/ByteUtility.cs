using System;
using System.Text;

namespace PureActive.Core.Utilities
{
    public static class ByteUtility
    {
        private const string HexIndex = "0123456789abcdef          ABCDEF";
        private const string HexChars = "0123456789ABCDEF";
        
        #region Public Members

        // byte
        public static bool GetBit(byte value, int position)
        {
            return (GetBits(value, position, 1) == 1);
        }

        public static byte SetBit(ref byte value, int position, bool flag)
        {
            return SetBits(ref value, position, 1, (flag ? (byte)1 : (byte)0));
        }

        public static byte GetBits(byte value, int position, int length)
        {
            if (length <= 0 || position >= 8)
                return 0;

            int mask = (2 << (length - 1)) - 1;

            return (byte)((value >> position) & mask);
        }
        
        public static byte SetBits(ref byte value, int position, int length, byte bits)
        {
            if (length <= 0 || position >= 8)
                return value;

            int mask = (2 << (length - 1)) - 1;

            value &= (byte)~(mask << position);
            value |= (byte)((bits & mask) << position);

            return value;
        }

        // ushort
        public static bool GetBit(ushort value, int position)
        {
            return (GetBits(value, position, 1) == 1);
        }

        public static ushort SetBit(ref ushort value, int position, bool flag)
        {
            return SetBits(ref value, position, 1, (flag ? (ushort)1 : (ushort)0));
        }

        public static ushort GetBits(ushort value, int position, int length)
        {
            if (length <= 0 || position >= 16)
                return 0;

            int mask = (2 << (length - 1)) - 1;

            return (ushort)((value >> position) & mask);
        }

        public static ushort SetBits(ref ushort value, int position, int length, ushort bits)
        {
            if (length <= 0 || position >= 16)
                return value;

            int mask = (2 << (length - 1)) - 1;

            value &= (ushort)~(mask << position);
            value |= (ushort)((bits & mask) << position);

            return value;
        }

        // string
        public static string GetSafeString(byte[] bytes)
        {
            return bytes != null ? GetStringSkipNull(bytes, 0, bytes.Length) : "Null";
        }

        public static string GetStringNullIfEmpty(byte[] bytes)
        {
            if (bytes == null) return null;
            var s = GetStringSkipNull(bytes, 0, bytes.Length);

            return string.IsNullOrEmpty(s) ?  null : s;
    }

        public static string GetString(byte[] bytes)
        {
            return bytes != null ? GetString(bytes, 0, bytes.Length) : String.Empty;
        }

        public static string GetString(byte[] bytes, int offset, int length)
        {
            StringBuilder sb = new StringBuilder();

            if (bytes != null)
            {
                for (int i = offset; i < length && i < bytes.Length; i++)
                    sb.Append((char)bytes[i]);
            }

            return sb.ToString();
        }


        public static string GetStringSkipNull(byte[] bytes, int offset, int length)
        {
            StringBuilder sb = new StringBuilder();

            if (bytes != null)
            {
                for (int i = offset; i < length && i < bytes.Length; i++)
                {
                    if (bytes[i] != 0)
                        sb.Append((char)bytes[i]);
                }
            }

            return sb.ToString();
        }

        public static string PrintByte(byte b)
        {
            return ByteToHex(b);
        }

        public static string BytesToHex(byte[] b)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var ch in b)
                sb.Append( ByteToHex(ch));

            return sb.ToString();
        }

        public static string ByteToHex(byte b)
        {
            int lowByte = b & 0x0F;
            int highByte = (b & 0xF0) >> 4;

            return new string(
                new[] { HexChars[highByte], HexChars[lowByte] }
            );
        }

        public static byte[] HexToByte(string s)
        {
            int l = s.Length / 2;
            byte[] data = new byte[l];
            int j = 0;

            for (int i = 0; i < l; i++)
            {
                char c = s[j++];
                int n = HexIndex.IndexOf(c);
                int b = (n & 0xf) << 4;

                c = s[j++];
                n = HexIndex.IndexOf(c);
                b += (n & 0xf);
                data[i] = (byte)b;
            }

            return data;
        }

        public static string PrintBytes(byte[] bytes)
        {
            return PrintBytes(bytes, bytes.Length);
        }

        public static string PrintSafeBytes(byte[] bytes)
        {
            if (bytes != null)
            {
                return PrintBytes(bytes, bytes.Length);
            }
            else
            {
                return "Null";
            }
        }

        public static string PrintBytes(byte[] bytes, bool wrapLines)
        {
            return PrintBytes(bytes, bytes.Length, wrapLines);
        }

        public static string PrintBytes(byte[] bytes, int length)
        {
            return PrintBytes(bytes, length, true);
        }

        public static string PrintBytes(byte[] bytes, int length, bool wrapLines)
        {
            StringBuilder sb = new StringBuilder();

            int c = 0;

            for (int i = 0; i < length && i < bytes.Length; i++)
            {
                sb.Append(PrintByte(bytes[i]));

                if (++c == 24 && wrapLines)
                {
                    sb.Append(Environment.NewLine);
                    c = 0;
                }
                else if (i < length - 1)
                {
                    sb.Append("-");
                }
            }

            return sb.ToString();
        }

        public static byte[] Combine(byte[] value1, byte[] value2)
        {
            byte[] value = new byte[value1.Length + value2.Length];
            Array.Copy(value1, value, value1.Length);
            Array.Copy(value2, 0, value, value1.Length, value2.Length);

            return value;
        }

        public static ushort ReverseByteOrder(ushort value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            byte[] reverse = new byte[bytes.Length];

            var j = 0;
            for (var i = bytes.Length - 1; i >= 0; i--)
            {
                reverse[j++] = bytes[i];
            }

            return BitConverter.ToUInt16(reverse, 0);
        }

        public static uint ReverseByteOrder(uint value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            byte[] reverse = new byte[bytes.Length];

            var j = 0;
            for (var i = bytes.Length - 1; i >= 0; i--)
            {
                reverse[j++] = bytes[i];
            }

            return BitConverter.ToUInt32(reverse, 0);
        }

        public static ulong ReverseByteOrder(ulong value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            byte[] reverse = new byte[bytes.Length];

            var j = 0;
            for (var i = bytes.Length - 1; i >= 0; i--)
            {
                reverse[j++] = bytes[i];
            }

            return BitConverter.ToUInt64(reverse, 0);
        }

        public static ushort NetworkToSystemByteOrder(ushort value) =>
            BitConverter.IsLittleEndian ? ReverseByteOrder(value) : value;
        
        public static uint NetworkToSystemByteOrder(uint value) =>
            BitConverter.IsLittleEndian ? ReverseByteOrder(value) : value;

        public static ulong NetworkToSystemByteOrder(ulong value) =>
            BitConverter.IsLittleEndian ? ReverseByteOrder(value) : value;

        public static byte[] ReverseByteOrder(byte[] bytes)
        {
            byte[] reverse = new byte[bytes.Length];

            int j = 0;
            for (var i = bytes.Length - 1; i >= 0; i--)
            {
                reverse[j++] = bytes[i];
            }

            return reverse;
        }

        public static int ByteSearch(byte[] searchIn, byte[] searchBytes, int start = 0)
        {
            int found = -1;
            bool matched = false;

            //only look at this if we have a populated search array and search bytes with a sensible start
            if (searchIn.Length > 0 && searchBytes.Length > 0 && start <= (searchIn.Length - searchBytes.Length) && searchIn.Length >= searchBytes.Length)
            {
                //iterate through the array to be searched
                for (var i = start; i <= searchIn.Length - searchBytes.Length; i++)
                {
                    //if the start bytes match we will start comparing all other bytes
                    if (searchIn[i] == searchBytes[0])
                    {
                        if (searchIn.Length > 1)
                        {
                            //multiple bytes to be searched we have to compare byte by byte
                            matched = true;
                            for (var y = 1; y <= searchBytes.Length - 1; y++)
                            {
                                if (searchIn[i + y] != searchBytes[y])
                                {
                                    matched = false;
                                    break;
                                }
                            }
                            //everything matched up
                            if (matched)
                            {
                                found = i;
                                break;
                            }
                        }
                        else
                        {
                            //search byte is only one bit nothing else to do
                            found = i;
                            break; //stop the loop
                        }

                    }
                }
            }

            return found;
        }

        public static bool Equality(byte[] a1, byte[] b1)
        {
            // If not same length, done
            if (a1.Length != b1.Length)
            {
                return false;
            }

            // If they are the same object, done
            if (ReferenceEquals(a1, b1))
            {
                return true;
            }

            // Loop all values and compare
            for (var i = 0; i < a1.Length; i++)
            {
                if (a1[i] != b1[i])
                {
                    return false;
                }
            }

            // If we got here, equal
            return true;
        }

        #endregion Public Members
    }
}

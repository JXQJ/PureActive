using System;
using System.Net;

namespace PureActive.Network.Abstractions.Types
{
    public class InternetAddress : IComparable
    {
        public static readonly InternetAddress Any = new InternetAddress(0, 0, 0, 0);
        public static readonly InternetAddress Broadcast = new InternetAddress(255, 255, 255, 255);

        private readonly byte[] _address = new byte[] { 0, 0, 0, 0 };

        public InternetAddress(params byte[] address)
        {
            if (address == null || address.Length != 4)
            {
                _address = null;
            }
            else
            {
                address.CopyTo(_address, 0);
            }
        }

        public InternetAddress(IPAddress address)
        {
            if (address == null || address.GetAddressBytes().Length != 4)
            {
                _address = null;
            }
            else
            {
                _address = address.GetAddressBytes();
            }
        }

        public InternetAddress(string address)
        {
            IPAddress ipAddress = IPAddress.Parse(address);

            _address = ipAddress.GetAddressBytes().Length != 4 ? null : ipAddress.GetAddressBytes();
        }

        public byte this[int index] => _address[index];

        public bool IsAny => Equals(Any);

        public bool IsBroadcast => Equals(Broadcast);

        internal InternetAddress NextAddress()
        {
            InternetAddress next = Copy();

            if (_address[3] == 255)
            {
                next._address[3] = 0;

                if (_address[2] == 255)
                {
                    next._address[2] = 0;

                    if (_address[1] == 255)
                    {
                        next._address[1] = 0;

                        if (_address[0] == 255)
                        {
                            throw new InvalidOperationException();
                        }
                        else
                        {
                            next._address[0] = (Byte)(_address[0] + 1);
                        }
                    }
                    else
                    {
                        next._address[1] = (Byte)(_address[1] + 1);
                    }
                }
                else
                {
                    next._address[2] = (Byte)(_address[2] + 1);
                }
            }
            else
            {
                next._address[3] = (Byte)(_address[3] + 1);
            }

            return next;
        }

        public int CompareTo(Object obj)
        {
            if (!(obj is InternetAddress other))
            {
                return 1;
            }

            for (int i = 0; i < 4; i++)
            {
                if (_address[i] > other._address[i])
                {
                    return 1;
                }
                else if (_address[i] < other._address[i])
                {
                    return -1;
                }
            }

            return 0;
        }

        public override bool Equals(Object obj)
        {
            return Equals(obj as InternetAddress);
        }

        public bool Equals(InternetAddress other)
        {
            return _address == null ||
                _address[0] == other._address[0] &&
                _address[1] == other._address[1] &&
                _address[2] == other._address[2] &&
                _address[3] == other._address[3];
        }

        public override Int32 GetHashCode()
        {
            return BitConverter.ToInt32(_address, 0);
        }

        public override string ToString()
        {
            if (_address != null)
                return this[0] + "." + this[1] + "." + this[2] + "." + this[3];
            else
                return "Null";
        }

        public InternetAddress Copy()
        {
            return new InternetAddress(_address[0], _address[1], _address[2], _address[3]);
        }

        public byte[] ToArray()
        {
            Byte[] array = new Byte[4];
            _address.CopyTo(array, 0);
            return array;
        }

        public IPAddress ToIPAddress()
        {
            return new IPAddress(ToArray());
        }

        public static InternetAddress Parse(String address)
        {
            return new InternetAddress(IPAddress.Parse(address).GetAddressBytes());
        }
    }
}
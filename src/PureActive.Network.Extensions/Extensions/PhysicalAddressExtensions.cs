using System.Net.NetworkInformation;
using System.Text;

namespace PureActive.Network.Extensions.Extensions
{
    public static class PhysicalAddressExtensions
    {
        private static readonly char[] PhysicalAddressDelims = new char[]
        {
            ':', '-'
        };


        public static byte[] ToArray(this PhysicalAddress physicalAddress)
        {
            return physicalAddress.GetAddressBytes();
        }

        public static PhysicalAddress NormalizedParse(string physicalAddressString)
        {
            // Need to normalize the Physical address
             if (string.IsNullOrWhiteSpace(physicalAddressString))
                return PhysicalAddress.None;

            var parts = physicalAddressString.Trim().ToUpper().Split(PhysicalAddressDelims);

            if (parts.Length == 0)
                return PhysicalAddress.None;
            else if (parts.Length == 1)
                return PhysicalAddress.Parse(parts[0]);

            var sb = new StringBuilder();

            foreach (var part in parts)
            {
                if (part.Length == 1)
                    sb.Append('0');

                sb.Append(part).Append("-");
            }

            // Remove trailing delim
            sb.Length -= 1;

            return PhysicalAddress.Parse(sb.ToString());

        }


        private static string ToDelimString(this PhysicalAddress physicalAddress, char chDelim)
        {
            var sb = new StringBuilder();
            var physicalAddressString = physicalAddress.ToString();

            if (string.IsNullOrWhiteSpace(physicalAddressString))
                return physicalAddressString;

            for (var i = 0; i < physicalAddressString.Length; i += 2)
            {
                sb.Append(physicalAddressString.Substring(i, 2)).Append(chDelim);
            }

            sb.Length -= 1;

            return sb.ToString();
        }


        public static string ToColonString(this PhysicalAddress physicalAddress) => ToDelimString(physicalAddress, ':');
        public static string ToDashString(this PhysicalAddress physicalAddress) => ToDelimString(physicalAddress, '-');
    }
}


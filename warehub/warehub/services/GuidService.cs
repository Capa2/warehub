using System;

using System;
using System.Text;

namespace warehub.services
{
    public static class GuidService
    {
        public static Guid GenerateId()
        {
            return Guid.NewGuid();
        }

        public static byte[] GuidToBinary(Guid guid)
        {
            return guid.ToByteArray();
        }

        public static Guid BinaryToGuid(byte[] binaryGuid)
        {
            if (binaryGuid.Length != 16)
            {
                throw new ArgumentException("Invalid binary format for Guid. Must be 16 bytes.");
            }
            return new Guid(binaryGuid);
        }

        public static string GuidToString(Guid guid)
        {
            return guid.ToString();
        }

        public static Guid StringToGuid(string guidString)
        {
            return Guid.Parse(guidString);
        }
    }
}

using NLog;

namespace warehub.services
{
    public static class GuidService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public static Guid GenerateId()
        {
            Guid guid = Guid.NewGuid();
            Logger.Trace("Generated new Guid: " + GuidToString(guid));
            return guid;
        }

        public static byte[] GuidToBinary(Guid guid) => guid.ToByteArray();

        public static Guid BinaryToGuid(byte[] binaryGuid)
        {
            if (binaryGuid.Length != 16)
            {
                Logger.Error("Invalid binary format for Guid. Must be 16 bytes.");
            }
            return new Guid(binaryGuid);
        }

        public static string GuidToString(Guid guid) => guid.ToString();

        public static Guid StringToGuid(string guidString) => Guid.Parse(guidString);
    }
}

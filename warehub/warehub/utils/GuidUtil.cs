//GuidService.cs
using NLog;

namespace warehub.utils
{
    /// <summary>
    /// Provides utility methods for working with Guids, including generation, 
    /// conversion to binary format, and parsing from strings.
    /// </summary>
    public static class GuidUtil
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Generates a new Guid.
        /// </summary>
        /// <returns>A newly generated Guid.</returns>
        public static Guid GenerateId()
        {
            Guid guid = Guid.NewGuid();
            Logger.Trace($"GuidService: Generated Guid: {guid}");
            return guid;
        }

        /// <summary>
        /// Converts a Guid to a byte array.
        /// </summary>
        /// <param name="guid">The Guid to convert.</param>
        /// <returns>A 16-byte array representing the Guid.</returns>
        public static byte[] GuidToBinary(Guid guid)
        {
            byte[] binary = guid.ToByteArray();
            Logger.Trace($"GuidService: Guid {guid} converted to byte array.");
            return binary;
        }

        /// <summary>
        /// Converts a 16-byte array back to a Guid.
        /// </summary>
        /// <param name="binaryGuid">The byte array representing the Guid.</param>
        /// <returns>The corresponding Guid.</returns>
        public static Guid BinaryToGuid(byte[] binaryGuid)
        {
            if (binaryGuid.Length != 16)
                Logger.Error("GuidService: Invalid binary format for Guid. Must be 16 bytes.");
            Guid guid = new Guid(binaryGuid);
            Logger.Trace($"GuidService: Converted byte array to Guid: {guid}");
            return guid;
        }

        /// <summary>
        /// Converts a Guid to its string representation.
        /// </summary>
        /// <param name="guid">The Guid to convert.</param>
        /// <returns>A string representation of the Guid.</returns>
        public static string GuidToString(Guid guid)
        {
            string guidString = guid.ToString();
            Logger.Trace($"GuidService: Guid {guid} converted to string.");
            return guidString;
        }

        /// <summary>
        /// Parses a string representation of a Guid back to a Guid.
        /// </summary>
        /// <param name="guidString">The string representation of the Guid.</param>
        /// <returns>The corresponding Guid.</returns>
        public static Guid StringToGuid(string guidString)
        {
            try
            {
                Guid guid = Guid.Parse(guidString);
                Logger.Trace($"GuidService: Parsed Guid from string: {guidString} to {guid}");
                return guid;
            }
            catch (FormatException ex)
            {
                Logger.Error($"GuidService: Failed to parse Guid from string: {guidString}. Error: {ex.Message}");
                throw;
            }
        }
    }
}
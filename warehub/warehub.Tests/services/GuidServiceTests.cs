using System;
using warehub.utils;
using Xunit;

namespace warehub.Tests.services
{
    /// <summary>
    /// Unit tests for the GuidUtil class.
    /// </summary>
    public class GuidUtilTests
    {
        [Fact]
        public void GenerateId_ShouldReturnNewGuid()
        {
            // Act
            var guid1 = GuidUtil.GenerateId();
            var guid2 = GuidUtil.GenerateId();

            // Assert
            Assert.NotEqual(Guid.Empty, guid1); // Ensure the GUID is not empty
            Assert.NotEqual(Guid.Empty, guid2);
            Assert.NotEqual(guid1, guid2); // Ensure the generated GUIDs are unique
        }

        [Fact]
        public void GuidToBinary_ShouldConvertGuidToByteArray()
        {
            // Arrange
            var guid = Guid.NewGuid();

            // Act
            var byteArray = GuidUtil.GuidToBinary(guid);

            // Assert
            Assert.NotNull(byteArray);
            Assert.Equal(16, byteArray.Length); // GUIDs should be 16 bytes
        }

        [Fact]
        public void BinaryToGuid_ShouldConvertByteArrayToGuid()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var byteArray = guid.ToByteArray();

            // Act
            var resultGuid = GuidUtil.BinaryToGuid(byteArray);

            // Assert
            Assert.Equal(guid, resultGuid); // The converted GUID should match the original
        }

        [Fact]
        public void BinaryToGuid_ShouldLogErrorForInvalidByteArray()
        {
            // Arrange
            var invalidByteArray = new byte[10]; // Invalid length

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => GuidUtil.BinaryToGuid(invalidByteArray));
        }

        [Fact]
        public void GuidToString_ShouldConvertGuidToString()
        {
            // Arrange
            var guid = Guid.NewGuid();

            // Act
            var guidString = GuidUtil.GuidToString(guid);

            // Assert
            Assert.False(string.IsNullOrEmpty(guidString), "Guid string should not be null or empty.");
            Assert.Equal(guid.ToString(), guidString); // The string representation should match
        }

        [Fact]
        public void StringToGuid_ShouldParseStringToGuid()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var guidString = guid.ToString();

            // Act
            var parsedGuid = GuidUtil.StringToGuid(guidString);

            // Assert
            Assert.Equal(guid, parsedGuid); // The parsed GUID should match the original
        }

        [Fact]
        public void StringToGuid_ShouldThrowExceptionForInvalidString()
        {
            // Arrange
            var invalidGuidString = "invalid-guid";

            // Act & Assert
            Assert.Throws<FormatException>(() => GuidUtil.StringToGuid(invalidGuidString));
        }
    }
}
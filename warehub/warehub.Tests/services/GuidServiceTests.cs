using System;
using warehub.services;
using Xunit;

namespace warehub.Tests.services
{
    /// <summary>
    /// Unit tests for the GuidService class.
    /// </summary>
    public class GuidServiceTests
    {
        [Fact]
        public void GenerateId_ShouldReturnNewGuid()
        {
            // Act
            var guid1 = GuidService.GenerateId();
            var guid2 = GuidService.GenerateId();

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
            var byteArray = GuidService.GuidToBinary(guid);

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
            var resultGuid = GuidService.BinaryToGuid(byteArray);

            // Assert
            Assert.Equal(guid, resultGuid); // The converted GUID should match the original
        }

        [Fact]
        public void BinaryToGuid_ShouldLogErrorForInvalidByteArray()
        {
            // Arrange
            var invalidByteArray = new byte[10]; // Invalid length

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => GuidService.BinaryToGuid(invalidByteArray));
        }

        [Fact]
        public void GuidToString_ShouldConvertGuidToString()
        {
            // Arrange
            var guid = Guid.NewGuid();

            // Act
            var guidString = GuidService.GuidToString(guid);

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
            var parsedGuid = GuidService.StringToGuid(guidString);

            // Assert
            Assert.Equal(guid, parsedGuid); // The parsed GUID should match the original
        }

        [Fact]
        public void StringToGuid_ShouldThrowExceptionForInvalidString()
        {
            // Arrange
            var invalidGuidString = "invalid-guid";

            // Act & Assert
            Assert.Throws<FormatException>(() => GuidService.StringToGuid(invalidGuidString));
        }
    }
}
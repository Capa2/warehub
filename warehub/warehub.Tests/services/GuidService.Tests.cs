using Xunit;
using warehub.services;
using System;

namespace warehub.tests
{
    public class GuidServiceTests
    {
        [Fact]
        public void GuidService_GeneratesUniqueIds()
        {
            // Act
            Guid id1 = GuidService.GenerateId();
            Guid id2 = GuidService.GenerateId();

            // Assert
            Assert.NotEqual(id1, id2);
            Assert.NotEqual(Guid.Empty, id1);
            Assert.NotEqual(Guid.Empty, id2);
        }

        [Fact]
        public void GuidService_ConvertsGuidToAndFromBinary()
        {
            // Arrange
            Guid originalGuid = Guid.NewGuid();

            // Act
            byte[] binaryGuid = GuidService.GuidToBinary(originalGuid);
            Guid convertedGuid = GuidService.BinaryToGuid(binaryGuid);

            // Assert
            Assert.Equal(originalGuid, convertedGuid);
        }

        [Fact]
        public void GuidService_ConvertsGuidToAndFromString()
        {
            // Arrange
            Guid originalGuid = Guid.NewGuid();

            // Act
            string guidString = GuidService.GuidToString(originalGuid);
            Guid convertedGuid = GuidService.StringToGuid(guidString);

            // Assert
            Assert.Equal(originalGuid, convertedGuid);
        }
    }
}

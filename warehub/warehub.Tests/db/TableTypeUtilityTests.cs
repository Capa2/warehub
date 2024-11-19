using System;
using System.Collections.Generic;
using Xunit;
using warehub.db.utils;

namespace warehub.Tests.db.utils
{
    public class TableTypeUtilityTests
    {
        [Fact]
        public void GetColumnTypeMapping_ShouldReturnMappingForValidTable()
        {
            // Act
            var columnMapping = TableTypeUtility.GetColumnTypeMapping("test_table");

            // Assert
            Assert.NotNull(columnMapping);
            Assert.Equal(typeof(Guid), columnMapping["id"]);
            Assert.Equal(typeof(string), columnMapping["name"]);
        }

        [Fact]
        public void GetColumnTypeMapping_ShouldThrowForInvalidTable()
        {
            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => TableTypeUtility.GetColumnTypeMapping("non_existent_table"));
        }

        [Fact]
        public void ConvertToType_ShouldConvertGuidSuccessfully()
        {
            // Arrange
            Guid expected = Guid.NewGuid();
            string input = expected.ToString();

            // Act
            var result = TableTypeUtility.ConvertToType(input, typeof(Guid));

            // Assert
            Assert.IsType<Guid>(result);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertToType_ShouldHandleDBNull()
        {
            // Act
            var result = TableTypeUtility.ConvertToType(DBNull.Value, typeof(string));

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ConvertToType_ShouldThrowOnInvalidConversion()
        {
            // Arrange
            string invalidInput = "invalid-guid";

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => TableTypeUtility.ConvertToType(invalidInput, typeof(Guid)));
        }
    }
}

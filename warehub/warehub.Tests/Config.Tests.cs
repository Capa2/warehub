using System;
using Microsoft.Extensions.Configuration;
using Moq;
using NLog;
using warehub;
using Xunit;

namespace warehub.Tests
{
    /// <summary>
    /// Unit tests for the Config class.
    /// </summary>
    public class ConfigTests
    {
        [Fact]
        public void GetInstance_ShouldReturnSingletonInstance()
        {
            // Arrange & Act
            var instance1 = Config.GetInstance();
            var instance2 = Config.GetInstance();

            // Assert
            Assert.NotNull(instance1);
            Assert.Same(instance1, instance2); // Ensure both references point to the same instance
        }

        [Fact]
        public void GetConnectionString_ShouldReturnValidConnectionString()
        {
            // Arrange
            var config = Config.GetInstance();

            // Act
            var connectionString = config.GetConnectionString("localhost");

            // Assert
            Assert.False(string.IsNullOrEmpty(connectionString), "Connection string should not be null or empty.");
        }

        [Fact]
        public void GetFileLogLevel_ShouldReturnLogLevel()
        {
            // Arrange
            var config = Config.GetInstance();

            // Act
            var logLevel = config.GetFileLogLevel();

            // Assert
            Assert.False(string.IsNullOrEmpty(logLevel), "File log level should not be null or empty.");
        }

        [Fact]
        public void GetConsoleLogLevel_ShouldReturnLogLevel()
        {
            // Arrange
            var config = Config.GetInstance();

            // Act
            var logLevel = config.GetConsoleLogLevel();

            // Assert
            Assert.False(string.IsNullOrEmpty(logLevel), "Console log level should not be null or empty.");
        }
    }
}
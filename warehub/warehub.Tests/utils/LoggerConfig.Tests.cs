// File: LoggerConfigTests.cs
using System;
using System.Linq; // Added for LINQ methods
using NLog;
using NLog.Config;
using NLog.Targets;
using warehub.utils;
using Xunit;

namespace warehub.Tests
{
    /// <summary>
    /// Unit tests for the LoggerConfig class.
    /// </summary>
    public class LoggerConfigTests
    {
        /// <summary>
        /// Verifies that ConfigureLogging sets up the logging configuration without throwing exceptions.
        /// </summary>
        [Fact]
        public void ConfigureLogging_ShouldSetConfigurationWithoutExceptions()
        {
            // Arrange & Act
            Exception? exception = Record.Exception(() => LoggerConfig.ConfigureLogging());

            // Assert
            Assert.Null(exception); // Ensure no exceptions were thrown during configuration
        }

        /// <summary>
        /// Verifies that logging configuration includes the expected targets.
        /// </summary>
        [Fact]
        public void ConfigureLogging_ShouldIncludeExpectedTargets()
        {
            // Arrange
            LoggerConfig.ConfigureLogging();
            var config = LogManager.Configuration;

            // Act & Assert
            Assert.Contains(config.AllTargets, target => target is ConsoleTarget && target.Name == "console");
            Assert.Contains(config.AllTargets, target => target is FileTarget && target.Name == "combinedFile");
            Assert.Contains(config.AllTargets, target => target is FileTarget && target.Name == "errorFile");
            Assert.Contains(config.AllTargets, target => target is FileTarget && target.Name == "warnFile");
            Assert.Contains(config.AllTargets, target => target is FileTarget && target.Name == "infoFile");
            Assert.Contains(config.AllTargets, target => target is FileTarget && target.Name == "debugFile");
            Assert.Contains(config.AllTargets, target => target is FileTarget && target.Name == "traceFile");
            Assert.Contains(config.AllTargets, target => target is OutputDebugStringTarget && target.Name == "outputDebug");
        }

        /// <summary>
        /// Verifies that logging rules are correctly set for the console target.
        /// </summary>
        [Fact]
        public void ConfigureLogging_ShouldSetConsoleLoggingRules()
        {
            // Arrange
            LoggerConfig.ConfigureLogging();
            var config = LogManager.Configuration;

            // Act
            var consoleRules = config.LoggingRules.SelectMany(rule => rule.Targets);

            // Assert
            Assert.Contains(consoleRules, target => target is ConsoleTarget);
        }

        /// <summary>
        /// Verifies that logging rules are correctly set for the file targets.
        /// </summary>
        [Fact]
        public void ConfigureLogging_ShouldSetFileLoggingRules()
        {
            // Arrange
            LoggerConfig.ConfigureLogging();
            var config = LogManager.Configuration;

            // Act
            var fileRules = config.LoggingRules.SelectMany(rule => rule.Targets);

            // Assert
            Assert.Contains(fileRules, target => target is FileTarget);
        }
    }
}
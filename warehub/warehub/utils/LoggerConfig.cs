using NLog;
using NLog.Config;
using NLog.Targets;

namespace warehub.utils
{
    /// <summary>
    /// Configures global logging targets and rules using 
    /// NLog, including console, file, and debug output, 
    /// based on the application's configuration settings.
    /// </summary>
    public class LoggerConfig
    {
        /// <summary>
        /// Sets up logging targets and rules for different log levels, including 
        /// console and file logging, based on the application's logging configuration.
        /// </summary>
        public static void ConfigureLogging()
        {
            LoggingConfiguration logConfig = new LoggingConfiguration();

            // Retrieve log levels from the configuration settings
            LogLevel fileLogLevel = LogLevel.FromString(Config.GetInstance().GetFileLogLevel());
            LogLevel consoleLogLevel = LogLevel.FromString(Config.GetInstance().GetConsoleLogLevel());

            // Define the console logging target
            var consoleTarget = new ConsoleTarget("console")
            {
                Layout = "${longdate} | ${level:uppercase=true} | ${message} ${exception:format=ToString}"
            };

            // Define a combined file target for logging all levels
            var combinedFileTarget = new FileTarget("combinedFile")
            {
                FileName = "${basedir}/logs/combined.log",
                Layout = "${longdate} | ${level:uppercase=true} | ${message} ${exception:format=ToString}"
            };

            // Define a file target for logging error-level messages and higher
            var errorFileTarget = new FileTarget("errorFile")
            {
                FileName = "${basedir}/logs/error.log",
                Layout = "${longdate} | ${level:uppercase=true} | ${message} ${exception:format=ToString}"
            };

            // Define a file target for logging warnings
            var warnFileTarget = new FileTarget("warnFile")
            {
                FileName = "${basedir}/logs/warn.log",
                Layout = "${longdate} | ${level:uppercase=true} | ${message} ${exception:format=ToString}"
            };

            // Define a file target for logging informational messages
            var infoFileTarget = new FileTarget("infoFile")
            {
                FileName = "${basedir}/logs/info.log",
                Layout = "${longdate} | ${level:uppercase=true} | ${message} ${exception:format=ToString}"
            };

            // Define a file target for debugging information
            var debugFileTarget = new FileTarget("debugFile")
            {
                FileName = "${basedir}/logs/debug.log",
                Layout = "${longdate} | ${level:uppercase=true} | ${message} ${exception:format=ToString}"
            };

            // Define a file target for detailed trace messages
            var traceFileTarget = new FileTarget("traceFile")
            {
                FileName = "${basedir}/logs/trace.log",
                Layout = "${longdate} | ${level:uppercase=true} | ${message} ${exception:format=ToString}"
            };

            // Define a target for outputting to the Visual Studio Output window
            var outputDebugTarget = new OutputDebugStringTarget("outputDebug")
            {
                Layout = "${longdate} | ${level:uppercase=true} | ${message} ${exception:format=ToString}"
            };

            // Add targets to the logging configuration
            logConfig.AddTarget(consoleTarget);
            logConfig.AddTarget(combinedFileTarget);
            logConfig.AddTarget(errorFileTarget);
            logConfig.AddTarget(warnFileTarget);
            logConfig.AddTarget(infoFileTarget);
            logConfig.AddTarget(debugFileTarget);
            logConfig.AddTarget(traceFileTarget);
            logConfig.AddTarget(outputDebugTarget);

            // Configure logging rules for error-level messages
            logConfig.AddRule(LogLevel.Error, LogLevel.Fatal, errorFileTarget);

            // Configure rules for specific log levels if they are enabled
            if (fileLogLevel <= LogLevel.Warn) logConfig.AddRule(LogLevel.Warn, LogLevel.Warn, warnFileTarget);
            if (fileLogLevel <= LogLevel.Info) logConfig.AddRule(LogLevel.Info, LogLevel.Info, infoFileTarget);
            if (fileLogLevel <= LogLevel.Debug) logConfig.AddRule(LogLevel.Debug, LogLevel.Debug, debugFileTarget);
            if (fileLogLevel <= LogLevel.Trace) logConfig.AddRule(LogLevel.Trace, LogLevel.Trace, traceFileTarget);

            // Configure a combined file target to log messages from all levels
            logConfig.AddRule(fileLogLevel, LogLevel.Fatal, combinedFileTarget);

            // Configure logging rules for console and Visual Studio Output window
            logConfig.AddRule(consoleLogLevel, LogLevel.Fatal, consoleTarget);
            logConfig.AddRule(consoleLogLevel, LogLevel.Fatal, outputDebugTarget);

            // Apply the logging configuration
            LogManager.Configuration = logConfig;
        }
    }
}
using NLog;
using NLog.Config;
using NLog.Targets;

namespace warehub.utils
{
    public class LoggerConfig
    {
        public static void ConfigureLogging()
        {
            LoggingConfiguration logConfig = new LoggingConfiguration();
            LogLevel fileLogLevel = LogLevel.FromString(Config.GetInstance().GetFileLogLevel());
            LogLevel consoleLogLevel = LogLevel.FromString(Config.GetInstance().GetConsoleLogLevel());

            // Basedir should not require manual configuration as it defaults to the directory of the executing assembly.
            // config.Variables["basedir"] = AppDomain.CurrentDomain.BaseDirectory;

            var consoleTarget = new ConsoleTarget("console")
            {
                Layout = "${longdate} | ${level:uppercase=true} | ${message} ${exception:format=ToString}"
            };

            var combinedFileTarget = new FileTarget("combinedFile")
            {
                FileName = "${basedir}/logs/combined.log",
                Layout = "${longdate} | ${level:uppercase=true} | ${message} ${exception:format=ToString}"
            };

            var errorFileTarget = new FileTarget("errorFile")
            {
                FileName = "${basedir}/logs/error.log",
                Layout = "${longdate} | ${level:uppercase=true} | ${message} ${exception:format=ToString}"
            };

            var warnFileTarget = new FileTarget("warnFile")
            {
                FileName = "${basedir}/logs/warn.log",
                Layout = "${longdate} | ${level:uppercase=true} | ${message} ${exception:format=ToString}"
            };

            var infoFileTarget = new FileTarget("infoFile")
            {
                FileName = "${basedir}/logs/info.log",
                Layout = "${longdate} | ${level:uppercase=true} | ${message} ${exception:format=ToString}"
            };

            var debugFileTarget = new FileTarget("debugFile")
            {
                FileName = "${basedir}/logs/debug.log",
                Layout = "${longdate} | ${level:uppercase=true} | ${message} ${exception:format=ToString}"
            };

            var traceFileTarget = new FileTarget("traceFile")
            {
                FileName = "${basedir}/logs/trace.log",
                Layout = "${longdate} | ${level:uppercase=true} | ${message} ${exception:format=ToString}"
            };

            // OutputDebugStringTarget for Visual Studio Output window
            var outputDebugTarget = new OutputDebugStringTarget("outputDebug")
            {
                Layout = "${longdate} | ${level:uppercase=true} | ${message} ${exception:format=ToString}"
            };

            logConfig.AddTarget(consoleTarget);
            logConfig.AddTarget(combinedFileTarget);
            logConfig.AddTarget(errorFileTarget);
            logConfig.AddTarget(warnFileTarget);
            logConfig.AddTarget(infoFileTarget);
            logConfig.AddTarget(debugFileTarget);
            logConfig.AddTarget(traceFileTarget);
            logConfig.AddTarget(outputDebugTarget);

            // Configure rules for file logging
            logConfig.AddRule(LogLevel.Error, LogLevel.Fatal, errorFileTarget);
            if (fileLogLevel <= LogLevel.Warn) logConfig.AddRule(LogLevel.Warn, LogLevel.Warn, warnFileTarget);
            if (fileLogLevel <= LogLevel.Info) logConfig.AddRule(LogLevel.Info, LogLevel.Info, infoFileTarget);
            if (fileLogLevel <= LogLevel.Debug) logConfig.AddRule(LogLevel.Debug, LogLevel.Debug, debugFileTarget);
            if (fileLogLevel <= LogLevel.Trace) logConfig.AddRule(LogLevel.Trace, LogLevel.Trace, traceFileTarget);

            // Combined file target for all levels
            logConfig.AddRule(fileLogLevel, LogLevel.Fatal, combinedFileTarget);

            // Configure rules for console and debug output
            logConfig.AddRule(consoleLogLevel, LogLevel.Fatal, consoleTarget);
            logConfig.AddRule(consoleLogLevel, LogLevel.Fatal, outputDebugTarget);


            LogManager.Configuration = logConfig;
        }
    }
}

class LogUseExample
{
    private static readonly ILogger logger = LogManager.GetCurrentClassLogger(); // Get a logger instance for the current class.

    public void DoSomething()
    {
        logger.Trace("Bip bop");
        logger.Info("Doing something...");
        logger.Debug("Debug something...");
        logger.Warn("This is a warning from LogUseExample.");
        logger.Error("An error occurred in LogUseExample.");
    }
}
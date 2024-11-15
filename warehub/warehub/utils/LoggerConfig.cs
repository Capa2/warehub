using NLog;
using NLog.Config;
using NLog.Targets;

namespace warehub.utils
{
    public class LoggerConfig
    {
        public static void ConfigureLogging()
        {
            var config = new LoggingConfiguration();

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

            config.AddTarget(consoleTarget);
            config.AddTarget(combinedFileTarget);
            config.AddTarget(errorFileTarget);
            config.AddTarget(warnFileTarget);
            config.AddTarget(infoFileTarget);
            config.AddTarget(debugFileTarget);
            config.AddTarget(traceFileTarget);

            // Configure the targets (files/console) of each log level.
            config.AddRule(LogLevel.Error, LogLevel.Fatal, errorFileTarget);
            config.AddRule(LogLevel.Warn, LogLevel.Warn, warnFileTarget);
            config.AddRule(LogLevel.Info, LogLevel.Info, infoFileTarget);
            config.AddRule(LogLevel.Debug, LogLevel.Debug, debugFileTarget);
            config.AddRule(LogLevel.Trace, LogLevel.Trace, traceFileTarget);
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, combinedFileTarget);
            config.AddRule(LogLevel.Info, LogLevel.Fatal, consoleTarget);

            LogManager.Configuration = config;
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
}
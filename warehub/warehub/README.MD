## Logging

Our application uses NLog for logging, providing detailed tracking and diagnostics. Logs are categorized into levels: Trace, Debug, Info, Warning, Error, and Fatal. These logs are used to monitor the application's behavior and diagnose issues effectively.

### Levels Policy

- **Trace**: Used for detailed diagnostics and tracking the flow of the application. Enabled only during development or for debugging complex issues.
- **Debug**: Contains information useful for developers during debugging. Not recommended for use in production.
- **Info**: General information about the application's workflow, such as successful operations. Used sparingly in production.
- **Warning**: Indicates potential issues that aren’t immediately harmful but could lead to errors.
- **Error**: Logs when something has gone wrong but the application can continue running.
- **Fatal**: Critical issues that cause the application to terminate or stop working properly.

### Logging Policy

- **Development**: Use Trace and Debug logs to understand application flow and troubleshoot issues.
- **Production**: Enable Info, Warning, Error, and Fatal levels. Disable Trace and Debug.
- **Performance**: Be cautious with logging inside loops or performance-critical code.

### Logging Setup

**Initialization**: Call `LoggerConfig.ConfigureLogging()` at the start of your program (e.g., in `Program.cs`) to set up logging globally.
**Shutdown**: Call `LogManager.Shutdown()` before the application exits to ensure all logs are flushed properly.

### Logging Useage

To use the logger, simply import and call it in your classes:

    using NLog;

    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

- To log a trace message: `Logger.Trace("Detailed trace message");`
- To log a debug message: `Logger.Debug("Debugging information");`
- To log an info message: `Logger.Info("General workflow message");`
- To log a warning: `Logger.Warn("Potential issue warning");`
- To log an error: `Logger.Error("Error encountered");`
- To log a fatal message: `Logger.Fatal("Critical issue");`

Once initialized the logger is available globally, including static classes.

### Log Configuration

Log output can be directed per level to multiple targets, such as files or the console. Refer to `utils/LoggerConfig.cs` for detailed settings.
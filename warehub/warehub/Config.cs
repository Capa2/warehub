using Microsoft.Extensions.Configuration;
using NLog;

namespace warehub
{
    /// <summary>
    /// The Config class is responsible for loading and accessing configuration settings from a specified JSON file.
    /// This class is implemented as a singleton to ensure only one instance is used throughout the application.
    /// </summary>
    public class Config
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        // Singleton instance of the Config class
        private static Config? _instance;

        // Lock object to ensure thread safety when initializing the singleton
        private static readonly object _lock = new();

        // The IConfiguration instance used to access the configuration settings
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Private constructor to prevent instantiation from outside.
        /// Initializes the configuration settings from a specified appsettings file.
        /// </summary>
        /// <param name="appSetting">
        /// The suffix of the appsettings file to load (e.g., "dev" loads "appsettings.dev.json").
        /// Defaults to "dev" if no argument is provided.
        /// </param>
        private Config(string appSetting)
        {
            Logger.Trace($"Config: Initializing configuration from appsettings.{appSetting}.json");

            try
            {
                _configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile($"appsettings.{appSetting}.json", optional: false, reloadOnChange: true)
                    .Build();

                Logger.Info($"Config: Configuration successfully loaded from appsettings.{appSetting}.json");
            }
            catch (Exception ex)
            {
                Logger.Error($"Config: Failed to load configuration from appsettings.{appSetting}.json. Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Gets the singleton instance of the Config class.
        /// </summary>
        /// <param name="appSetting">The suffix of the appsettings file to load, defaults to "dev".</param>
        /// <returns>The singleton instance of the Config class.</returns>
        public static Config GetInstance(string appSetting = "dev")
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        Logger.Trace($"Config: Creating new Config instance with appSetting: {appSetting}");
                        _instance = new Config(appSetting);
                    }
                }
            }
            else
            {
                Logger.Trace("Config: Returning existing Config instance");
            }
            return _instance;
        }

        /// <summary>
        /// Retrieves a connection string from the configuration.
        /// </summary>
        /// <param name="name">The name of the connection string to retrieve. Defaults to "localhost".</param>
        /// <returns>The specified connection string.</returns>
        public string? GetConnectionString(string name = "localhost")
        {
            Logger.Trace($"Config: Retrieving connection string for: {name}");
            string? connectionString = _configuration.GetConnectionString(name);

            if (string.IsNullOrEmpty(connectionString))
            {
                Logger.Warn($"Config: Connection string for '{name}' is null or empty.");
            }
            else
            {
                Logger.Trace($"Config: Connection string for '{name}' successfully retrieved.");
            }

            return connectionString;
        }

        /// <summary>
        /// Retrieves the file log level from the configuration.
        /// </summary>
        /// <returns>The file log level as a string.</returns>
        public string? GetFileLogLevel()
        {
            Logger.Trace("Config: Retrieving file log level");
            string? loglevel = _configuration["Logging:LogLevel:File"];
            if (string.IsNullOrEmpty(loglevel))
            {
                Logger.Warn("Config: File log level is null or empty.");
            }
            return _configuration["Logging:LogLevel:File"];
        }

        /// <summary>
        /// Retrieves the console log level from the configuration.
        /// </summary>
        /// <returns>The console log level as a string.</returns>
        public string? GetConsoleLogLevel()
        {
            Logger.Trace("Config: Retrieving console log level");
            string? loglevel = _configuration["Logging:LogLevel:Console"];
            if (string.IsNullOrEmpty(loglevel))
            {
                Logger.Warn("Config: Console log level is null or empty.");
            }
            return _configuration["Logging:LogLevel:Console"];
        }
    }
}

// Usage example:
// Config config = Config.GetInstance();
// _instance = new DbConnection(config.GetConnectionString());

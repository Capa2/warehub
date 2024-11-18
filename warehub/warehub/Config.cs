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
        private static Config? _instance;
        private static readonly object _lock = new();

        // The IConfiguration instance used to access the configuration settings.
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
            Logger.Debug("Loading configuration settings from appsettings." + appSetting + ".json");
            Logger.Trace(String.Join("appSetting:\n",appSetting));

            _configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile($"appsettings.{appSetting}.json", optional: false, reloadOnChange: true)
                .Build();
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
                        _instance = new Config(appSetting);
                    }
                }
            }
            return _instance;
        }

        /// <summary>
        /// Retrieves a connection string from the configuration.
        /// </summary>
        /// <param name="name">The name of the connection string to retrieve. Defaults to "localhost".</param>
        /// <returns>The specified connection string.</returns>
        public string GetConnectionString(string name = "localhost") => _configuration.GetConnectionString(name);
        public string GetFileLogLevel() => _configuration["Logging:LogLevel:File"];
        public string GetConsoleLogLevel() => _configuration["Logging:LogLevel:Console"];
    }
}

// Usage example:
// Config config = Config.GetInstance();
// _instance = new DbConnection(config.GetConnectionString());

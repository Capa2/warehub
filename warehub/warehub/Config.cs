using Microsoft.Extensions.Configuration;

namespace warehub
{
    /// <summary>
    /// The Config class is responsible for loading and accessing configuration settings from a specified JSON file.
    /// </summary>
    public class Config
    {
        // The IConfiguration instance used to access the configuration settings.
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the Config class, loading settings from a specified appsettings file.
        /// </summary>
        /// <param name="appSetting">
        /// The suffix of the appsettings file to load (e.g., "dev" loads "appsettings.dev.json").
        /// Defaults to "dev" if no argument is provided.
        /// </param>
        public Config(string appSetting = "dev")
        {
            // Build the configuration using the specified appsettings file.
            _configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory) // Set the base path to the current directory.
                .AddJsonFile($"appsettings.{appSetting}.json", optional: false, reloadOnChange: true) // Load the JSON config file.
                .Build(); // Build the IConfiguration instance.
        }

        /// <summary>
        /// Retrieves a connection string from the configuration.
        /// </summary>
        /// <param name="name">The name of the connection string to retrieve. Defaults to "localhost".</param>
        /// <returns>The specified connection string.</returns>
        public string GetConnectionString(string name = "localhost") => _configuration.GetConnectionString(name);

    }
}
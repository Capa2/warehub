using MySql.Data.MySqlClient;
using NLog;
using warehub;


namespace warehub.db
{
    /// <summary>
    /// Represents a thread-safe singleton class for managing a MySQL database connection.
    /// </summary>
    public class DbConnection
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private static DbConnection? _instance;
        private readonly MySqlConnection _connection;
        private static readonly object _lock = new(); // Lock object to ensure thread safety when initializing the singleton

        // Private constructor to avoid instantiation from outside
        private DbConnection(string connectionString) => _connection = new MySqlConnection(connectionString);

        /// <summary>
        /// Gets the singleton instance of DbConnection.
        /// </summary>
        private static DbConnection Instance
        {
            get
            {
                if (_instance == null)
                {
                    Logger.Warn("DbConnection has not been initialized. Call Initialize() first.");
                }
                return _instance;
            }
        }

        /// <summary>
        /// Initialize the singleton instance of DbConnection with connection string from config.
        /// </summary>
        private static void Initialize(string connectionString)
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    try {
                        Config config = Config.GetInstance();
                        _instance = new DbConnection(config.GetConnectionString(connectionString));
                        Logger.Info($"DbConnection singleton instance created targeting {connectionString}.");
                    } 
                    catch (Exception ex)
                    {
                        Logger.Error($"Failed to initialize DbConnection. Error: {ex.Message}");
                    }
                }
                else
                {
                    Logger.Warn("DbConnection singleton instance is already initialized. Ignoring re-initialization.");
                }
            }
        }
        
        /// <summary>
        /// Opens the MySQL connection if it is not already open.
        /// </summary>
        private void Connect()
        {
            try
            {
                if (_connection.State != System.Data.ConnectionState.Open)
                {
                    _connection.Open();
                    Logger.Info("Database connection successfully opened.");
                }
                else
                {
                    Logger.Debug("Database connection is already open.");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error occurred while connecting to the database.");
            }
        }

        /// <summary>
        /// Gets the MySqlConnection instance and ensures the connection is open.
        /// </summary>
        public static MySqlConnection GetConnection(string connectionString = "localhost")
        {
            if (_instance == null) Initialize(connectionString);
            Instance.Connect();
            return Instance._connection;
        }

        /// <summary>
        /// Closes the MySQL connection if it is open.
        /// </summary>
        public static void Disconnect()
        {
            try
            {
                if (Instance._connection.State != System.Data.ConnectionState.Closed)
                {
                    Instance._connection.Close();
                    Logger.Info("Database connection successfully closed.");
                }
                else
                {
                    Logger.Debug("Database connection is already closed.");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error occurred while disconnecting from the database.");
            }
        }
    }
}

// Usage example (Ensure appsettings.json and Config setup is correct)
// Get the connection for SQL operations, and it will automatically be open
// MySqlConnection connection = DbConnection.GetConnection();
// Use 'connection' to execute your SQL commands

// Close the connection when done
//DbConnection.Disconnect();
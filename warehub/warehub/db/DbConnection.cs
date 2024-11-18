using MySql.Data.MySqlClient;
using NLog;
using warehub;


namespace warehub.db
{
    public class DbConnection
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private static DbConnection? _instance;
        private readonly MySqlConnection _connection;

        // Private constructor to avoid instantiation from outside
        private DbConnection(string connectionString) => _connection = new MySqlConnection(connectionString);

        /// <summary>
        /// Gets the singleton instance of DbConnection with connection string from config.
        /// </summary>
        public static DbConnection Instance
        {
            get
            {
                if (_instance == null)
                {
                    Config config = Config.GetInstance();
                    string connectionString = config.GetConnectionString();
                    _instance = new DbConnection(connectionString);
                    Logger.Info("DbConnection singleton instance created.");
                }
                return _instance;
            }
        }

        /// <summary>
        /// Gets the MySqlConnection instance and ensures the connection is open.
        /// </summary>
        public static MySqlConnection GetConnection()
        {
            Instance.Connect();
            return Instance._connection;
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
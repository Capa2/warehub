using MySql.Data.MySqlClient;
using warehub;


namespace warehub.db
{
    public class DbConnection
    {
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
                    _instance = new DbConnection(config.GetConnectionString());
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
        public void Connect()
        {
            try
            {
                if (_connection.State != System.Data.ConnectionState.Open)
                {
                    _connection.Open();
                    Console.WriteLine("Database connection opened.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error connecting to database: " + ex.Message);
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
                    Console.WriteLine("Database connection closed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error disconnecting from database: " + ex.Message);
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
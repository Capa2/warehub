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
                    Config config = new();
                    _instance = new DbConnection(config.GetConnectionString());
                }
                return _instance;
            }
        }

        /// <summary>
        /// Gets the MySqlConnection instance.
        /// </summary>
        public MySqlConnection GetConnection() => _connection;

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
        public void Disconnect()
        {
            try
            {
                if (_connection.State != System.Data.ConnectionState.Closed)
                {
                    _connection.Close();
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
// DbConnection dbConnection = DbConnection.Instance;
// dbConnection.Connect();
// MySqlConnection connection = dbConnection.GetConnection();
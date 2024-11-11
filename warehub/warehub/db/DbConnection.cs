using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace warehub.db
{
    public class DbConnection
    {
        private static DbConnection _instance;
        private readonly MySqlConnection _connection;

        // Primary constructor accepting a connection string, it is private to avoid being instanciated from outside
        private DbConnection(string connectionString) => _connection = new MySqlConnection(connectionString);

        /// <summary>
        /// Gets the singleton instance of DbConnection with a specified connection string.
        /// </summary>
        /// <param name="connectionString">The connection string for MySQL.</param>
        /// <returns>The singleton instance of DbConnection.</returns>
        public static DbConnection GetInstance(string connectionString)
        {
            if (_instance == null)
            {
                _instance = new DbConnection(connectionString);
            }
            return _instance;
        }

        /// <summary>
        /// Retrieves the MySqlConnection instance.
        /// </summary>
        /// <returns>The active MySqlConnection.</returns>
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

//Example usage

//string connectionString = "Server=YOUR_SERVER;Database=YOUR_DATABASE;User ID=YOUR_USER;Password=YOUR_PASSWORD;";
//DbConnection dbConnection = DbConnection.GetInstance(connectionString);
//dbConnection.Connect();
//MySqlConnection connection = dbConnection.GetConnection();
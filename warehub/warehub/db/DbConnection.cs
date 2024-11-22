using MySql.Data.MySqlClient;
using NLog;
using warehub;


namespace warehub.db
{
    public class DbConnection(string connectionString) : IDbConnection
    {
        private readonly MySqlConnection _connection = new MySqlConnection(connectionString);

        public MySqlConnection GetConnection()
        {
            Connect();
            return _connection;
        }

        private void Connect()
        {
            try
            {
                if (_connection.State != System.Data.ConnectionState.Open)
                {
                    _connection.Open();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to open database connection.", ex);
            }
        }

        public void Disconnect()
        {
            try
            {
                if (_connection.State != System.Data.ConnectionState.Closed)
                {
                    _connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to close database connection.", ex);
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
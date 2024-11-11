using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using warehub.db;

namespace warehub.db
{
    public class CRUDService<T>
    {
        private readonly MySqlConnection _connection;

        // Primary constructor accepting MySqlConnection instance from DbConnections
        public CRUDService(MySqlConnection connection) => _connection = connection;

        /// <summary>
        /// Creates a new entry in the database.
        /// </summary>
        /// <param name="query">The SQL insert query string with parameters.</param>
        /// <param name="parameters">A dictionary of parameter names and values.</param>
        public void Create(string query, Dictionary<string, object> parameters)
        {
            try
            {
                using (var command = new MySqlCommand(query, _connection))
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value);
                    }
                    _connection.Open();
                    command.ExecuteNonQuery();
                    Console.WriteLine("Item created successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating item: " + ex.Message);
            }
            finally
            {
                _connection.Close();
            }
        }

        /// <summary>
        /// Reads entries from the database.
        /// </summary>
        /// <param name="query">The SQL select query string.</param>
        /// <returns>A list of dictionaries, each representing a row with column-value pairs.</returns>
        public List<Dictionary<string, object>> Read(string query)
        {
            var results = new List<Dictionary<string, object>>();
            try
            {
                using (var command = new MySqlCommand(query, _connection))
                {
                    _connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var row = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row[reader.GetName(i)] = reader.GetValue(i);
                            }
                            results.Add(row);
                        }
                    }
                }
                Console.WriteLine("Data retrieved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading data: " + ex.Message);
            }
            finally
            {
                _connection.Close();
            }
            return results;
        }

        /// <summary>
        /// Updates an existing entry in the database.
        /// </summary>
        /// <param name="query">The SQL update query string with parameters.</param>
        /// <param name="parameters">A dictionary of parameter names and values.</param>
        public void Update(string query, Dictionary<string, object> parameters)
        {
            try
            {
                using (var command = new MySqlCommand(query, _connection))
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value);
                    }
                    _connection.Open();
                    command.ExecuteNonQuery();
                    Console.WriteLine("Item updated successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating item: " + ex.Message);
            }
            finally
            {
                _connection.Close();
            }
        }

        /// <summary>
        /// Deletes an entry from the database.
        /// </summary>
        /// <param name="query">The SQL delete query string with parameters.</param>
        /// <param name="parameters">A dictionary of parameter names and values.</param>
        public void Delete(string query, Dictionary<string, object> parameters)
        {
            try
            {
                using (var command = new MySqlCommand(query, _connection))
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value);
                    }
                    _connection.Open();
                    command.ExecuteNonQuery();
                    Console.WriteLine("Item deleted successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting item: " + ex.Message);
            }
            finally
            {
                _connection.Close();
            }
        }
    }


}

//Example usage

// Create a new product.
//var parameters = new Dictionary<string, object>
//{
//    { "@name", "Product Name" },
//    { "@price", 19.99 },
//    { "@weight", 0.5 }
//};
//crudService.Create("INSERT INTO Products (Name, Price, Weight) VALUES (@name, @price, @weight)", parameters);

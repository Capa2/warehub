using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace warehub.db
{
    public class CRUDService
    {
        private readonly MySqlConnection _connection;

        // Primary constructor accepting MySqlConnection instance from DbConnection
        public CRUDService(MySqlConnection connection) => _connection = connection;

        /// <summary>
        /// Creates a new entry in the database.
        /// </summary>
        /// <param name="query">The SQL insert query string with parameters.</param>
        /// <param name="parameters">A dictionary of parameter names and values.</param>
        public bool Create(string query, Dictionary<string, object> parameters)
        {
            try
            {
                using (var command = new MySqlCommand(query, _connection))
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value);
                    }
                    command.ExecuteNonQuery();
                    Console.WriteLine("Item created successfully.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating item: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Reads entries from the database.
        /// </summary>
        /// <param name="query">The SQL select query string.</param>
        /// <returns>A list of dictionaries, each representing a row with column-value pairs.</returns>
        public (bool, List<Dictionary<string, object>>) Read(string query)
        {
            bool status = false;
            var results = new List<Dictionary<string, object>>();
            try
            {
                using (var command = new MySqlCommand(query, _connection))
                {
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
                status = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading data: " + ex.Message);
            }

            return (status, results);
        }

        /// <summary>
        /// Updates an existing entry in the database.
        /// </summary>
        /// <param name="query">The SQL update query string with parameters.</param>
        /// <param name="parameters">A dictionary of parameter names and values.</param>
        public bool Update(string query, Dictionary<string, object> parameters)
        {
            try
            {
                using (var command = new MySqlCommand(query, _connection))
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value);
                    }
                    command.ExecuteNonQuery();
                    Console.WriteLine("Item updated successfully.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating item: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Deletes an entry from the database.
        /// </summary>
        /// <param name="query">The SQL delete query string with parameters.</param>
        /// <param name="parameters">A dictionary of parameter names and values.</param>
        public bool Delete(string query, Dictionary<string, object> parameters)
        {
            try
            {
                using (var command = new MySqlCommand(query, _connection))
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value);
                    }
                    command.ExecuteNonQuery();
                    Console.WriteLine("Item deleted successfully.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting item: " + ex.Message);
                return false;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using ZstdSharp;

namespace warehub.db
{
    public class CRUDService
    {
        private readonly MySqlConnection _connection;

        // Primary constructor accepting MySqlConnection instance from DbConnection
        public CRUDService(MySqlConnection connection) => _connection = connection;

        /// <summary>
        /// Inserts a new entry into the specified table.
        /// </summary>
        public bool Create(string table, Dictionary<string, object> parameters)
        {
            var columns = string.Join(", ", parameters.Keys.Select(k => k));
            var values = string.Join(", ", parameters.Keys);
            string query = $"INSERT INTO {table} ({columns}) VALUES ({values})";

            return ExecuteNonQuery(query, parameters, "Item created successfully.");
        }

        /// <summary>
        /// Reads entries from the specified table with optional filtering.
        /// </summary>
        public (bool, List<Dictionary<string, object>>) Read(string table, Dictionary<string, object> parameters)
        {
            //The following line changes:
            //parameters = { "@id": 1, "@name": "Product" }
            //to
            //whereClause = "WHERE id = @id AND name = @name"
            string whereClause = parameters.Any()
                ? "WHERE " + string.Join(" AND ", parameters.Keys.Select(k => $"{k} = @{k}"))
                : "";

            string query = $"SELECT * FROM {table} {whereClause}";

            return ExecuteQuery(query, parameters, "Data retrieved successfully.");
        }

        /// <summary>
        /// Updates an existing entry in the specified table.
        /// </summary>
        public bool Update(string table, Dictionary<string, object> parameters, string idColumn, object idValue)
        {
            var setClause = string.Join(", ", parameters.Keys.Select(k => $"{k} = @{k}"));
            string query = $"UPDATE {table} SET {setClause} WHERE {idColumn} = @id";

            parameters["@id"] = idValue;
            return ExecuteNonQuery(query, parameters, "Item updated successfully.");
        }

        /// <summary>
        /// Deletes an entry from the specified table.
        /// </summary>
        public bool Delete(string table, string idColumn, object idValue)
        {
            string query = $"DELETE FROM {table} WHERE {idColumn} = @id";
            var parameters = new Dictionary<string, object> { { "@id", idValue } };

            return ExecuteNonQuery(query, parameters, "Item deleted successfully.");
        }

        /// <summary>
        /// Executes a non-query command such as INSERT, UPDATE, or DELETE.
        /// </summary>
        private bool ExecuteNonQuery(string query, Dictionary<string, object> parameters, string successMessage)
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
                    Console.WriteLine(successMessage);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Executes a query command and retrieves results as a list of dictionaries.
        /// </summary>
        private (bool, List<Dictionary<string, object>>) ExecuteQuery(string query, Dictionary<string, object> parameters, string successMessage)
        {
            var results = new List<Dictionary<string, object>>();
            bool status = false;

            try
            {
                using (var command = new MySqlCommand(query, _connection))
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value);
                    }

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
                Console.WriteLine(successMessage);
                status = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
            }

            return (status, results);
        }
    }
}

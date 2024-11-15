using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using ZstdSharp;
using warehub.services;

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
            try
            {
                var columns = string.Join(", ", parameters.Keys);
                var values = string.Join(", ", parameters.Keys.Select(k => $"@{k}"));
                string query = $"INSERT INTO {table} ({columns}) VALUES ({values})";

                return ExecuteNonQuery(query, parameters, "Item created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Create Error: {ex.Message}");
                return false;
            }
        }


        /// <summary>
        /// Reads entries from the specified table with optional filtering.
        /// </summary>
        public (bool, List<Dictionary<string, object>>) Read(string table, Dictionary<string, object> parameters)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"Read Error: {ex.Message}");
                return (false, null);
            }
        }

        /// <summary>
        /// Updates an existing entry in the specified table.
        /// </summary>
        public bool Update(string table, Dictionary<string, object> parameters, string idColumn, object idValue)
        {
            try
            {
                var setClause = string.Join(", ", parameters.Keys.Select(k => $"{k} = @{k}"));
                string query = $"UPDATE {table} SET {setClause} WHERE {idColumn} = @{idColumn}";

                parameters[idColumn] = idValue;
                return ExecuteNonQuery(query, parameters, "Item updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update Error: {ex.Message}");
                return false;
            }
        }


        /// <summary>
        /// Deletes an entry from the specified table.
        /// </summary>
        public bool Delete(string table, string idColumn, object idValue)
        {
            try
            {
                string query = $"DELETE FROM {table} WHERE {idColumn} = @{idColumn}";
                var parameters = new Dictionary<string, object> { { idColumn, idValue } };

                return ExecuteNonQuery(query, parameters, "Item deleted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Delete Error: {ex.Message}");
                return false;
            }
        }


        /// <summary>
        /// Executes a non-query command such as INSERT, UPDATE, or DELETE.
        /// </summary>
        private bool ExecuteNonQuery(string query, Dictionary<string, object> parameters, string successMessage, bool commitTransaction = true)
        {
            MySqlTransaction transaction = null;

            try
            {
                transaction = _connection.BeginTransaction();

                using (var command = new MySqlCommand(query, _connection, transaction))
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue($"@{param.Key}", param.Value);
                    }

                    int affectedRows = command.ExecuteNonQuery();
                    if (affectedRows > 0 && commitTransaction)
                    {
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                    }

                    return affectedRows > 0;
                }
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                Console.WriteLine($"SQL Error: {ex.Message}\nQuery: {query}");
                return false;
            }
            finally
            {
                transaction?.Dispose();
            }
        }



        /// <summary>
        /// Executes a query command and GETS results as a list of dictionaries.
        /// </summary>
        private (bool, List<Dictionary<string, object>>) ExecuteQuery(string query, Dictionary<string, object> parameters, string successMessage)
        {
            var results = new List<Dictionary<string, object>>();
            bool status = false;

            try
            {
                // Ensure the connection is active (should be managed by the singleton connection)
                if (_connection.State != System.Data.ConnectionState.Open)
                {
                    throw new InvalidOperationException("Database connection is not open.");
                }

                using (var command = new MySqlCommand(query, _connection))
                {
                    // Add parameters to the command
                    foreach (var param in parameters)
                    {
                        var value = param.Key == "id" ? GuidService.GuidToString((Guid)param.Value) : param.Value;
                        command.Parameters.AddWithValue($"@{param.Key}", value);
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        // Read and collect all rows from the result set
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

                    Console.WriteLine(successMessage);
                    status = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}\nQuery: {query}");
            }

            return (status, results);
        }
    }
}

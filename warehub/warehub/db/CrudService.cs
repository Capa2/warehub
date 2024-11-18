using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using ZstdSharp;
using warehub.services;
using NLog;

namespace warehub.db
{
    public class CRUDService
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
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

                if (ExecuteNonQuery(query, parameters, $"Item created in table '{table}' with values: {string.Join(", ", parameters.Select(p => $"{p.Key}={p.Value}"))}."))
                {
                    Logger.Debug($"Create operation successful for table '{table}'. Parameters: {string.Join(", ", parameters.Select(p => $"{p.Key}={p.Value}"))}.");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Create operation failed for table '{table}'. Parameters: {string.Join(", ", parameters.Select(p => $"{p.Key}={p.Value}"))}.");
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
                if (_connection.State != System.Data.ConnectionState.Open)
                {
                    Logger.Error("Attempted to read data while the database connection was not open.");
                    return (false, null); // Graceful failure
                }

                if (!TableTypeRegistry.TableColumnMappings.TryGetValue(table, out var columnTypeMapping))
                {
                    Logger.Error($"No type mapping found for table: {table}");
                    return (false, null); // Graceful failure
                }

                string whereClause = parameters.Any()
                    ? "WHERE " + string.Join(" AND ", parameters.Keys.Select(k => $"{k} = @{k}"))
                    : "";

                string query = $"SELECT * FROM {table} {whereClause}";
                Logger.Trace($"Generated Query for Read: {query}");

                var (status, results) = ExecuteQuery(query, parameters, $"Data retrieved from table '{table}'.", columnTypeMapping);

                if (status)
                {
                    Logger.Debug($"Read operation successful for table '{table}'. Retrieved {results.Count} items.");
                    if (results.Count > 100)
                    {
                        Logger.Info($"Read operation retrieved a large dataset ({results.Count} items) from table '{table}'.");
                    }
                }
                else
                {
                    Logger.Debug($"Read operation failed for table '{table}'.");
                }

                return (status, results);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Unexpected error during read operation for table '{table}'.");
                return (false, null); // Graceful failure
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

                if (ExecuteNonQuery(query, parameters, $"Item updated successfully in table '{table}' with {idColumn}={idValue}."))
                {
                    Logger.Debug($"Update operation successful for table '{table}'. Parameters: {string.Join(", ", parameters.Select(p => $"{p.Key}={p.Value}"))}.");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Update operation failed for table '{table}'. Parameters: {string.Join(", ", parameters.Select(p => $"{p.Key}={p.Value}"))}.");
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

                if (ExecuteNonQuery(query, parameters, $"Item deleted successfully in table '{table}' with {idColumn}={idValue}."))
                {
                    Logger.Debug($"Delete operation successful for table '{table}'. {idColumn}={idValue}.");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Delete operation failed for table '{table}'. {idColumn}={idValue}.");
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
                        Logger.Trace($"SuccessMessage: ({successMessage})");
                        return true;
                    }
                    else
                    {
                        transaction.Rollback();
                        Logger.Warn($"Transaction rolled back for query: {query}");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                Logger.Error(ex, $"SQL Error in ExecuteNonQuery. Query: {query}");
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
        private (bool, List<Dictionary<string, object>>) ExecuteQuery(string query, Dictionary<string, object> parameters, string successMessage, Dictionary<string, Type> columnTypeMapping)
        {
            var results = new List<Dictionary<string, object>>();
            bool status = false;

            try
            {
                using (var command = new MySqlCommand(query, _connection))
                {
                    foreach (var param in parameters)
                    {
                        var value = param.Key == "id" ? GuidService.GuidToString((Guid)param.Value) : param.Value;
                        command.Parameters.AddWithValue($"@{param.Key}", value);
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var row = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                string columnName = reader.GetName(i);
                                object value = reader.GetValue(i);

                                if (columnTypeMapping.TryGetValue(columnName, out var targetType))
                                {
                                    value = ConvertToType(value, targetType);
                                }

                                row[columnName] = value;
                            }
                            results.Add(row);
                        }
                    }

                    Logger.Trace($"SuccessMessage: ({successMessage})");
                    status = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"SQL Error in ExecuteQuery. Query: {query}");
            }

            return (status, results);
        }


        public static class TableTypeRegistry
        {
            public static readonly Dictionary<string, Dictionary<string, Type>> TableColumnMappings = new()
            {
                {
                    "products", // Table name
                    new Dictionary<string, Type>
                    {
                        { "id", typeof(Guid) },
                        { "name", typeof(string) },
                        { "price", typeof(decimal) }
                    }
                },
                {
                    "test_table",
                    new Dictionary<string, Type>
                    {
                        { "id", typeof(Guid) },
                        { "name", typeof(string) }
                    }
                },
                // Add mappings for additional tables as needed
                {
                    "another_table",
                    new Dictionary<string, Type>
                    {
                        { "column1", typeof(int) },
                        { "column2", typeof(DateTime) }
                    }
                }
            };
        }

        private object ConvertToType(object value, Type targetType)
        {
            if (value == DBNull.Value)
                return null;

            if (targetType == typeof(Guid))
            {
                return Guid.Parse(value.ToString());
            }

            return Convert.ChangeType(value, targetType);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using NLog;
using warehub.db.interfaces;
using warehub.db.utils;

namespace warehub.db
{
    /// <summary>
    /// Handles CRUD (Create, Read, Update, Delete) operations on a database table.
    /// </summary>
    public class CRUDService : ICRUDService
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private readonly MySqlConnection _connection;
        private readonly QueryExecutor _queryExecutor;

        /// <summary>
        /// Initializes a new instance of the <see cref="CRUDService"/> class.
        /// </summary>
        /// <param name="connection">An active MySQL database connection.</param>
        public CRUDService(MySqlConnection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _queryExecutor = new QueryExecutor(connection);
        }

        /// <summary>
        /// Inserts a new entry into the specified table.
        /// </summary>
        /// <param name="table">The name of the table.</param>
        /// <param name="parameters">A dictionary containing column names and their values.</param>
        /// <returns>True if the operation is successful; otherwise, false.</returns>
        public bool Create(string table, Dictionary<string, object> parameters)
        {
            try
            {
                var columns = string.Join(", ", parameters.Keys);
                var values = string.Join(", ", parameters.Keys.Select(k => $"@{k}"));
                string query = $"INSERT INTO {table} ({columns}) VALUES ({values})";

                if (_queryExecutor.ExecuteNonQuery(query, parameters, $"Item created in table '{table}' with values: {string.Join(", ", parameters.Select(p => $"{p.Key}={p.Value}"))}."))
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
        /// <param name="table">The name of the table.</param>
        /// <param name="parameters">Optional filtering criteria as a dictionary of column names and values.</param>
        /// <returns>A tuple containing a success flag and a list of retrieved rows as dictionaries.</returns>
        public (bool, List<Dictionary<string, object>>) Read(string table, Dictionary<string, object> parameters)
        {
            try
            {
                if (_connection.State != System.Data.ConnectionState.Open)
                {
                    Logger.Error("Attempted to read data while the database connection was not open.");
                    return (false, null); // Graceful failure
                }

                var columnTypeMapping = TableTypeUtility.GetColumnTypeMapping(table);

                string whereClause = parameters.Any()
                    ? "WHERE " + string.Join(" AND ", parameters.Keys.Select(k => $"{k} = @{k}"))
                    : "";

                string query = $"SELECT * FROM {table} {whereClause}";
                Logger.Trace($"Generated Query for Read: {query}");

                var (status, results) = _queryExecutor.ExecuteQuery(query, parameters, $"Data retrieved from table '{table}'.", columnTypeMapping);

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
        /// <param name="table">The name of the table.</param>
        /// <param name="parameters">A dictionary containing column names and their updated values.</param>
        /// <param name="idColumn">The column name for identifying the row to update.</param>
        /// <param name="idValue">The value of the identifying column.</param>
        /// <returns>True if the operation is successful; otherwise, false.</returns>
        public bool Update(string table, Dictionary<string, object> parameters, string idColumn, object idValue)
        {
            try
            {
                var setClause = string.Join(", ", parameters.Keys.Select(k => $"{k} = @{k}"));
                string query = $"UPDATE {table} SET {setClause} WHERE {idColumn} = @{idColumn}";

                parameters[idColumn] = idValue;

                if (_queryExecutor.ExecuteNonQuery(query, parameters, $"Item updated successfully in table '{table}' with {idColumn}={idValue}."))
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
        /// <param name="table">The name of the table.</param>
        /// <param name="idColumn">The column name for identifying the row to delete.</param>
        /// <param name="idValue">The value of the identifying column.</param>
        /// <returns>True if the operation is successful; otherwise, false.</returns>
        public bool Delete(string table, string idColumn, object idValue)
        {
            try
            {
                string query = $"DELETE FROM {table} WHERE {idColumn} = @{idColumn}";
                var parameters = new Dictionary<string, object> { { idColumn, idValue } };

                if (_queryExecutor.ExecuteNonQuery(query, parameters, $"Item deleted successfully in table '{table}' with {idColumn}={idValue}."))
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
    }
}

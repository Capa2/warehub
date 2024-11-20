using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using NLog;
using warehub.utils;

namespace warehub.db.utils
{
    /// <summary>
    /// Handles the execution of database queries and commands for non-CRUD-specific operations. Mainly Works in corrolation with CrudService.cs
    /// </summary>
    public class QueryExecutor
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private readonly MySqlConnection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryExecutor"/> class.
        /// </summary>
        /// <param name="connection">An active MySQL database connection.</param>
        public QueryExecutor(MySqlConnection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        /// <summary>
        /// Executes a non-query SQL command such as INSERT, UPDATE, or DELETE.
        /// </summary>
        /// <param name="query">The SQL query to execute.</param>
        /// <param name="parameters">A dictionary of parameters for the query.</param>
        /// <param name="successMessage">A message to log upon successful execution.</param>
        /// <param name="commitTransaction">Indicates whether the operation should be committed as a transaction.</param>
        /// <returns>True if the operation is successful; otherwise, false.</returns>
        public bool ExecuteNonQuery(string query, Dictionary<string, object> parameters, string successMessage, bool commitTransaction = true)
        {
            MySqlTransaction? transaction = null;

            try
            {
                // Begin a transaction for the operation
                transaction = _connection.BeginTransaction();

                using (var command = new MySqlCommand(query, _connection, transaction))
                {
                    command.CommandTimeout = 30; // Avoid endless execution

                    // Add parameters to the query
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue($"@{param.Key}", param.Value);
                    }

                    // Execute the non-query command
                    int affectedRows = command.ExecuteNonQuery();

                    // Commit or rollback the transaction based on the result
                    if (affectedRows > 0 && commitTransaction)
                    {
                        transaction.Commit();
                        Logger.Trace($"Transaction committed for query: {query}");
                        Logger.Debug($"Success Message: {successMessage}");
                        return true;
                    }
                    else
                    {
                        Logger.Warn($"No rows affected. Rolling back transaction for query: {query}");
                        transaction.Rollback();
                        return false;
                    }
                }
            }
            catch (MySqlException ex)
            {
                transaction?.Rollback();
                Logger.Error(ex, $"SQL Error in ExecuteNonQuery. Query: {query}");
                return false; // Gracefully fail on SQL error
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                Logger.Error(ex, $"Unexpected error in ExecuteNonQuery. Query: {query}");
                throw; // Re-throw for unexpected issues
            }
            finally
            {
                transaction?.Dispose();
            }
        }


        /// <summary>
        /// Executes a query and retrieves the results as a list of dictionaries.
        /// </summary>
        /// <param name="query">The SQL query to execute.</param>
        /// <param name="parameters">A dictionary of parameters for the query.</param>
        /// <param name="successMessage">A message to log upon successful execution.</param>
        /// <param name="columnTypeMapping">A dictionary mapping column names to their expected data types.</param>
        /// <returns>
        /// A tuple containing a success flag and a list of rows, where each row is represented as a dictionary of column names and values.
        /// </returns>
        public (bool, List<Dictionary<string, object>>) ExecuteQuery(
            string query,
            Dictionary<string, object> parameters,
            string successMessage,
            Dictionary<string, Type> columnTypeMapping)
        {
            var results = new List<Dictionary<string, object>>();
            bool status = false;

            MySqlTransaction? transaction = null;

            try
            {
                // Begin a transaction to ensure data consistency
                transaction = _connection.BeginTransaction();

                using (var command = new MySqlCommand(query, _connection, transaction))
                {
                    // Add parameters to the query
                    foreach (var param in parameters)
                    {
                        object value;

                        if (param.Key == "id" && param.Value is Guid guidValue)
                        {
                            value = GuidUtil.GuidToString(guidValue);
                        }
                        else
                        {
                            value = param.Value;
                        }

                        command.Parameters.AddWithValue($"@{param.Key}", value);
                    }

                    // Execute the query and process the results
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var row = new Dictionary<string, object>();

                            // Map the data types and populate the row dictionary
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                string columnName = reader.GetName(i);
                                object? value = reader.GetValue(i);

                                if (columnTypeMapping.TryGetValue(columnName, out var targetType))
                                {
                                    value = ConvertToType(value, targetType);
                                }

                                row[columnName] = value;
                            }

                            results.Add(row);
                        }
                    }

                    // Commit the transaction after successfully reading the data
                    transaction.Commit();
                    Logger.Debug(successMessage);
                    status = true;
                }
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                Logger.Error(ex, $"SQL Error in ExecuteQuery. Query: {query}");
            }
            finally
            {
                transaction?.Dispose();
            }

            return (status, results);
        }


        /// <summary>
        /// Converts a database value to a specified .NET type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The target type to convert the value to.</param>
        /// <returns>The converted value.</returns>
        private object? ConvertToType(object value, Type targetType)
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

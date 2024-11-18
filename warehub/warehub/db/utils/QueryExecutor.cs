﻿using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using NLog;
using warehub.services;

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
            MySqlTransaction transaction = null;

            try
            {
                // Begin a transaction for the operation
                transaction = _connection.BeginTransaction();

                using (var command = new MySqlCommand(query, _connection, transaction))
                {
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
                        Logger.Debug(successMessage);
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

            try
            {
                using (var command = new MySqlCommand(query, _connection))
                {
                    // Add parameters to the query
                    foreach (var param in parameters)
                    {
                        var value = param.Key == "id" ? GuidService.GuidToString((Guid)param.Value) : param.Value;
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

                    Logger.Debug(successMessage);
                    status = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"SQL Error in ExecuteQuery. Query: {query}");
            }

            return (status, results);
        }

        /// <summary>
        /// Converts a database value to a specified .NET type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The target type to convert the value to.</param>
        /// <returns>The converted value.</returns>
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
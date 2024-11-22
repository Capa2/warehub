using System;
using System.Collections.Generic;
using NLog;

namespace warehub.db.utils
{
    /// <summary>
    /// Provides utility methods and data for managing table-to-column type mappings
    /// and converting database values to .NET types.
    /// </summary>
    public static class TableTypeUtility
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Registry containing type mappings for columns in different tables.
        /// Each table maps column names to their corresponding .NET types.
        /// </summary>
        public static readonly Dictionary<string, Dictionary<string, Type>> TableColumnMappings = new()
        {
            {
                "products",
                new Dictionary<string, Type>
                {
                    { "id", typeof(Guid) },
                    { "name", typeof(string) },
                    { "price", typeof(decimal) },
                    { "amount", typeof(int) }
                }
            },
            {
                "another_table",
                new Dictionary<string, Type>
                {
                    { "column1", typeof(int) },
                    { "column2", typeof(DateTime) }
                }
            }
        };

        /// <summary>
        /// Converts a database value to the specified .NET type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The target .NET type for the conversion.</param>
        /// <returns>The converted value or null if the conversion is not possible.</returns>
        public static object ConvertToType(object value, Type targetType)
        {
            Logger.Trace($"Attempting to convert value '{value}' to type '{targetType.FullName}'.");

            if (value == DBNull.Value)
            {
                Logger.Debug($"Value is DBNull. Returning null.");
                return null;
            }

            try
            {
                // Handle Guid conversion from string
                if (targetType == typeof(Guid) && value is string stringValue)
                {
                    Logger.Debug($"Value is a string. Attempting to parse as Guid.");
                    var result = Guid.Parse(stringValue);
                    Logger.Trace($"Successfully converted '{value}' to Guid.");
                    return result;
                }

                // Convert to the target type
                var convertedValue = Convert.ChangeType(value, targetType);
                Logger.Trace($"Successfully converted '{value}' to type '{targetType.FullName}'.");
                return convertedValue;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Failed to convert value '{value}' to type '{targetType.FullName}'.");
                throw new InvalidOperationException($"Failed to convert value '{value}' to type '{targetType.FullName}'.", ex);
            }
        }

        /// <summary>
        /// Gets the column type mapping for the specified table.
        /// </summary>
        /// <param name="tableName">The name of the table.</param>
        /// <returns>A dictionary of column names to their .NET types.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the table mapping is not found.</exception>
        public static Dictionary<string, Type> GetColumnTypeMapping(string tableName)
        {
            Logger.Trace($"Attempting to retrieve column type mapping for table '{tableName}'.");

            if (TableColumnMappings.TryGetValue(tableName, out var columnMapping))
            {
                Logger.Debug($"Column type mapping found for table '{tableName}'.");
                return columnMapping;
            }

            Logger.Warn($"No column type mapping found for table '{tableName}'.");
            throw new InvalidOperationException($"No type mapping found for table: {tableName}");
        }
    }
}

using System;
using System.Collections.Generic;

namespace warehub.db.utils
{
    /// <summary>
    /// Provides utility methods and data for managing table-to-column type mappings
    /// and converting database values to .NET types.
    /// </summary>
    public static class TableTypeUtility
    {
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
            if (value == DBNull.Value)
                return null;

            try
            {
                if (targetType == typeof(Guid))
                {
                    return Guid.Parse(value.ToString());
                }

                return Convert.ChangeType(value, targetType);
            }
            catch (Exception ex)
            {
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
            if (TableColumnMappings.TryGetValue(tableName, out var columnMapping))
            {
                return columnMapping;
            }

            throw new InvalidOperationException($"No type mapping found for table: {tableName}");
        }
    }
}

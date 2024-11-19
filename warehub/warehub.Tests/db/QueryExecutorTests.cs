using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Xunit;
using warehub.db;
using warehub.db.utils;

namespace warehub.Tests.db.utils
{
    public class QueryExecutorTests : IDisposable
    {
        private readonly MySqlConnection _connection;
        private readonly QueryExecutor _queryExecutor;

        public QueryExecutorTests()
        {
            // Use a new connection specifically for this test class
            _connection = new MySqlConnection("your_connection_string_here");
            _connection.Open();
            _queryExecutor = new QueryExecutor(_connection);

            // Ensure test table exists
            CreateTestTable();
        }

        [Fact]
        public void ExecuteNonQuery_ShouldInsertItem()
        {
            // Arrange
            string insertQuery = "INSERT INTO test_table (id, name) VALUES (@id, @name)";
            var parameters = new Dictionary<string, object>
            {
                { "id", Guid.NewGuid() },
                { "name", "Test Item" }
            };

            // Act
            bool result = _queryExecutor.ExecuteNonQuery(insertQuery, parameters, "Insert succeeded.");

            // Assert
            Assert.True(result, "Failed to insert item into the database.");
        }

        [Fact]
        public void ExecuteQuery_ShouldRetrieveInsertedItem()
        {
            // Arrange
            Guid testId = Guid.NewGuid();
            string insertQuery = "INSERT INTO test_table (id, name) VALUES (@id, @name)";
            var insertParameters = new Dictionary<string, object>
            {
                { "id", testId },
                { "name", "Test Item" }
            };
            _queryExecutor.ExecuteNonQuery(insertQuery, insertParameters, "Insert succeeded.");

            string selectQuery = "SELECT * FROM test_table WHERE id = @id";
            var selectParameters = new Dictionary<string, object>
            {
                { "id", testId }
            };

            // Act
            var (success, result) = _queryExecutor.ExecuteQuery(selectQuery, selectParameters, "Select succeeded.", TableTypeUtility.GetColumnTypeMapping("test_table"));

            // Assert
            Assert.True(success, "Failed to retrieve the item from the database.");
            Assert.Single(result);
            Assert.Equal("Test Item", result[0]["name"]);
        }

        [Fact]
        public void ExecuteNonQuery_ShouldRollbackOnError()
        {
            // Arrange
            string invalidQuery = "INSERT INTO test_table (id, name) VALUES (@id, @non_existing_column)";
            var parameters = new Dictionary<string, object>
            {
                { "id", Guid.NewGuid() },
                { "name", "Test Item" }
            };

            // Act
            bool result = _queryExecutor.ExecuteNonQuery(invalidQuery, parameters, "Insert should fail.");

            // Assert
            Assert.False(result, "Query should have failed and rolled back.");
        }

        private void CreateTestTable()
        {
            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS test_table (
                    id CHAR(36) PRIMARY KEY,
                    name VARCHAR(50)
                );";

            using (var command = new MySqlCommand(createTableQuery, _connection))
            {
                command.ExecuteNonQuery();
            }
        }

        public void Dispose()
        {
            string dropTableQuery = "DROP TABLE IF EXISTS test_table";

            using (var command = new MySqlCommand(dropTableQuery, _connection))
            {
                command.ExecuteNonQuery();
            }

            _connection.Dispose();
        }
    }
}

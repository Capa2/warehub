using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using NLog;
using Xunit;
using warehub.db;
using warehub.db.utils;

namespace warehub.Tests.db.utils
{
    public class QueryExecutorTests : IDisposable
    {
        private readonly MySqlConnection _connection;
        private readonly QueryExecutor _queryExecutor;
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public QueryExecutorTests()
        {
            try
            {
                Logger.Trace("Initializing QueryExecutorTests...");
                _connection = DbConnection.GetConnection("test");
                Logger.Debug("Connection to test database established.");

                _queryExecutor = new QueryExecutor(_connection);

                // Ensure the products table exists
                CreateProductsTable();
                Logger.Debug("Products table ensured for testing.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to initialize QueryExecutorTests.");
                throw;
            }
        }

        [Fact]
        public void ExecuteNonQuery_ShouldInsertProduct()
        {
            Logger.Trace("Starting test: ExecuteNonQuery_ShouldInsertProduct");
            // Arrange
            string insertQuery = "INSERT INTO products (id, name, price, amount) VALUES (@id, @name, @price, @amount)";
            var parameters = new Dictionary<string, object>
            {
                { "id", Guid.NewGuid() },
                { "name", "Test Product" },
                { "price", 19.99m },
                { "amount", 100 }
            };

            // Act
            bool result = _queryExecutor.ExecuteNonQuery(insertQuery, parameters, "Insert succeeded.");

            // Assert
            Assert.True(result, "Failed to insert product into the database.");
            Logger.Debug("Test ExecuteNonQuery_ShouldInsertProduct passed.");
        }

        [Fact]
        public void ExecuteQuery_ShouldRetrieveInsertedProduct()
        {
            Logger.Trace("Starting test: ExecuteQuery_ShouldRetrieveInsertedProduct");
            // Arrange
            Guid testId = Guid.NewGuid();
            string insertQuery = "INSERT INTO products (id, name, price, amount) VALUES (@id, @name, @price, @amount)";
            var insertParameters = new Dictionary<string, object>
            {
                { "id", testId },
                { "name", "Test Product" },
                { "price", 19.99m },
                { "amount", 100 }
            };
            _queryExecutor.ExecuteNonQuery(insertQuery, insertParameters, "Insert succeeded.");

            string selectQuery = "SELECT * FROM products WHERE id = @id";
            var selectParameters = new Dictionary<string, object>
            {
                { "id", testId }
            };

            // Act
            var (success, result) = _queryExecutor.ExecuteQuery(selectQuery, selectParameters, "Select succeeded.", TableTypeUtility.GetColumnTypeMapping("products"));

            // Assert
            Assert.True(success, "Failed to retrieve the product from the database.");
            Assert.Single(result);
            Assert.Equal("Test Product", result[0]["name"]);
            Assert.Equal(19.99m, result[0]["price"]);
            Assert.Equal(100, result[0]["amount"]);
            Logger.Debug("Test ExecuteQuery_ShouldRetrieveInsertedProduct passed.");
        }

        [Fact]
        public void ExecuteNonQuery_ShouldRollbackOnError()
        {
            Logger.Trace("Starting test: ExecuteNonQuery_ShouldRollbackOnError");

            // Arrange
            string invalidQuery = "INSERT INTO products (id, name, price, amount) VALUES (@id, @non_existing_column, @price, @amount)";
            var parameters = new Dictionary<string, object>
            {
                { "id", Guid.NewGuid() },
                { "name", "Test Product" },
                { "price", 19.99m },
                { "amount", 100 }
            };

            // Act
            bool result = false;
            try
            {
                result = _queryExecutor.ExecuteNonQuery(invalidQuery, parameters, "Insert should fail.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Expected error occurred during invalid query execution.");
            }

            // Assert
            Assert.False(result, "Query should have failed and rolled back.");
            Logger.Debug("Test ExecuteNonQuery_ShouldRollbackOnError passed.");
        }


        private void CreateProductsTable()
        {
            Logger.Trace("Ensuring products table exists...");
            try
            {
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS products (
                        id CHAR(36) PRIMARY KEY,
                        name VARCHAR(50),
                        price DECIMAL(10, 2),
                        amount INT
                    );";

                using (var command = new MySqlCommand(createTableQuery, _connection))
                {
                    command.CommandTimeout = 30; // Add timeout to avoid indefinite hang
                    command.ExecuteNonQuery();
                }
                Logger.Debug("Products table created or already exists.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to create products table.");
                throw new InvalidOperationException("Failed to create products table. Check the test schema.", ex);
            }
        }

        public void Dispose()
        {
            Logger.Trace("Cleaning up QueryExecutorTests...");
            try
            {
                string dropTableQuery = "DROP TABLE IF EXISTS products";

                using (var command = new MySqlCommand(dropTableQuery, _connection))
                {
                    command.CommandTimeout = 30; // Add timeout to avoid indefinite hang
                    command.ExecuteNonQuery();
                }
                Logger.Debug("Products table dropped successfully.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error during test cleanup.");
            }
            finally
            {
                _connection.Close();
                _connection.Dispose();
                Logger.Debug("Connection to test database closed.");
            }
        }
    }
}

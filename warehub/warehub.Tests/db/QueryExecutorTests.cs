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
            // Initialize the DbConnection with the test schema
            _connection = DbConnection.GetConnection("test");
            _queryExecutor = new QueryExecutor(_connection);

            // Ensure the products table exists
            CreateProductsTable();
        }

        [Fact]
        public void ExecuteNonQuery_ShouldInsertProduct()
        {
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
        }

        [Fact]
        public void ExecuteQuery_ShouldRetrieveInsertedProduct()
        {
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
        }

        [Fact]
        public void ExecuteNonQuery_ShouldRollbackOnError()
        {
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
            bool result = _queryExecutor.ExecuteNonQuery(invalidQuery, parameters, "Insert should fail.");

            // Assert
            Assert.False(result, "Query should have failed and rolled back.");
        }

        private void CreateProductsTable()
        {
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
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to create products table. Check the test schema.", ex);
            }
        }

        public void Dispose()
        {
            try
            {
                string dropTableQuery = "DROP TABLE IF EXISTS products";

                using (var command = new MySqlCommand(dropTableQuery, _connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during test cleanup: {ex.Message}");
            }
            finally
            {
                DbConnection.Disconnect();
            }
        }
    }
}

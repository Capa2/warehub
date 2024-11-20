using System;
using System.Collections.Generic;
using Xunit;
using warehub.db;
using MySql.Data.MySqlClient;

namespace warehub.Tests.db
{
    public class DatabaseFixture : IDisposable
    {
        private readonly MySqlConnection _connection;

        public CRUDService CrudService { get; }

        public DatabaseFixture()
        {
            // Use a dedicated connection for the test schema
            DbConnection.Initialize("test");
            DbConnection.Connect();
            _connection = new MySqlConnection(DbConnection.GetConnection().ConnectionString);
            _connection.Open();
            CrudService = new CRUDService(_connection);

            // Ensure the products table exists
            EnsureTestTableExists();
        }

        /// <summary>
        /// Ensures the products table exists in the test schema.
        /// </summary>
        /// <summary>
        /// Ensures the products table exists in the test schema.
        /// </summary>
        private void EnsureTestTableExists()
        {
            try
            {
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS products (
                    id CHAR(36) PRIMARY KEY,
                    name VARCHAR(100) NOT NULL,
                    price DECIMAL(10, 2) NOT NULL,
                    amount INT NOT NULL
                 );";

                using (var command = new MySqlCommand(createTableQuery, _connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to ensure the products table exists. Check the test schema and connection.", ex);
            }
        }

        /// <summary>
        /// Cleans up the database by dropping the products table.
        /// </summary>
        /// <summary>
        /// Cleans up resources without dropping the products table.
        /// </summary>
        public void Dispose()
        {
            try
            {
                // No table dropping is performed here.
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during resource cleanup: {ex.Message}");
            }
            finally
            {
                _connection.Close();
                _connection.Dispose();
            }
        }

    }

    public class CRUDServiceIntegrationTests : IClassFixture<DatabaseFixture>
    {
        private readonly CRUDService _crudService;
        private readonly Guid _testId = Guid.NewGuid(); // Shared test ID

        public CRUDServiceIntegrationTests(DatabaseFixture fixture)
        {
            _crudService = fixture.CrudService;
        }

        [Fact]
        public void Create_ShouldInsertItem()
        {
            // Arrange
            var createParameters = new Dictionary<string, object>
            {
                { "id", _testId },
                { "name", "Test Item" },
                { "price", 199.99 },
                { "amount", 100 }
            };

            // Act
            bool createStatus = _crudService.Create("products", createParameters);

            // Assert
            Assert.True(createStatus, "Failed to create item in database.");
        }

        [Fact]
        public void Read_ShouldRetrieveItem()
        {
            // Arrange
            EnsureTestItemExists();

            // Act
            var (readStatus, readResult) = _crudService.Read("products", new Dictionary<string, object> { { "id", _testId } });

            // Assert
            Assert.True(readStatus, "Read operation failed.");
            Assert.Single(readResult);
            Assert.Equal("Test Item", readResult[0]["name"]);
        }

        [Fact]
        public void Update_ShouldModifyItem()
        {
            // Arrange
            EnsureTestItemExists();
            var updateParameters = new Dictionary<string, object> { { "name", "Updated Item" } };

            // Act
            bool updateStatus = _crudService.Update("products", updateParameters, "id", _testId);

            // Assert
            Assert.True(updateStatus, "Update operation failed.");

            // Verify update
            var (readStatus, readResult) = _crudService.Read("products", new Dictionary<string, object> { { "id", _testId } });
            Assert.True(readStatus, "Read operation failed after update.");
            Assert.Single(readResult);
            Assert.Equal("Updated Item", readResult[0]["name"]);
        }

        [Fact]
        public void Delete_ShouldRemoveItem()
        {
            // Arrange
            EnsureTestItemExists();

            // Act
            bool deleteStatus = _crudService.Delete("products", "id", _testId);

            // Assert
            Assert.True(deleteStatus, "Delete operation failed.");

            // Verify deletion
            var (readStatus, readResult) = _crudService.Read("products", new Dictionary<string, object> { { "id", _testId } });
            Assert.True(readStatus, "Read operation failed after deletion.");
            Assert.Empty(readResult);
        }

        /// <summary>
        /// Ensures a test item exists in the database for tests that require it.
        /// </summary>
        private void EnsureTestItemExists()
        {
            var createParameters = new Dictionary<string, object>
            {
                { "id", _testId },
                { "name", "Test Item" },
                { "price", 199.99 },
                { "amount", 100 }
            };

            var (readStatus, readResult) = _crudService.Read("products", new Dictionary<string, object> { { "id", _testId } });

            if (!readStatus || readResult.Count == 0)
            {
                bool createStatus = _crudService.Create("products", createParameters);
                if (!createStatus)
                {
                    throw new InvalidOperationException("Test setup failed: unable to create test item.");
                }
            }
        }
    }
}

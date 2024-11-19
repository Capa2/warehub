using System;
using System.Collections.Generic;
using Xunit;
using warehub.db;
using MySql.Data.MySqlClient;

namespace warehub.Tests.db
{
    public class DatabaseFixture : IDisposable
    {
        public CRUDService CrudService { get; }

        public DatabaseFixture()
        {
            var connection = DbConnection.GetConnection();
            CrudService = new CRUDService(connection);

            // Ensure the test table exists
            EnsureTestTableExists();
        }

        /// <summary>
        /// Ensures the test table exists before running tests.
        /// </summary>
        private void EnsureTestTableExists()
        {
            try
            {
                string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS test_table (
                    id CHAR(36) PRIMARY KEY,
                    name VARCHAR(50)
                );";

                using (var command = new MySqlCommand(createTableQuery, DbConnection.GetConnection()))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error ensuring test table existence: " + ex.Message);
                throw new InvalidOperationException("Test setup failed: unable to create or verify test_table.", ex);
            }
        }

        /// <summary>
        /// Cleans up the database by dropping the test table.
        /// </summary>
        public void Dispose()
        {
            try
            {
                string dropTableQuery = "DROP TABLE IF EXISTS test_table";
                using (var command = new MySqlCommand(dropTableQuery, DbConnection.GetConnection()))
                {
                    command.ExecuteNonQuery();
                }
            }
            finally
            {
                DbConnection.Disconnect();
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
                { "name", "Test Item" }
            };

            // Act
            bool createStatus = _crudService.Create("test_table", createParameters);

            // Assert
            Assert.True(createStatus, "Failed to create item in database.");
        }

        [Fact]
        public void Read_ShouldRetrieveItem()
        {
            // Arrange
            EnsureTestItemExists();

            // Act
            var (readStatus, readResult) = _crudService.Read("test_table", new Dictionary<string, object> { { "id", _testId } });

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
            bool updateStatus = _crudService.Update("test_table", updateParameters, "id", _testId);

            // Assert
            Assert.True(updateStatus, "Update operation failed.");

            // Verify update
            var (readStatus, readResult) = _crudService.Read("test_table", new Dictionary<string, object> { { "id", _testId } });
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
            bool deleteStatus = _crudService.Delete("test_table", "id", _testId);

            // Assert
            Assert.True(deleteStatus, "Delete operation failed.");

            // Verify deletion
            var (readStatus, readResult) = _crudService.Read("test_table", new Dictionary<string, object> { { "id", _testId } });
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
                { "name", "Test Item" }
            };

            var (readStatus, readResult) = _crudService.Read("test_table", new Dictionary<string, object> { { "id", _testId } });

            if (!readStatus || readResult.Count == 0)
            {
                bool createStatus = _crudService.Create("test_table", createParameters);
                if (!createStatus)
                {
                    throw new InvalidOperationException("Test setup failed: unable to create test item.");
                }
            }
        }
    }
}

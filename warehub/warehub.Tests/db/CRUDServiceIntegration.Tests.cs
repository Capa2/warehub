using System;
using System.Collections.Generic;
using Xunit;
using warehub.db;
using warehub.db.utils;
using MySql.Data.MySqlClient;

namespace warehub.Tests.db
{
    /// <summary>
    /// Sets up the database for integration tests and provides a shared instance of CRUDService.
    /// </summary>
    public class DatabaseFixture : IDisposable
    {
        public CRUDService CrudService { get; }

        public DatabaseFixture()
        {
            // Initialize the CRUDService with the MySQL connection
            var connection = DbConnection.GetConnection();
            var queryExecutor = new QueryExecutor(connection); // New dependency
            CrudService = new CRUDService(connection);

            // Create the test table once
            CreateTestTable();
        }

        /// <summary>
        /// Creates a test table used for integration tests.
        /// </summary>
        private void CreateTestTable()
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
                Console.WriteLine("Error creating test table: " + ex.Message);
                throw new InvalidOperationException("Test setup failed: unable to create test_table.", ex);
            }
        }

        /// <summary>
        /// Cleans up by dropping the test table and disconnecting the database connection.
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
                // Ensure database disconnection
                DbConnection.Disconnect();
            }
        }
    }

    /// <summary>
    /// Integration tests for CRUDService operations.
    /// </summary>
    public class CRUDServiceIntegrationTests : IClassFixture<DatabaseFixture>
    {
        private readonly CRUDService _crudService;

        /// <summary>
        /// Initializes the test class with a shared instance of CRUDService from the fixture.
        /// </summary>
        /// <param name="fixture">The database fixture providing a shared CRUDService instance.</param>
        public CRUDServiceIntegrationTests(DatabaseFixture fixture)
        {
            _crudService = fixture.CrudService;
        }

        [Fact]
        public void CRUDOperations_ShouldPerformAllSteps()
        {
            // Generate a new GUID for the test
            var testId = Guid.NewGuid();

            // Step 1: Create
            var createParameters = new Dictionary<string, object>
            {
                { "id", testId.ToString() },
                { "name", "Test Item" }
            };

            bool createStatus = _crudService.Create("test_table", createParameters);
            Assert.True(createStatus, "Failed to create item in database.");

            // Verify creation
            var (readStatus, readResult) = _crudService.Read("test_table", new Dictionary<string, object> { { "id", testId.ToString() } });
            Assert.True(readStatus, "Read operation failed after creation.");
            Assert.Single(readResult);
            Assert.Equal("Test Item", readResult[0]["name"]);

            // Step 2: Update
            var updateParameters = new Dictionary<string, object> { { "name", "Updated Item" } };
            bool updateStatus = _crudService.Update("test_table", updateParameters, "id", testId.ToString());
            Assert.True(updateStatus, "Update operation failed.");

            // Verify update
            var (readAfterUpdateStatus, readAfterUpdateResult) = _crudService.Read("test_table", new Dictionary<string, object> { { "id", testId.ToString() } });
            Assert.True(readAfterUpdateStatus, "Read operation failed after update.");
            Assert.Single(readAfterUpdateResult);
            Assert.Equal("Updated Item", readAfterUpdateResult[0]["name"]);

            // Step 3: Delete
            bool deleteStatus = _crudService.Delete("test_table", "id", testId.ToString());
            Assert.True(deleteStatus, "Delete operation failed.");

            // Verify deletion
            var (readAfterDeleteStatus, readAfterDeleteResult) = _crudService.Read("test_table", new Dictionary<string, object> { { "id", testId.ToString() } });
            Assert.True(readAfterDeleteStatus, "Read operation failed after deletion.");
            Assert.Empty(readAfterDeleteResult);
        }
    }
}

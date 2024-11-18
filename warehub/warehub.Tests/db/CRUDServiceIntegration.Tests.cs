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
            // Initialize the CRUD service with the MySQL connection
            CrudService = new CRUDService(DbConnection.GetConnection());

            // Create the test table once
            CreateTestTable();
        }

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

        public void Dispose()
        {
            // Drop the test table once after all tests
            string dropTableQuery = "DROP TABLE IF EXISTS test_table";
            using (var command = new MySqlCommand(dropTableQuery, DbConnection.GetConnection()))
            {
                command.ExecuteNonQuery();
            }

            // Disconnect from the database
            DbConnection.Disconnect();
        }
    }


    public class CRUDServiceIntegrationTests : IClassFixture<DatabaseFixture>
    {
        private readonly CRUDService _crudService;

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

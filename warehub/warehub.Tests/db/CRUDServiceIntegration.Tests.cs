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
                    id INT PRIMARY KEY,
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
        public void Create_ShouldInsertDataIntoDatabase()
        {
            var parameters = new Dictionary<string, object>
            {
                { "id", 1 },
                { "name", "Test Item" }
            };

            bool createStatus = _crudService.Create("test_table", parameters);

            Assert.True(createStatus, "Failed to create item in database.");

            var (status, result) = _crudService.Read("test_table", new Dictionary<string, object> { { "id", 1 } });
            Assert.True(status, "Read operation failed.");
            Assert.Single(result);
            Assert.Equal("Test Item", result[0]["name"]);
        }

        [Fact]
        public void Read_ShouldRetrieveDataFromDatabase()
        {
            _crudService.Create("test_table", new Dictionary<string, object> { { "id", 2 }, { "name", "Read Test" } });

            var (status, result) = _crudService.Read("test_table", new Dictionary<string, object> { { "id", 2 } });

            Assert.True(status, "Read operation failed.");
            Assert.Single(result);
            Assert.Equal("Read Test", result[0]["name"]);
        }

        [Fact]
        public void Update_ShouldModifyExistingData()
        {
            _crudService.Create("test_table", new Dictionary<string, object> { { "id", 3 }, { "name", "Old Name" } });
            var updateParameters = new Dictionary<string, object> { { "name", "New Name" } };

            bool updateStatus = _crudService.Update("test_table", updateParameters, "id", 3);

            Assert.True(updateStatus, "Update operation failed.");

            var (status, result) = _crudService.Read("test_table", new Dictionary<string, object> { { "id", 3 } });
            Assert.True(status, "Read operation failed.");
            Assert.Single(result);
            Assert.Equal("New Name", result[0]["name"]);
        }

        [Fact]
        public void Delete_ShouldRemoveDataFromDatabase()
        {
            _crudService.Create("test_table", new Dictionary<string, object> { { "id", 4 }, { "name", "To Delete" } });

            bool deleteStatus = _crudService.Delete("test_table", "id", 4);

            Assert.True(deleteStatus, "Delete operation failed.");

            var (status, result) = _crudService.Read("test_table", new Dictionary<string, object> { { "id", 4 } });
            Assert.True(status, "Read operation failed.");
            Assert.Empty(result);
        }
    }
}

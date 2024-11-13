using System;
using System.Collections.Generic;
using Xunit;
using warehub.db;
using MySql.Data.MySqlClient;

namespace warehub.Tests.db
{
    public class CRUDServiceIntegrationTests : IDisposable
    {
        private readonly CRUDService _crudService;

        public CRUDServiceIntegrationTests()
        {
            // Initialize CRUD service with the MySQL connection from DbConnection
            _crudService = new CRUDService(DbConnection.GetConnection());

            // Create the test table if it doesn't exist
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

                // Verify if the table was created by checking if it exists in the database
                string checkTableQuery = "SHOW TABLES LIKE 'test_table'";
                using (var command = new MySqlCommand(checkTableQuery, DbConnection.GetConnection()))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            throw new InvalidOperationException("Failed to create test_table. Tests cannot proceed.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating test table: " + ex.Message);
                throw new InvalidOperationException("Test setup failed: unable to create test_table.", ex);
            }
        }

        [Fact]
        public void Create_ShouldInsertDataIntoDatabase()
        {
            // Arrange
            var parameters = new Dictionary<string, object>
            {
                { "@id", 1 },
                { "@name", "Test Item" }
            };

            // Act
            bool createStatus = _crudService.Create("test_table", parameters);

            // Assert Create status
            Assert.True(createStatus, "Failed to create item in database.");

            // Retrieve and assert the inserted record
            var (status, result) = _crudService.Read("test_table", new Dictionary<string, object> { { "@id", 1 } });
            Assert.True(status, "Read operation failed.");
            Assert.Single(result);
            Assert.Equal("Test Item", result[0]["name"]);
        }

        [Fact]
        public void Read_ShouldRetrieveDataFromDatabase()
        {
            // Arrange: Insert data to read later
            _crudService.Create("test_table", new Dictionary<string, object> { { "@id", 2 }, { "@name", "Read Test" } });

            // Act
            var (status, result) = _crudService.Read("test_table", new Dictionary<string, object> { { "@id", 2 } });

            // Assert
            Assert.True(status, "Read operation failed.");
            Assert.Single(result);
            Assert.Equal("Read Test", result[0]["name"]);
        }

        [Fact]
        public void Update_ShouldModifyExistingData()
        {
            // Arrange
            _crudService.Create("test_table", new Dictionary<string, object> { { "@id", 3 }, { "@name", "Old Name" } });
            var updateParameters = new Dictionary<string, object> { { "@name", "New Name" } };

            // Act
            bool updateStatus = _crudService.Update("test_table", updateParameters, "id", 3);

            // Assert Update status
            Assert.True(updateStatus, "Update operation failed.");

            // Verify the update with a read
            var (status, result) = _crudService.Read("test_table", new Dictionary<string, object> { { "@id", 3 } });
            Assert.True(status, "Read operation failed.");
            Assert.Single(result);
            Assert.Equal("New Name", result[0]["name"]);
        }

        [Fact]
        public void Delete_ShouldRemoveDataFromDatabase()
        {
            // Arrange
            _crudService.Create("test_table", new Dictionary<string, object> { { "@id", 4 }, { "@name", "To Delete" } });

            // Act
            bool deleteStatus = _crudService.Delete("test_table", "id", 4);

            // Assert Delete status
            Assert.True(deleteStatus, "Delete operation failed.");

            // Confirm deletion with a read
            var (status, result) = _crudService.Read("test_table", new Dictionary<string, object> { { "@id", 4 } });
            Assert.True(status, "Read operation failed.");
            Assert.Empty(result);
        }

        public void Dispose()
        {
            // Clean up by dropping the test table
            string dropTableQuery = "DROP TABLE IF EXISTS test_table";
            using (var command = new MySqlCommand(dropTableQuery, DbConnection.GetConnection()))
            {
                command.ExecuteNonQuery();
            }

            // Disconnect from the database
            DbConnection.Disconnect();
        }
    }
}

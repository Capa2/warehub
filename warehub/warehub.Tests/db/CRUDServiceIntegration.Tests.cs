using System;
using System.Collections.Generic;
using Xunit;
using warehub.db;
using MySql.Data.MySqlClient;

namespace warehub.Tests.db
{
    public class CRUDServiceIntegrationTests : IDisposable
    {
        private readonly CRUDService<Dictionary<string, object>> _crudService;
        private readonly DbConnection _dbConnection;

        public CRUDServiceIntegrationTests()
        {
            // Setup connection and CRUD service
            _dbConnection = DbConnection.Instance;
            _dbConnection.Connect();
            _crudService = new CRUDService<Dictionary<string, object>>(_dbConnection.GetConnection());

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

                using (var command = new MySqlCommand(createTableQuery, _dbConnection.GetConnection()))
                {
                    command.ExecuteNonQuery();
                }

                // Verify if the table was created by checking if it exists in the database
                string checkTableQuery = "SHOW TABLES LIKE 'test_table'";
                using (var command = new MySqlCommand(checkTableQuery, _dbConnection.GetConnection()))
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
                // Log the error if needed
                Console.WriteLine("Error creating test table: " + ex.Message);
                throw new InvalidOperationException("Test setup failed: unable to create test_table.", ex);
            }
        }


        [Fact]
        public void Create_ShouldInsertDataIntoDatabase()
        {
            // Arrange
            string query = "INSERT INTO test_table (id, name) VALUES (@id, @name)";
            var parameters = new Dictionary<string, object>
            {
                { "@id", 1 },
                { "@name", "Test Item" }
            };

            // Act
            _crudService.Create(query, parameters);

            // Assert
            var result = _crudService.Read("SELECT * FROM test_table WHERE id = @id", new Dictionary<string, object> { { "@id", 1 } });
            Assert.Single(result);
            Assert.Equal("Test Item", result[0]["name"]);
        }

        [Fact]
        public void Read_ShouldRetrieveDataFromDatabase()
        {
            // Arrange: Insert data to read later
            _crudService.Create("INSERT INTO test_table (id, name) VALUES (@id, @name)",
                                new Dictionary<string, object> { { "@id", 2 }, { "@name", "Read Test" } });

            // Act
            var result = _crudService.Read("SELECT * FROM test_table WHERE id = @id", new Dictionary<string, object> { { "@id", 2 } });

            // Assert
            Assert.Single(result);
            Assert.Equal("Read Test", result[0]["name"]);
        }

        [Fact]
        public void Update_ShouldModifyExistingData()
        {
            // Arrange
            _crudService.Create("INSERT INTO test_table (id, name) VALUES (@id, @name)",
                                new Dictionary<string, object> { { "@id", 3 }, { "@name", "Old Name" } });
            string updateQuery = "UPDATE test_table SET name = @name WHERE id = @id";
            var updateParameters = new Dictionary<string, object> { { "@id", 3 }, { "@name", "New Name" } };

            // Act
            _crudService.Update(updateQuery, updateParameters);

            // Assert
            var result = _crudService.Read("SELECT * FROM test_table WHERE id = @id", new Dictionary<string, object> { { "@id", 3 } });
            Assert.Single(result);
            Assert.Equal("New Name", result[0]["name"]);
        }

        [Fact]
        public void Delete_ShouldRemoveDataFromDatabase()
        {
            // Arrange
            _crudService.Create("INSERT INTO test_table (id, name) VALUES (@id, @name)",
                                new Dictionary<string, object> { { "@id", 4 }, { "@name", "To Delete" } });
            string deleteQuery = "DELETE FROM test_table WHERE id = @id";
            var deleteParameters = new Dictionary<string, object> { { "@id", 4 } };

            // Act
            _crudService.Delete(deleteQuery, deleteParameters);

            // Assert
            var result = _crudService.Read("SELECT * FROM test_table WHERE id = @id", deleteParameters);
            Assert.Empty(result);
        }

        public void Dispose()
        {
            // Clean up by dropping the test table
            string dropTableQuery = "DROP TABLE IF EXISTS test_table";
            using (var command = new MySqlCommand(dropTableQuery, _dbConnection.GetConnection()))
            {
                command.ExecuteNonQuery();
            }

            _dbConnection.Disconnect();
        }
    }
}

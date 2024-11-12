using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using warehub.db;

class Program
{
    static void Main(string[] args)
    {
    ///   // Define your connection string
    ///   string connectionString = "Server=localhost;Database=user;User ID=root;Password=password;";
    ///
    ///   // Initialize the singleton DbConnection
    ///   var dbConnection = DbConnection.GetInstance(connectionString);
    ///   dbConnection.Connect();
    ///
    ///   // Verify that the connection is open
    ///   if (dbConnection.GetConnection().State == System.Data.ConnectionState.Open)
    ///   {
    ///       Console.WriteLine("Connection to the database was successful.");
    ///   }
    ///   else
    ///   {
    ///       Console.WriteLine("Failed to connect to the database.");
    ///       return;
    ///   }
    ///
    ///   // Test CRUD operations
    ///   var crudService = new CRUDService<Product>(dbConnection.GetConnection());
    ///
    ///   // Generate a unique ID for the new product
    ///   string newProductId = Guid.NewGuid().ToString();
    ///
    ///   // Create a sample product with UUID for ID
    ///   var createParams = new Dictionary<string, object>
    ///   {
    ///       { "@id", newProductId },
    ///       { "@name", "Sample Product" },
    ///       { "@price", 29.99 }
    ///   };
    ///   crudService.Create("INSERT INTO Products (ID, Name, Price) VALUES (@id, @name, @price)", createParams);
    ///
    ///   // Read all products
    ///   List<Dictionary<string, object>> products = crudService.Read("SELECT * FROM Products");
    ///   Console.WriteLine("Products in the database:");
    ///   foreach (var product in products)
    ///   {
    ///       foreach (var kvp in product)
    ///       {
    ///           Console.WriteLine($"{kvp.Key}: {kvp.Value}");
    ///       }
    ///       Console.WriteLine();
    ///   }
    ///
    ///   // Update a product (use newProductId for updating the correct product)
    ///   var updateParams = new Dictionary<string, object>
    ///   {
    ///       { "@name", "Updated Product" },
    ///       { "@price", 39.99 },
    ///       { "@id", newProductId }
    ///   };
    ///   crudService.Update("UPDATE Products SET Name = @name, Price = @price WHERE ID = @id", updateParams);
    ///
    ///   //// Delete a product (use newProductId for deletion)
    ///   //var deleteParams = new Dictionary<string, object>
    ///   //{
    ///   //    { "@id", newProductId }
    ///   //};
    ///   //crudService.Delete("DELETE FROM Products WHERE ID = @id", deleteParams);
    ///
    ///   // Close the connection
    ///   dbConnection.Disconnect();
    }
}

// Define a sample Product class (if needed for type safety in CRUDService)
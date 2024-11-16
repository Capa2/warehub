using System;
using warehub.model.interfaces;
using warehub.services;
using NLog;
using System.Reflection;

namespace warehub.model
{
    public static class ProductFactory
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static Product CreateProduct(string name, decimal price)
        {
            Logger.Trace(string.Join("Factory is creating product with name: ", name, ", and price:", price));
            return new Product(name, price);
        }

        public static Product CreateProduct(Guid id, string name, decimal price)
        {
            Logger.Trace(string.Join("Factory is creating product with id: ", id, ", name: ",name, ", and price:", price));
            return new Product(id, name, price);
        }

       // public static T ConvertToProduct<T>(Dictionary<string, object> dict) where T : new()
       // {
       //     T obj = new(); // Create an instance of the target type
       //     Type objType = typeof(T); // Get the type information
       // 
       //     foreach (var kvp in dict)
       //     {
       //         // Find the property on the object that matches the key
       //         PropertyInfo? property = objType.GetProperty(kvp.Key, BindingFlags.Public | BindingFlags.Instance);
       // 
       //         if (property != null && property.CanWrite)
       //         {
       //             try
       //             {
       //                 // Convert the value to the property's type and assign it
       //                 object? convertedValue = Convert.ChangeType(kvp.Value, property.PropertyType);
       //                 property.SetValue(obj, convertedValue);
       //             }
       //             catch (Exception ex)
       //             {
       //                 Console.WriteLine($"Error setting property '{kvp.Key}': {ex.Message}");
       //             }
       //         }
       //     }
       // 
       //     return obj;
       // }
    }
}

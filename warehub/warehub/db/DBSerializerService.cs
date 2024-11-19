using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using warehub.db.DTO;

namespace warehub.db
{
    internal static class DBSerializerService
    {
        //public static List<T> DeserialzeDBContentResponseStrong<T>(MySqlDataReader reader)
        //{
        //    var results = new List<T>();
        //
        //    while (reader.Read())
        //    {
        //
        //
        //
        //        T? deserializedObject = default;
        //
        //        if (typeof(T) == typeof(ProductDTO))
        //        {
        //            deserializedObject = (T)(object)new ProductDTO(
        //                id: reader.GetGuid(reader.GetOrdinal("Id")),
        //                name: reader.GetString(reader.GetOrdinal("Name")),
        //                price: reader.GetDecimal(reader.GetOrdinal("Price"))
        //            );
        //        }
        //        if (deserializedObject != null)
        //        {
        //            results.Add(deserializedObject);
        //        }
        //    }
        //    return results;
        //}


        public static List<T> DeserialzeDBContentResponse<T>(MySqlDataReader reader)
        {
            var results = new List<T>();
            while (reader.Read())
            {
                // Create an instance of T using reflection
                var instance = Activator.CreateInstance<T>();

                // Optionally populate instance properties (if needed)
                foreach (var prop in typeof(T).GetProperties())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal(prop.Name)))
                    {
                        var value = reader.GetValue(reader.GetOrdinal(prop.Name));
                        prop.SetValue(instance, value);
                    }
                }

                results.Add(instance);
            }
            return results;



            //var properties = typeof(T).GetProperties(); // Get all properties of T

            //while (reader.Read())
            //{
            //    T deserializedObject = new(); // Create a new instance of T
            //
            //    foreach (var property in properties)
            //    {
            //        // Check if the property exists in the DataReader columns
            //        if (reader.GetOrdinal(property.Name) >= 0)
            //        {
            //            var value = reader[property.Name];
            //            if (value != DBNull.Value)
            //            {
            //                property.SetValue(deserializedObject, Convert.ChangeType(value, property.PropertyType));
            //            }
            //        }
            //    }
            //
            //    results.Add(deserializedObject);
            //}
            //
            //return results;
        }
    }
}

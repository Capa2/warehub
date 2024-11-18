using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using warehub.model;

namespace warehub.controller
{
    internal class JsonCustomConverter
    {
        public class ProductConverter : JsonConverter<Product>
        {
            public override Product Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                // Parse the JSON object
                if (reader.TokenType != JsonTokenType.StartObject)
                {
                    throw new JsonException("Expected StartObject token.");
                }

                string name = null;
                decimal price = 0;
                int amount = 0;

                // Read each property
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject)
                    {
                        break; // End of the object
                    }

                    if (reader.TokenType == JsonTokenType.PropertyName)
                    {
                        string propertyName = reader.GetString();
                        reader.Read(); // Move to the value

                        switch (propertyName)
                        {
                            case "Name":
                                name = reader.GetString();
                                break;
                            case "Price":
                                price = reader.GetDecimal();
                                break;
                            case "Amount":
                                amount = reader.GetInt32();
                                break;
                        }
                    }
                }

                // Use the factory method to create the Product
                return ProductFactory.CreateProduct(name, price, amount);
            }

            public override void Write(Utf8JsonWriter writer, Product value, JsonSerializerOptions options)
            {
                // Serialize Product back to JSON
                writer.WriteStartObject();
                writer.WriteString("Name", value.Name);
                writer.WriteNumber("Price", value.Price);
                writer.WriteNumber("Amount", value.Amount);
                writer.WriteEndObject();
            }
        }
    }
}

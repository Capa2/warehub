using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using warehub.model;

namespace warehub.db.DTO
{
    public class ProductDTO
    {
        public ProductDTO(Guid id, string name, decimal price)
        {
            Id = id;
            Name = name;
            Price = price;
        }

        public ProductDTO()
        {
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public List<ProductDTO> DeserialzeDBResponse(MySqlDataReader reader)
        {
            var results = new List<ProductDTO>();
            while (reader.Read())
            {
                var product = new ProductDTO(
                    id: reader.GetGuid(reader.GetOrdinal("Id")),
                    name: reader.GetString(reader.GetOrdinal("Name")),
                    price: reader.GetDecimal(reader.GetOrdinal("Price"))
                );

                results.Add(product);
            }
            return results;
        }
    }
}

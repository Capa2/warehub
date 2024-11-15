using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using warehub.db;
using warehub.model;
using warehub.services;

namespace warehub
{
    public static class ProductPopulater
    {
        public static void Populate()
        {
            ProductRepository productRepository = new ProductRepository();
            ProductService productService = new ProductService(productRepository);
            

            Product product = ProductFactory.CreateProduct("Blue T-Shirt BIG", 10);

            productService.AddProduct(product);
            var products = productService.GetAllProducts();
            if (products != null)
            {
                var productToUpdate = products.FirstOrDefault();
                var result = productService.UpdateProduct(product);
            }
            productService.DeleteProduct(product.Id);

        }

        

    }
}

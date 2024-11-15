using NLog;
using warehub.db;
using warehub.model;
using warehub.services;

namespace warehub
{
    public static class ProductPopulater
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public static void Populate()
        {
            ProductRepository productRepository = new ProductRepository();
            ProductService productService = new ProductService(productRepository);

            Product product = ProductFactory.CreateProduct("Blue T-Shirt BIG", 10);
            Logger.Info("Adding product: " + product.ToString());
            productService.AddProduct(product);
            var products = productService.GetAllProducts();
            Logger.Info("Products: ");
            foreach (Product p in products)
            {
                Logger.Info("Product: " + p.ToString());
            }
            if (products != null)
            {
                var productToUpdate = products.FirstOrDefault();
                var result = productService.UpdateProduct(product);
            }
            productService.DeleteProduct(product.Id);

        }

        

    }
}

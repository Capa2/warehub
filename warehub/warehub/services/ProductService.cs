using System;
using System.Collections.Generic;
using warehub.model;
using warehub.repository;
using warehub.services.interfaces;
using NLog;

namespace warehub.services
{
    /// <summary>
    /// Service class responsible for managing Product-related business logic.
    /// </summary>
    public class ProductService : IProductSerivce
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IProductRepository _productRepository;

        /// <summary>
        /// Initializes a new instance of the ProductService class.
        /// </summary>
        /// <param name="productRepository">The product repository to be used for data access operations.</param>
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        /// Retrieves all products from the repository.
        /// </summary>
        /// <returns>A list of products if successful; otherwise, null.</returns>
        public async Task<List<Product>?> GetAllProducts()
        {
            try
            {
                Logger.Trace("ProductService: Attempting to retrieve all products from the repository.");
                var responseObject = await _productRepository.GetAll();

                if (responseObject.IsSuccess)
                {
                    if (responseObject.Data == null || responseObject.Data.Count == 0)
                    {
                        Logger.Warn("ProductService: No products found in the repository.");
                        return null;
                    }

                    Logger.Trace($"ProductService: Retrieved {responseObject.Data.Count} products successfully.");
                    return responseObject.Data;
                }

                Logger.Warn("ProductService: Failed to retrieve products: Operation was not successful.");
                return null;
            }
            catch (Exception ex)
            {
                Logger.Error($"ProductService: An error occurred while retrieving all products: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Retrieves a product by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product.</param>
        /// <returns>The product if found; otherwise, null.</returns>
        public async Task<Product?> GetProductByIdAsync(Guid id)
        {
            try
            {
                Logger.Trace($"ProductService: Attempting to retrieve product with ID: {id}");
                var response = await _productRepository.GetById(id);

                if (response.IsSuccess)
                {
                    if (response.Data == null)
                    {
                        Logger.Warn($"ProductService: Product with ID {id} was not found in the repository.");
                        return null;
                    }

                    Logger.Trace($"ProductService: Product with ID {id} retrieved successfully.");
                    return response.Data;
                }

                Logger.Warn($"ProductService: Failed to retrieve product with ID {id}: Operation was not successful.");
                return null;
            }
            catch (Exception ex)
            {
                Logger.Error($"ProductService: An error occurred while retrieving product by ID: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Adds a new product to the repository.
        /// </summary>
        /// <param name="product">The product to add.</param>
        /// <returns>True if the product was added successfully; otherwise, false.</returns>
        public async Task<bool> AddProduct(Product product)
        {
            try
            {
                Logger.Trace($"ProductService: Attempting to add product: {product.Name}");
                var response = await _productRepository.Add(product);

                if (response.IsSuccess)
                {
                    Logger.Trace($"ProductService: Product {product.Name} added successfully.");
                    return true;
                }

                Logger.Warn($"ProductService: Failed to add product {product.Name}: Operation was not successful.");
                return false;
            }
            catch (Exception ex)
            {
                Logger.Error($"ProductService: An error occurred while adding product: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Updates an existing product in the repository.
        /// </summary>
        /// <param name="product">The product to update.</param>
        /// <returns>True if the product was updated successfully; otherwise, false.</returns>
        public async Task<bool> UpdateProduct(Product product)
        {
            try
            {
                Logger.Trace($"ProductService: Attempting to update product with ID: {product.Id}");
                var getResponse = await _productRepository.GetById(product.Id);

                if (!getResponse.IsSuccess || getResponse.Data == null)
                {
                    Logger.Warn($"ProductService: Product with ID {product.Id} not found. Update operation aborted.");
                    return false;
                }

                var response = await _productRepository.Update(product);

                if (response.IsSuccess)
                {
                    Logger.Trace($"ProductService: Product with ID {product.Id} updated successfully.");
                    return true;
                }

                Logger.Warn($"ProductService: Failed to update product with ID {product.Id}: Operation was not successful.");
                return false;
            }
            catch (Exception ex)
            {
                Logger.Error($"ProductService: An error occurred while updating product: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Deletes a product from the repository by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product to delete.</param>
        /// <returns>True if the product was deleted successfully; otherwise, false.</returns>
        public async Task<bool> DeleteProduct(Guid id)
        {
            try
            {
                Logger.Trace($"ProductService: Attempting to delete product with ID: {id}");
                var getResponse = await _productRepository.GetById(id);

                if (!getResponse.IsSuccess || getResponse.Data == null)
                {
                    Logger.Warn($"ProductService: Product with ID {id} not found. Delete operation aborted.");
                    return false;
                }

                var response = await _productRepository.Delete(id);

                if (response.IsSuccess)
                {
                    Logger.Trace($"ProductService: Product with ID {id} deleted successfully.");
                    return true;
                }

                Logger.Warn($"ProductService: Failed to delete product with ID {id}: Operation was not successful.");
                return false;
            }
            catch (Exception ex)
            {
                Logger.Error($"ProductService: An error occurred while deleting product: {ex.Message}");
                return false;
            }
        }
    }
}

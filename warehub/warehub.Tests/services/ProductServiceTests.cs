using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using warehub.model;
using warehub.repository;
using warehub.services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace warehub.Tests.services
{
    public class ProductServiceTests
    {
        private readonly IProductRepository _productRepository;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _productRepository = Substitute.For<IProductRepository>();
            _productService = new ProductService(_productRepository);
        }

        [Fact]
        public void GetAllProducts_ReturnsListOfProducts_WhenResponseIsSuccessful()
        {
            // Arrange: Mock the response of GetAll to return a successful result with a list of products.
            var Data = new List<Product>
            {
                new Product(Guid.NewGuid(), "Product1", 10.99m, 5),
            };
            var mockResponse = new GenericResponseDTO<List<Product>>(Data, true);

            _productRepository.GetAll().Returns(mockResponse);

            // Act: Call the method
            var sut = _productService.GetAllProducts().Result;

            // Assert: Verify that the result is the list of products
            Assert.Equal(1, sut.Count);
            Assert.Equal(Data.First().Id, sut[0].Id);
        }

        [Fact]
        public void GetAllProducts_ReturnsNull_WhenResponseIsUnsuccessful()
        {
            // Arrange: Mock the response of GetAll to return an unsuccessful result.
            var Data = new List<Product>
            {
            };
            var mockResponse = new GenericResponseDTO<List<Product>>(Data, false);

            _productRepository.GetAll().Returns(mockResponse);

            // Act: Call the method
            var sut = _productService.GetAllProducts().Result;

            // Assert: Verify that the result is null
            Assert.Null(sut);
        }

        [Fact]
        public void GetProductById_ReturnsListOfProducts_WhenResponseIsSuccessful()
        {
            // Arrange: Mock the response of GetById to return a successful result with a products.
            var product = new Product(Guid.NewGuid(), "Product1", 10.99m, 5);
            var mockResponse = new GenericResponseDTO<Product>(product, true);

            _productRepository.GetById(product.Id).Returns(mockResponse);

            // Act: Call the method
            var sut = _productService.GetProductByIdAsync(product.Id).Result;

            // Assert: Verify that the result is the list of products
            Assert.Equal(product.Id, sut.Id);
        }

        [Fact]
        public void GetProductById_ReturnsNull_WhenResponseIsUnsuccessful()
        {
            // Arrange: Mock the response of GetById to return an unsuccessful result.
            var product = new Product(Guid.NewGuid(), "Product1", 10.99m, 5);
            var mockResponse = new GenericResponseDTO<Product>(product, false);
            _productRepository.GetById(product.Id).Returns(mockResponse);

            // Act: Call the method
            var sut = _productService.GetProductByIdAsync(product.Id).Result;

            // Assert: Verify that the result is null
            Assert.Null(sut);
        }


        [Fact]
        public void AddProduct_ReturnsTrue_WhenResponseIsSuccessful()
        {
            // Arrange: Mock the Add method to return a successful response.
            var product = new Product(Guid.NewGuid(), "Product1", 10.99m, 5);
            var mockResponse = new GenericResponseDTO<Product>(product, true);

            _productRepository.Add(product).Returns(mockResponse);

            // Act: Call the method
            var sut = _productService.AddProduct(product).Result;

            // Assert: Verify that the result is true
            Assert.True(sut);
        }

        [Fact]
        public void AddProduct_ReturnsFalse_WhenResponseIsUnsuccessful()
        {
            // Arrange: Mock the Add method to return an unsuccessful response.
            var product = new Product(Guid.NewGuid(), "Product1", 10.99m, 5);
            var mockResponse = new GenericResponseDTO<Product>(product, false);

            _productRepository.Add(product).Returns(mockResponse);

            // Act: Call the method
            var sut = _productService.AddProduct(product).Result;

            // Assert: Verify that the result is false
            Assert.False(sut);
        }

        [Fact]
        public void DeleteProduct_ReturnsTrue_WhenResponseIsSuccessful()
        {
            // Arrange: Mock the Add method to return a successful response.
            var product = new Product(Guid.NewGuid(), "Product1", 10.99m, 5);
            var mockResponseGetById = new GenericResponseDTO<Product>(product, true);
            var mockResponseDelete = new GenericResponseDTO<Guid>(product.Id, true);

            _productRepository.GetById(product.Id).Returns(mockResponseGetById);
            _productRepository.Delete(product.Id).Returns(mockResponseDelete);
            // Act: Call the method
            var sut = _productService.DeleteProduct(product.Id).Result;

            // Assert: Verify that the result is true
            Assert.True(sut);
        }

        [Fact]
        public void DeleteProduct_ReturnsFalse_WhenResponseIsUnsuccessful()
        {
            // Arrange: Mock the Add method to return an unsuccessful response.
            var product = new Product(Guid.NewGuid(), "Product1", 10.99m, 5);
            var mockResponseGetById = new GenericResponseDTO<Product>(product, true);
            var mockResponseDelete = new GenericResponseDTO<Guid>(product.Id, false);

            _productRepository.GetById(product.Id).Returns(mockResponseGetById);
            _productRepository.Delete(product.Id).Returns(mockResponseDelete);

            // Act: Call the method
            var sut = _productService.DeleteProduct(product.Id).Result;

            // Assert: Verify that the result is false
            Assert.False(sut);
        }

        [Fact]
        public void DeleteProduct_ReturnsFalse_WhenProductDoesNotExist()
        {
            // Arrange: Mock the Add method to return an unsuccessful response.
            var product = new Product(Guid.NewGuid(), "Product1", 10.99m, 5);
            var mockResponseGetById = new GenericResponseDTO<Product>(product, false);
            var mockResponseDelete = new GenericResponseDTO<Guid>(product.Id, true);

            _productRepository.GetById(product.Id).Returns(mockResponseGetById);
            _productRepository.Delete(product.Id).Returns(mockResponseDelete);

            // Act: Call the method
            var sut = _productService.DeleteProduct(product.Id).Result;

            // Assert: Verify that the result is false
            Assert.False(sut);
        }

        [Fact]
        public void UpdateProduct_ReturnsTrue_WhenResponseIsSuccessful()
        {
            // Arrange: Mock the Add method to return a successful response.
            var product = new Product(Guid.NewGuid(), "Product1", 10.99m, 5);
            var mockResponseGetById = new GenericResponseDTO<Product>(product, true);
            var mockResponseUpdate = new GenericResponseDTO<Product>(product, true);

            _productRepository.GetById(product.Id).Returns(mockResponseGetById);
            _productRepository.Update(product).Returns(mockResponseUpdate);
            // Act: Call the method
            var sut = _productService.UpdateProduct(product).Result;

            // Assert: Verify that the result is true
            Assert.True(sut);
        }

        [Fact]
        public void UpdateProduct_ReturnsFalse_WhenResponseIsUnsuccessful()
        {
            // Arrange: Mock the Add method to return an unsuccessful response.
            var product = new Product(Guid.NewGuid(), "Product1", 10.99m, 5);
            var mockResponseGetById = new GenericResponseDTO<Product>(product, true);
            var mockResponseUpdate = new GenericResponseDTO<Product>(product, false);

            _productRepository.GetById(product.Id).Returns(mockResponseGetById);
            _productRepository.Update(product).Returns(mockResponseUpdate);
            // Act: Call the method
            var sut = _productService.UpdateProduct(product).Result;

            // Assert: Verify that the result is false
            Assert.False(sut);
        }

        [Fact]
        public void UpdateProduct_ReturnsFalse_WhenProductDoesNotExist()
        {
            // Arrange: Mock the Add method to return an unsuccessful response.
            var product = new Product(Guid.NewGuid(), "Product1", 10.99m, 5);
            var mockResponseGetById = new GenericResponseDTO<Product>(product, false);
            var mockResponseUpdate = new GenericResponseDTO<Product>(product, true);

            _productRepository.GetById(product.Id).Returns(mockResponseGetById);
            _productRepository.Update(product).Returns(mockResponseUpdate);
            // Act: Call the method
            var sut = _productService.UpdateProduct(product).Result;

            // Assert: Verify that the result is false
            Assert.False(sut);
        }
    }
}

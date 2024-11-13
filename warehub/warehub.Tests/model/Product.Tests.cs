using Xunit;
using warehub.model;
using System;

namespace warehub.tests
{
    public class ProductTests
    {
        [Fact]
        public void Product_CreatesWithValidNameAndPrice()
        {
            // Arrange
            string name = "Test Product";
            int price = 100;

            // Act
            Product product = new Product(name, price);

            // Assert
            Assert.Equal(name, product.Name);
            Assert.Equal(price, product.Price);
            Assert.NotEqual(Guid.Empty, product.Id); // Check that the ID is generated
        }

        [Fact]
        public void Product_CreatesWithSpecifiedGuid()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            string name = "Test Product";
            int price = 200;

            // Act
            Product product = new Product(id, name, price);

            // Assert
            Assert.Equal(id, product.Id);
            Assert.Equal(name, product.Name);
            Assert.Equal(price, product.Price);
        }
    }
}

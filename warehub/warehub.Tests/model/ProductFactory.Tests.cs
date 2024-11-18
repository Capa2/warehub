using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using warehub.model;

namespace warehub.Tests.model
{

    public class ProductFactoryTests
    {
        [Fact]
        public void CreateProduct_ShouldCreateProductWithCorrectProperties()
        {
            // Arrange: Create the necessary data and mocks
            var productName = "Smartphone X";
            var productPrice = 699.99m;
            var productAmount = 100;

            // SUT: Call the method we're testing (System Under Test)
            var sut = ProductFactory.CreateProduct(productName, productPrice, productAmount);

            // Assert: Verify that the SUT behaves as expected
            Assert.NotNull(sut);
            Assert.Equal(productName, sut.Name);
            Assert.Equal(productPrice, sut.Price);
            Assert.Equal(productAmount, sut.Amount);
        }
    }
}

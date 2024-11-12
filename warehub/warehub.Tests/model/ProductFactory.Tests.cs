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
            public void CreateProduct_WithNameAndPrice_ReturnsProductWithGivenValues()
            {
                // Arrange
                string expectedName = "Sample Product";
                int expectedPrice = 100;

                // Act
                Product result = ProductFactory.CreateProduct(expectedName, expectedPrice);

                // Assert
                Assert.NotNull(result); // Ensure a product was created
                Assert.Equal(expectedName, result.Name);
                Assert.Equal(expectedPrice, result.Price);
                Assert.NotEqual(Guid.Empty, result.Id); // Ensure a unique ID is generated
            }
        }
}

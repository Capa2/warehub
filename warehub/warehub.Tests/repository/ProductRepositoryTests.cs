using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using warehub.db;
using warehub.repository;
using warehub.services;

namespace warehub.Tests.repository
{
    public class ProductRepositoryTests
    {
        private readonly ICRUDService _CRUDService;
        private readonly ProductRepository _productRepository;

        public ProductRepositoryTests()
        {
            _CRUDService = Substitute.For<ICRUDService>();
            //_productRepository = new ProductRepository();
        }



    }
}

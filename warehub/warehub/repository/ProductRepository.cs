using MySql.Data.MySqlClient;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using warehub.db;
using warehub.db.interfaces;
using warehub.model;
using warehub.model.interfaces;
using warehub.repository.interfaces;
using warehub.repository.utils;
using warehub.services.interfaces;

namespace warehub.repository
{
    public class ProductRepository : IProductRepository
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ICRUDService? _cRUDService;

        public ProductRepository()
        {
            MySqlConnection? connection = DbConnection.GetConnection();
            if (connection == null)
            {
                Logger.Error("Failed to initialize ProductRepository. Connection is null.");
                return;
            }
            _cRUDService = new CRUDService(connection);
        }

        public GenericResponseDTO<IProduct> Add(IProduct product)
        {
            var parameters = new Dictionary<string, object>
            {
                { "name", product.Name },
                { "price", product.Price },
                { "id", product.Id },
                { "amount", product.Amount }
            };
            bool status = _cRUDService.Create("products", parameters);
            var returnObject = new GenericResponseDTO<IProduct>(product)
            {
                IsSuccess = status
            };
            return returnObject;
        }

        public GenericResponseDTO<Guid> Delete(Guid id)
        {
            bool status = _cRUDService.Delete("products", "id", id);
            var returnObject = new GenericResponseDTO<Guid>(id)
            {
                IsSuccess = status
            };
            return returnObject;
        }

        public GenericResponseDTO<List<IProduct>> GetAll()
        {
            var (status, products) = _cRUDService.Read("products", new Dictionary<string, object>());
            List<IProduct> listOfProducts = ObjectMapper.MapDictToProducts(products);
            var returnObject = new GenericResponseDTO<List<IProduct>>(listOfProducts)
            {
                IsSuccess = status
            };
            return returnObject;
        }

        public GenericResponseDTO<IProduct> GetById(Guid id)
        {
            var (status, products) = _cRUDService.Read("products", new Dictionary<string, object> { { "id", id } });
            List<IProduct> listOfProducts = ObjectMapper.MapDictToProducts(products);

            var product = listOfProducts.FirstOrDefault(p => p.Id == id);
            var returnObject = new GenericResponseDTO<IProduct>(product)
            {
                IsSuccess = status
            };
            return returnObject;
        }

        public GenericResponseDTO<IProduct> Update(IProduct product)
        {
            var updateParams = new Dictionary<string, object>
            {
                { "name", product.Name },
                { "price", product.Price },
                { "amount", product.Amount }
            };
            bool status = _cRUDService.Update("products", updateParams, "id", product.Id);

            var returnObject = new GenericResponseDTO<IProduct>(product)
            {
                IsSuccess = status
            };
            return returnObject;
        }
    }
}

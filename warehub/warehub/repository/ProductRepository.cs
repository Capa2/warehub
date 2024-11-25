using MySql.Data.MySqlClient;
using NLog;
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
        /// <summary>
        /// Provides data access functionality for managing products 
        /// in the database as an intermediary between the service 
        /// layer and the database layer.
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ICRUDService? _cRUDService;

        public ProductRepository()
        {
            MySqlConnection connection = DbConnection.GetConnection();
            _cRUDService = new CRUDService(connection);
        }

        public async Task<GenericResponseDTO<IProduct>> Add(IProduct product)
        {
            var parameters = new Dictionary<string, object>
            {
                { "name", product.Name },
                { "price", product.Price },
                { "id", product.Id },
                { "amount", product.Amount }
            };
            bool status = await _cRUDService.Create("products", parameters);
            var returnObject = new GenericResponseDTO<IProduct>(product)
            {
                IsSuccess = status
            };
            return returnObject;
        }

        public async Task<GenericResponseDTO<Guid>> Delete(Guid id)
        {
            bool status = await _cRUDService.Delete("products", "id", id);
            var returnObject = new GenericResponseDTO<Guid>(id)
            {
                IsSuccess = status
            };
            return returnObject;
        }

        public async Task<GenericResponseDTO<List<IProduct>>> GetAll()
        {
            var (status, products) = await _cRUDService.Read("products", new Dictionary<string, object>());
            List<IProduct> listOfProducts = ProductMapper.MapDictToProducts(products);

            var returnObject = new GenericResponseDTO<List<IProduct>>(listOfProducts)
            {
                IsSuccess = status
            };
            return returnObject;
        }

        public async Task<GenericResponseDTO<IProduct>> GetById(Guid id)
        {
            var (status, products) = await _cRUDService.Read("products", new Dictionary<string, object> { { "id", id } });
            List<IProduct> listOfProducts = ProductMapper.MapDictToProducts(products);

            var product = listOfProducts.FirstOrDefault(p => p.Id == id);
            var returnObject = new GenericResponseDTO<IProduct>(product)
            {
                IsSuccess = status
            };
            return returnObject;
        }

        public async Task<GenericResponseDTO<IProduct>> Update(IProduct product)
        {
            var updateParams = new Dictionary<string, object>
            {
                { "name", product.Name },
                { "price", product.Price },
                { "amount", product.Amount }
            };
            bool status = await _cRUDService.Update("products", updateParams, "id", product.Id);

            var returnObject = new GenericResponseDTO<IProduct>(product)
            {
                IsSuccess = status
            };
            return returnObject;
        }
    }
}

using warehub.model;

namespace warehub.repository
{
    public interface IProductRepository
    {
        Task<GenericResponseDTO<Product>> Add(Product product);
        Task<GenericResponseDTO<Guid>> Delete(Guid id);
        Task<GenericResponseDTO<List<Product>>> GetAll();
        Task<GenericResponseDTO<Product>> GetById(Guid id);
        Task<GenericResponseDTO<Product>> Update(Product product);
    }
}
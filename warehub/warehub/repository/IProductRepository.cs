using warehub.model;

namespace warehub.repository
{
    public interface IProductRepository
    {
        GenericResponseDTO<Product> Add(Product product);
        GenericResponseDTO<Guid> Delete(Guid id);
        GenericResponseDTO<List<Product>> GetAll();
        GenericResponseDTO<Product> GetById(Guid id);
        GenericResponseDTO<Product> Update(Product product);
    }
}
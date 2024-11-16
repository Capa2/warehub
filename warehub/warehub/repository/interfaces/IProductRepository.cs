using warehub.model;
using warehub.repository.returnObjects;

namespace warehub.repository.interfaces
{
    public interface IProductRepository
    {
        GenericResponseDTO<Product> Add(Product product);
        List<Product> ConvertToProducts(List<Dictionary<string, object>> products);
        GenericResponseDTO<Guid> Delete(Guid id);
        GenericResponseDTO<List<Product>> GetAll();
        GenericResponseDTO<Product> GetById(Guid id);
        GenericResponseDTO<Product> Update(Product product);
    }
}
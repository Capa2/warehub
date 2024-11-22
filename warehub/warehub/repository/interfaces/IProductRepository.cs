using warehub.model;
using warehub.model.interfaces;

namespace warehub.repository.interfaces
{
    public interface IProductRepository
    {
        GenericResponseDTO<IProduct> Add(IProduct product);
        GenericResponseDTO<Guid> Delete(Guid id);
        GenericResponseDTO<List<IProduct>> GetAll();
        GenericResponseDTO<IProduct> GetById(Guid id);
        GenericResponseDTO<IProduct> Update(IProduct product);
    }
}
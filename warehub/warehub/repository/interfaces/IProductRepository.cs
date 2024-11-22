using warehub.model;
using warehub.model.interfaces;

namespace warehub.repository.interfaces
{
    public interface IProductRepository
    {
        Task<GenericResponseDTO<IProduct>> Add(IProduct product);
        Task<GenericResponseDTO<Guid>> Delete(Guid id);
        Task<GenericResponseDTO<List<IProduct>>> GetAll();
        Task<GenericResponseDTO<IProduct>> GetById(Guid id);
        Task<GenericResponseDTO<IProduct>> Update(IProduct product);
    }
}
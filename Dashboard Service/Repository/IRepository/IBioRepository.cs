using Dashboard_Service.Models.DTO;

namespace Dashboard_Service.Repository.IRepository
{
    public interface IBioRepository
    {
        Task<int> CreateBioAsync(CreateBioDTO createBioDto);        
        Task<int> UpdateBioAsync(CreateBioDTO updateBioDto);
        Task<CreateBioDTO> GetBioByUserIdAsync();
    }
}

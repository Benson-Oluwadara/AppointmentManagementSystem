using Dashboard_Service.Models.DTO;

namespace Dashboard_Service.Services.IService
{
    public interface IBioService
    {
        Task<int> CreateBioAsync(CreateBioDTO createBioDto);
        Task<int> UpdateBioAsync(CreateBioDTO updateBioDto);
        Task<CreateBioDTO> GetBioByUserIdAsync();



    }
}

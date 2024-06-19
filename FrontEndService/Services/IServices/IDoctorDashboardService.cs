using FrontEndService.Models.WebDTO.BioDTO;

namespace FrontEndService.Services.IServices
{
    public interface IDoctorDashboardService
    {
        
        Task<T> UpdateBioAsync<T>(BioDTO biodto);
        Task<T> GetBioByUserIdAsync<T>();
        Task<T> CreateBioAsync<T>(BioDTO biodto);
    }
}

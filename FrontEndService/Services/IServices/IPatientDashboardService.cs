using FrontEndService.Models.WebDTO.BioDTO;

namespace FrontEndService.Services.IServices
{
    public interface IPatientDashboardService
    {
        Task<T> GetBioByUserIdAsync<T>();
        Task<T> UpdateBioAsync<T>(PatientBioDTO biodto);
    }
}

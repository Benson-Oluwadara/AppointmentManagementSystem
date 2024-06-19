using PatientDashboardService.Models.DTO;

namespace PatientDashboardService.Services.IService
{
    public interface IPatientService
    {
        Task<int> CreatePatientAsync(CreatePatientDTO createPatientDto);
        Task<int> UpdatePatientAsync(UpdatePatientDTO updatePatientDto);
        Task<CreatePatientDTO> GetPatientByUserIdAsync();
    }
}

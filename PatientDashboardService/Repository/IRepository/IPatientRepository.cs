using PatientDashboardService.Models.DTO;

namespace PatientDashboardService.Repository.IRepository
{
    public interface IPatientRepository
    {
        Task<int> CreatePatientAsync(CreatePatientDTO createPatientDto);
        Task<int> UpdatePatientAsync(UpdatePatientDTO updatePatientDto);
        Task<CreatePatientDTO> GetPatientByUserIdAsync();
    }
}

using PatientService.Models.DTO;
using PatientService.Models;

namespace PatientService.Services.IService
{
    public interface IPatientService
    {
        Task<Guid> CreatePatientAsync(CreatePatientDTO createPatientDto);
        Task<IEnumerable<Patient>> GetAllPatientsAsync();
        Task<Patient> GetPatientByIdAsync(Guid id);
        Task<int> UpdatePatientAsync(Guid id, CreatePatientDTO updatePatientDto);
        Task<int> DeletePatientAsync(Guid id);
    }
}

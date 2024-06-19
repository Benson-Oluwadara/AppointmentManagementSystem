

using PatientService.Models;

namespace PatientService.Repository.IRepository
{
    public interface IPatientRepository
    {
        Task<Guid> CreatePatientAsync(Patient patient);
        Task<IEnumerable<Patient>> GetAllPatientsAsync();
        Task<Patient> GetPatientByIdAsync(Guid id);
        Task<int> UpdatePatientAsync(Patient patient);

        Task<int> DeletePatientAsync(Guid id);
        
    }
}

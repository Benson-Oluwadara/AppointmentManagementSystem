using PatientDashboardService.Models.DTO;
using PatientDashboardService.Repository.IRepository;
using PatientDashboardService.Services.IService;

namespace PatientDashboardService.Services.Service
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<int> CreatePatientAsync(CreatePatientDTO createPatientDto)
        {
            return await _patientRepository.CreatePatientAsync(createPatientDto);
        }

        public async Task<int> UpdatePatientAsync(UpdatePatientDTO updatePatientDto)
        {
            return await _patientRepository.UpdatePatientAsync(updatePatientDto);
        }

        public async Task<CreatePatientDTO> GetPatientByUserIdAsync()
        {
            return await _patientRepository.GetPatientByUserIdAsync();
        }
    }
}

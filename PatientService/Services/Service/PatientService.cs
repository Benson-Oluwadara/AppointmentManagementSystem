using AutoMapper;
using PatientService.Models.DTO;
using PatientService.Models;
using PatientService.Repository.IRepository;
using PatientService.Services.IService;

namespace PatientService.Services.Service
{
    public class PatientService:IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;

        public PatientService(IPatientRepository patientRepository, IMapper mapper)
        {
            _patientRepository = patientRepository;
            _mapper = mapper;
        }

        public async Task<Guid> CreatePatientAsync(CreatePatientDTO createPatientDto)
        {
            var patient = _mapper.Map<Patient>(createPatientDto);
            await _patientRepository.CreatePatientAsync(patient);

            return patient.PatientID;
        }

        public async Task<IEnumerable<Patient>> GetAllPatientsAsync()
        {
            return await _patientRepository.GetAllPatientsAsync();
        }

        public async Task<Patient> GetPatientByIdAsync(Guid id)
        {
            return await _patientRepository.GetPatientByIdAsync(id);
        }

        public async Task<int> UpdatePatientAsync(Guid id, CreatePatientDTO updatePatientDto)
        {
            var patient = _mapper.Map<Patient>(updatePatientDto);
            patient.PatientID = id;
            return await _patientRepository.UpdatePatientAsync(patient);
        }

        public async Task<int> DeletePatientAsync(Guid id)
        {
          
            return await _patientRepository.DeletePatientAsync(id);
        }
    }
}

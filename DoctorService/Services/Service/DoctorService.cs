using AutoMapper;
using DoctorService.Models;
using DoctorService.Models.DTO;
using DoctorService.Repository.IRepository;
using DoctorService.Services.IService;

namespace DoctorService.Services.Service
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;

        public DoctorService(IDoctorRepository doctorRepository, IMapper mapper)
        {
            _doctorRepository = doctorRepository;
            _mapper = mapper;
        }

        public async Task<Guid> CreateDoctorAsync(DoctorDTO doctorDTO)
        {
            var doctor = _mapper.Map<Doctor>(doctorDTO);
            await _doctorRepository.CreateDoctorAsync(doctor);
            return doctor.DoctorID;
        }

        public async Task<DoctorDTO> GetDoctorByIdAsync(Guid doctorId)
        {
            var doctor = await _doctorRepository.GetDoctorByIdAsync(doctorId);
            return _mapper.Map<DoctorDTO>(doctor);
        }

        public async Task<IEnumerable<DoctorDTO>> GetAllDoctorsAsync()
        {
            var doctors = await _doctorRepository.GetAllDoctorsAsync();
            return _mapper.Map<IEnumerable<DoctorDTO>>(doctors);
        }

        public async Task<int> UpdateDoctorAsync(DoctorDTO doctorDTO)
        {
            var doctor = _mapper.Map<Doctor>(doctorDTO);
            return await _doctorRepository.UpdateDoctorAsync(doctor);
        }

        public async Task<int> DeleteDoctorAsync(Guid doctorId)
        {
            return await _doctorRepository.DeleteDoctorAsync(doctorId);
        }
    }
}

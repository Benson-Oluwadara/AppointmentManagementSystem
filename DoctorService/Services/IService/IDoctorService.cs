using DoctorService.Models;
using DoctorService.Models.DTO;

namespace DoctorService.Services.IService
{
    public interface IDoctorService
    {
        Task<Guid> CreateDoctorAsync(DoctorDTO doctorDTO);
        Task<DoctorDTO> GetDoctorByIdAsync(Guid doctorId);
        Task<IEnumerable<DoctorDTO>> GetAllDoctorsAsync();
        Task<int> UpdateDoctorAsync(DoctorDTO doctorDTO);
        Task<int> DeleteDoctorAsync(Guid doctorId);
    }
}

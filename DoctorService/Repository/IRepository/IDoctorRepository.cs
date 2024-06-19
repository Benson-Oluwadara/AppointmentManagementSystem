

using DoctorService.Models;

namespace DoctorService.Repository.IRepository
{
    public interface IDoctorRepository
    {
        Task<Guid> CreateDoctorAsync(Doctor doctor);
        Task<Doctor> GetDoctorByIdAsync(Guid doctorId);
        Task<IEnumerable<Doctor>> GetAllDoctorsAsync();
        Task<int> UpdateDoctorAsync(Doctor doctor);
        Task<int> DeleteDoctorAsync(Guid doctorId);
    }
}

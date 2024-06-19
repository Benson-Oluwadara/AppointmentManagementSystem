using Dapper;
using DoctorService.Database.IDapperRepositorys;
using DoctorService.Models;
using DoctorService.Repository.IRepository;

namespace DoctorService.Repository.Repository
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly IDapperRepository _dapperRepository;

        public DoctorRepository(IDapperRepository dapperRepository)
        {
            _dapperRepository = dapperRepository;
        }

        public async Task<Guid> CreateDoctorAsync(Doctor doctor)
        {
            
            var sql = @"
                INSERT INTO Doctors (DoctorID, Name, Email, PhoneNumber, Specialization, CreatedDate, LastUpdatedDate)
                VALUES (@DoctorID, @Name, @Email, @PhoneNumber, @Specialization, @CreatedDate, @LastUpdatedDate)";

            var parameters = new
            {
                DoctorID = Guid.NewGuid(),
                doctor.Name,
                doctor.Email,
                doctor.PhoneNumber,
                doctor.Specialization,
                CreatedDate = DateTime.UtcNow,
                LastUpdatedDate = DateTime.UtcNow
            };

          await _dapperRepository.ExecuteAsync(sql, parameters);
            return parameters.DoctorID;
        }

        public async Task<Doctor> GetDoctorByIdAsync(Guid doctorId)
        {
            var sql = "SELECT * FROM Doctors WHERE DoctorID = @DoctorID";
            return await _dapperRepository.QuerySingleOrDefaultAsync<Doctor>(sql, new { DoctorID = doctorId });
        }

        public async Task<IEnumerable<Doctor>> GetAllDoctorsAsync()
        {
            var sql = "SELECT * FROM Doctors";
            return await _dapperRepository.GetAllAsync<Doctor>(sql);
        }

        public async Task<int> UpdateDoctorAsync(Doctor doctor)
        {
            var sql = @"
                UPDATE Doctors
                SET Name = @Name, Email = @Email, PhoneNumber = @PhoneNumber, Specialization = @Specialization, LastUpdatedDate = @LastUpdatedDate
                WHERE DoctorID = @DoctorID";

            doctor.LastUpdatedDate = DateTime.UtcNow;

            return await _dapperRepository.ExecuteAsync(sql, doctor);
        }

        public async Task<int> DeleteDoctorAsync(Guid doctorId)
        {
            var sql = "DELETE FROM Doctors WHERE DoctorID = @DoctorID";
            return await _dapperRepository.ExecuteAsync(sql, new { DoctorID = doctorId });
        }
    }
}

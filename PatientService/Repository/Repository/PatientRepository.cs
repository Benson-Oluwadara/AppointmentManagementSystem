using AutoMapper;
using PatientService.Database.IDapperRepositorys;
using PatientService.Models;
using PatientService.Repository.IRepository;

namespace PatientService.Repository.Repository
{
    public class PatientRepository : IPatientRepository
    {
        private readonly IDapperRepository _dapperRepository;
        //private readonly IMapper _mapper;
        public PatientRepository(IDapperRepository dapperRepository/*, IMapper mapper*/)
        {
            _dapperRepository = dapperRepository;
            //_mapper = mapper;
        }

        public async Task<Guid> CreatePatientAsync(Patient patient)
        {
            var sql = @"
                INSERT INTO Patients (PatientID, Name, Email, PhoneNumber, DateOfBirth, CreatedDate, LastUpdatedDate)
                VALUES (@PatientID, @Name, @Email, @PhoneNumber, @DateOfBirth, @CreatedDate, @LastUpdatedDate)";

            var parameters = new
            {
                PatientID = Guid.NewGuid(),
                patient.Name,
                patient.Email,
                patient.PhoneNumber,
                patient.DateOfBirth,
                CreatedDate = DateTime.UtcNow,
                LastUpdatedDate = DateTime.UtcNow
            };

            await _dapperRepository.ExecuteAsync(sql, parameters);
            return parameters.PatientID;
        }

        public async Task<IEnumerable<Patient>> GetAllPatientsAsync()
        {
            var sql = "SELECT * FROM [dbo].[Patients]";
            return await _dapperRepository.GetAllAsync<Patient>(sql);
        }

        public async Task<Patient> GetPatientByIdAsync(Guid id)
        {
            var sql = "SELECT * FROM [dbo].[Patients] WHERE PatientID = @Id";
            return await _dapperRepository.GetAsync<Patient>(sql, new { Id = id });
        }

        public async Task<int> UpdatePatientAsync(Patient patient)
        {
            var sql = @"
                UPDATE Patients
                SET Name = @Name, Email = @Email, PhoneNumber = @PhoneNumber, DateOfBirth = @DateOfBirth,
                    LastUpdatedDate = @LastUpdatedDate
                WHERE PatientID = @PatientID";

            var parameters = new
            {
                patient.Name,
                patient.Email,
                patient.PhoneNumber,
                patient.DateOfBirth,
                patient.PatientID,
                LastUpdatedDate = DateTime.UtcNow
            };

            return await _dapperRepository.ExecuteAsync(sql, parameters);
        }

        public async Task<int> DeletePatientAsync(Guid id)
        {
            var sql = "DELETE FROM [dbo].[Patients] WHERE PatientID = @Id";
            return await _dapperRepository.ExecuteAsync(sql, new { Id = id });
        }

    }


}

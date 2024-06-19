using PatientDashboardService.Repository.IRepository;
using PatientDashboardService.Database.IDapperRepositorys;
using PatientDashboardService.Services.Service;
using PatientDashboardService.Models.DTO;

namespace PatientDashboardService.Repository.Repository
{
    public class PatientRepository: IPatientRepository
    {
        private readonly IDapperRepository _dapperRepository;
        private readonly AuthService _authService;

        public PatientRepository(IDapperRepository dapperRepository, AuthService authService)
        {
            _dapperRepository = dapperRepository;
            _authService = authService;
        }

        public async Task<int> CreatePatientAsync(CreatePatientDTO createPatientDto)
        {
            try
            {
                string userId = _authService.GetCurrentUserId();

                string sql = @"INSERT INTO PatientsBioTable (UserId, FullName, ContactNumber, EmailAddress, Address, DateOfBirth, MedicalHistory)
                               VALUES (@UserId, @FullName, @ContactNumber, @EmailAddress, @Address, @DateOfBirth, @MedicalHistory)";

                var parameters = new
                {
                    userId,
                    createPatientDto.FullName,
                    createPatientDto.ContactNumber,
                    createPatientDto.EmailAddress,
                    createPatientDto.Address,
                    createPatientDto.DateOfBirth,
                    createPatientDto.MedicalHistory
                };

                return await _dapperRepository.ExecuteAsync(sql, parameters);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> UpdatePatientAsync(UpdatePatientDTO updatePatientDto)
        {
            try
            {
                string userId = _authService.GetCurrentUserId();

                string sql = @"UPDATE PatientsBioTable 
                               SET FullName = @FullName,
                                   ContactNumber = @ContactNumber,
                                   EmailAddress = @EmailAddress,
                                   Address = @Address,
                                   DateOfBirth = @DateOfBirth,
                                   MedicalHistory = @MedicalHistory
                               WHERE UserId = @UserId";

                var parameters = new
                {
                    userId,
                    updatePatientDto.FullName,
                    updatePatientDto.ContactNumber,
                    updatePatientDto.EmailAddress,
                    updatePatientDto.Address,
                    updatePatientDto.DateOfBirth,
                    updatePatientDto.MedicalHistory
                };

                return await _dapperRepository.ExecuteAsync(sql, parameters);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CreatePatientDTO> GetPatientByUserIdAsync()
        {
            try
            {
                string userId = _authService.GetCurrentUserId();

                string sql = @"SELECT FullName, ContactNumber, EmailAddress, Address, DateOfBirth, MedicalHistory 
                               FROM PatientsBioTable
                               WHERE UserId = @UserId";

                var parameters = new { UserId = userId };

                return await _dapperRepository.GetAsync<CreatePatientDTO>(sql, parameters);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

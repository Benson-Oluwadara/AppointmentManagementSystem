using Dashboard_Service.Models.DTO;
using Dashboard_Service.Repository.IRepository;
using Dashboard_Service.Database.IDapperRepositorys;
using Dashboard_Service.Services.Service;
using System.Reflection.Metadata.Ecma335;

namespace Dashboard_Service.Repository.Repository
{
    public class BioRepository : IBioRepository
    {
        private readonly IDapperRepository _dapperRepository;
        private readonly AuthService _authService;

        public BioRepository(IDapperRepository dapperRepository, AuthService authService)
        {
            _dapperRepository = dapperRepository;
            _authService = authService;
        }

        public async Task<int> CreateBioAsync(CreateBioDTO createBioDto)
        {

            try
            {
                // Get the UserId from the current user's context
                string userId = _authService.GetCurrentUserId();

                string sql = @"INSERT INTO BioTable (UserId, FullName, Specialization, ContactNumber, EmailAddress, OfficeAddress)
                               VALUES (@UserId, @FullName, @Specialization, @ContactNumber, @EmailAddress, @OfficeAddress)";

                var parameters = new
                {
                    userId,
                    createBioDto.FullName,
                    createBioDto.Specialization,
                    createBioDto.ContactNumber,
                    createBioDto.EmailAddress,
                    createBioDto.OfficeAddress
                };

                return await _dapperRepository.ExecuteAsync(sql, parameters);
                // Rest of the implementation remains the same
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                throw;
            }

        }
        public async Task<int> UpdateBioAsync(CreateBioDTO updateBioDto)
        {
            try
            {
                string UserId = _authService.GetCurrentUserId();

                string sql = @"UPDATE BioTable 
                               SET FullName = @FullName,
                                   Specialization = @Specialization,
                                   ContactNumber = @ContactNumber,
                                   EmailAddress = @EmailAddress,
                                   OfficeAddress = @OfficeAddress
                               WHERE UserId = @UserId";

                var parameters = new
                {
                    UserId,
                    updateBioDto.FullName,
                    updateBioDto.Specialization,
                    updateBioDto.ContactNumber,
                    updateBioDto.EmailAddress,
                    updateBioDto.OfficeAddress
                };

                return await _dapperRepository.ExecuteAsync(sql, parameters);
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                throw;
            }
        }

        public async Task<CreateBioDTO> GetBioByUserIdAsync()
        {
            try
            {
                string userId = _authService.GetCurrentUserId();

                string sql = @"SELECT FullName, Specialization, ContactNumber, EmailAddress, OfficeAddress 
                               FROM BioTable
                               WHERE UserId = @UserId";

                var parameters = new { UserId = userId };

                return await _dapperRepository.GetAsync<CreateBioDTO>(sql, parameters);
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                throw;
            }
        }
    }
}

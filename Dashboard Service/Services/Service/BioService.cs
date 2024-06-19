using Dashboard_Service.Models.DTO;
using Dashboard_Service.Repository.IRepository;
using Dashboard_Service.Services.IService;
using Dashboard_Service.Services.Service;

namespace Dashboard_Service.Services.Services_
{
    public class BioService:IBioService
    {
        private readonly IBioRepository _bioRepository;
        private readonly AuthService _authService;

        public BioService(IBioRepository bioRepository, AuthService authService)
        {
            _bioRepository = bioRepository;
            _authService = authService;
        }

        public async Task<int> CreateBioAsync(CreateBioDTO createBioDto)
        {
            // Get the UserId from the current user's context
            //createBioDto.UserId = _authService.GetCurrentUserId();
            return await _bioRepository.CreateBioAsync(createBioDto);
        }
        public async Task<int> UpdateBioAsync(CreateBioDTO updateBioDto)
        {
            // Get the UserId from the current user's context
            //updateBioDto.UserId = _authService.GetCurrentUserId();
            return await _bioRepository.UpdateBioAsync(updateBioDto);
        }
        public async Task<CreateBioDTO> GetBioByUserIdAsync()
        {
            //string userId = _authService.GetCurrentUserId();
            return await _bioRepository.GetBioByUserIdAsync(/*userId*/);
        }
    }
}

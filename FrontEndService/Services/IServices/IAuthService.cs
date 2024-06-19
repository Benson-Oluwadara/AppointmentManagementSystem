using FrontEndService.Models.WebDTO.LoginDTO;
using FrontEndService.Models.WebDTO.SignUpDTO;

namespace FrontEndService.Services.IServices
{
    public interface IAuthService
    {
        Task<T> RegisterAsync<T>(RegisterUserDTO registrationRequestDto);
        Task<T> LoginAsync<T>(LoginUserDTO loginRequestDto);
        
        //Task<T> AssignRoleAsync<T>(string email, string roleName);

    }
}

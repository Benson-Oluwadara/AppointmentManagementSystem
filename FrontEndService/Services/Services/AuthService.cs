using FrontEndService.Models;
using FrontEndService.Models.WebDTO.LoginDTO;
using FrontEndService.Models.WebDTO.SignUpDTO;
using FrontEndService.Services.IServices;
using FrontEndService.Utility;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FrontEndService.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IBaseService baseService, ILogger<AuthService> logger)
        {
            _baseService = baseService;
            _logger = logger;
        }

        public async Task<T> LoginAsync<T>(LoginUserDTO loginRequestDto)
        {
            try
            {
                _logger.LogInformation("Attempting to log in user: {UserName}", loginRequestDto.UserName);

                var apiResponse = await _baseService.SendAsync<T>(new WebAPIRequest()
                {
                    apiType = SD.ApiType.POST,
                    Data = loginRequestDto,
                    Url = SD.AuthAPIBase.TrimEnd('/') + "/api/AuthAPI/login"
                }, "AuthAPI");

                var apiResponseJson = JsonConvert.SerializeObject(apiResponse);
                _logger.LogInformation("User {UserName} logged in successfully", loginRequestDto.UserName);
                _logger.LogInformation("API Response: {ApiResponseJson}", apiResponseJson);

                return apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging in user: {UserName}", loginRequestDto.UserName);

                var errorResponse = new WebAPIResponse
                {
                    IsSuccess = false,
                    ErrorMessages = new List<string> { ex.Message }
                };

                var errorResponseJson = JsonConvert.SerializeObject(errorResponse);
                return JsonConvert.DeserializeObject<T>(errorResponseJson);
            }
        }

        public async Task<T> RegisterAsync<T>(RegisterUserDTO registrationRequestDto)
        {
            try
            {
                _logger.LogInformation("Attempting to register user: {Email}", registrationRequestDto.Email);

                var apiResponse = await _baseService.SendAsync<T>(new WebAPIRequest()
                {
                    apiType = SD.ApiType.POST,
                    Data = registrationRequestDto,
                    Url = SD.AuthAPIBase.TrimEnd('/') + "/api/AuthAPI/register"
                }, "AuthAPI");

                var apiResponseJson = JsonConvert.SerializeObject(apiResponse);
                _logger.LogInformation("User {Email} registered successfully", registrationRequestDto.Email);
                _logger.LogInformation("API Response: {ApiResponseJson}", apiResponseJson);

                return apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering user: {Email}", registrationRequestDto.Email);

                var errorResponse = new WebAPIResponse
                {
                    IsSuccess = false,
                    ErrorMessages = new List<string> { ex.Message }
                };

                var errorResponseJson = JsonConvert.SerializeObject(errorResponse);
                return JsonConvert.DeserializeObject<T>(errorResponseJson);
            }
        }

    }
}

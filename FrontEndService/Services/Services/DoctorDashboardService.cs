using FrontEndService.Models;
using FrontEndService.Models.WebDTO.BioDTO;
using FrontEndService.Services.IServices;
using FrontEndService.Utility;
using Newtonsoft.Json;

namespace FrontEndService.Services.Services
{
    public class DoctorDashboardService : IDoctorDashboardService
    {
        private readonly IBaseService _baseService;

        
        public DoctorDashboardService(IBaseService baseService)
        {
            _baseService = baseService;
            
        }
        public async Task<T> GetBioByUserIdAsync<T>()
        {
            try
            {
                Console.WriteLine("Sending request to fetch bio."); // Debugging log

                var response = await _baseService.SendAsync<T>(new WebAPIRequest
                {
                    apiType = SD.ApiType.GET,
                    //Url = "api/Bio/get",
                    Url = SD.DashboardAPIBase.TrimEnd('/') + "/api/Bio/get"
                }, "BioAPI");

                Console.WriteLine($"Received response: {JsonConvert.SerializeObject(response)}"); // Debugging log
                var apiResponseJson = JsonConvert.SerializeObject(response);

                Console.WriteLine($"API Response: {apiResponseJson}");
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting bio by user ID: {ex.Message}");
                throw;
            }
        }

        public async Task<T> UpdateBioAsync<T>(BioDTO biodto)
        {
            // Implement logic to update bio information using the provided DTO
            try
            {
                // Send request to the backend API to update bio
                var response = await _baseService.SendAsync<T>(new WebAPIRequest
                {
                    apiType = SD.ApiType.PUT, // Use PUT request
                    Data = biodto, // BioDTO object to update bio information
                    Url = SD.DashboardAPIBase.TrimEnd('/') + "/api/Bio/update" // Endpoint to update bio
                }, "BioAPI");
                var apiResponseJson = JsonConvert.SerializeObject(response);

                Console.WriteLine($"API Response: {apiResponseJson}");
                return response;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine($"Error updating bio: {ex.Message}");
                throw; // Propagate the exception
            }
        }


        public async Task<T> CreateBioAsync<T>(BioDTO biodto)
        {
            // Implement logic to create new bio entry using the provided DTO
            try
            {
                // Send request to the backend API to create bio
                var response = await _baseService.SendAsync<T>(new WebAPIRequest
                {
                    apiType = SD.ApiType.POST, // Use POST request
                    Data = biodto, // BioDTO object to create bio entry
                    Url = SD.DashboardAPIBase.TrimEnd('/') + "/api/Bio/create" // Endpoint to create bio
                }, "BioAPI");
                var apiResponseJson = JsonConvert.SerializeObject(response);

                Console.WriteLine($"API Response: {apiResponseJson}");
                return response;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine($"Error creating bio: {ex.Message}");
                throw; // Propagate the exception
            }
        }
    }
}

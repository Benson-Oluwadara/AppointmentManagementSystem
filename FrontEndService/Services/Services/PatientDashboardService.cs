using FrontEndService.Models.WebDTO.BioDTO;
using FrontEndService.Models;
using FrontEndService.Services.IServices;
using FrontEndService.Utility;
using Newtonsoft.Json;

namespace FrontEndService.Services.Services
{
    public class PatientDashboardService : IPatientDashboardService
    {
        private readonly IBaseService _baseService;

        public PatientDashboardService(IBaseService baseService)
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
                    Url = SD.PatientAPIBase.TrimEnd('/') + "/api/Patient/get"
                }, "PatientBioAPI");
                Console.WriteLine($"Received response: {JsonConvert.SerializeObject(response)}"); // Debugging log
                var apiResponseJson = JsonConvert.SerializeObject(response);

                Console.WriteLine($"API Response: {apiResponseJson}");
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting bio by user ID: " + ex.Message);
            }
        }

        public async Task<T> UpdateBioAsync<T>(PatientBioDTO biodto)
        {
            try
            {
                var response = await _baseService.SendAsync<T>(new WebAPIRequest
                {
                    apiType = SD.ApiType.PUT,
                    Data = biodto,
                    Url = SD.PatientAPIBase.TrimEnd('/') + "/api/Patient/update"
                }, "PatientBioAPI");
                var apiResponseJson = JsonConvert.SerializeObject(response);

                Console.WriteLine($"API Response: {apiResponseJson}");

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating bio: " + ex.Message);
            }
        }
    }
}

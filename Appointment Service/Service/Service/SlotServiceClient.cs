using Appointment_Service.Models.DTO;
using Appointment_Service.Service.IService;
using Newtonsoft.Json;
using Slot_Service.Models.DTO;
using Slot_Service.Service.Service;
using System.Net.Http;

namespace Appointment_Service.Service.Service
{
    public class SlotServiceClient:ISlotServiceClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SlotServiceClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        //public async Task<SlotDTO> GetSlotsByCodeAsync(string SlotCodeName)
        //{
        //    var client = _httpClientFactory.CreateClient("SlotServiceAPI");
        //    var response = await client.GetAsync($"api/SlotService/code/{SlotCodeName}");

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var content = await response.Content.ReadAsStringAsync();
        //        //Console.WriteLine("String Content::::::" + content);
        //        //var slots = JsonConvert.DeserializeObject<IEnumerable<SlotDTO>>(content);
        //        //Console.WriteLine("Number of slots retrieved: " + slots.Count()); // Logging the number of slots retrieved
        //        //return slots;
        //        return JsonConvert.DeserializeObject<SlotDTO>(content); // Deserialize as a single object

        //    }

        //    Console.WriteLine("Failed to retrieve slots. Status code: " + response.StatusCode); // Logging the failure
        //    return new SlotDTO(); // Return an empty list if the request is unsuccessful
        //}

        public async Task<IEnumerable<SlotCodeDTO>> GetSlotsByCodeAsync(string SlotCodeName)
        {
            var client = _httpClientFactory.CreateClient("SlotServiceAPI");
            var response = await client.GetAsync($"api/SlotService/code/{SlotCodeName}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response Content: " + content);
                
                return JsonConvert.DeserializeObject<IEnumerable<SlotCodeDTO>>(content);
            }

            return new List<SlotCodeDTO>();
        }

        //public async Task<IEnumerable<ReadAppointmentDTO>> ReadandGetSlotByCodeAsync(string SlotCodeName)
        //{
        //    //return await _slotRepository.GetSlotByCodeAsync(SlotCodeName);
        //}

        //public async Task<IEnumerable<SlotDTO>> GetSlotsByCodeAsync(string SlotCodeName)
        //{
        //    try
        //    {
        //        var client = _httpClientFactory.CreateClient("SlotServiceAPI");

        //        // Construct the URL with the provided SlotCodeName
        //        string requestUrl = $"api/SlotService/code/{SlotCodeName}";

        //        // Send the GET request
        //        var response = await client.GetAsync(requestUrl);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            // Read and deserialize the response content
        //            var content = await response.Content.ReadAsStringAsync();
        //            return JsonConvert.DeserializeObject<IEnumerable<SlotDTO>>(content);
        //        }
        //        else
        //        {
        //            // Handle unsuccessful response (e.g., log error, return empty list)
        //            // You can customize this based on your application's requirements
        //            return new List<SlotDTO>();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle any exceptions (e.g., log error)
        //        Console.WriteLine($"An error occurred: {ex.Message}");
        //        throw; // Rethrow the exception or handle it accordingly
        //    }
        //}
    }
}

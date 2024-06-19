using FrontEndService.Models;
using FrontEndService.Models.WebDTO.SlotDTO;
using FrontEndService.Services.IServices;
using FrontEndService.Utility;
using Newtonsoft.Json;

namespace FrontEndService.Services.Services
{
    public class DoctorSlotService:IDoctorSlotService
    {
        private readonly IBaseService _baseService;

        public DoctorSlotService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<T> CreateSlotAsync<T>(SlotDTO createslotdto)
        {
            try
            {
                Console.WriteLine("Sending request to create Slot."); // Debugging log
               

                var response=await _baseService.SendAsync<T>(new WebAPIRequest
                {
                    apiType = SD.ApiType.POST,
                    Data=createslotdto,
                    Url = SD.SlotAPIBase.TrimEnd('/') + "/api/SlotService/CreateSlot"
                }, "DoctorAvailabilityAPI");

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

        public async Task<T> GetSlotAsync<T>(int slotId)
        {
            try
            {
                var response = await _baseService.SendAsync<T>(new WebAPIRequest
                {
                    apiType = SD.ApiType.GET,
                    Url = SD.SlotAPIBase.TrimEnd('/') + $"/api/SlotService/GetSlotById/{slotId}"
                }, "DoctorAvailabilityAPI");

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching slot details: " + ex.Message);
            }
        }


        public async Task<T> UpdateSlotAsync<T>(int id,SlotDTO slotDto)
        {
            try
            {
                var response = await _baseService.SendAsync<T>(new WebAPIRequest
                {
                    apiType = SD.ApiType.PUT,
                    Url = SD.SlotAPIBase.TrimEnd('/') + $"/api/SlotService/UpdateSlot/{id}",
                    Data = slotDto
                }, "DoctorAvailabilityAPI");

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating slot: " + ex.Message);
            }
        }

        public async Task<T> DeleteSlotAsync<T>(int slotId)
        {
            try
            {
                var response = await _baseService.SendAsync<T>(new WebAPIRequest
                {
                    apiType = SD.ApiType.DELETE,
                    Url = SD.SlotAPIBase.TrimEnd('/') + $"/api/SlotService/DeleteSlot/{slotId}"
                }, "DoctorAvailabilityAPI");

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting slot: " + ex.Message);
            }
        }

        public async Task<List<SlotDTO>> GetAllSlot<T>()
        {
            try
            {
                var response = await _baseService.SendAsync<List<SlotDTO>>(new WebAPIRequest
                {
                    apiType = SD.ApiType.GET,
                    Url = SD.SlotAPIBase.TrimEnd('/') + $"/api/SlotService/GetAllSlots"
                }, "DoctorAvailabilityAPI");

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting all slots: " + ex.Message);
            }
        }
    }




}


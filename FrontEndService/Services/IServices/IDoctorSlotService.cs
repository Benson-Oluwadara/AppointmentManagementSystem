using FrontEndService.Models.WebDTO.BioDTO;
using FrontEndService.Models.WebDTO.SlotDTO;

namespace FrontEndService.Services.IServices
{
    public interface IDoctorSlotService
    {
        
        Task<T> CreateSlotAsync<T>(SlotDTO createslotdto);
        Task<T> GetSlotAsync<T>(int slotId);
        Task<T> UpdateSlotAsync<T>(int id, SlotDTO slotDto);
        Task<T> DeleteSlotAsync<T>(int slotId);
        Task<List<SlotDTO>> GetAllSlot<T>();
    }
}

using Slot_Service.Models.DTO;

namespace Slot_Service.Service.IService
{
    //public interface ISlotService
    //{
    //    Task<SlotDTO> CreateSlotAsync(CreateSlotDTO createSlotDto);
    //    Task<IEnumerable<SlotDTO>> GetAllSlotsAsync();
    //    Task<SlotDTO> GetSlotByIdAsync(int id);
    //    //Task<bool> UpdateSlotAsync(int id, CreateSlotDTO updateSlotDto);
    //    Task<bool> UpdateSlotAsync(string SlotCodeName, int id, CreateSlotDTO updateSlotDto);
    //    Task<bool> DeleteSlotAsync(string slotCodeName, int id);
    //    //Task<SlotDTO> GetSlotByCodeAsync(string SlotCodeName);
    //    Task<IEnumerable<SlotDTO>> GetSlotByCodeAsync();
    //    //Task<bool> UpdateSlotAsync(int id, CreateSlotDTO updateSlotDto);
    //    //Task<bool> DeleteSlotAsync(int id);
    //}

    public interface ISlotService
    {
        Task<SlotDTO> CreateSlotAsync(CreateSlotDTO createSlotDto, string slotCodeName);
        Task<IEnumerable<SlotDTO>> GetSlotByCodeAsync(string slotCodeName);
        Task<SlotDTO> GetSlotByIdAndCodeAsync(int id, string slotCodeName);
        Task<bool> UpdateSlotAsync(string slotCodeName, int id, CreateSlotDTO updateSlotDto);
        Task<bool> DeleteSlotAsync(string slotCodeName, int id);
    }
}

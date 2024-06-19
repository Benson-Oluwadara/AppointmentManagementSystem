using Slot_Service.Models;
using Slot_Service.Models.DTO;

namespace Slot_Service.Repository.IRepository
{
    public interface ISlotRepository
    {
        //Task<int> CreateSlotAsync(CreateSlotDTO createSlotDto);
        //Task<IEnumerable<SlotDTO>> GetAllSlotsAsync();
        //Task<SlotDTO> GetSlotByIdAsync(int id);
        ////Task<bool> UpdateSlotAsync(int id, CreateSlotDTO updateSlotDto);
        ////Task<bool> DeleteSlotAsync(int id);
        //Task<bool> UpdateSlotAsync(string SlotCodeName, int id, CreateSlotDTO updateSlotDto);
        //Task<bool> DeleteSlotAsync(string slotCodeName, int id);
        ////Task<bool> DeleteSlotAsync(string slotCodeName, int id);
        ////Task<SlotDTO> GetSlotByCodeAsync(string SlotCodeName);
        //Task<IEnumerable<SlotDTO>> GetSlotByCodeAsync();
        Task<int> CreateSlotAsync(Slot slot);
        Task<IEnumerable<SlotDTO>> GetSlotByCodeAsync(string slotCodeName);
        Task<SlotDTO> GetSlotByIdAndCodeAsync(int id, string slotCodeName);
        Task<bool> UpdateSlotAsync(string slotCodeName, int id, CreateSlotDTO updateSlotDto);
        Task<bool> DeleteSlotAsync(string slotCodeName, int id);
    }
}

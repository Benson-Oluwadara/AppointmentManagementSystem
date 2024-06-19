using Slot_Service.Models.DTO;

namespace Appointment_Service.Service.IService
{
    public interface ISlotServiceClient
    {
        Task<IEnumerable<SlotCodeDTO>> GetSlotsByCodeAsync(string SlotCodeName);

    }
}

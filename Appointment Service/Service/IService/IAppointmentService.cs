using Appointment_Service.Models.DTO;
using Slot_Service.Models;

namespace Appointment_Service.Service.IService
{
    public interface IAppointmentService
    {
        Task<AppointmentDTO> CreateAppointmentAsync(CreateAppointmentDTO createAppointmentDto);
        Task<IEnumerable<ReadAppointmentDTO>> GetAppointmentsBySlotCodeNameAsync(string slotCodeName);
        //Task<int> UpdateAppointmentToBookedAsync(string slotCodeName, DateTime appointmentDateTime);
        //Task<int> UpdateAppointmentToBookedAsync(UpdateAppointmentToBookedDTO updateAppointmentDto);
        Task<IEnumerable<Slot>> GetAvailableSlotsAsync();
        Task<IEnumerable<Slot>> GetBookedSlotsByCodeNameAsync(string slotCodeName);
        Task UpdateAppointmentToBookedAsync(UpdateAppointmentToBookedDTO dto);
        Task CancelAppointmentAsync(CancelAppointmentDTO dto);
    }
}

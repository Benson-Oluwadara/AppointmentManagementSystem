using Appointment_Service.Models;
using Appointment_Service.Models.DTO;
using Slot_Service.Models;

namespace Appointment_Service.Repository.IRepository
{
    public interface IAppointmentRepository
    {
        Task<int> CreateAppointmentAsync(Appointment appointment);
        Task<IEnumerable<AppointmentDTO>> GetAllAppointmentsAsync();
        Task<Appointment> GetAppointmentByIdAsync(int id);
        Task<IEnumerable<ReadAppointmentDTO>> GetAppointmentsBySlotCodeNameAsync(string slotCodeName);
        //Task<int> UpdateAppointmentToBookedAsync(string slotCodeName, DateTime appointmentDateTime);
        //Task<IEnumerable<AppointmentDTO>> GetAllAppointmentsAsync();
        //Task<AppointmentDTO> GetAppointmentByIdAsync(int id);
        //Task<bool> UpdateAppointmentAsync(UpdateAppointmentDTO updateappointment);
        //Task<bool> DeleteAppointmentAsync(int id);

        Task<IEnumerable<Slot>> GetAvailableSlotsAsync();
        Task<IEnumerable<Slot>> GetBookedSlotsByCodeNameAsync(string slotCodeName);
        Task<int> UpdateSlotToBookedAsync(int slotId, DateTime newDateTime);
        Task<int> UpdateSlotToAvailableAsync(int slotId);


        Task CancelAppointmentAsync(CancelAppointmentDTO dto);
    }
}

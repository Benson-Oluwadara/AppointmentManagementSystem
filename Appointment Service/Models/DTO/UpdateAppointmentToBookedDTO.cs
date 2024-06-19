namespace Appointment_Service.Models.DTO
{
    public class UpdateAppointmentToBookedDTO
    {
        public string SlotCodeName { get; set; }
        public DateTime CurrentDateTime { get; set; }
        public DateTime NewDateTime { get; set; }
    }
}

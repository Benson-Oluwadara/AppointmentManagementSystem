using System.Globalization;

namespace Appointment_Service.Models.DTO
{
    public class ReadAppointmentDTO
    {
        public DateTime SlotDateTime { get; set; }

        public override string ToString()
        {
            return $"SlotDateTime: {SlotDateTime}";
        }


    }
}

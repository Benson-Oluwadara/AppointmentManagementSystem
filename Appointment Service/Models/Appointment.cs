namespace Appointment_Service.Models
{
    public class Appointment
    {
        public int AppointmentID { get; set; }
        public DateTime DateTime { get; set; }//Slot Availability 
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }
}

namespace DoctorService.Models.DTO
{
    public class DoctorDTO
    {
        public Guid DoctorID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Specialization { get; set; }
    }
}

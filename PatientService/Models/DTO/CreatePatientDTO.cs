namespace PatientService.Models.DTO
{
    public class CreatePatientDTO
    {
        public Guid PatientID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

}

namespace PatientService.Models
{
    public class Patient
    {
        public Guid PatientID { get; set; } // Use GUID for unique identifier
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }

}

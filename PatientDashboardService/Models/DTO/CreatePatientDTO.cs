namespace PatientDashboardService.Models.DTO
{
    public class CreatePatientDTO
    {
        public string FullName { get; set; }
        public string ContactNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string MedicalHistory { get; set; }
    }
}

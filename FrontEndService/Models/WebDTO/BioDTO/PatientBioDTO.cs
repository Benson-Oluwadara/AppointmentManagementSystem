namespace FrontEndService.Models.WebDTO.BioDTO
{
    public class PatientBioDTO
    {
        public string? FullName { get; set; }
        public string? ContactNumber { get; set; }
        public string? EmailAddress { get; set; }
        public string? Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? MedicalHistory { get; set; }
    }
}

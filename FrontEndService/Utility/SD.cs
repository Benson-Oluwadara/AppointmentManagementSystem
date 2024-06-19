namespace FrontEndService.Utility
{
    public class SD
    {
        
        public static string AuthAPIBase { get; set; }
        public static string DashboardAPIBase { get; set; }
        public static string PatientAPIBase { get; set; }
        public static string SlotAPIBase { get; set; }

        public const string TokenCookie = "JWTToken";
        public const string RolePatient = "Patient";
        public const string RoleDoctor = "Doctor";
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}

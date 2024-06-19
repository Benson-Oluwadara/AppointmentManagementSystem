using System.ComponentModel.DataAnnotations;

namespace UserManagementService.Models.Authentication.SignUp
{
    public class PreRegisteredUser
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string PasswordHash { get; set; }
        //[Required]
        //public string PlainPassword { get; set; }
        [Required]
        public string SecurityStamp { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        public string TwoFactorToken { get; set; }

        public DateTime TokenExpiry { get; set; }
    }
}

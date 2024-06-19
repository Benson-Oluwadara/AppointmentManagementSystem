using System.ComponentModel.DataAnnotations;

namespace FrontEndService.Models.WebDTO.LoginDTO
{
    public class LoginUserDTO
    {
        [Required(ErrorMessage = "Username is required.")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string? Password { get; set; }
    }
}

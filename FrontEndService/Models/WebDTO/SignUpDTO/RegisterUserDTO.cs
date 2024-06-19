using System.ComponentModel.DataAnnotations;

namespace FrontEndService.Models.WebDTO.SignUpDTO
{
    public class RegisterUserDTO
    {     
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
    }
}

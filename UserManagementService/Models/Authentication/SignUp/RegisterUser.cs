using System.ComponentModel.DataAnnotations;

namespace UserManagementService.Models.Authentication.SignUp
{
    public class RegisterUser
    {
        [Required(ErrorMessage ="User Name is Required")]
        public string ? UserName { get; set; }

        [Required(ErrorMessage = "User Name is Required")]
        public string? Password { get; set; }

        [Required(ErrorMessage ="Password is Required")]
        public string? Email { get; set; }

    }
}

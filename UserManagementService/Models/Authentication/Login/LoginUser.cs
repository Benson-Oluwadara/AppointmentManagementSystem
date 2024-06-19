using System.ComponentModel.DataAnnotations;

namespace UserManagementService.Models.Authentication.Login
{
    public class LoginUser
    {
        [Required(ErrorMessage = "User Name is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace WebAppPerfumes01.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Enter Email Address")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter Your Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }

}

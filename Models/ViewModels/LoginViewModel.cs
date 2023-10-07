
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [DisplayName("Email Address")]
        [EmailAddress]
        [RegularExpression(@".*\.com$", ErrorMessage = "The email must end with .com")]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}

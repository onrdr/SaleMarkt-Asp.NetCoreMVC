using System.ComponentModel.DataAnnotations;
namespace Models.ViewModels;

public class ResetPasswordViewModel
{
    [Required]
    [Display(Name = "Email Address")]
    [EmailAddress]
    [RegularExpression(@".*\.com$", ErrorMessage = "The email must end with .com")]
    public string Email { get; set; }   
}

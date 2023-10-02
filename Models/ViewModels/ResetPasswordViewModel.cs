using System.ComponentModel.DataAnnotations;
namespace Models.ViewModels;

public class ResetPasswordViewModel
{
    [Required]
    [Display(Name = "Email Address")]
    [EmailAddress]
    public string Email { get; set; }   
}

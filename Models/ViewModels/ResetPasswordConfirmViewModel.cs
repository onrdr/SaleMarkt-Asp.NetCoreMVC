using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels;

public class ResetPasswordConfirmViewModel
{
    [Required]
    [Display(Name = "New Password")]
    [DataType(DataType.Password)]
    [MinLength(6)]
    public string NewPassword { get; set; }

    [Required]
    [Display(Name = "Confirm New Password")]
    [DataType(DataType.Password)]
    [Compare(nameof(NewPassword))]
    [MinLength(6)]
    public string ConfirmNewPassword { get; set; }

    public string? UserId { get; set; }
    public string? Token { get; set; }
}

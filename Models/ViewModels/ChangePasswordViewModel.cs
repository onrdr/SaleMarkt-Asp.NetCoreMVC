using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels;

public class ChangePasswordViewModel
{
    [Required]
    [DataType(DataType.Password)]
    [Display(Name ="Old Password")]
    [MinLength(6)]
    public string OldPassword { get; set; }

    [Required()]
    [DataType(DataType.Password)]
    [Display(Name = "New Password")]
    [MinLength(6)]
    public string NewPassword{ get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(NewPassword))]
    [Display(Name = "Confirm New Password")]
    [MinLength(6)]
    public string ConfirmNewPassword { get; set; }
}

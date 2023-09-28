
using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Display(Name ="Old Password")]
        [Required]
        [DataType(DataType.Password)]
        [MinLength(6)]
        public string OldPassword { get; set; }

        [Display(Name = "New Password")]
        [Required()]
        [DataType(DataType.Password)]
        [MinLength(6)]
        public string NewPassword{ get; set; }

        [Display(Name = "Confirm New Password")]
        [Required]
        [DataType(DataType.Password)]
        [MinLength(4)]
        [Compare(nameof(NewPassword))]
        public string NewPasswordConfirm { get; set; }
    }
}

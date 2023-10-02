using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Models.ViewModels;

public class UserViewModel
{
    [Required] 
    public string Name { get; set; }

    [Required]
    [DisplayName("Email Address")]
    [EmailAddress] 
    [RegularExpression(@".*\.com$", ErrorMessage = "Email must end with .com")]
    public string Email { get; set; } 

    [Required]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; }

    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
}

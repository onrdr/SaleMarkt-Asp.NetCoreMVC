using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels;

public class CompanyViewModel
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    [DisplayName("Email Address")]
    [EmailAddress]
    [RegularExpression(@".*\.com$", ErrorMessage = "The email must end with .com")]
    public string Email { get; set; }

    [Required]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; }

    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    public string? ImageUrl { get; set; }
}

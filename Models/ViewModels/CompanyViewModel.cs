using Models.ViewModels.Abstract;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels;

public class CompanyViewModel : IImageViewModel
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    [DisplayName("Email Address")]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; }

    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    public string? ImageUrl { get; set; }
    public string FolderName { get; set; } = "logo";
}

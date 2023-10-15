using Microsoft.AspNetCore.Identity;
using Models.Entities.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Models.Identity;

public class AppUser : IdentityUser<Guid>, IBaseEntity
{
    [Required]
    public string Name { get; set; }

    public bool IsSuspended { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; } 
}

using Models.Entities.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Models.Entities.Concrete;

public class Company : IBaseEntity
{
    public Company()
    {
        Id = Guid.NewGuid();
    }

    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Email { get; set; }
    
    [Required] 
    public string PhoneNumber { get; set; }
    
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    public string? ImageUrl { get; set; }
}

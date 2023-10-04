using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Models.Identity;
using Models.Entities.Abstract;

namespace Models.Entities.Concrete;

public class OrderHeader : IBaseEntity
{
    public OrderHeader()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string PhoneNumber { get; set; }

    [Required]
    public string Address { get; set; }

    [Required]
    public string City { get; set; }

    [Required]
    public string Country { get; set; }

    [Required]
    public string PostalCode { get; set; }

    public DateTime OrderDate { get; set; }
    public DateTime ShippingDate { get; set; }
    public double OrderTotal { get; set; }
    public string? OrderStatus { get; set; }
    public string? PaymentStatus { get; set; } 
    public DateTime PaymentDate { get; set; }
    public DateTime PaymentDueDate { get; set; } 

    public Guid AppUserId { get; set; }
    [ForeignKey("AppUserId")] 
    public AppUser AppUser { get; set; } 
    
}

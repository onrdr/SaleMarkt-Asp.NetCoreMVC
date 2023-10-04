using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Models.Identity;
using Models.Entities.Abstract;

namespace Models.Entities.Concrete;

public class ShoppingCart : IBaseEntity
{
    public ShoppingCart()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; set; }

    public Guid ProductId { get; set; }
    [ForeignKey("ProductId")] 
    public Product Product { get; set; }

    [Range(1, 1000, ErrorMessage = "Please enter a value between 1 and 1000")]
    public int Count { get; set; }

    [NotMapped]
    public double Price { get; set; }

    public Guid AppUserId { get; set; }
    [ForeignKey("AppUserId")] 
    public AppUser AppUser { get; set; }
}


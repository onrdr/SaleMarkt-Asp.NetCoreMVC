using Models.Entities.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Models.Entities.Concrete;

public class Product : IBaseEntity
{
    public Product()
    {
        Id = Guid.NewGuid();
    }

    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public string ISBN { get; set; }

    [Required]
    public string Author { get; set; }

    [Required]
    public double ListPrice { get; set; }

    [Required]
    public double Price{ get; set; }

    [Required]
    public double Price50 { get; set; }

    [Required]
    public double Price100 { get; set; }

    [Required]
    public Guid CategoryId { get; set; }
    public Category Category { get; set; } 
}

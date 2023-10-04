using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models.Entities.Concrete;

public class ProductImage
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string ImageUrl { get; set; }

    public Guid ProductId { get; set; }
    [ForeignKey("ProductId")]
    public Product Product { get; set; }
}
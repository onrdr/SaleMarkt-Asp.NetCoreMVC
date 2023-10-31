using Models.Entities.Abstract;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models.Entities.Concrete;

public class Category : IBaseEntity
{
    public Category()
    {
        Id = Guid.NewGuid();
    }

    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public int DisplayOrder { get; set; }

    [Required]
    public string Description { get; set; }

    public string? ImageUrl { get; set; }

    public IEnumerable<Product> Products { get; set; }
}

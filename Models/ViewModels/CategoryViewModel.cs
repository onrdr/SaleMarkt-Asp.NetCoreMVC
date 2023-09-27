using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels;

public class CategoryViewModel
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(30)]
    [DisplayName("Category Name")]
    public string Name { get; set; }


    [DisplayName("Display Order")]
    [Range(1, 100)]
    public int DisplayOrder { get; set; }
}

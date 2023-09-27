﻿using Models.Entities.Concrete;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels;

public class ProductViewModel
{
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
    [DisplayName("List Price")]
    [Range(1, 10000)]
    public double ListPrice { get; set; }

    [Required]
    [DisplayName("Price for 1-50")]
    [Range(1, 10000)]
    public double Price { get; set; }


    [Required]
    [DisplayName("Price for 50+")]
    [Range(1, 10000)]
    public double Price50 { get; set; }


    [Required]
    [DisplayName("Price for 100+")]
    [Range(1, 10000)]
    public double Price100 { get; set; }

    [Required(ErrorMessage ="Category required")]
    public Guid CategoryId { get; set; }
}

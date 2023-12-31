﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Models.Entities.Abstract;

namespace Models.Entities.Concrete;

public class OrderDetail : IBaseEntity
{
    public OrderDetail()
    {
        Id = Guid.NewGuid();
    }

    [Required]
    public Guid Id { get; set; }

    public int Count { get; set; }

    public double Price { get; set; }

    public string ProductSize { get; set; }

    [Required]
    public Guid OrderHeaderId { get; set; }

    [ForeignKey("OrderHeaderId")] 
    public OrderHeader OrderHeader { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    [ForeignKey("ProductId")] 
    public Product Product { get; set; } 
}

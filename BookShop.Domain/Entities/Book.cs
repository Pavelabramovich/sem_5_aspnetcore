using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace BookShop.Domain.Entities;

public class Book
{
    public int Id { get; init; }

    public string? Title { get; init; }
    public Category? Category { get; set; }

    public decimal Price { get; set; }

    public string? ImagePath { get; set; }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Domain.Entities;

public class Category
{
    public int Id { get; init; }
    public string? Name { get; set; }

    public ICollection<Book> Books { get; }

    public Category() 
    {
        Books = new List<Book>();
    }

}

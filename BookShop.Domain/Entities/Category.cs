using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Domain.Entities;

public class Category : Entity, IEquatable<Category>
{
    public string? Name { get; set; }

    public ICollection<Book> Books { get; }

    public Category() 
    {
        Books = new List<Book>();
    }

    public bool Equals(Category? other) => other is not null ? this.Id == other.Id : false;
    public override bool Equals(object? other) => Equals(other as Category);

    public static bool operator ==(Category? first, Category? second) => first is null && second is null ? true : first?.Equals(second) ?? false;
    public static bool operator !=(Category? first, Category? second) => !(first == second);

    public override int GetHashCode() => Id.GetHashCode();
}

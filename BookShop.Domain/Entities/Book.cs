using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace BookShop.Domain.Entities;

public class Book : Entity, IEquatable<Book>
{
    public string? Title { get; set; }

    public int? CategoryId { get; set; }
    public Category? Category { get; set; }

    public decimal Price { get; set; }

    public string? ImagePath { get; set; }




    public bool Equals(Book? other) => other is not null ? this.Id == other.Id : false;
    public override bool Equals(object? other) => Equals(other as Book);

    public static bool operator ==(Book? first, Book? second) => first is null && second is null ? true : first?.Equals(second) ?? false;
    public static bool operator !=(Book? first, Book? second) => !(first == second);

    public override int GetHashCode() => Id.GetHashCode();
}

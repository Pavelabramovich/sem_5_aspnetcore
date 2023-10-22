using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Domain.Entities;


public interface ICart
{
    public void AddToCart(Book book);

    public void RemoveOne(int id);

    public void RemoveItems(int id);

    public void Clear();
}


public class Cart : ICart
{
    public Dictionary<int, CartItem> CartItems { get; set; } = new();


    public virtual void AddToCart(Book book)
    {
        if (CartItems.ContainsKey(book.Id))
        {
            CartItems[book.Id].Count++;
        }
        else
        {
            CartItems[book.Id] = new CartItem() { Book = book };
        }
    }

    public virtual void RemoveOne(int id)
    {
        if (CartItems.ContainsKey(id))
        {
            int count = CartItems[id].Count;

            if (count > 1)
            {
                CartItems[id].Count--;
            }
            else
            {
                CartItems.Remove(id);
            }     
        }
    }

    public virtual void RemoveItems(int id)
    {
        CartItems.Remove(id);
    }

    public virtual void Clear()
    { 
        CartItems.Clear();
    }


    public int Count => CartItems.Values.Sum(item => item.Count);

    public int TotalPrice => CartItems.Values.Sum(item => item.Book.Price * item.Count);


    public IEnumerator<CartItem> GetEnumerator()
    {
        return CartItems.Values.GetEnumerator();
    }
}


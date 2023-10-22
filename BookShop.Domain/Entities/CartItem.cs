using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 

namespace BookShop.Domain.Entities;


public class CartItem
{
    public required Book Book {  get; set; }

    public int Count { get; set; } = 1;
}

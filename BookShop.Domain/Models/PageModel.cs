using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Domain.Models;

public class PageModel<T>
{
    public List<T> Items { get; init; } = new();

    public int PageNum { get; init; } = 0;
    public int PagesCount { get; init; } = 0;
}
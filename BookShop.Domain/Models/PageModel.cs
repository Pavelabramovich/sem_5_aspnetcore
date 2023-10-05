using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Domain.Models;

public class PageModel<T>
{
    public IEnumerable<T> Items { get; init; }

    public int PageNum { get; init; }
    public int PagesCount { get; init; }


    public PageModel(IEnumerable<T> items, int pagesCount, int pageNum = 0)
    {
        Items = items;
        PagesCount = pagesCount;
        PageNum = pageNum;
    }
}
   
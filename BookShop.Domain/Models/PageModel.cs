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

    public void Deconstruct(out IEnumerable<T> items, out int pagesCount, out int pageNum)
    {
        items = Items;
        pagesCount = PagesCount;
        pageNum = PageNum;
    }
}
   
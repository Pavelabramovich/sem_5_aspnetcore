using BookShop.Domain.Entities;


namespace BookShop.Api.Services;


public class BookImageService : EntityImageService<Book>
{

    public BookImageService(
       IBookService bookService,  
       IHttpContextAccessor httpContextAccessor,
       IConfiguration config,
       IWebHostEnvironment env
    ) : base(bookService, httpContextAccessor, config, env)   
    { }

    protected override string? GetImageField(Book book) => book.ImagePath;
    protected override void SetImageField(Book entity, string value) => entity.ImagePath = value;
}

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.IdentityModel.Tokens;

namespace BookShop.TagHelpers;


public class EmailTagHelper : TagHelper
{
    private const string EmailDomain = "example.com";

    public string MailTo { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "a";
        string address = MailTo + "@" + EmailDomain;
        output.Attributes.SetAttribute("href", "mailto:" + address);
        output.Content.SetContent(address);
    }
}

[HtmlTargetElement("copyright")]
public class CopyrightTagHelper : TagHelper
{
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var content = await output.GetChildContentAsync();
        string copyright = $"<p>© {DateTime.Now.Year} {content.GetContent()}</p>";
        output.Content.SetHtmlContent(copyright);
    }
}


public abstract class PagerTagHelper : TagHelper
{
    public int CurrentPage { get; set; } = 0;
    public int TotalPages { get; set; } = 3;


    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (TotalPages <= 1)
        {
            output.SuppressOutput();
        }
        else
        {
            output.TagName = "nav";
            output.Attributes.RemoveAll("current-page");
            output.Attributes.RemoveAll("total-pages");

            output.Content.SetHtmlContent(GetAllPages());
        }
    }


    private TagBuilder GetNextPageLink()
    {
        int next = CurrentPage + 1 < TotalPages ? CurrentPage + 1 : TotalPages - 1;

        return GetPageLink(next, "»");
    }
    private TagBuilder GetPrevPageLink()
    {
        int prev = CurrentPage - 1 >= 0 ? CurrentPage - 1 : 0;

        return GetPageLink(prev, "«");
    }

    protected abstract TagBuilder GetPageLink(int index, string? text = null);


    private static TagBuilder GetPageListItem(TagBuilder innerHtml, bool isActive = false)
    {
        var li = new TagBuilder("li");

        li.AddCssClass("page-item");

        if (isActive)
            li.AddCssClass("active");

        li.InnerHtml.AppendHtml(innerHtml);

        return li;
    }


    private TagBuilder GetAllPages()
    {
        var ul = new TagBuilder("ul");
        ul.AddCssClass("pagination");


        ul.InnerHtml.AppendHtml(GetPageListItem(GetPrevPageLink()));

        for (int i = 0; i < TotalPages; i++)
            ul.InnerHtml.AppendHtml(GetPageListItem(GetPageLink(i), isActive: i == CurrentPage));

        ul.InnerHtml.AppendHtml(GetPageListItem(GetNextPageLink()));

        return ul;
    }
}


public class BpgrTagHelper : PagerTagHelper
{
    protected readonly LinkGenerator _linkGenerator;

    public string? CategoryName { get; set; }


    public BpgrTagHelper(LinkGenerator linkGenerator)
    {
        _linkGenerator = linkGenerator;
    }

    protected override TagBuilder GetPageLink(int index, string? text = null)
    {
        ArgumentNullException
            .ThrowIfNull(nameof(CategoryName));

        var a = new TagBuilder("a");

        a.AddCssClass("page-link");

        a.InnerHtml.AppendLine(text ?? (index + 1).ToString());

        var href = _linkGenerator.GetPathByAction("Index", "Book", new { categoryName = CategoryName, pageNum = index });

        a.Attributes.Add("href", href); 

        return a;
    }
}

public class AbpgTagHelper : PagerTagHelper
{
    protected readonly LinkGenerator _linkGenerator;

    private readonly HttpContext _httpContext;


    public AbpgTagHelper(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
    {
        _linkGenerator = linkGenerator;
        _httpContext = httpContextAccessor.HttpContext;
    }

    protected override TagBuilder GetPageLink(int index, string? text = null)
    {
        var a = new TagBuilder("a");

        a.AddCssClass("page-link");

        a.InnerHtml.AppendLine(text ?? (index + 1).ToString());

        var href = _linkGenerator.GetPathByPage(_httpContext, "Index", values: new { Area = "Admin", pageNum = index });

        a.Attributes.Add("href", href);

        return a;
    }
}
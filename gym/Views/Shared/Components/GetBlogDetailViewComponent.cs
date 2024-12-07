using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using gym.Models;

public class GetBlogDetailViewComponent : ViewComponent
{
    private readonly GymContext _context;

    public GetBlogDetailViewComponent(GymContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync(string meta, int newsId)
    {
        var newsItem = await _context.News
            .FirstOrDefaultAsync(n => n.NewsId == newsId && n.Meta.Equals(meta));

        return View(newsItem);
    }
}
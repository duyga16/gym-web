using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using gym.Models;

public class GetBlogViewComponent : ViewComponent
{
    private readonly GymContext _context;

    public GetBlogViewComponent(GymContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        // Lấy danh sách bài blog có meta chứa "su-kien"
        var blogPosts = await _context.News
                                       .Where(b => b.Meta.Equals("su-kien"))
                                       .OrderByDescending(n => n.Datebegin) // Sắp xếp theo ngày mới nhất (giảm dần)
                                       .ToListAsync();

        return View(blogPosts);
    }
}
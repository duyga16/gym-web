using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using gym.Models;

public class GetPostViewComponent : ViewComponent
{
    private readonly GymContext _context;

    public GetPostViewComponent(GymContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        // Lấy 5 bài post mới nhất từ cơ sở dữ liệu có meta chứa "tin-tuc/"
        var latestPosts = await _context.News
            .Where(n => n.Hide == 1 && n.Meta.Equals("tin-tuc")) // Kiểm tra cột meta
            .OrderByDescending(n => n.Datebegin) // Sắp xếp theo ngày đăng mới nhất
            .Take(4) // Lấy 5 bài mới nhất
            .ToListAsync();
        return View(latestPosts);
    }
}
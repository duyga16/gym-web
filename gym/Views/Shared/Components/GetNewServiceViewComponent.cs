using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using gym.Models;

public class GetNewServiceViewComponent : ViewComponent
{
    private readonly GymContext _context;

    public GetNewServiceViewComponent(GymContext context)
    {
        _context = context;
    }

    // Hàm này sẽ lấy dữ liệu cho ViewComponent
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var service = await _context.News
            .Where(n => n.Hide == 1 && n.Meta.Contains("dich-vu")) // Lọc theo cột meta
            .OrderByDescending(n => n.Datebegin) // Sắp xếp theo ngày mới nhất (giảm dần)
            .Take(4) // Lấy 4 bài viết đầu tiên
            .ToListAsync();

        return View(service); // Trả về View với danh sách bài viết
    }
}
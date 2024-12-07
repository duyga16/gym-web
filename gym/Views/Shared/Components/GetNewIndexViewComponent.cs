using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using gym.Models;

public class GetNewIndexViewComponent : ViewComponent
{
    private readonly GymContext _context;

    public GetNewIndexViewComponent(GymContext context)
    {
        _context = context;
    }

    // Hàm này sẽ lấy dữ liệu cho ViewComponent
    public async Task<IViewComponentResult> InvokeAsync()
    {
        // Lấy 4 tin có cột meta chứa "gioi-thieu/" và không bị ẩn
        var news = await _context.News
            .Where(n => n.Hide == 1 && n.Meta.Equals("gioi-thieu")) // Kiểm tra cột meta
            .OrderByDescending(n => n.Datebegin) // Sắp xếp theo ngày mới nhất (giảm dần)
            .Take(4) // Lấy 4 tin đầu tiên
            .ToListAsync();

        return View(news); // Trả về View với danh sách tin tức
    }
}
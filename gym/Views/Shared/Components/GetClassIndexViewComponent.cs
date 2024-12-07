using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using gym.Models;

public class GetClassIndexViewComponent : ViewComponent
{
    private readonly GymContext _context;

    public GetClassIndexViewComponent(GymContext context)
    {
        _context = context;
    }

    // Hàm này sẽ lấy dữ liệu cho ViewComponent
    public async Task<IViewComponentResult> InvokeAsync(int take = 5) // lấy 6 lớp
    {
        var classes = await _context.Classes
                                    .Where(c => c.Hide == 1 && c.Order != 5) // Bỏ lớp có order = 5
                                    .OrderBy(c => c.Order)                   // Sắp xếp theo thứ tự
                                    .Take(take)                              // Giới hạn số lượng lớp
                                    .ToListAsync();

        // Trả về View cùng với model là danh sách các lớp học
        return View(classes);
    }
}
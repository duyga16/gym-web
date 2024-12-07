using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using gym.Models;

public class GetRankIndexViewComponent : ViewComponent
{
    private readonly GymContext _context;

    public GetRankIndexViewComponent(GymContext context)
    {
        _context = context;
    }

    // Hàm này sẽ lấy dữ liệu cho ViewComponent
    public async Task<IViewComponentResult> InvokeAsync(int take = 3) // Giới hạn số lượng gói hiển thị
    {
        var ranks = await _context.Rankusers
                                   .Where(r => r.Hide == 1) // Chỉ lấy các gói không bị ẩn
                                   .OrderBy(r => r.Order)    // Sắp xếp theo thứ tự
                                   .Take(take)               // Giới hạn số lượng gói
                                   .ToListAsync();

        return View(ranks); // Trả về View cùng với danh sách gói
    }
}
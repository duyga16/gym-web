using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using gym.Models;

public class FeedbackPTViewComponent : ViewComponent
{
    private readonly GymContext _context;

    public FeedbackPTViewComponent(GymContext context)
    {
        _context = context;
    }

    // Hàm này sẽ lấy dữ liệu cho ViewComponent
    public async Task<IViewComponentResult> InvokeAsync(int ptId)
    {
        var feedbacks = await _context.Feedbacks
            .Include(f => f.User)
            .Where(f => f.PtId == ptId) // Lọc feedback theo PtId
            .OrderByDescending(f => f.Datebegin) // Sắp xếp theo thời gian mới nhất
            .Take(3)
            .ToListAsync();

        return View(feedbacks); // Truyền danh sách feedback vào view
    }
}
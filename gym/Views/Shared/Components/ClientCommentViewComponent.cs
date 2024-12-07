using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using gym.Models;

public class ClientCommentViewComponent : ViewComponent
{
    private readonly GymContext _context;

    public ClientCommentViewComponent(GymContext context)
    {
        _context = context;
    }

    // Hàm này sẽ lấy dữ liệu cho ViewComponent
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var feedbacks = _context.Feedbacks
            .Include(f => f.User)
            .OrderByDescending(f => f.Datebegin)  // Sắp xếp theo ngày tạo
            .Take(3)  // Hiển thị 3 bình luận gần nhất
            .ToList();

        return View(feedbacks);  // Trả về danh sách feedback
    }
}
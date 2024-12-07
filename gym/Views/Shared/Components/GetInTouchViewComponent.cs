using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using gym.Models;

public class GetInTouchViewComponent : ViewComponent
{
    private readonly GymContext _context;

    public GetInTouchViewComponent(GymContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync(string userName)
    {
        var user = await _context.Users
                                  .FirstOrDefaultAsync(u => u.UserName == userName);
        if (user == null)
        {
            return Content("User not found"); // Hoặc trả về View khác nếu không tìm thấy
        }
        return View(user); // Trả về View với thông tin user
    }
}
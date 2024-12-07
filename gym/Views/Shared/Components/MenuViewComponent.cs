using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using gym.Models;

public class MenuViewComponent : ViewComponent
{
    private readonly GymContext _context;

    public MenuViewComponent(GymContext context)
    {
        _context = context;
    }

    // Hàm này sẽ lấy dữ liệu cho ViewComponent
    public async Task<IViewComponentResult> InvokeAsync()
    {

        var menus = await _context.Menus
                                  .Where(m => m.Hide == 1)
                                  .OrderBy(m => m.Order)
                                  .ToListAsync();

        // Trả về View cùng với model là danh sách menu
        return View(menus);
    }
}
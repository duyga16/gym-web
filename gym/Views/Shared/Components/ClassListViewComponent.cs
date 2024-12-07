using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using gym.Models;

public class ClassListViewComponent : ViewComponent
{
    private readonly GymContext _context;

    public ClassListViewComponent(GymContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        // Lấy tất cả các lớp học
        var classes = await _context.Classes
            .Where(c => c.Hide == 1)
            .OrderBy(c => c.Order)
            .ToListAsync();
        return View(classes);
    }
}

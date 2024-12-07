using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using gym.Models;

public class ClassDetailsViewComponent : ViewComponent
{
    private readonly GymContext _context;

    public ClassDetailsViewComponent(GymContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync(int id)
    {
        var classDetail = _context.Classes.Find(id);
        return View(classDetail);
    }
}

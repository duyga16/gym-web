using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using gym.Models;

public class GetPtDetailsViewComponent : ViewComponent
{
    private readonly GymContext _context;

    public GetPtDetailsViewComponent(GymContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync(int trainerId)
    {
        var trainer = await _context.Pts.FirstOrDefaultAsync(t => t.PtId == trainerId);
        if (trainer == null)
        {
            return Content("Huấn luyện viên không tồn tại.");
        }

        return View(trainer);
    }
}
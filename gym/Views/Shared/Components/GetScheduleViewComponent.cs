using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using gym.Models;

public class GetScheduleViewComponent : ViewComponent
{
    private readonly GymContext _context;

    public GetScheduleViewComponent(GymContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke(int classId)
    {
        // Lấy dữ liệu từ bảng Schedule dựa trên ClassId
        var schedules = _context.Schdules
            .Where(s => s.ClassId == classId) // Thay đổi từ NameClass sang ClassId
            .OrderBy(s => s.DayOfWeeks)
            .ThenBy(s => s.TimeSlot)
            .ToList();

        return View(schedules);  // Truyền dữ liệu vào view
    }
}
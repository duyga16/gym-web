using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using gym.Models;

public class GetPtIndexViewComponent : ViewComponent
{
    private readonly GymContext _context;

    public GetPtIndexViewComponent(GymContext context)
    {
        _context = context;
    }

    // Hàm này sẽ lấy dữ liệu cho ViewComponent
    public async Task<IViewComponentResult> InvokeAsync()
    {
        // Lấy danh sách PT từ cơ sở dữ liệu và trả về những PT không bị ẩn (hide = false)
        var ptList = _context.Pts
            .Where(pt => pt.Hide == 1)
            .OrderBy(pt => pt.Order)
            .ToList();

        return View(ptList); // Trả về danh sách PT cho View
    }
}
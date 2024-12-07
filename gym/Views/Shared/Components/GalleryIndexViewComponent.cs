using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using gym.Models;

public class GalleryIndexViewComponent : ViewComponent
{
    private readonly GymContext _context;

    public GalleryIndexViewComponent(GymContext context)
    {
        _context = context;
    }

    // Hàm này sẽ lấy dữ liệu cho ViewComponent
    public async Task<IViewComponentResult> InvokeAsync()
    {
        // Lấy dữ liệu từ bảng Gallery
        var galleryItems = await _context.Galleries.ToListAsync();

        // Trả về view với model là danh sách galleryItems
        return View(galleryItems);
    }
}
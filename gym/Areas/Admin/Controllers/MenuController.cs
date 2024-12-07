using gym.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace gym.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Menu")]
    [Authorize(Roles = "Admin")] 
    public class MenuController : Controller
    {
        private readonly GymContext _context;

        public MenuController(GymContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var menus = await _context.Menus.ToListAsync();
            return View(menus);
        }
        // Action để thêm menu mới
        [HttpPost("AddMenu")]
        public IActionResult AddMenu(string menuName, string meta, int hide, int order)
        {
            try
            {
                var newMenu = new Menu
                {
                    MenuName = menuName,
                    Meta = meta,
                    Hide = hide,
                    Order = order,
                };

                // Lưu vào cơ sở dữ liệu
                _context.Menus.Add(newMenu);
                _context.SaveChanges();
                // Sau khi lưu thành công, trả về trang trước đí
                return Redirect(Request.Headers["Referer"].ToString()); 
            }
            catch (Exception)
            {
                return Redirect(Request.Headers["Referer"].ToString());
            }
        }

        // sửa
        [HttpPost("EditMenu")]
        public IActionResult EditMenu(Menu menu)
        {
            if (ModelState.IsValid)
            {
                var existingMenu = _context.Menus.Find(menu.MenuId);
                if (existingMenu != null)
                {
                    existingMenu.MenuName = menu.MenuName;
                    existingMenu.Meta = menu.Meta;
                    existingMenu.Hide = menu.Hide;
                    existingMenu.Order = menu.Order;
                    existingMenu.Datebegin = DateTime.Now;

                    _context.SaveChanges();
                }
                return Redirect(Request.Headers["Referer"].ToString());
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }

        // xóa
        [HttpPost("Delete")]
        public IActionResult Delete(int MenuId)
        {
            // Lấy menu cần xóa từ cơ sở dữ liệu
            var menu = _context.Menus.FirstOrDefault(m => m.MenuId == MenuId);
            if (menu != null)
            {
                _context.Menus.Remove(menu); // Xóa menu
                _context.SaveChanges();      // Lưu thay đổi vào CSDL
            }

            // Quay lại danh sách menu
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}

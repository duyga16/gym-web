using gym.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;


namespace gym.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GymContext _context;

        public HomeController(ILogger<HomeController> logger, GymContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Index()
        {
            
            // Chuyển hướng đến trang với metaMenu  mặc định là "trang-chu"
            return RedirectToAction("DynamicPage", new { meta = "trang-chu" });
        }

        [Route("{meta}")]
        public IActionResult DynamicPage(string meta)
        {
            var menu = _context.Menus.FirstOrDefault(m => m.Meta == meta && m.Hide == 1);
            if (menu != null)
            {
                if (menu.Link == "dang-nhap")
                {
                    return View("~/Views/Account/Login.cshtml"); // Chỉ định rõ view "Login" trong controller "Account"
                }
                if (menu.Link == "dang-ky")
                {
                    return View("~/Views/Accounts/Register.cshtml"); // Chỉ định rõ view "Login" trong controller "Account"
                }
                // Nếu menu có Link, trả về view từ controller Home
                var viewName = menu.Link;
                return View(viewName); // MVC sẽ tự động tìm kiếm view trong Views/Home hoặc Views/{ControllerName}
            }
            string username = HttpContext.Session.GetString("Username");
            if (username != null)
            {
                // Làm gì đó với tên người dùng
                ViewBag.Username = username;
            }
            else
            {
                // Nếu không tìm thấy trong session, có thể người dùng chưa đăng nhập
                ViewBag.Username = "Khách";
            }
            return NotFound("Trang không tồn tại.");
        }

        // đăng kí thành viên
        [HttpPost("rankUser")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> rankUser(int rankID)
        {
            var userName = HttpContext.Session.GetString("Username");

            // Kiểm tra người dùng chưa đăng nhập
            if (userName == null)
            {
                TempData["ErrorMessage"] = "Vui lòng đăng nhập để đăng ký gói thành viên!";
                return RedirectToAction("Login", "Accounts");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null)
            {
                TempData["ErrorMessage"] = "Người dùng không tồn tại!";
                return RedirectToAction("Login", "Accounts");
            }
            // Kiểm tra và xử lý logic đăng ký hạng thành viên ở đây
            user.RankId = rankID;
            var ranks = await _context.Rankusers.FirstOrDefaultAsync(u => u.RankId == rankID);
            user.RankPrice = ranks?.Price;
            user.Datebegin = DateTime.Now;
            try
            {
                TempData["SuccessMessage"] = "Bạn đã đăng ký thành công hạng thành viên!";
                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
            }
            catch
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra trong quá trình đăng ký.";
            }

            // Trở lại trang trước đó
            return Redirect(Request.Headers["Referer"].ToString());
        }

        //  Form liên hệ với Admin cho người dùng (CÒn lỗi)
        [HttpPost("ContactAdmin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactAdmin(Contact model)
        {
            // Kiểm tra xem người dùng đã đăng nhập chưa
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Accounts"); // Chuyển hướng đến trang đăng nhập
            }

            // Lấy UserId từ người dùng hiện tại
            var userId = _context.Users
                .Where(u => u.UserName == User.Identity.Name)
                .Select(u => u.UserId)
                .FirstOrDefault();

            if (userId == 0)
            {
                return Unauthorized(); // Trường hợp User không tồn tại
            }

            // Lưu thông tin liên hệ
            model.UserId = userId; // Gắn UserId từ tài khoản đăng nhập
            _context.Contacts.Add(model);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Bạn đã gửi liên hệ thành công!";
             
            // Trở lại trang trước đó
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}

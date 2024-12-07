using Microsoft.AspNetCore.Mvc;
using gym.Models;
using Microsoft.EntityFrameworkCore;
using gym.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
namespace gym.Controllers
{
    public class PTsController : Controller
    {
        private readonly GymContext _context;
        public PTsController(GymContext context)
        {
            _context = context;
        }

        [Route("doi-ngu/{meta}")]
        public async Task<IActionResult> Details(string meta)
        {
            var trainer = await _context.Pts.FirstOrDefaultAsync(t => t.Meta == meta);
            if (trainer == null)
            {
                return NotFound(); // Trả về lỗi 404 nếu không tìm thấy
            }
            return View(trainer); // Trả về view chi tiết huấn luyện viên
        }

        // tính trung bình tổng số đánh giá PT
        public async Task UpdatePtRating(int ptId)
        {
            // Lấy danh sách feedback của PT
            var feedbacks = await _context.Feedbacks
                .Where(f => f.PtId == ptId)
                .ToListAsync();

            // Kiểm tra nếu không có feedback
            if (feedbacks == null || !feedbacks.Any())
            {
                return; // Không làm gì nếu không có feedback
            }

            // Tính trung bình Rating
            double avgRating = feedbacks.Average(f => (double?)f.Rating) ?? 0.0;

            // Lấy PT cần cập nhật
            var pt = await _context.Pts.FirstOrDefaultAsync(p => p.PtId == ptId);
            if (pt == null)
            {
                return; // Không làm gì nếu không tìm thấy PT
            }

            // Cập nhật Rating làm tròn xuống
            pt.Rating = (int)Math.Round(avgRating, MidpointRounding.AwayFromZero);

            // Lưu thay đổi
            _context.Pts.Update(pt);
            await _context.SaveChangesAsync();
        }

        // bình luận PT
        [HttpPost("SubmitFeedback")]
        [Authorize]  // Đảm bảo người dùng đã đăng nhập
        [ValidateAntiForgeryToken]  // Đảm bảo CSRF protection
        public async Task<IActionResult> SubmitFeedback(Feedback model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Accounts"); // Chuyển hướng nếu chưa đăng nhập
            }

            if (ModelState.IsValid)
            {
                // Lấy UserId từ user đăng nhập
                var userId = await _context.Users
                    .Where(u => u.UserName == User.Identity.Name)
                    .Select(u => u.UserId)
                    .FirstOrDefaultAsync();

                if (userId == 0) return Unauthorized(); // Trường hợp không lấy được UserId                
                model.UserId = userId;


                _context.Feedbacks.Add(model);
                await _context.SaveChangesAsync();  // Dùng SaveChangesAsync để tránh blocking thread chính

                // Cập nhật lại Rating của PT
                var ptAverage = await _context.Pts
                    .Where(pt => pt.PtId == model.PtId)
                    .Select(pt => pt.PtId)
                    .FirstOrDefaultAsync();
                await UpdatePtRating(ptAverage);

                // lấy PT theo meta
                var ptMeta = await _context.Pts
                    .Where(pt => pt.PtId == model.PtId)
                    .Select(pt => pt.Meta)
                    .FirstOrDefaultAsync();
                // Điều hướng về trang chi tiết PT (nếu meta tồn tại)
                if (!string.IsNullOrEmpty(ptMeta))
                {
                    return Redirect("/doi-ngu/" + ptMeta);
                }
                return View(model);
            }
            return View(model); // Nếu không hợp lệ, giữ lại form
        }
    }
}

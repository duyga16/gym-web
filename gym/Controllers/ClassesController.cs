using Microsoft.AspNetCore.Mvc;
using gym.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;
namespace gym.Controllers
{
    public class ClassesController : Controller
    {
        private readonly GymContext _context;

        public ClassesController(GymContext context)
        {
            _context = context;
        }
        [Route("lop-tap")]
        public IActionResult DefaultClass()
        {
            // Chuyển hướng đến action Details với meta là body-building
            return RedirectToAction("Details", new { meta = "body-building" });
        }

        [HttpGet("lop-tap/{meta}")]
        public IActionResult Details(string meta)
        {
            var classDetail = _context.Classes.FirstOrDefault(c => c.Meta == meta);
            if (classDetail == null)
            {
                return NotFound();
            }
            return View(classDetail);
        }

        // tạo lịch tập
        [HttpPost("ConfirmSchedule")]
        public IActionResult ConfirmSchedule(string scheduleIds)
        {
            // Lấy UserId từ người dùng hiện tại
            var userId = _context.Users
                .Where(u => u.UserName == User.Identity.Name)
                .Select(u => u.UserId)
                .FirstOrDefault();

            if (userId == 0)
            {
                // Trường hợp User không tồn tại
                TempData["ErrorMessage"] = "Người dùng không hợp lệ.";
                return RedirectToAction("DefaultClass", "Classes"); // Hoặc trang thích hợp nếu User không hợp lệ
            }

            // Chuyển chuỗi JSON thành mảng các scheduleId
            List<int> scheduleIdsArray = null;
            try
            {
                scheduleIdsArray = JsonConvert.DeserializeObject<List<int>>(scheduleIds);
                Console.WriteLine($"Deserialized scheduleIds: {string.Join(", ", scheduleIdsArray)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deserializing scheduleIds: {ex.Message}");
                TempData["ErrorMessage"] = "Lỗi xử lý dữ liệu lịch. Vui lòng thử lại.";
                return RedirectToAction("DefaultClass", "Classes"); // Hoặc trang thích hợp nếu User không hợp lệ
            }

            // Kiểm tra tính hợp lệ của scheduleIdsArray
            if (scheduleIdsArray == null || !scheduleIdsArray.Any())
            {
                TempData["ErrorMessage"] = "Không có lịch hợp lệ được chọn.";
                return RedirectToAction("DefaultClass", "Classes"); // Hoặc trang thích hợp nếu User không hợp lệ
            }

            // Nếu user hợp lệ và scheduleIds hợp lệ
            try
            {
                foreach (var scheduleIdItem in scheduleIdsArray)
                {
                    // Kiểm tra xem scheduleId có hợp lệ không
                    var schedule = _context.Schdules.FirstOrDefault(s => s.ScheduleId == scheduleIdItem);
                    if (schedule == null)
                    {
                        Console.WriteLine($"Schedule not found for scheduleId: {scheduleIdItem}");
                        TempData["ErrorMessage"] = "Một số lịch không hợp lệ.";
                        return Redirect(Request.Headers["Referer"].ToString());
                    }
                    else
                    {
                        // Kiểm tra số lượng ca đã đăng ký của người dùng cho lớp này
                        var userScheduleCount = _context.UserSchedules
                            .Count(us => us.UserId == userId && us.Schdule.ClassId == schedule.ClassId);

                        if (userScheduleCount >= 4)
                        {
                            TempData["ErrorMessage"] = "Bạn đã đăng ký tối đa 4 ca cho lớp này.";
                            return Redirect(Request.Headers["Referer"].ToString());
                        }

                        // kiểm tra trùng lịch
                        var conflictingSchedule = _context.UserSchedules
                            .Where(us => us.UserId == userId)
                            .Join(_context.Schdules, us => us.ScheduleId, s => s.ScheduleId, (us, s) => new { UserSchedule = us, Schedule = s })
                            .FirstOrDefault(us => us.Schedule.TimeSlot == schedule.TimeSlot && us.Schedule.DayOfWeeks == schedule.DayOfWeeks);

                        if (conflictingSchedule != null)
                        {
                            TempData["ErrorMessage"] = "Lịch học này trùng với lịch bạn đã đăng ký rồi.";
                            return Redirect(Request.Headers["Referer"].ToString());
                        }
                        // Kiểm tra đã đăng ký lịch này chưa
                        var existingUserSchedule = _context.UserSchedules
                            .FirstOrDefault(us => us.UserId == userId && us.ScheduleId == scheduleIdItem);
                        if (existingUserSchedule == null)
                        {
                            var userSchedule = new UserSchedule
                            {
                                UserId = userId,
                                ScheduleId = scheduleIdItem
                            };

                            _context.UserSchedules.Add(userSchedule);
                            // Cập nhật trường Hide của lịch thành 0 (khóa lịch)
                            schedule.Hide = 0;
                            // Kiểm tra xem UserClass đã tồn tại chưa
                            var existingUserClass = _context.UserClasses
                                .FirstOrDefault(uc => uc.UserId == userId && uc.ClassId == schedule.ClassId);

                            if (existingUserClass == null)
                            {
                                // Nếu chưa có, thì thêm mới
                                var userClass = new UserClass
                                {
                                    UserId = userId,
                                    ClassId = schedule.ClassId // Lấy ClassId từ Schedule
                                };
                                _context.UserClasses.Add(userClass);
                                _context.SaveChanges();
                            }
                            else
                            {
                                Console.WriteLine("Đã tồn tại lớp này rồi");
                            }

                            // Tính tổng tiền học phí của user
                            var totalFee = _context.UserClasses
                                .Where(uc => uc.UserId == userId)
                                .Join(_context.Classes, uc => uc.ClassId, c => c.ClassId, (uc, c) => c.Price)
                                .Sum(price => price);  // Tính tổng tiền học phí (sử dụng giá của các lớp đã đăng ký)

                            // Lưu tổng tiền học phí vào bảng User hoặc bất kỳ bảng nào bạn muốn
                            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
                            if (user != null)
                            {
                                user.Price = totalFee; // Lưu vào trường Price dưới dạng chuỗi với định dạng tiền tệ
                            }

                            // tính tổng số lớp đã đăng kí
                            var distinctClassCount = _context.UserClasses
                                .Where(uc => uc.UserId == userId)   // Lọc theo userId
                                .GroupBy(uc => uc.ClassId)           // Nhóm theo ClassId
                                .Count();                            // Đếm số nhóm (số lớp không trùng lặp)
                            if (distinctClassCount > 0 && user != null)
                            {
                                user.CountClass = distinctClassCount;
                            }
                            // Lưu các thay đổi vào cơ sở dữ liệu sau khi tính toán tổng học phí và tổng số lớp
                            _context.SaveChanges();
                        }
                        else
                        {
                            Console.WriteLine($"User already registered for scheduleId: {scheduleIdItem}");
                            TempData["ErrorMessage"] = "Bạn đã đăng ký lịch này rồi.";
                            return Redirect(Request.Headers["Referer"].ToString());
                        }

                    }
                    // Lưu tất cả các thay đổi vào cơ sở dữ liệu
                    _context.SaveChanges();
                }

                TempData["SuccessMessage"] = "Bạn đã đăng ký thành công!";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving changes: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                TempData["ErrorMessage"] = "Đã xảy ra lỗi trong quá trình đăng ký.";
            }

            // Trở lại trang trước đó
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}

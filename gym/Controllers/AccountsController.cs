using gym.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
using BCrypt.Net;
using gym.Services;
using gym.ViewModels;
using System.Security.Principal;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
namespace gym.Controllers
{
    public class AccountsController : Controller
    {
        private readonly GymContext _context;
        private readonly IEmailService _emailService;
        private readonly IWebHostEnvironment _env; // Biến _env để truy cập môi trường web

        public AccountsController(GymContext context, IEmailService emailService, IWebHostEnvironment env)
        {
            _context = context;
            _emailService = emailService;
            _env = env;
        }
        // Action GET cho trang đăng ký
        [Route("dang-ky")]
        public IActionResult Register()
        {
            return View();
        }

        // Action POST cho trang đăng ký
        [HttpPost("dang-ky")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!model.AcceptTerms)
                {
                    ModelState.AddModelError("AcceptTerms", "Bạn phải đồng ý với điều khoản.");
                    return View(model);
                }

                var existingUser = await _context.Accounts
                    .FirstOrDefaultAsync(a => a.UserName == model.Username || a.Email == model.Email);

                if (existingUser != null)
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc email đã tồn tại.");
                    return View(model);
                }

                // thêm vào dữ liệu bảng accounts
                var account = new Account
                {
                    UserName = model.Username,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                    IsEmailConfirmed = 0
                };
                // Tạo mã xác nhận email
                var token = Guid.NewGuid().ToString();
                account.EmailConfirmationToken = token;

                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();

                // thêm vào dữ liệu bảng users
                var users = new User
                {
                    AccountId = account.AccountId,
                    UserName = model.Username,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber
                };
                _context.Users.Add(users);
                await _context.SaveChangesAsync();

                // Tạo URL xác nhận
                var confirmationLink = Url.Action("ConfirmEmail", "Accounts", new { userId = account.AccountId, token = token }, Request.Scheme);

                // Gửi email xác nhận
                await _emailService.SendEmailAsync(account.Email, "Xác nhận email của bạn",
                    $"Vui lòng xác nhận email của bạn bằng cách nhấn vào liên kết này: <a href='{confirmationLink}'>Xác nhận Email</a>");

                ViewData["Message"] = "Vui lòng kiểm tra email để xác nhận tài khoản.";
                ViewData["MessageTitle"] = "Xác nhận đăng ký!";
                ViewData["AlertClass"] = "success";
                return View(model);
            }
            // Đăng ký thất bại
            ViewData["Message"] = "Vui lòng thử lại.";
            ViewData["MessageTitle"] = "Đăng ký thất bại!";
            ViewData["AlertClass"] = "danger"; // Lớp thông báo thất bại của Bootstrap
            return View(model);
        }

        // Action xác nhận email
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(int userId, string token)
        {
            var account = await _context.Accounts.FindAsync(userId);
            if (account == null || account.EmailConfirmationToken != token)
            {
                return NotFound("Liên kết xác nhận không hợp lệ.");
            }

            account.IsEmailConfirmed = 1;
            account.EmailConfirmationToken = null;
            await _context.SaveChangesAsync();
            // Lưu userName vào TempData để chuyển đến trang đăng nhập
            TempData["UserName"] = account.UserName;

            return Redirect("/dang-nhap");
        }

        // Action cho trang đăng nhập
        [Route("dang-nhap")]
        public IActionResult Login()
        {
            if (TempData["UserName"] != null)
            {
                ViewData["UserName"] = TempData["UserName"].ToString();
            }

            return View();
        }

        // Action POST cho trang đăng nhập
        [HttpPost("dang-nhap")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var account = await _context.Accounts
                    .FirstOrDefaultAsync(a => a.UserName == model.Username || a.Email == model.Username);

                if (account == null)
                {
                    // Nếu tài khoản không tồn tại, thêm lỗi vào ModelState
                    ModelState.AddModelError("Username", "Chưa đăng ký tài khoản với tên người dùng này.");
                    return View(model);
                }

                // Kiểm tra mật khẩu (nếu có xác thực mật khẩu)
                if (!BCrypt.Net.BCrypt.Verify(model.Password, account.PasswordHash)) // Giả sử mật khẩu đã mã hóa
                {
                    ModelState.AddModelError("Password", "Mật khẩu không đúng.");
                    return View(model);
                }

                if (account.IsEmailConfirmed != 1)
                {
                    ModelState.AddModelError("", "Bạn cần xác nhận email trước khi đăng nhập.");
                    return View(model);
                }

                // Lưu thông tin người dùng vào session 
                HttpContext.Session.SetString("Username", account.UserName);
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
                // Lưu thông tin vào Claims
                var role = account.Roles ?? "User"; // Nếu không có role, mặc định là "User"
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, account.UserName),
            new Claim(ClaimTypes.Role, role)
        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                // Kiểm tra Role và chuyển hướng
                if (role == "Admin") // Nếu là Admin
                {
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                }
                else // Người dùng bình thường
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        // action get cho quên mật khẩu
        [Route("quen-mat-khau")]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        // action post cho quên mật khẩu
        [HttpPost("quen-mat-khau")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Accounts.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("Email", "Email không tồn tại trong hệ thống.");
                    return View(model);
                }

                // Tạo mã xác thực (OTP)
                var otp = new Random().Next(100000, 999999).ToString();
                user.ResetToken = otp;
                user.ResetTokenExpires = DateTime.Now.AddMinutes(10);
                await _context.SaveChangesAsync();

                // Gửi email
                var subject = "Mã xác thực quên mật khẩu";
                var body = $"Mã xác thực của bạn là: <b>{otp}</b>. Mã này sẽ hết hạn sau 10 phút.";
                await _emailService.SendEmailAsync(model.Email, subject, body);

                return RedirectToAction("VerifyOtp", new { email = model.Email });
            }
            return View(model);
        }

        // action get cho mã xác thực
        [Route("verifyOtp")]
        public IActionResult VerifyOtp(string email)
        {
            return View(new VerifyOtpViewModel { Email = email });
        }

        // action post cho mã xác thực
        [HttpPost("verifyOtp")]
        public async Task<IActionResult> VerifyOtp(VerifyOtpViewModel model)
        {
            var user = await _context.Accounts.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null || user.ResetToken != model.Otp || user.ResetTokenExpires < DateTime.Now)
            {
                ModelState.AddModelError("Otp", "Mã xác thực không hợp lệ hoặc đã hết hạn.");
                return View(model);
            }

            return RedirectToAction("ResetPassword", new { email = model.Email });
        }

        // action get cho reset mật khẩu
        [Route("resetPassword")]
        public IActionResult ResetPassword(string email)
        {
            // Truyền email vào View để xác định người dùng
            var model = new ResetPasswordViewModel { Email = email };
            return View(model);
        }
        // action post cho reset mật khẩu
        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _context.Accounts.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Người dùng không tồn tại.");
                return View(model);
            }

            // Cập nhật mật khẩu
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
            user.ResetToken = null;
            user.ResetTokenExpires = null;
            await _context.SaveChangesAsync();

            TempData["UserName"] = user.UserName;
            // Nếu chưa đăng nhập, chuyển hướng đến trang Đăng nhập
            return Redirect("/dang-nhap");
        }
        // action cho đăng xuất
        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            // Đăng xuất khỏi cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Xóa thông tin session
            HttpContext.Session.Clear();

            // Chuyển hướng về trang chủ
            return RedirectToAction("index", "Home");
        }

        // get cho tài khoản người dùng
        [HttpGet("tai-khoan")]
        public IActionResult account()
        {
            // Lấy tên người dùng từ Session
            var username = HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(username))
            {
                // Nếu không có username, chuyển hướng đến trang đăng nhập
                return RedirectToAction("Login", "Accounts");
            }

            // Lấy thông tin người dùng từ cơ sở dữ liệu
            var user = _context.Users.FirstOrDefault(u => u.UserName == username);
            if (user == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            // Lấy tất cả người dùng cùng với thông tin Rank của họ
            var users = _context.Users
                .Include(u => u.Rankuser)  // Dùng Include để tải bảng Rank liên quan
                .ToList();
            // Lấy tất cả lớp tập đã mua của người dùng
            // Lấy thông tin người dùng từ cơ sở dữ liệu
            var userClass = _context.Users
                .Include(u => u.UserClasses)  // Bao gồm lớp tập đã mua của người dùng
                .ThenInclude(uc => uc.Class)  // Bao gồm thông tin lớp tập
                .FirstOrDefault(u => u.UserName == username);
            // Trả về view với thông tin người dùng
            return View(user);
        }

        // post cập nhật tài khoản
        [HttpPost("UpdateUserInfo")]
        public IActionResult UpdateUserInfo(UserUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                // Lấy người dùng từ database
                var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                if (user != null)
                {
                    // Cập nhật thông tin người dùng
                    user.FullName = model.FullName;
                    user.Email = model.Email;
                    user.DateOfBirth = model.DateOfBirth;
                    user.PhoneNumber = model.PhoneNumber;
                    user.Gender = model.Gender;
                    user.Addess = model.Addess;

                    // Lưu thay đổi
                    _context.SaveChanges();

                    // Thông báo cập nhật thành công
                    TempData["SuccessMessage"] = "Cập nhật thông tin thành công.";
                    return Redirect(Request.Headers["Referer"].ToString());
                }

                ModelState.AddModelError("", "Không tìm thấy người dùng.");
            }

            return View(model); // Hiển thị lại form với lỗi nếu có
        }



        // Action GET để hiển thị form đổi mật khẩu
        [HttpGet("doi-mat-khau")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        // Action POST để xử lý đổi mật khẩu
        [HttpPost("doi-mat-khau")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Kiểm tra các lỗi của ModelState
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }

                return View(model);
            }

            // Lấy tên người dùng từ Session
            var username = HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(username))
            {
                // Nếu không có username, chuyển hướng đến trang đăng nhập
                return RedirectToAction("Login", "Accounts");
            }

            // Lấy thông tin người dùng từ cơ sở dữ liệu
            var user = _context.Accounts.FirstOrDefault(u => u.UserName == username);
            if (user == null)
            {
                return RedirectToAction("Login", "Accounts");
            }


            // Kiểm tra mật khẩu cũ
            if (!BCrypt.Net.BCrypt.Verify(model.PasswordOld, user.PasswordHash))
            {
                ModelState.AddModelError("PasswordOld", "Mật khẩu cũ không đúng.");
                return View(model);
            }

            // Cập nhật mật khẩu
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
            user.ResetToken = null;
            user.ResetTokenExpires = null;
            await _context.SaveChangesAsync();

            return RedirectToAction("account", "Accounts");  // Chuyển hướng về trang tài khoản
        }

        // lịch tập của người dùng
        [Route("lich-tap")]
        public IActionResult classTimetable()
        {
            // Lấy UserId từ người dùng hiện tại
            var userId = _context.Users
                .Where(u => u.UserName == User.Identity.Name)
                .Select(u => u.UserId)
                .FirstOrDefault();

            if (userId == 0)
            {
                return RedirectToAction("Login", "Accounts"); // Trường hợp người dùng không hợp lệ
            }

            // Lấy danh sách các ca tập mà người dùng đã đăng ký
            var userSchedules = _context.UserSchedules
                .Where(us => us.UserId == userId)
                .Select(us => us.ScheduleId)
                .ToList();


            if (userSchedules == null || !userSchedules.Any())
            {
                // Không có lịch tập, trả về thông báo lỗi hoặc danh sách trống
                return View(new List<Schdule>());
            }

            var schedules = _context.Schdules
                 .Where(s => userSchedules.Contains(s.ScheduleId))
                 .ToList();

            if (schedules == null)
            {
                // Không có lịch nào được tìm thấy, xử lý trường hợp này
                return View(new List<Schdule>());
            }

            return View(schedules);
        }

        // bmi
        [Route("bmi")]
        public IActionResult bmiCalculator()
        {
            var userId = _context.Users
                .Where(u => u.UserName == User.Identity.Name)
                .Select(u => u.UserId)
                .FirstOrDefault();

            var healthInfo = _context.Healthinfos.FirstOrDefault(h => h.UserId == userId);

            return View(healthInfo); // Truyền healthInfo làm Model
        }
        // tính chỉ số BMI
        [HttpPost("bmi")]
        public async Task<IActionResult> bmiCalculator(double height, double weight, int age, string gender)
        {
            // Lấy UserId từ người dùng hiện tại
            var userId = _context.Users
                .Where(u => u.UserName == User.Identity.Name)
                .Select(u => u.UserId)
                .FirstOrDefault();

            if (userId == 0)
            {
                return RedirectToAction("Login", "Accounts"); // Trường hợp người dùng không hợp lệ
            }

            // Tính chỉ số BMI
            double heightInMeters = height / 100;  // Chuyển chiều cao từ cm sang m
            decimal? bmi = Convert.ToDecimal(weight / (heightInMeters * heightInMeters)); // dùng Convert.ToDecimal()
                                                                                          // Kiểm tra xem đã có dữ liệu HealthInfo cho người dùng này chưa
            var existingHealthInfo = _context.Healthinfos.FirstOrDefault(h => h.UserId == userId);

            if (existingHealthInfo != null)
            {
                // Cập nhật dữ liệu hiện có
                existingHealthInfo.Height = Convert.ToDecimal(height);
                existingHealthInfo.Weight = Convert.ToDecimal(weight);
                existingHealthInfo.Age = age;
                existingHealthInfo.Gender = gender;
                existingHealthInfo.Bmi = bmi;

                _context.Healthinfos.Update(existingHealthInfo);
            }
            else
            {
                // Tạo đối tượng mới nếu chưa có
                var healthInfo = new Healthinfo
                {
                    Height = Convert.ToDecimal(height),
                    Weight = Convert.ToDecimal(weight),
                    Age = age,
                    Gender = gender,
                    Bmi = bmi,
                    UserId = userId
                };

                _context.Healthinfos.Add(healthInfo);
            }
            await _context.SaveChangesAsync();
            
            TempData["SuccessMessage"] = "Tính toán chỉ số BMI thành công!";
            // Trở lại trang trước đó
            return Redirect(Request.Headers["Referer"].ToString());
        }

        // xóa tài khoản
        [HttpPost("DeleteAccount")]
        public IActionResult DeleteAccount(int id)
        {
            try
            {
                // Lấy dữ liệu từ database
                var user = _context.Users.Find(id);
                if (user != null)
                {
                    _context.Users.Remove(user); // Xóa tài khoản
                    _context.SaveChanges();     // Lưu thay đổi
                }
                return RedirectToAction("index", "Home"); // Quay lại trang chính sau khi xóa
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu cần
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // up file ảnh
        [HttpPost("UpdateProfileImage")]
        public async Task<IActionResult> UpdateProfileImage(IFormFile ProfileImage)
        {
            if (ProfileImage != null && ProfileImage.Length > 0)
            {
                // Lấy user hiện tại
                var userName = User.Identity.Name; // Hoặc ID user nếu cần
                var user = _context.Users.FirstOrDefault(u => u.UserName == userName);

                if (user == null) return NotFound("User không tồn tại.");

                // Tạo đường dẫn lưu file
                var uploadsFolder = Path.Combine(_env.WebRootPath, "img", "users");
                var fileName = $"{userName}_{Path.GetFileName(ProfileImage.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                // Lưu file vào thư mục
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ProfileImage.CopyToAsync(stream);
                }

                // Cập nhật đường dẫn vào database
                user.Img = fileName;
                _context.Update(user);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Cập nhật ảnh đại diện thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = "Vui lòng chọn file hợp lệ!";
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}

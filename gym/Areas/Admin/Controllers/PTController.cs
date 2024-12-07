using gym.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using static System.Net.Mime.MediaTypeNames;

namespace gym.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/PT")]
    [Authorize(Roles = "Admin")] 
    public class PTController : Controller
    {
        private readonly GymContext _context;
        private readonly IWebHostEnvironment _env; // Biến _env để truy cập môi trường web

        public PTController(GymContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            var Pts = await _context.Pts.ToListAsync();
            return View(Pts);
        }

        // Action để thêm menu mới
        [HttpPost("AddPT")]
        public async Task<IActionResult> AddPT(string NamePT,string Email,string PhoneNumber, IFormFile Image, string Height, string Weight, int Age,string Gender, string Skills,string Specialty, int ExperienceYears, string Descriptions,string Slogan, string meta, int hide, int order)
        {
            try
            {
                // Tạo đường dẫn lưu file
                var uploadsFolder = Path.Combine(_env.WebRootPath, "img", "team");
                var fileName = $"{meta}_{Path.GetFileName(Image.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                // Lưu file vào thư mục
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Image.CopyToAsync(stream); // Sử dụng CopyToAsync để tránh blocking
                }

                // thêm mới
                var newPT = new Pt
                {
                    NamePt = NamePT,
                    Email = Email,
                    PhoneNumber = PhoneNumber,
                    Img = fileName,
                    Height = Height,
                    Weight = Weight,
                    Age = Age,
                    Gender = Gender,
                    Skills = Skills,
                    Specialty = Specialty,
                    ExperienceYears = ExperienceYears,
                    Descriptions = Descriptions,
                    Slogan = Slogan,    
                    Meta = meta,
                    Hide = hide,
                    Order = order,
                };

                // Lưu vào cơ sở dữ liệu
                _context.Pts.Add(newPT);
                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
                // Sau khi lưu thành công, trả về trang trước đí
                return Redirect(Request.Headers["Referer"].ToString());
            }
            catch
            {
                return Redirect(Request.Headers["Referer"].ToString());
            }
        }

        // CKfinder, CKeditor
        [HttpPost]
        [Route("ckfinder/connector")]
        public IActionResult Upload(IFormFile upload)
        {
            if (upload != null && upload.Length > 0)
            {
                try
                {
                    // Tạo thư mục nếu chưa tồn tại
                    var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    if (!Directory.Exists(uploadDir))
                    {
                        Directory.CreateDirectory(uploadDir);
                    }

                    // Lưu file
                    var fileName = Path.GetFileName(upload.FileName);
                    var filePath = Path.Combine(uploadDir, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        upload.CopyTo(stream);
                    }

                    // Trả về URL cho file vừa upload
                    var fileUrl = $"/uploads/{fileName}";
                    return Json(new
                    {
                        uploaded = 1,
                        fileName = fileName,
                        url = fileUrl
                    });
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi
                    return Json(new
                    {
                        uploaded = 0,
                        error = new { message = $"File upload failed: {ex.Message}" }
                    });
                }
            }

            // Trường hợp không có file hoặc lỗi khác
            return Json(new
            {
                uploaded = 0,
                error = new { message = "No file was uploaded." }
            });
        }

        // sửa
        [HttpPost("EditPT")]
        public async Task<IActionResult> EditPT(Pt pts, IFormFile Img)
        {
            try
            {
                var existingPT = await _context.Pts.FindAsync(pts.PtId); // Lấy lớp cần cập nhật từ DB

                if (existingPT != null)
                {
                    // Cập nhật các thuộc tính từ form
                    existingPT.NamePt = pts.NamePt;
                    existingPT.Email = pts.Email;
                    existingPT.PhoneNumber = pts.PhoneNumber;
                    existingPT.Height = pts.Height;
                    existingPT.Weight = pts.Weight;
                    existingPT.Age = pts.Age;
                    existingPT.Gender = pts.Gender;
                    existingPT.Skills = pts.Skills;
                    existingPT.Specialty = pts.Specialty;
                    existingPT.ExperienceYears = pts.ExperienceYears;
                    existingPT.Descriptions = pts.Descriptions;
                    existingPT.Slogan = pts.Slogan;
                    existingPT.Meta = pts.Meta;
                    existingPT.Hide = pts.Hide;
                    existingPT.Order = pts.Order;
                    existingPT.Datebegin = DateTime.Now;

                    // Nếu có hình ảnh mới, thay đổi hình ảnh
                    if (Img != null)
                    {
                        // Xử lý hình ảnh: xóa hình cũ và lưu hình mới
                        var uploadsFolder = Path.Combine(_env.WebRootPath, "img", "team");
                        var oldFilePath = Path.Combine(uploadsFolder, existingPT.Img);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath); // Xóa file cũ
                        }

                        // Tạo tên mới cho file hình ảnh
                        existingPT.Img = $"{pts.Meta}_{Path.GetFileName(Img.FileName)}";

                        // Lưu file mới vào thư mục
                        var filePath = Path.Combine(uploadsFolder, existingPT.Img);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await Img.CopyToAsync(stream);
                        }
                    }

                    // Lưu các thay đổi vào database
                    await _context.SaveChangesAsync();

                    // Chuyển hướng người dùng trở lại trang trước đó
                    return Redirect(Request.Headers["Referer"].ToString());
                }
                else
                {
                    // Nếu không tìm thấy lớp cần cập nhật
                    ModelState.AddModelError(string.Empty, "Class not found.");
                    return Redirect(Request.Headers["Referer"].ToString());
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu có
                ModelState.AddModelError(string.Empty, ex.Message);
                return Redirect(Request.Headers["Referer"].ToString());
            }
        }

        // xóa
        [HttpPost("Delete")]
        public IActionResult Delete(int PtId)
        {
            // Lấy menu cần xóa từ cơ sở dữ liệu
            var pts = _context.Pts.FirstOrDefault(m => m.PtId == PtId);
            if (pts != null)
            {
                _context.Pts.Remove(pts); // Xóa menu
                _context.SaveChanges();      // Lưu thay đổi vào CSDL
            }

            // Quay lại danh sách menu
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}

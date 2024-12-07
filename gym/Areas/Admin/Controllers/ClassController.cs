using gym.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using static System.Net.Mime.MediaTypeNames;
namespace gym.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Class")]
    [Authorize(Roles = "Admin")] 
    public class ClassController : Controller
    {
        private readonly GymContext _context;
        private readonly IWebHostEnvironment _env; // Biến _env để truy cập môi trường web

        public ClassController(GymContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            var classes = await _context.Classes.ToListAsync();
            return View(classes);
        }

        // Action để thêm menu mới
        [HttpPost("AddClass")]
        public async Task<IActionResult> AddClass(string className, IFormFile Image, string price, string title,string description,string detail,string meta,int hide,int order)
        {
            try
            {
                // Chuyển đổi Price từ string sang decimal?
                decimal? parsedPrice = null;
                if (!string.IsNullOrEmpty(price) && decimal.TryParse(price, out var result))
                {
                    parsedPrice = result;
                }
                // Tạo đường dẫn lưu file
                var uploadsFolder = Path.Combine(_env.WebRootPath, "img", "classes");
                var fileName = $"{className}_{Path.GetFileName(Image.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                // Lưu file vào thư mục
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Image.CopyToAsync(stream); // Sử dụng CopyToAsync để tránh blocking
                }

                // thêm mới
                var newClass = new Class
                {
                    NameClass = className,
                    Img = fileName,
                    Price = parsedPrice,
                    Title = title,
                    Descriptions = description,
                    Detail = detail,
                    Meta = meta,
                    Hide = hide,
                    Order = order,
                };

                // Lưu vào cơ sở dữ liệu
                _context.Classes.Add(newClass);
                _context.SaveChanges();
                // Sau khi lưu thành công, trả về trang trước đí
                return Redirect(Request.Headers["Referer"].ToString());
            }
            catch
            {
                return Redirect(Request.Headers["Referer"].ToString());
            }
        }

        // sửa
        [HttpPost("EditClass")]
        public async Task<IActionResult> EditClass(Class classes, IFormFile Img)
        {
            try
            {
                var existingClass = await _context.Classes.FindAsync(classes.ClassId); // Lấy lớp cần cập nhật từ DB

                if (existingClass != null)
                {
                    // Cập nhật các thuộc tính từ form
                    existingClass.NameClass = classes.NameClass;
                    existingClass.Price = classes.Price;
                    existingClass.Title = classes.Title;
                    existingClass.Descriptions = classes.Descriptions;
                    existingClass.Detail = classes.Detail;
                    existingClass.Meta = classes.Meta;
                    existingClass.Hide = classes.Hide;
                    existingClass.Order = classes.Order;
                    existingClass.Datebegin = DateTime.Now;

                    // Nếu có hình ảnh mới, thay đổi hình ảnh
                    if (Img != null)
                    {
                        // Xử lý hình ảnh: xóa hình cũ và lưu hình mới
                        var uploadsFolder = Path.Combine(_env.WebRootPath, "img", "classes");
                        var oldFilePath = Path.Combine(uploadsFolder, existingClass.Img);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath); // Xóa file cũ
                        }

                        // Tạo tên mới cho file hình ảnh
                        existingClass.Img = $"{classes.NameClass}_{Path.GetFileName(Img.FileName)}";

                        // Lưu file mới vào thư mục
                        var filePath = Path.Combine(uploadsFolder, existingClass.Img);
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
        public IActionResult Delete(int ClassId)
        {
            // Lấy menu cần xóa từ cơ sở dữ liệu
            var classes = _context.Classes.FirstOrDefault(m => m.ClassId == ClassId);
            if (classes != null)
            {
                _context.Classes.Remove(classes); // Xóa menu
                _context.SaveChanges();      // Lưu thay đổi vào CSDL
            }

            // Quay lại danh sách menu
            return Redirect(Request.Headers["Referer"].ToString());
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

    }
}

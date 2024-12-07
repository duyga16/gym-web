using gym.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using static System.Net.Mime.MediaTypeNames;

namespace gym.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/News")]
    [Authorize(Roles = "Admin")] 
    public class NewsController : Controller
    {
        private readonly GymContext _context;
        private readonly IWebHostEnvironment _env; // Biến _env để truy cập môi trường web

        public NewsController(GymContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            var News = await _context.News.ToListAsync();
            return View(News);
        }
        // Action để thêm menu mới
        [HttpPost("AddNews")]
        public async Task<IActionResult> AddNews(string NewsName, string Script, IFormFile Image, string Descriptions, string Detail, string Title, string Slogan, string SubMeta, string Meta, int hide, int order)
        {
            try
            {
                string metaName = "";  // Biến để gán giá trị theo điều kiện

                if (Meta == "dich-vu") { metaName = "service"; }
                else if (Meta == "gioi-thieu") { metaName = "gioithieu"; }
                else if (Meta == "su-kien") { metaName = "blogNew"; }
                else if (Meta == "tin-tuc") { metaName = "latestNews"; }
                else { metaName = ""; }

                // Tạo đường dẫn lưu file
                var uploadsFolder = Path.Combine(_env.WebRootPath, "img", metaName);
                var fileName = $"{SubMeta}_{Path.GetFileName(Image.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                // Lưu file vào thư mục
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Image.CopyToAsync(stream); // Sử dụng CopyToAsync để tránh blocking
                }

                // thêm mới
                var newNews = new News
                {
                    NewsName = NewsName,
                    Script = Script,
                    Img = fileName,
                    Descriptions = Descriptions,
                    Detail = Detail,
                    Title = Title,
                    Slogan = Slogan,
                    subMeta = SubMeta,
                    Meta = Meta,
                    Hide = hide,
                    Order = order,
                };

                // Lưu vào cơ sở dữ liệu
                _context.News.Add(newNews);
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
        [HttpPost("EditNews")]
        public async Task<IActionResult> EditNews(News news, string editMeta,IFormFile Img)
        {
            try
            {

                var existingNews = await _context.News.FindAsync(news.NewsId); // Lấy lớp cần cập nhật từ DB

                if (existingNews != null)
                {
                    // Cập nhật các thuộc tính từ form
                    existingNews.NewsName = news.NewsName;
                    existingNews.Script = news.Script;
                    existingNews.Title = news.Title;
                    existingNews.Descriptions = news.Descriptions;
                    existingNews.Detail = news.Detail;
                    existingNews.Slogan = news.Slogan;
                    existingNews.subMeta = news.subMeta;
                    existingNews.Meta = editMeta;
                    existingNews.Hide = news.Hide;
                    existingNews.Order = news.Order;
                    existingNews.Datebegin = DateTime.Now;

                    // Nếu có hình ảnh mới, thay đổi hình ảnh
                    if (Img != null)
                    {
                        string metaName = "";  // Biến để gán giá trị theo điều kiện

                        if (editMeta == "dich-vu") { metaName = "service"; }
                        else if (editMeta == "gioi-thieu") { metaName = "gioithieu"; }
                        else if (editMeta == "su-kien") { metaName = "blogNew"; }
                        else if (editMeta == "tin-tuc") { metaName = "latestNews"; }
                        else { metaName = ""; }

                        // Xử lý hình ảnh: xóa hình cũ và lưu hình mới
                        var uploadsFolder = Path.Combine(_env.WebRootPath, "img", metaName);
                        var oldFilePath = Path.Combine(uploadsFolder, existingNews.Img);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath); // Xóa file cũ
                        }

                        // Tạo tên mới cho file hình ảnh
                        existingNews.Img = $"{news.Meta}_{Path.GetFileName(Img.FileName)}";

                        // Lưu file mới vào thư mục
                        var filePath = Path.Combine(uploadsFolder, existingNews.Img);
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
        public IActionResult Delete(int NewsId)
        {
            // Lấy menu cần xóa từ cơ sở dữ liệu
            var news = _context.News.FirstOrDefault(m => m.NewsId == NewsId);
            if (news != null)
            {
                _context.News.Remove(news); // Xóa menu
                _context.SaveChanges();      // Lưu thay đổi vào CSDL
            }

            // Quay lại danh sách menu
            return Redirect(Request.Headers["Referer"].ToString());
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using gym.Models;
using Microsoft.EntityFrameworkCore;
namespace gym.Controllers
{
    public class BlogsController : Controller
    {
        private readonly GymContext _context;

        public BlogsController(GymContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Details(string meta, string subMeta)
        {
            var newsItem = await _context.News
            .FirstOrDefaultAsync(n => n.Meta == meta && n.subMeta == subMeta);

            if (newsItem == null)
            {
                return NotFound();
            }

            return View(newsItem); // Trả về view chi tiết
        }

        // like bài viết
        [HttpPost("LikeNews")]
        public IActionResult LikeNews(int newsId)
        {
            var news = _context.News.FirstOrDefault(n => n.NewsId == newsId);
            if (news != null)
            {
                news.IsLike += 1;
                _context.SaveChanges();
                Console.WriteLine($"News {newsId} updated. IsLike = {news.IsLike}");
                return Redirect(Request.Headers["Referer"].ToString());
            }
            return NotFound();
        }
    }
}

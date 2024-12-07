using gym.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace gym.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Rank")]
    [Authorize(Roles = "Admin")] 
    public class RankController : Controller
    {
        private readonly GymContext _context;

        public RankController(GymContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var ranks = await _context.Rankusers.ToListAsync();
            return View(ranks);
        }
        // sửa
        [HttpPost("EditRank")]
        public IActionResult EditRank(Rankuser rank)
        {
            if (ModelState.IsValid)
            {
                var existingRank = _context.Rankusers.Find(rank.RankId);
                if (existingRank != null)
                {
                    existingRank.MembershipType = rank.MembershipType;
                    existingRank.RankType = rank.RankType;
                    existingRank.Details = rank.Details;
                    existingRank.Price = rank.Price;
                    existingRank.Meta = rank.Meta;
                    existingRank.Hide = rank.Hide;
                    existingRank.Order = rank.Order;
                    existingRank.Datebegin = DateTime.Now;

                    _context.SaveChanges();
                }
                return Redirect(Request.Headers["Referer"].ToString());
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }
       
    }
}

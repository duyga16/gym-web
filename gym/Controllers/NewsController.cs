using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using gym.Models;

public class NewsController : Controller
{
    private readonly GymContext _context;

    public NewsController(GymContext context)
    {
        _context = context;
    }

    
}
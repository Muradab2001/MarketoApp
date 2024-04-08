using Marketo.Core.Entities;
using Marketo.DataAccess.Contexts;
using Marketo.UI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace Marketo.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Moderator")]


    public class DashboardController : Controller
    {     
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        public DashboardController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, AppDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
     
           
            public async Task<IActionResult> Index()
            {
                HomeVM homeVM = new HomeVM
                {
                    Sliders = await _context.Sliders.ToListAsync(),
                    Furnitures = await _context.Furnitures.Include(c => c.Furnitureimages).ToListAsync(),
                    Categories = await _context.Categories.Include(c => c.Furnitures).ToListAsync(),
                    Contacts = await _context.Contacts.ToListAsync(),
                    Orders = await _context.Orders.ToListAsync(),
                };
                return View(homeVM);
            }
        [HttpGet]    
           public JsonResult GetRandomSalary()
           {
            // Replace this with your logic to fetch actual salary data
            Random random = new Random();
            double salary = random.Next(50000, 100000);
            return Json(salary);
           }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}

using Marketo.Core.Entities;
using Marketo.DataAccess.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marketo.UI.Controllers
{
   
        public class ManagerController : Controller
        {
            private readonly AppDbContext _context;
            private readonly UserManager<AppUser> _userManager;

            public ManagerController(AppDbContext context, UserManager<AppUser> userManager)
            {
                _context = context;
                _userManager = userManager;
            }

            public async Task<IActionResult> Index()
            {
                ViewBag.Orders = await _context.Orders.ToListAsync();
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user == null) return NotFound();
                AppUser name = await _context.Users.Include(n => n.Orders).FirstOrDefaultAsync(n => n.Id == user.Id);
                return View(name);
            }
            [HttpPost]
           
            public async Task<IActionResult> Index(AppUser usernew)
            {
                if (usernew == null) return NotFound();
                ViewBag.Orders = await _context.Orders.ToListAsync();
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user.UserName != usernew.UserName)
            {
                AppUser check = await _userManager.FindByNameAsync(usernew.UserName);
                TempData["Error"] = "There cannot be 2 users with the same name";
                if (check != null) return RedirectToAction(nameof(Index));
            }
              
                user.FirstName = usernew.FirstName;
                user.LastName = usernew.LastName;
                user.UserName = usernew.UserName;
                _context.SaveChanges();
                TempData["name"] = "User management has been successfully replaced";
                return RedirectToAction(nameof(Index));
            }
        }
    }


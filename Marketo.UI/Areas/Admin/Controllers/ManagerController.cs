using Marketo.Core.Entities;
using Marketo.DataAccess.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marketo.UI.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = "Admin")]


    public class ManagerController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public ManagerController(UserManager<AppUser> userManager, AppDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult Index(int page = 1)
        {
            ViewBag.TotalPage = Math.Ceiling((decimal)_context.Users.Count() / 4);
            ViewBag.CurrentPage = page;
            List<AppUser> user = _userManager.Users.Skip((page - 1) * 4).Take(4).ToList();

            return View(user);
        }
        public async Task<IActionResult> ManagerChange(string id)
        {

            AppUser user = await _userManager.FindByIdAsync(id);
            if (user.Admin == null)
            {
                user.Admin = false;
                await _userManager.RemoveFromRoleAsync(user, "Member");
                await _userManager.AddToRoleAsync(user, "Moderator");

            }
            else if (user.Admin == false)
            {
                user.Admin = true;

                await _userManager.RemoveFromRoleAsync(user, "Moderator");
                await _userManager.AddToRoleAsync(user, "Admin");
            }
            else if (user.Admin == true)
            {
                user.Admin = null;

                await _userManager.RemoveFromRoleAsync(user, "Admin");

                await _userManager.AddToRoleAsync(user, "Member");
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}

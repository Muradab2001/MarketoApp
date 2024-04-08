using Marketo.Core.Entities;
using Marketo.DataAccess.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marketo.UI.Controllers
{
    public class FurnitureController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public FurnitureController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Detail(int? id)
        {
            ViewBag.furniture = _context.Furnitures.ToList();
            ViewBag.Category = _context.Categories.ToList();
            ViewBag.Desc = _context.FurnitureDescriptions.ToList();
            if (id == 0 || id == null) return NotFound();
            Furniture furniture = await _context.Furnitures
                .Include(c => c.Furnitureimages)
                .Include(c => c.FurnitureDescription)
                .Include(c => c.Categories)
                .Include(c => c.Comments).ThenInclude(c => c.AppUser)
                //.Include(c => c.Rates).ThenInclude(c => c.AppUser)
                .FirstOrDefaultAsync(c => c.Id == id);
            return View(furniture);
        }
        public async Task<IActionResult> AddComment(int medid, string text)
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) return RedirectToAction("Login", "Contact");
            if (text is null) return RedirectToAction("Detail", "Medicine", new { id = medid });
            if (!ModelState.IsValid) return RedirectToAction("Detail", "Medicine", new { id = medid });
            Comment cmnt = new Comment
            {
                 Text = text,
                FurnitureId = medid,
                Date = DateTime.Now,
                AppUser = user,
                AppUserId = user.Id,

            };
            _context.Comments.Add(cmnt);
            _context.SaveChanges();
            Furniture furniture = _context.Furnitures.Include(m => m.Comments).ThenInclude(c => c.AppUser).FirstOrDefault(f => f.Id == medid);
            return PartialView("_commentPartial", furniture);

        }

        public async Task<IActionResult> DeleteComment(int id)
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (!ModelState.IsValid) return RedirectToAction("Detail", "Furniture");
            if (User.IsInRole("Admin"))
            {
                Comment commentadmin = _context.Comments.FirstOrDefault(c => c.Id == id);
                _context.Comments.Remove(commentadmin);
                _context.SaveChanges();
                return RedirectToAction("Detail", "Furniture", new { id = commentadmin.FurnitureId });
            }
            else if (!_context.Comments.Any(c => c.Id == id && c.AppUserId == user.Id))
            {
                return NotFound();
            }
            Comment comment = _context.Comments.FirstOrDefault(c => c.Id == id && c.AppUserId == user.Id);
            _context.Comments.Remove(comment);
            _context.SaveChanges();
            return RedirectToAction("Detail", "Furniture", new { id = comment.FurnitureId });
        }

        //// rate pr id user id
        //// rate rate user id pr id 5
        /// text user id product id 




    }

    }


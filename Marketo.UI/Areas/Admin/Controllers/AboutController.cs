using FianlProject.Extensions;
using Marketo.Core.Entities;
using Marketo.DataAccess.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marketo.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Moderator")]

    public class AboutController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AboutController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            List<About> abouts = _context.Abouts.ToList();
            _context.SaveChanges();
            return View(abouts);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();
            About about = await _context.Abouts.FirstOrDefaultAsync(s => s.Id == id);
            if (about == null) return NotFound();
            return View(about);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(int? id, About about)
        {
            if (id == null || id == 0) return NotFound();
            About existed = await _context.Abouts.FindAsync(id);
            if (existed == null) return NotFound();
            if (about.Photo == null)
            {
                string filename = existed.Image;
                _context.Entry(existed).CurrentValues.SetValues(about);
                existed.Image = filename;
            }
            else
            {
                if (!about.Photo.ImageIsOkey(3))
                {
                    ModelState.AddModelError("Photo", "choose image file");
                    return View(existed);
                }
                FileExtension.FileDelete(_env.WebRootPath, "assets/image", existed.Image);
                _context.Entry(existed).CurrentValues.SetValues(about);
                existed.Image = await about.Photo.FileCreate(_env.WebRootPath, "assets/image");
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}

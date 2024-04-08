using Marketo.Core.Entities;
using Marketo.DataAccess.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marketo.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Moderator")]


    public class FaqController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public FaqController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            List<Faq> faqs = _context.Faqs.ToList();
            _context.SaveChanges();
            return View(faqs);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();
            Faq faq = _context.Faqs.FirstOrDefault(c => c.Id == id);
            if (faq == null) return NotFound();
            return View(faq);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(int? id, Faq newfaq)
        {
            if (id == null || id == 0) return NotFound();
            if (!ModelState.IsValid) return View();
            Faq existed = await _context.Faqs.FirstOrDefaultAsync(x => x.Id == id);
            if (existed == null) return BadRequest();
            _context.Entry(existed).CurrentValues.SetValues(newfaq);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

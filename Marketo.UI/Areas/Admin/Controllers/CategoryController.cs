using FianlProject.Extensions;
using Marketo.Core.Entities;
using Marketo.DataAccess.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marketo.UI.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin, Moderator")]


public class CategoryController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;

    public CategoryController(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }
    public IActionResult Index()
    
    {

        List<Category> model = _context.Categories.OrderByDescending(m => m.Id).ToList();
        return View(model);
    }
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Create(Category category)
    {
        if (category is null) return NotFound();
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0) return NotFound();
        Category category = _context.Categories.FirstOrDefault(c => c.Id == id);
        if (category == null) return NotFound();
        return View(category);
    }

    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Edit(int? id, Category NewCategory)
    {
        if (id == null || id == 0) return NotFound();
        Category existed = _context.Categories.FirstOrDefault(c => c.Id == id);
        if (existed == null) return NotFound();
        bool RepeatCategory = _context.Categories.Any(c => c.Id != existed.Id && c.Name == NewCategory.Name);
        if (RepeatCategory)
        {
            ModelState.AddModelError("Name", "can not dubilcate name");
            return View();
        }
      existed.Name = NewCategory.Name;
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Detail(int? id)
    {
        if (id == null || id == 0) return NotFound();
        Category category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        if (category == null) return NotFound();
        return View(category);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || id == 0) return NotFound();
        Category category = await _context.Categories.FindAsync(id);
        if (category == null) return NotFound();
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}

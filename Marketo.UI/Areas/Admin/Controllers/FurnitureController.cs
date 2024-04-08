using FianlProject.Extensions;
using Marketo.Core.Entities;
using Marketo.DataAccess.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Marketo.UI.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin, Moderator")]


public class FurnitureController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;

    public FurnitureController(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }
    public async Task<IActionResult> Index()
    {
        List<Furniture> model = await _context.Furnitures
        .Include(c => c.FurnitureDescription)
        .Include(c => c.Categories)
                .Include(c => c.Furnitureimages).Include(f => f.Categories).ToListAsync();

        return View(model);

    }
    public IActionResult Create()
    {
        ViewBag.Description = _context.FurnitureDescriptions.ToList();
        ViewBag.Categories = _context.Categories.ToList();
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(Furniture furniture)
    {
        ViewBag.Description = _context.FurnitureDescriptions.ToList();
        ViewBag.Categories = _context.Categories.ToList();
        if (furniture.MainPhoto == null || furniture.Photos == null)
        {
            ModelState.AddModelError(string.Empty, "must choose 1 main photo");
            return View();
        }
        if (furniture.Article == null)
        {
            ModelState.AddModelError(string.Empty, "Article empty");
            return View();
        }
        if (!furniture.MainPhoto.ImageIsOkey(2))
        {
            ModelState.AddModelError(string.Empty, "choose image file");
            return View();
        }
        furniture.Image = await FileExtension.FileCreate(furniture.MainPhoto, _env.WebRootPath, "assets/image/shop");
        furniture.Furnitureimages = new List<FurnitureImage>();
        TempData["Filename"] = null;
        List<IFormFile> removeable = new List<IFormFile>();
        foreach (var photo in furniture.Photos.ToList())
        {
            if (!photo.ImageIsOkey(2))
            {
                removeable.Add(photo);
                TempData["Filename"] += photo.FileName + ",";
                continue;
            }
            FurnitureImage otherphoto = new FurnitureImage
            {
                Name = await photo.FileCreate(_env.WebRootPath, "assets/image/shop"),
                IsMain = false,
                Alternative = furniture.Name,
                Furniture = furniture
            };
            furniture.Furnitureimages.Add(otherphoto);
        }
        furniture.Furnitureimages.RemoveAll(c => removeable.Any(f => f.FileName == f.FileName));
        FurnitureImage main = new FurnitureImage
        {
            Name = furniture.Image,
            IsMain = true,
            Alternative = furniture.Name,
            Furniture = furniture
        };
        furniture.Furnitureimages.Add(main);

        await _context.Furnitures.AddAsync(furniture);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || id == 0) return NotFound();
        Furniture furniture = await _context.Furnitures.FindAsync(id);
        if (furniture == null) return NotFound();
       



        _context.Furnitures.Remove(furniture);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == 0 || id == null) return NotFound();
        if (!ModelState.IsValid) return View();
        Furniture furniture = await _context.Furnitures
            .Include(c => c.Furnitureimages)
            .Include(c => c.FurnitureDescription)
            .Include(c => c.Categories).FirstOrDefaultAsync(c => c.Id == id);
        if (furniture == null) return NotFound();

        ViewBag.Description = _context.FurnitureDescriptions.ToList();
        ViewBag.Category = _context.Categories.ToList();
        ViewBag.images = _context.FurnitureImages.ToList();
        return View(furniture);
    }

    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Edit(int? id, Furniture furniture)
    {
        ViewBag.Category = _context.Categories.ToList();
        ViewBag.images = _context.FurnitureImages.ToList();
        ViewBag.Description = _context.FurnitureDescriptions.ToList();
        Furniture existed = _context.Furnitures.Include(m => m.Furnitureimages).Include(m => m.Categories).FirstOrDefault(m => m.Id == id);
        if (existed == null) return NotFound();
        //if (!ModelState.IsValid) return View();
        if (furniture.ImagesId == null && furniture.Photos == null)
        {
            ModelState.AddModelError(string.Empty, "Please choose at least one main photo");
            return View(existed);
        }
        if (furniture.MainPhoto == null)
        {
            furniture.Image = existed.Image;
        }
        TempData["FileName"] = default(string);

        if (furniture.Photos != null)
        {
            if (furniture.ImagesId is null)
            {
                foreach (FurnitureImage medicineImage in existed.Furnitureimages.Where(m => m.IsMain == false))
                {
                    FileExtension.FileDelete(_env.WebRootPath, "assets/image/shop", medicineImage.Name);
                }
                existed.Furnitureimages.RemoveAll(p => p.IsMain == false);
            }
            else
            {
                List<FurnitureImage> furnitureimages = existed.Furnitureimages
                  .Where(p => p.IsMain == false && !furniture.ImagesId
                  .Contains(p.Id)).ToList();

                existed.Furnitureimages
                    .RemoveAll(p => furnitureimages.Any(r => p.Id == r.Id));
            }
            foreach (IFormFile image in furniture.Photos.ToList())
            {
                if (furniture.Photos.Count == 0)
                {
                    ModelState.AddModelError("Photos", "None of the detail images are valid type");
                    return View(existed);
                }

                if (!image.ImageIsOkey(2))
                {
                    furniture.Photos.Remove(image);
                    TempData["FileName"] += image.FileName + ",";
                    continue;
                }
                FurnitureImage photos = new FurnitureImage
                {
                    Name = await image.FileCreate(_env.WebRootPath,
                        "assets/image/shop"),
                    IsMain = false,
                    Furniture = existed,
                    Alternative = furniture.Name,
                };
                await _context.FurnitureImages.AddAsync(photos);
            }

        }
        else
        {
            if (furniture.ImagesId is null)
            {
                foreach (FurnitureImage furnitureimage in existed.Furnitureimages.Where(m => m.IsMain == false))
                {
                    FileExtension.FileDelete(_env.WebRootPath, "assets/image/shop", furnitureimage.Name);
                }
                existed.Furnitureimages.RemoveAll(p => p.IsMain == false);
            }
            else
            {
                List<FurnitureImage> furnitureimages = existed.Furnitureimages
                  .Where(p => p.IsMain == false && !furniture.ImagesId
                  .Contains(p.Id)).ToList();
                foreach (FurnitureImage fimage in furnitureimages)
                {
                    FileExtension.FileDelete(_env.WebRootPath, "assets/image/shop", fimage.Name);
                }

                existed.Furnitureimages
                    .RemoveAll(p => furnitureimages.Any(r => p.Id == r.Id));
            }
        }
        if (furniture.MainPhoto != null)
        {
            if (!furniture.MainPhoto.ImageIsOkey(2))
            {
                ModelState.AddModelError("Photos", "Please choose valid image file");
                return View(existed);
            }
            FurnitureImage main = new FurnitureImage
            {
                IsMain = true,
                Alternative = furniture.Name,
                Name = await furniture.MainPhoto
                .FileCreate(_env.WebRootPath,
                "assets/image/shop"),
                Furniture = existed,
            };
            furniture.Image = main.Name;
            string mainPhoto = existed.Furnitureimages
                .FirstOrDefault(p => p.IsMain == true).Name;
            FileExtension.FileDelete(_env.WebRootPath,
                "assets/image/shop", mainPhoto);
            existed.Furnitureimages.FirstOrDefault(p => p.IsMain == true).Name = main.Name;
            existed.Furnitureimages.FirstOrDefault(p => p.IsMain == true)
                .Alternative = main.Alternative;
        }
        _context.Entry(existed).CurrentValues.SetValues(furniture);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Detail(int? id)
    {
        if (id == null || id == 0) return NotFound();
        Furniture furniture = await _context.Furnitures.FirstOrDefaultAsync(c => c.Id == id);
        if (furniture == null) return NotFound();
        return View(furniture);
    }
}

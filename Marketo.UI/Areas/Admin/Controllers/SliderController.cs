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

    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            List<Slider>sliders =_context.Sliders.ToList();
            return View(sliders);
        }
        public IActionResult Create()
        {
            ViewBag.Description = _context.Numbers.Include(n => n.slider)
            .Where(number => number.slider.Count == 0)
            .ToList();

            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(Slider slider)
        {
            ViewBag.Description = _context.Numbers.Include(n => n.slider).ToList();
            if (slider.Photo is null)
            {
                ModelState.AddModelError("Photo", "plase choose image file");
            }
            int count = 0;
            foreach (var item in _context.Sliders)
            {
                if (item.Id != null)
                    count++;
            }
            if (count == 3)
            {
                ModelState.AddModelError("Order", "You have exceeded the image limit");
                return View();
            }
            //if (!ModelState.IsValid) return View();
            if (!slider.Photo.ImageIsOkey(2))
            {
                ModelState.AddModelError("Photo", "plase choose image file");
                return View();
            }
            slider.Image = await slider.Photo.FileCreate(_env.WebRootPath, "assets/image/slider");
            await _context.Sliders.AddAsync(slider);
            _context.SaveChanges();

           
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();
            Slider slider = await _context.Sliders.FindAsync(id);
            if (slider == null) return NotFound();
            _context.Sliders.Remove(slider);
            var rootpath = Path.Combine(_env.WebRootPath, "assets/image/slider", slider.Image);
            System.IO.File.Delete(rootpath);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();
            Slider slider = await _context.Sliders.Include(s=>s.OrderItemNumber).FirstOrDefaultAsync(s => s.Id == id);
            if (slider == null) return NotFound();
            List<OrderItemNumber> orderItemNumbers = new List<OrderItemNumber>();

            List<OrderItemNumber> numbers = _context.Numbers
            .Include(n => n.slider)
            .Where(number => number.slider.Count == 0)
            .ToList();
            orderItemNumbers.AddRange(numbers);
            orderItemNumbers.Add(slider.OrderItemNumber);
            ViewBag.Description = orderItemNumbers;
            return View(slider);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(int? id, Slider slider)
        {
            if (id == null || id == 0) return NotFound();
            Slider existed = await _context.Sliders.FindAsync(id);
            if (existed == null) return NotFound();
          
            if (slider.Photo == null)
            {
                string filename = existed.Image;
                _context.Entry(existed).CurrentValues.SetValues(slider);
                existed.Image = filename;
            }
            else
            {
                if (!slider.Photo.ImageIsOkey(3))
                {
                    ModelState.AddModelError("Photo", "choose image file");
                    return View(existed);
                }
                FileExtension.FileDelete(_env.WebRootPath, "assets/image", existed.Image);
                _context.Entry(existed).CurrentValues.SetValues(slider);
                existed.Image = await slider.Photo.FileCreate(_env.WebRootPath, "assets/image");
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}

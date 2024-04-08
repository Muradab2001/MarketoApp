using Marketo.Core.Entities;
using Marketo.DataAccess.Contexts;
using Marketo.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marketo.UI.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }
    public IActionResult Index(string str)
    {
        List <Slider> sliders= _context.Sliders.ToList();
        List<Category> categories = _context.Categories.ToList();
        List<Furniture>furnitures= _context.Furnitures.ToList();
        List<WishlistItem> wishlistItems=_context.WishlistItems.ToList();
        HomeVM vm = new HomeVM
        {
            Sliders = sliders,
            Categories = categories,
            Furnitures=furnitures,
            wishlistItems=wishlistItems
        };



        if (str!=null) 
        {

            List<Furniture> Serach = _context.Furnitures.Include(x => x.Furnitureimages).Where(x => x.Name.Trim().ToLower().Contains(str)).ToList();
            vm.Furnitures = Serach;


        }
        return View(vm);
    }
    public async Task<IActionResult> Faq()
    {
        List<Faq> faqs = await _context.Faqs.ToListAsync();
        return View(faqs);
    }

    public async Task<IActionResult> About()
    {
        ViewBag.furniture = _context.Furnitures.ToList();
        ViewBag.user = _context.Users.ToList();
        List<About> abouts = await _context.Abouts.ToListAsync();
        return View(abouts);
    }
}

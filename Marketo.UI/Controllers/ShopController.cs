using Marketo.Core.Entities;
using Marketo.DataAccess.Contexts;
using Marketo.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marketo.UI.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext _context;

        public ShopController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? id, int key = 0, int page = 1, int max = 0, int min = 0)
        {
            ViewBag.CurrentKey = key;
            ViewBag.CurrentPage = page;
            ViewBag.Max = max;
            ViewBag.Min = min;
            ViewBag.CurrentCategory = id;
            HomeVM home = new HomeVM
            {
                Categories = _context.Categories.Include(c => c.Furnitures).ToList(),
                

            };
            if (id == null)
            {
                home.Furnitures = _context.Furnitures.ToList();
            }
            else
            {
                home.Furnitures = _context.Furnitures.Where(m => m.CategoryId == id).ToList();
            }
            ViewBag.TotalPage = Math.Ceiling((decimal)home.Furnitures.Count() / 4);

         
            if (max != 0 || min != 0)
            {
                home.Furnitures = home.Furnitures.Where(m => m.Price > min && m.Price < max).ToList();
                ViewBag.TotalPage = Math.Ceiling((decimal)home.Furnitures.Count() / 4);

            }

            switch (key)
            {
                case 0:

                    home.Furnitures = home.Furnitures.Skip((page - 1) * 4).Take(4).ToList();
                    break;
                case 2:


                    home.Furnitures = home.Furnitures.OrderBy(Furnitures => Furnitures.Name).Skip((page - 1) * 4).Take(4).ToList();
                    break;
                case 3:
                    home.Furnitures = home.Furnitures.OrderByDescending(Furnitures => Furnitures.Price).Skip((page - 1) * 4).Take(4).ToList();

                    break;
                case 4:
                    home.Furnitures = home.Furnitures.OrderBy(Furnitures => Furnitures.Price).Skip((page - 1) * 4).Take(4).ToList();

                    break;
                default:
                    home.Furnitures = home.Furnitures.OrderBy(Furnitures => Furnitures.Id).Skip((page - 1) * 4).Take(4).ToList();

                    break;

            }

            return View(home);
        }
    }
    }




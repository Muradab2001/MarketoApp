using Marketo.Core.Entities;
using Marketo.DataAccess.Contexts;
using Marketo.UI.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Marketo.UI.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public CartController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");

            return View();
        }
        public IActionResult GetPartial()
        {
            return PartialView("_basketPartial");
        }

        [HttpPost]
        public async Task<IActionResult> increase(int Id)
        {
            int quantity = 0;
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            BasketItem basket = _context.BasketItems.Include(m => m.Furniture).FirstOrDefault(m => m.FurnitureId == Id && m.AppUserId == user.Id);
            basket.Quantity++;
            _context.SaveChanges();
            decimal TotalPrice = 0;
            decimal Price = basket.Quantity * basket.Price;
            quantity = basket.Quantity;
            List<BasketItem> basketItems = _context.BasketItems.Include(m => m.AppUser).Include(m => m.Furniture).Where(m => m.AppUserId == user.Id).ToList();
            foreach (BasketItem item in basketItems)
            {
                Furniture furniture = _context.Furnitures.Include(m => m.Categories).FirstOrDefault(m => m.Id == item.FurnitureId);

                BasketItemVM basketItemVM = new BasketItemVM
                {
                    Furniture = furniture,
                    Quantity = item.Quantity
                };
                basketItemVM.Price = furniture.Price;

                TotalPrice += basketItemVM.Price * basketItemVM.Quantity;

            }

            return Json(new { totalPrice = TotalPrice, Price, quantity });
        }
        public async Task<IActionResult> decrease(int Id)
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            BasketItem basket = _context.BasketItems.Include(m => m.Furniture).FirstOrDefault(m => m.FurnitureId == Id && m.AppUserId == user.Id);
            int quantity = 0;
            if (basket.Quantity == 1)
            {
                basket.Quantity = 1;
            }
            else
            {
                basket.Quantity--;
            }
            _context.SaveChanges();
            decimal TotalPrice = 0;
            decimal Price = basket.Quantity * basket.Price;
            List<BasketItem> basketItems = _context.BasketItems.Include(m => m.AppUser).Include(m => m.Furniture).Where(b => b.AppUserId == user.Id).ToList();
            quantity = basket.Quantity;
            foreach (BasketItem item in basketItems)
            {
                Furniture furniture = _context.Furnitures.FirstOrDefault(m => m.Id == item.FurnitureId);

                BasketItemVM basketItemVM = new BasketItemVM
                {
                    Furniture = furniture,
                    Quantity = item.Quantity
                };
                basketItemVM.Price = furniture.Price;
                TotalPrice += basketItemVM.Price * basketItemVM.Quantity;

            }

            return Json(new { totalPrice = TotalPrice, Price, quantity });
        }
        public async Task<IActionResult> AddBasket(int id)
        {
            Furniture furniture = _context.Furnitures.Include(m => m.Furnitureimages).Include(m => m.Categories).FirstOrDefault(m => m.Id == id);
            if (furniture == null) return View("This is error msg");

            if (User.Identity.IsAuthenticated && User.IsInRole("Member"))
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

                BasketItem basketItem = _context.BasketItems.FirstOrDefault(m => m.FurnitureId == furniture.Id && m.AppUserId == user.Id);
                if (basketItem == null)
                {
                    basketItem = new BasketItem
                    {
                        AppUserId = user.Id,
                        Price = furniture.Price,
                        FurnitureId = furniture.Id,
                        Quantity = 1
                    };
                    _context.BasketItems.Add(basketItem);
                }
                else
                {
                    basketItem.Quantity++;
                }
                _context.SaveChanges();
                //TempData["name"] = "Product added to cart successfully";
                return PartialView("_basketPartial");
            }
            else
            {
                string basket = HttpContext.Request.Cookies["Basket"];

                if (basket == null)
                {
                    List<BasketCookieItemVM> basketCookieItems = new List<BasketCookieItemVM>();

                    basketCookieItems.Add(new BasketCookieItemVM
                    {
                        Id = furniture.Id,
                        Quantity = 1
                    });

                    string basketStr = JsonConvert.SerializeObject(basketCookieItems);


                    HttpContext.Response.Cookies.Append("Basket", basketStr);
                    return PartialView("_basketPartial");
                    //return RedirectToAction("Index", "Home");

                }
                else
                {
                    List<BasketCookieItemVM> basketCookieItems = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(basket);

                    BasketCookieItemVM cookieItem = basketCookieItems.FirstOrDefault(c => c.Id == furniture.Id);

                    if (cookieItem == null)
                    {
                        cookieItem = new BasketCookieItemVM
                        {
                            Id = furniture.Id,
                            Quantity = 1
                        };
                        basketCookieItems.Add(cookieItem);
                    }
                    else
                    {
                        cookieItem.Quantity++;
                    }

                    string basketStr = JsonConvert.SerializeObject(basketCookieItems);

                    HttpContext.Response.Cookies.Append("Basket", basketStr);
                    return PartialView("_basketPartial");
                    //return RedirectToAction("Index", "Home");

                }
            }
        }
        public async Task<IActionResult> DeleteBasketitem(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                List<BasketItem> basketItems = _context.BasketItems.Where(m => m.FurnitureId == id && m.AppUserId == user.Id).ToList();
                foreach (var item in basketItems)
                {
                    _context.BasketItems.Remove(item);
                }
            }
            else
            {
                string basket = HttpContext.Request.Cookies["Basket"];

                List<BasketCookieItemVM> basketCookieItems = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(basket);

                BasketCookieItemVM cookieItem = basketCookieItems.FirstOrDefault(c => c.Id == id);


                basketCookieItems.Remove(cookieItem);

                string basketStr = JsonConvert.SerializeObject(basketCookieItems);

                HttpContext.Response.Cookies.Append("Basket", basketStr);

            }
            _context.SaveChanges();
            //TempData["name"] = "Product added to cart successfully";
            return PartialView("_basketPartial");
        }
        public async Task<IActionResult> removeCartItem(int Id)
        {
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

                List<BasketItem> basketItems = _context.BasketItems.Where(m => m.FurnitureId == Id && m.AppUserId == user.Id).ToList();
                if (basketItems == null) return Json(new { status = 404 });
                foreach (var item in basketItems)
                {
                    _context.BasketItems.Remove(item);
                }
            }

            _context.SaveChanges();


            return Json(new { status = 200 });
        }
    }

}

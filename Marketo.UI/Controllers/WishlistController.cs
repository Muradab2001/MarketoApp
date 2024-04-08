using Marketo.Core.Entities;
using Marketo.DataAccess.Contexts;
using Marketo.DataAccess.Migrations;
using Marketo.UI.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Marketo.UI.Controllers
{
    public class WishlistController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _http;

        public WishlistController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, AppDbContext context, IHttpContextAccessor http)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
            _http = http;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddWishlist(int id)
        {
            Furniture furniture = await _context.Furnitures.Include(m => m.Categories).Include(m => m.Furnitureimages).FirstOrDefaultAsync(m => m.Id == id);

            if (User.Identity.IsAuthenticated && User.IsInRole("Member"))
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

                WishlistItem wishlistItem = await _context.WishlistItems.FirstOrDefaultAsync(m => m.FurnitureId == furniture.Id && m.AppUserId == user.Id);
                if (wishlistItem == null)
                {
                    wishlistItem = new WishlistItem
                    {
                        AppUserId = user.Id,
                        FurnitureId = furniture.Id,
                        Count = 1,
                    };
                    _context.WishlistItems.Add(wishlistItem);
                }
                else
                {
                    wishlistItem.Count = 1;
                }
                _context.SaveChanges();
                return PartialView("_WishlistPartialView");
            }
            else
            {
                string wishlist = HttpContext.Request.Cookies["Wishlist"];

                if (wishlist == null)
                {
                    List<WishlistCookieItemVM> wishlistCookieItems = new List<WishlistCookieItemVM>();

                    wishlistCookieItems.Add(new WishlistCookieItemVM
                    {
                        Id = furniture.Id,
                        Quantity = 1
                    });

                    string wishlistStr = JsonConvert.SerializeObject(wishlistCookieItems);

                    HttpContext.Response.Cookies.Append("Wishlist", wishlistStr);
                    return PartialView("_WishlistPartialView");

                }
                else
                {
                    List<WishlistCookieItemVM> wishlistCookieItems = JsonConvert.DeserializeObject<List<WishlistCookieItemVM>>(wishlist);

                    WishlistCookieItemVM cookieItem = wishlistCookieItems.FirstOrDefault(b => b.Id == furniture.Id);

                    if (cookieItem == null)
                    {
                        cookieItem = new WishlistCookieItemVM
                        {
                            Id = furniture.Id,
                            Quantity = 1
                        };
                        wishlistCookieItems.Add(cookieItem);
                    }
                    else
                    {
                        cookieItem.Quantity = 1;
                    }

                    string wishlistStr = JsonConvert.SerializeObject(wishlistCookieItems);

                    HttpContext.Response.Cookies.Append("Wishlist", wishlistStr);

                    return PartialView("_WishlistPartialView");
                }
            }
        }

        public IActionResult GetWishlistPartial()
        {
            return PartialView("_WishlistPartialView");
        }

        public async Task<IActionResult> DeleteWishListItem(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                List<WishlistItem> wishlistItems = _context.WishlistItems.Where(m => m.FurnitureId == id && m.AppUserId == user.Id).ToList();
                foreach (var item in wishlistItems)
                {
                    _context.WishlistItems.Remove(item);
                }
            }
            else
            {
                string basket = HttpContext.Request.Cookies["Wishlist"];

                List<WishlistCookieItemVM> wishlistCookieItems = JsonConvert.DeserializeObject<List<WishlistCookieItemVM>>(basket);

                WishlistCookieItemVM cookieItem = wishlistCookieItems.FirstOrDefault(c => c.Id == id);


                wishlistCookieItems.Remove(cookieItem);

                string wishlistStr = JsonConvert.SerializeObject(wishlistCookieItems);

                HttpContext.Response.Cookies.Append("Wishlist", wishlistStr);

            }
            _context.SaveChanges();
            return PartialView("_WishlistPartialView");
        }

        //public async Task<IActionResult> DeleteWishListItem(int? id)
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
        //        List<WishlistItem> wishlistItemss = _context.WishlistItems.Where(m => m.FurnitureId == id && m.AppUserId == user.Id).ToList();
        //        foreach (var item in wishlistItemss)
        //        {
        //            _context.WishlistItems.Remove(item);
        //        }
        //    }
        //    else
        //    {
        //        string basket = HttpContext.Request.Cookies["Wishlist"];

        //        List<WishlistCookieItemVM> wishlistCookieItems = JsonConvert.DeserializeObject<List<WishlistCookieItemVM>>(basket);

        //        WishlistCookieItemVM cookieItem = wishlistCookieItems.FirstOrDefault(c => c.Id == id);


        //        wishlistCookieItems.Remove(cookieItem);

        //        string wishlistStr = JsonConvert.SerializeObject(wishlistCookieItems);

        //        HttpContext.Response.Cookies.Append("Wishlist", wishlistStr);

        //    }
        //    _context.SaveChanges();
        //    return PartialView("_WishlistPartialView");
        //}
        //public async Task<IActionResult> DeleteAllWishList(int id)
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
        //        List<WishlistItem> wishlistItemss = _context.WishlistItems.Where(m => m.AppUserId == user.Id).ToList();
        //        foreach (var item in wishlistItemss)
        //        {
        //            _context.WishlistItems.Remove(item);
        //        }
        //    }
        //    else
        //    {
        //        string basket = HttpContext.Request.Cookies["Wishlist"];

        //        List<WishlistCookieItemVM> wishlistCookieItems = JsonConvert.DeserializeObject<List<WishlistCookieItemVM>>(basket);

        //        wishlistCookieItems.Clear();

        //        string wishlistStr = JsonConvert.SerializeObject(wishlistCookieItems);
        //        HttpContext.Response.Cookies.Append("Wishlist", wishlistStr);

        //    }
        //    _context.SaveChanges();
        //    return PartialView("_WishlistPartialView");
        //}
    }
}



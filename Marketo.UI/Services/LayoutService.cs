using Marketo.Core.Entities;
using Marketo.DataAccess.Contexts;
using Marketo.UI.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static System.Net.WebRequestMethods;

namespace Marketo.UI.Services;

public class LayoutService
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _http;
    private readonly UserManager<AppUser> _userManager;

    public LayoutService(AppDbContext context, IHttpContextAccessor http, UserManager<AppUser> userManager)
    {
        _context = context;
        _http = http;
        _userManager = userManager;
    }

    public List<Setting> getSettings()
    {
        List<Setting> settings = _context.Settings.ToList();
        return settings;
    }
    public List<Furniture> GetFurnitures()
    {
        List<Furniture> furniture = _context.Furnitures.ToList();
        return furniture;
    }
    public List<Category> GetCategories()
    {
        List<Category> categories = _context.Categories.ToList();
        return categories;
    }

    public int GetFurnitureCategory(int? id)
    {
        List<Furniture> furnitures = _context.Furnitures.ToList();
        Category category = _context.Categories.FirstOrDefault(c => c.Id == id);
        int count = 0;
        foreach (var item in furnitures)
        {
            if (category.Id == item.CategoryId)
            {
                count++;
            }
        }
        return count;
    }

    public async Task<BasketVM> ShowBasket()
    {
        string basket = _http.HttpContext.Request.Cookies["Basket"];

        BasketVM basketData = new BasketVM
        {
            TotalPrice = 0,
            BasketItems = new List<BasketItemVM>(),
            Count = 0
        };
        if (_http.HttpContext.User.Identity.IsAuthenticated)
        {
            AppUser user = await _userManager.FindByNameAsync(_http.HttpContext.User.Identity.Name);
            List<BasketItem> basketItems = _context.BasketItems.Include(b => b.AppUser).Where(b => b.AppUserId == user.Id).ToList();
            foreach (BasketItem item in basketItems)
            {
                Furniture furniture = _context.Furnitures.Include(m => m.Furnitureimages).Include(m => m.Categories).FirstOrDefault(m => m.Id == item.FurnitureId);
                if (furniture != null)
                {

                    BasketItemVM basketItemVM = new BasketItemVM
                    {
                        Furniture = furniture,
                        Quantity = item.Quantity,
                    };
                    basketItemVM.Price = furniture.Price;
                    basketData.BasketItems.Add(basketItemVM);
                    basketData.Count++;
                    basketData.TotalPrice += basketItemVM.Price * basketItemVM.Quantity;
                }
            }
        }
        else
        {
            if (!string.IsNullOrEmpty(basket))
            {
                List<BasketCookieItemVM> basketCookieItems = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(basket);

                foreach (BasketCookieItemVM item in basketCookieItems)
                {
                    Furniture medicine = _context.Furnitures.FirstOrDefault(f => f.Id == item.Id);
                    if (medicine != null)
                    {
                        BasketItemVM basketItem = new BasketItemVM
                        {
                            Furniture = _context.Furnitures.Include(m => m.Furnitureimages).Include(m => m.Categories).FirstOrDefault(m => m.Id == item.Id),
                            Quantity = item.Quantity

                        };
                        basketItem.Price = medicine.Price;
                        basketData.BasketItems.Add(basketItem);
                        basketData.Count++;
                        basketData.TotalPrice += basketItem.Price * basketItem.Quantity;
                    }
                }

            }
        }

        return basketData;

    }
    public async Task<WishlistVM> GetWishlist()
    {
        string wishlist = _http.HttpContext.Request.Cookies["Wishlist"];

        WishlistVM wishlistData = new WishlistVM
        {
            WishlistItems = new List<WishlistItemVM>(),
            Count = 0
        };
        if (_http.HttpContext.User.Identity.IsAuthenticated)
        {
            AppUser user = await _userManager.FindByNameAsync(_http.HttpContext.User.Identity.Name);
            List<WishlistItem> wishlistItems = _context.WishlistItems.Include(b => b.AppUser).Where(b => b.AppUserId == user.Id).ToList();
            foreach (WishlistItem item in wishlistItems)
            {
                Furniture furniture = _context.Furnitures.Include(m => m.Categories).Include(m => m.Furnitureimages).FirstOrDefault(m => m.Id == item.FurnitureId);
                if (furniture != null)
                {
                    WishlistItemVM wishlistItemVM = new WishlistItemVM
                    {
                        Furniture = furniture,
                        Quantity = item.Count
                    };
                    wishlistItemVM.Price = furniture.Price;
                    wishlistData.WishlistItems.Add(wishlistItemVM);
                    wishlistData.Count++;
                }
            }
        }
        else
        {
            if (!string.IsNullOrEmpty(wishlist))
            {
                List<WishlistCookieItemVM> wishlistCookieItems = JsonConvert.DeserializeObject<List<WishlistCookieItemVM>>(wishlist);

                foreach (WishlistCookieItemVM item in wishlistCookieItems)
                {
                    Furniture furniture = _context.Furnitures.Include(m => m.Categories).Include(m => m.Furnitureimages).FirstOrDefault(m => m.Id == item.Id);
                    if (furniture != null)
                    {
                        WishlistItemVM wishlistItem = new WishlistItemVM
                        {
                            Furniture = _context.Furnitures.Include(m => m.Categories).Include(m => m.Furnitureimages).FirstOrDefault(m => m.Id == item.Id),
                            Quantity = item.Quantity
                        };

                        wishlistItem.Price = furniture.Price;
                        wishlistData.WishlistItems.Add(wishlistItem);
                        wishlistItem.Quantity++;
                    }
                }

            }
        }
        return wishlistData;
    }

}
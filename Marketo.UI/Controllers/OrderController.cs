using Marketo.Core.Entities;
using Marketo.DataAccess.Contexts;
using Marketo.UI.Areas.Admin.Controllers;
using Marketo.UI.Services;
using Marketo.UI.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Marketo.UI.Controllers
{
    public class OrderController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;
        private readonly IHubContext<ChatHub> _hubContext;

        public OrderController(UserManager<AppUser> userManager, AppDbContext context, IHubContext<ChatHub> hub)
        {
            _userManager = userManager;
            _context = context;
            _hubContext = hub;
        }
        public async Task<IActionResult> ViewCart()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) return RedirectToAction("Login", "Account");

            OrderVM model = new OrderVM
            {

                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.UserName,
                Email = user.Email,
                BasketItems = _context.BasketItems.Include(m => m.Furniture).Where(m => m.AppUserId == user.Id).ToList(),

            };
            return View(model);

        }
        public async Task<IActionResult> Checkout()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            OrderVM model = new OrderVM
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.UserName,
                Email = user.Email,
                BasketItems = _context.BasketItems.Include(m => m.Furniture).Where(m => m.AppUserId == user.Id).ToList()

            };
         
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(OrderVM orderVM)
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            OrderVM model = new OrderVM
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.UserName,
                Email = user.Email,
                BasketItems = _context.BasketItems.Include(m => m.Furniture).Where(m => m.AppUserId == user.Id).ToList()
            };
           

            TempData["Succeeded"] = false;

            if (model.BasketItems.Count == 0) return RedirectToAction("index", "home");
            Order order = new Order
            {
                Country = orderVM.Country,
                City = orderVM.City,
                Address = orderVM.Address,
                PhoneNumber = orderVM.PhoneNumber,
                TotalPrice = 0,
                Date = DateTime.Now,
                AppUserId = user.Id
            };
          
            foreach (BasketItem item in model.BasketItems)
            {
                order.TotalPrice += item.Furniture.Price * item.Quantity;
                OrderItem orderItem = new OrderItem
                {
                    Name = item.Furniture.Name,
                    Price = item.Furniture.Price,
                    Quantity = item.Quantity,
                    FurnitureId = item.Furniture.Id,
                    Order = order
                };
                List<Furniture> furnitures = _context.Furnitures.ToList();
                foreach (var furniture in furnitures)
                {
                    if (furniture.Id == item.FurnitureId)
                    {
                        furniture.BestSeller += item.Quantity;
                    }
                }
                _context.OrderItems.Add(orderItem);
            }
           
            _context.BasketItems.RemoveRange(model.BasketItems);
            _context.Orders.Add(order);
            _context.SaveChanges();
            await _hubContext.Clients.All.SendAsync("SendSalary", order.TotalPrice,order.Date.ToString("MM/dd/yyyy HH:mm"));
            TempData["Succeeded"] = true;
            TempData["name"] = "Your order has been successfully confirmed";
            return RedirectToAction("index", "home");
        }
    }
}

using Marketo.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Marketo.DataAccess.Contexts;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    public DbSet<Slider> Sliders { get; set; } 
    public DbSet<Setting> Settings { get; set; }
    public DbSet<Furniture> Furnitures { get; set; }
    public DbSet<FurnitureImage> FurnitureImages { get; set; }
    public DbSet<FurnitureDescription> FurnitureDescriptions { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Rate> Rates { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<BasketItem> BasketItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Faq> Faqs { get; set; }
    public DbSet<About> Abouts { get; set; }
    public DbSet<WishlistItem> WishlistItems { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<OrderItemNumber>Numbers { get; set; }
}

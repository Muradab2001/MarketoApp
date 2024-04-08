using Microsoft.AspNetCore.Identity;

namespace Marketo.Core.Entities;

public class AppUser:IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool? Admin { get; set; }
    public bool Active { get; set; }
    public List<BasketItem> BasketItems { get; set; }
    public List<Order> Orders { get; set; }
    public List<Message> Messages { get; set; }
    public List<WishlistItem> WishlistItems { get; set; }
}

using Marketo.Core.Entities;

namespace Marketo.UI.ViewModels
{
    public class HomeVM
    {
        public List<Slider> Sliders { get; set; }
        public List<Category> Categories { get; set; }
        public List<Furniture> Furnitures { get; set; }
        public List<Order> Orders { get; set; }
        public List<Contact> Contacts { get; set; }
        public List<WishlistItem> wishlistItems { get; set; }


    }
}

using Marketo.Core.Entities;

namespace Marketo.UI.ViewModels
{
    public class BasketItemVM
    {
        public Furniture Furniture { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}

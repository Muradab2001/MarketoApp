using Marketo.Core.Entities;

namespace Marketo.UI.ViewModels
{
    public class OrderVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public List<BasketItem> BasketItems { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalPrice { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public int PhoneNumber { get; set; }
    }
}

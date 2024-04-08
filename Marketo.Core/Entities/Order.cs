namespace Marketo.Core.Entities;

public class Order:BaseEntity
{
    public DateTime Date { get; set; }
    public decimal TotalPrice { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string Address { get; set; }
    public int PhoneNumber { get; set; }
    public bool Status { get; set; }
    public List<OrderItem> OrderItems { get; set; }
    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }
}

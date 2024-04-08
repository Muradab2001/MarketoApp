namespace Marketo.Core.Entities;

public class BasketItem:BaseEntity
{
    public int FurnitureId { get; set; }
    public Furniture Furniture { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }
}

namespace Marketo.Core.Entities;

public class WishlistItem:BaseEntity
{
    public int FurnitureId { get; set; }
    public Furniture Furniture { get; set; }
    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }
    public int Count { get; set; }
}

namespace Marketo.Core.Entities;

public class Comment:BaseEntity
{
    public DateTime Date { get; set; }
    public string Text { get; set; }
    public Furniture Furniture { get; set; }
    public int FurnitureId { get; set; }
    public AppUser AppUser { get; set; }
    public string AppUserId { get; set; }

}

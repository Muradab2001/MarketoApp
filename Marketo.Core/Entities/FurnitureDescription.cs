namespace Marketo.Core.Entities;

public class FurnitureDescription:BaseEntity
{
    public string Image { get; set; }
    public List<Furniture> Furnitures { get; set; }
}

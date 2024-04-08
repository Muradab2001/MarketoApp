namespace Marketo.Core.Entities;

public class FurnitureImage:BaseEntity
{
    public string Name { get; set; }
    public string Alternative { get; set; }
    public bool IsMain { get; set; }
    public int FurnitureId { get; set; }
    public Furniture Furniture { get; set; }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Marketo.Core.Entities
{
    public class Furniture:BaseEntity
    {
        public string Image { get; set; }
        [Required]
        public string Name { get; set; }
        public string SKU { get; set; }
        public string Article { get; set; }
        public decimal Price { get; set; }
        public bool Stock { get; set; }
        public int BestSeller { get; set; }
        public bool Trend { get; set; } = false;
        public int FurnitureDescriptionId { get; set; }
        public FurnitureDescription FurnitureDescription { get; set; }
        public List<FurnitureImage> Furnitureimages { get; set; }
        public int CategoryId { get; set; }
        public Category Categories { get; set; }
    
        public List<Comment> Comments { get; set; }
        [NotMapped]
        public IFormFile MainPhoto { get; set; }
        [NotMapped]
        public List<IFormFile> Photos { get; set; }
        [NotMapped]
        public List<int> ImagesId { get; set; }
    }
}

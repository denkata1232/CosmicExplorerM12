using System.ComponentModel.DataAnnotations;
using static BookCatalogWebApp_M12.Data.DataConstants.Category;
namespace BookCatalogWebApp_M12.Data.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;
        public IEnumerable<spaceObject> spaceObjects { get; set; } = new List<spaceObject>();
    }
}

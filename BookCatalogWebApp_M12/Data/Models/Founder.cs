using System.ComponentModel.DataAnnotations;
using static BookCatalogWebApp_M12.Data.DataConstants.Founders;
namespace BookCatalogWebApp_M12.Data.Models
{
    public class Founder

    {
        public int Id { get; set; }
        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;
        public ICollection<spaceObjectFounder> spaceObjectsFounder { get; set; } = new List<spaceObjectFounder>();

    }
}

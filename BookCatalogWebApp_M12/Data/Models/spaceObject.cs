using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static BookCatalogWebApp_M12.Data.DataConstants.spaceObject;
namespace BookCatalogWebApp_M12.Data.Models
{
    public class spaceObject
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; } = null!;

        [Required]
        public string OwnerId { get; set; } = null!;

        public IdentityUser Owner { get; set; } = null!;

        public ICollection<spaceObjectFounder> spaceObjectsFounders { get; set; }
         = new List<spaceObjectFounder>();
    }
}

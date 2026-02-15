namespace BookCatalogWebApp_M12.Data.Models

/// <summary>
/// Междинна таблица за реализиране на връзка Many-to-Many между Книги и Автори.
/// </summary>
{
    public class spaceObjectFounder
    {
        public int spaceObjectId { get; set; }
        public spaceObject spaceObject { get; set; } = null!;
        public int FounderId { get; set; }
        public Founder Founder { get; set; } = null!;
     
    }
}

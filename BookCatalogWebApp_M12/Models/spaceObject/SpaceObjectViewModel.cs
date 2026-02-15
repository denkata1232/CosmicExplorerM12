namespace BookCatalogWebApp_M12.Models.Book
{
    /// <summary>
    /// Модел, използван за визуализация на книга в списъка или детайлите.
    /// </summary>
    public class SpaceObjectViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string Owner { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string CreatedOn { get; set; } = null!;
        public string OwnerId { get; set; } = null!;
        public List<string> Founders { get; set; } = new List<string>();
    }
}

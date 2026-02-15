namespace BookCatalogWebApp_M12.Models.Home
{
    /// <summary>
    /// модел за показване на статистика по категории на началната страница.
    /// </summary>
    public class HomeCategoryViewModel
    {
        public string CategoryName { get; set; } = null!;
        public int spaceObjectsCount { get; set; }
    }
}

namespace BookCatalogWebApp_M12.Models.Home
{
    /// <summary>
    /// Основен модел за началната страница.
    /// /// </summary>
    public class HomeViewModel
    {
        /// <summary>
        /// Общият брой на всички книги, регистрирани в системата.
        /// </summary>
        public int AllspaceObjectsCount { get; set; }

        /// <summary>
        /// Броят книги, добавени от текущо вписания потребител.
        /// Стойността е -1, ако потребителят е анонимен.
        /// </summary>
        public int UserspaceObjectsCount { get; set; }

        /// <summary>
        /// Списък с категории и броя книги във всяка от тях за статистическото табло.
        /// </summary>
        public List<HomeCategoryViewModel> CategoriesWithBooksCount { get; set; }
            = new List<HomeCategoryViewModel>();
    }
}
